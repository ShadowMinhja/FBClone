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
using Microsoft.AspNet.Identity;
using FBClone.WebAPI.Common;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using System.IO;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Configuration;
using System.Web;

namespace FBClone.WebAPI.Areas.Admin.Controllers
{
    [CustomAuthorize()]
    public class MenuItemsController : BaseController
    {
        private readonly IMenuService menuService;

        public MenuItemsController() {
        }

        public MenuItemsController(IMenuService menuService)
        {
            this.menuService = menuService;
        }

        [ResponseType(typeof(MenuItem))]
        public IHttpActionResult Get(string id)
        {
            try
            {
                var menuItem = menuService.GetMenuItemById(id);
                if (menuItem == null)
                    return NotFound();
                else
                    return Ok(menuItem);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ResponseType(typeof(MenuItem))]
        public IHttpActionResult Post([FromBody]MenuItem menuItem)
        {
            try
            {
                if (menuItem == null)
                    return BadRequest("MenuItem cannot be null");
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                menuItem.Id = Guid.NewGuid().ToString();
                var newMenuItem = menuService.AddMenuItem(menuItem);
                if (newMenuItem == null)
                    return Conflict();
                return Created<MenuItem>(Request.RequestUri + menuItem.ItemText.ToString(), menuItem);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ResponseType(typeof(MenuItem))]
        public IHttpActionResult Put(string id, [FromBody]MenuItem menuItem)
        {
            try {
                if (menuItem == null)
                    return BadRequest("MenuItem cannot be null");
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var updatedMenuItem = menuService.UpdateMenuItem(menuItem);
                if (updatedMenuItem == null)
                    return NotFound();
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/MenuItems/UploadMenuImage")]
        [ResponseType(typeof(string))]
        public async Task<HttpResponseMessage> Post(string menuItemId)
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
                if (file != null && file.Headers.ContentType.MediaType.Contains("image"))
                {
                    var fileName = file.Headers.ContentDisposition.FileName.Replace("\"", "");
                    string extension = fileName.Split('.').Last().ToLower();
                    string uniqueBlobName = String.Format("menuImage_{0}.{1}", menuItemId, extension);
                    //Delete from Server if already Existing
                    File.Delete(String.Format("{0}\\{1}", root, uniqueBlobName));

                    var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnection"].ConnectionString);
                    CloudBlobClient blobStorage = storageAccount.CreateCloudBlobClient();
                    CloudBlobContainer container = blobStorage.GetContainerReference(ConfigurationManager.AppSettings["BlobContainer"]);
                    var blobs = container.ListBlobs();
                    if (blobs.Any())
                    {
                        foreach (CloudBlockBlob b in blobs)
                        {
                            if (b.Name.ToUpper().Contains(String.Format("MENUIMAGE_{0}", menuItemId).ToUpper()))
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
                        //Don't delete the file to keep a copy on the server. That way, we can retrieve the files
                        //File.Delete(String.Format("{0}\\{1}", root, uniqueBlobName));
                        File.Move(file.LocalFileName, String.Format("{0}\\{1}", root, uniqueBlobName));
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

        [ResponseType(typeof(MenuItem))]
        public void Delete(string id)
        {
            menuService.DeleteMenuItem(id);
        }
    }
}
