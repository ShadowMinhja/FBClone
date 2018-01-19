using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ModelBinding;
using FBClone.Model;
using FBClone.Service;
using System.Web.Http.OData;
using FBClone.WebAPI.Controllers;
using System.Data.Entity.Validation;
using System.Web.Http.Description;
using FBClone.WebAPI.Common;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using System.Configuration;
using System.Web;
using System.IO;
using System.Threading.Tasks;

namespace FBClone.WebAPI.Areas.Admin.Controllers
{
    [CustomAuthorize()]
    public class PromotionsController : BaseController
    {
        private readonly IBlobStorageService blobStorageStore;
        private readonly IApplicationSettingService applicationSettingService;
        public PromotionsController() {

        }

        public PromotionsController(IBlobStorageService blobStorageStore, IApplicationSettingService applicationSettingService) {
            this.blobStorageStore = blobStorageStore;
            this.applicationSettingService = applicationSettingService;
        }

        // GET: api/Promotions
        [EnableQuery()]
        [ResponseType(typeof(string))]
        public IHttpActionResult Get()
        {
            string imagePath;
            try {
                imagePath = this.blobStorageStore.GetBrandingImagePath(userId,
                        ConfigurationManager.ConnectionStrings["StorageConnection"].ConnectionString, 
                        ConfigurationManager.AppSettings["BlobContainer"], "PROMOTION");
                return Ok<string>(imagePath);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ResponseType(typeof(string))]
        public async Task<HttpResponseMessage> Post()
        {
            try {
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
                if(file != null && file.Headers.ContentType.MediaType.Contains("image"))
                {
                    var fileName = file.Headers.ContentDisposition.FileName.Replace("\"", "");
                    string uniqueBlobName = String.Format("promotion_{0}.{1}", userId, file.Headers.ContentType.MediaType.Split('/').Last());
                    var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnection"].ConnectionString);
                    CloudBlobClient blobStorage = storageAccount.CreateCloudBlobClient();
                    CloudBlobContainer container = blobStorage.GetContainerReference(ConfigurationManager.AppSettings["BlobContainer"]);
                    var blobs = container.ListBlobs();
                    if (blobs.Any())
                    {
                        foreach (CloudBlockBlob b in blobs)
                        {
                            if (b.Name.ToUpper().Contains(String.Format("PROMOTION_{0}", userId).ToUpper()))
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
                        var applicationSetting = applicationSettingService.GetById(userId);                        
                        applicationSetting.PromotionSetup = true;
                        applicationSetting.PromoLogoUrl = uniqueBlobName;
                        applicationSettingService.Update(applicationSetting);
                    }
                    return Request.CreateResponse<String>(HttpStatusCode.OK, blob.Uri.ToString());
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
