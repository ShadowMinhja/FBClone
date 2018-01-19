using FBClone.Model;
using FBClone.Service;
using FBClone.WebAPI.Common;
using FBClone.WebAPI.Controllers;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using System.Web.Http.OData;

namespace FBClone.WebAPI.Areas.Common.Controllers
{
    public class MenuQrCodeController : BaseController
    {
        private readonly IMenuQrCodeService menuQrCodeService;

        public MenuQrCodeController()
        {
        }

        public MenuQrCodeController(IMenuQrCodeService menuQrCodeService)
        {
            this.menuQrCodeService = menuQrCodeService;
        }

        // GET: api/MenuQrCode
        [CustomAuthorize()]
        [EnableQuery()]
        [ResponseType(typeof(MenuQrCode))]
        public IHttpActionResult Get(string locationId)
        {
            try
            {
                var QRCode = menuQrCodeService.AllIncluding(x => x.LocationId == locationId && x.UserId == userId);
                return Ok(QRCode);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //Public facing route to resolve menu QR bar codes
        [Route("api/MenuQrCode/GetQrCodeById")]
        [ResponseType(typeof(MenuQrCode))]
        public IHttpActionResult Get(long menuQrCodeId)
        {
            try
            {
                var QRCode = menuQrCodeService.GetAll().Where(x => x.Id == menuQrCodeId).AsQueryable();
                return Ok(QRCode);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [CustomAuthorize()]
        [ResponseType(typeof(MenuQrCode))]
        public IHttpActionResult Post(string locationId)
        {
            try
            {
                //Delete Previous Menu QR Code if Exist
                var previousMenuQRCode = menuQrCodeService.GetAll().Where(x => x.LocationId == locationId).FirstOrDefault();
                var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnection"].ConnectionString);
                CloudBlobClient blobStorage = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobStorage.GetContainerReference(ConfigurationManager.AppSettings["BlobContainer"]);
                var blobs = container.ListBlobs();
                if (previousMenuQRCode != null)
                {
                    if (blobs.Any())
                    {
                        foreach (CloudBlockBlob b in blobs)
                        {
                            if (b.Name.ToUpper().Contains(String.Format("MENUQRCODE_{0}", previousMenuQRCode.Id).ToUpper()))
                                b.DeleteIfExists();
                        }
                    }
                    menuQrCodeService.Delete(previousMenuQRCode);
                }
                //Add New Menu QR Code
                MenuQrCode addMenuQrCode = new MenuQrCode
                {
                    UserId = userId,
                    LocationId = locationId,
                    QrImageUrl = "blank",
                    CreatedBy = User.Identity.Name
                };
                var newMenuQrCode = menuQrCodeService.Add(addMenuQrCode);
                return Created<MenuQrCode>(Request.RequestUri + newMenuQrCode.Id.ToString(), newMenuQrCode);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [CustomAuthorize()]
        [Route("api/MenuQrCode/UploadMenuQrCodeImage")]
        [ResponseType(typeof(MenuQrCode))]
        public async Task<HttpResponseMessage> Post(int menuQrCodeId)
        {
            try
            {
                // Check if the request contains multipart/form-data.
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                string root = HttpContext.Current.Server.MapPath("~/App_Data");
                var provider = new MultipartFormDataStreamProvider(root);

                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                var file = provider.FileData[0];
                var menuQrCode = menuQrCodeService.GetById(menuQrCodeId);

                if (file != null && file.Headers.ContentType.MediaType.Contains("image"))
                {
                    var fileName = file.Headers.ContentDisposition.FileName.Replace("\"", "");
                    string uniqueBlobName = String.Format("menuQrCode_{0}.{1}", menuQrCodeId, file.Headers.ContentType.MediaType.Split('/').Last());
                    var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnection"].ConnectionString);
                    CloudBlobClient blobStorage = storageAccount.CreateCloudBlobClient();
                    CloudBlobContainer container = blobStorage.GetContainerReference(ConfigurationManager.AppSettings["BlobContainer"]);
                    var blobs = container.ListBlobs();
                    if (blobs.Any())
                    {
                        foreach (CloudBlockBlob b in blobs)
                        {
                            if (b.Name.ToUpper().Contains(String.Format("MENUQRCODE_{0}", menuQrCodeId).ToUpper()))
                                b.DeleteIfExists();
                        }
                    }
                    CloudBlockBlob blob = container.GetBlockBlobReference(uniqueBlobName);
                    blob.Properties.ContentType = file.Headers.ContentType.MediaType;
                    var inputStream = File.OpenRead(file.LocalFileName);
                    blob.UploadFromStream(inputStream);
                    if (blob.Uri != null && blob.Uri.ToString() != String.Empty)
                    {
                        //Rename local file
                        inputStream.Close();
                        File.Delete(String.Format("{0}\\{1}", root, uniqueBlobName));
                        File.Move(file.LocalFileName, String.Format("{0}\\{1}", root, uniqueBlobName));
                        //Update MenuQrCode with Url
                        menuQrCode.QrImageUrl = blob.Uri.ToString();
                        menuQrCodeService.Update(menuQrCode);
                    }
                    return Request.CreateResponse<MenuQrCode>(HttpStatusCode.OK, menuQrCode);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<String>(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }
    }
}
