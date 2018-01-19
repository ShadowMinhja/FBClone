using EvoPdf;
using EvoPdf.HtmlToPdfClient;
using FBClone.Model;
using FBClone.Service;
using FBClone.Service.Common;
using FBClone.WebAPI.Common;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FBClone.WebAPI.Controllers
{
    public class GrabController : Controller
    {
        private readonly IBiteService biteService;
        private CloudQueue imageRequestQueue;
        private CloudBlobContainer container;
        public GrabController()
        {
        }

        public GrabController(IBiteService biteService)
        {
            this.biteService = biteService;
            container = AzureUtil.InitializeStorage();
        }

        // GET: Bite
        [OutputCache(CacheProfile = "GrabBiteCache")]
        public ActionResult Bite(string biteId)
        {
            var result = this.biteService.GetById(biteId);
            if (result != null)
            {
                if(result.QuestionResponseSet != null)
                {
                    ViewBag.Survey = Utils.GetSurveyScores(result.QuestionResponseSet);
                }
                ViewBag.MetaTag = HomeMetaTags(result);
                ViewBag.RootURI = GetBaseURI();
                ViewBag.FBAppId = ConfigurationManager.AppSettings["FACEBOOK_APP_ID"];
                return View(result);
            }
            else
            {
                return View("Error");
            }
        }

        [OutputCache(CacheProfile = "FBShareImages")]
        public async Task<FileResult> BiteImage(string biteId)
        {
            var result = this.biteService.GetById(biteId);
            if (result == null)
            {
                return null;
            }
            // get the HTML code of this view
            //string htmlToConvert = RenderViewAsString("Bite", result);
            string filename = String.Concat("FB_", biteId, ".jpg");
            string fbShareUri = String.Concat(container.Uri, "/", filename);
            if (CheckIfFileExists(fbShareUri))
            {
                return new FileStreamResult(DownloadFBShareImage(fbShareUri), "image/jpeg");
            }
            else
            {
                try
                {
                    string baseUrl = GetBaseURI();
                    HttpResponseMessage response =   await AzureUtil.InvokeImageWebJob(filename, baseUrl, biteId);
                    if (response.IsSuccessStatusCode)
                    {
                        //Get blob image                    
                        CheckThrice(fbShareUri); //TODO: Remove this hack for getting this to work
                        return new FileStreamResult(DownloadFBShareImage(fbShareUri), "image/jpeg");
                    }
                    else
                    {
                        return null; //TODO: Add Default Image to Share
                                     //string path = Server.MapPath("~/assets/images/peter-avatar.jpg");
                                     //return new FileStreamResult(new FileStream(path, FileMode.Open), "image/jpeg");
                    }
                    //MemoryStream memoryStream = InvokeEvoImageCloudService(htmlToConvert, baseUrl); //Not Used Unless Using Evo PDF
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        #region Controller Methods

        private void CheckThrice(string fbShareUri)
        {
            int count = 0;
            while(!CheckIfFileExists(fbShareUri) && count < 3)
            {
                System.Threading.Thread.Sleep(500);
                count += 1;
            }
        }

        private bool CheckIfFileExists(string fbShareUri)
        {
            HttpWebResponse response = null;
            var request = (HttpWebRequest)WebRequest.Create(fbShareUri);
            request.Method = "HEAD";

            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                /* A WebException will be thrown if the status of the response is not `200 OK` */
                return false;
            }
            finally
            {
                // Don't forget to close your response.
                if (response != null)
                {
                    response.Close();
                }
            }
            return true;
        }
        private MemoryStream DownloadFBShareImage(string fbShareUri)
        {
            WebClient wc = new WebClient();
            MemoryStream memoryStream = new MemoryStream(wc.DownloadData(fbShareUri));
            return memoryStream;
        }

        private MemoryStream InvokeEvoImageCloudService(string htmlToConvert, string baseUrl)
        {
            string cloudAppConversionServer = ConfigurationManager.AppSettings["CLOUD_APP_SERVER"];
            uint cloudAppConversionPort = (uint)Convert.ToInt32(ConfigurationManager.AppSettings["CLOUD_APP_PORT"]);
            HtmlToImageConverter htmlToImageConverter = new HtmlToImageConverter(cloudAppConversionServer, cloudAppConversionPort);
            htmlToImageConverter.ServicePassword = ConfigurationManager.AppSettings["CLOUD_APP_PW"];
            htmlToImageConverter.NavigationTimeout = 45;
            htmlToImageConverter.ConversionDelay = 0;
            htmlToImageConverter.TriggeringMode = TriggeringMode.Auto;
            htmlToImageConverter.HtmlViewerWidth = 900;
            //var jpegBytes = htmlToImageConverter.ConvertUrl($"{baseUrl}/Grab/BiteImage/{biteId}", ImageType.Jpeg);
            var jpegBytes = htmlToImageConverter.ConvertHtml(htmlToConvert, baseUrl, ImageType.Jpeg);
            MemoryStream memoryStream = new MemoryStream(jpegBytes);
            return memoryStream;
        }

        private string RenderViewAsString(string viewName, object model)
        {
            string result = null;
            // create a string writer to receive the HTML code
            using (StringWriter stringWriter = new StringWriter())
            {

                // get the view to render
                ViewEngineResult viewResult = ViewEngines.Engines.FindView(ControllerContext, viewName, null);
                // create a context to render a view based on a model
                ViewContext viewContext = new ViewContext(
                        ControllerContext,
                        viewResult.View,
                        new ViewDataDictionary(model),
                        new TempDataDictionary(),
                        stringWriter
                        );

                viewContext.ViewBag.RootURI = GetBaseURI();
                // render the view to a HTML code
                viewResult.View.Render(viewContext, stringWriter);

                // return the HTML code
                result = stringWriter.ToString();
            }
            return result;
        }

        private string HomeMetaTags(Bite bite)
        {   
            string url = Request.Url.AbsoluteUri;
            if (GlobalConstants.ENVIRONMENT == "PROD")
                url = url.Replace("http", "https");
            string filename = String.Concat("FB_", bite.Id, ".jpg");
            string fbShareUri = String.Concat(container.Uri, "/", filename);
            System.Text.StringBuilder strMetaTag = new System.Text.StringBuilder();
            strMetaTag.AppendFormat(@"<meta property = 'fb:app_id' content='{0}' />", ConfigurationManager.AppSettings["FACEBOOK_APP_ID"]);
            strMetaTag.AppendFormat(@"<meta property = 'og:image' content='{0}' />", fbShareUri);
            strMetaTag.AppendFormat(@"<meta property = 'og:image:width' content='1200' />");
            strMetaTag.AppendFormat(@"<meta property = 'og:image:height' content='630' />");
            strMetaTag.AppendFormat($"<meta property = 'og:title' content=\"{bite.Comment}\" />");
            strMetaTag.AppendFormat($"<meta property = 'og:description' content=\"{bite.Hashtags}\" />");
            strMetaTag.AppendFormat(@"<meta property = 'og:site_name' content='{0}' />", "fbClone.io");
            strMetaTag.AppendFormat(@"<meta property = 'og:url' content='{0}' />", url);
            strMetaTag.AppendFormat(@"<meta property = 'og:type' content='{0}' />", "website");
            return strMetaTag.ToString();
        }

        private string GetBaseURI()
        {
            string baseUrl = String.Format("{0}://{1}{2}",
                                GlobalConstants.ENVIRONMENT == "PROD" ? "https" : Request.Url.Scheme,
                                Request.Url.Host,
                                Request.Url.Port == 80 ? String.Empty : ":" + Request.Url.Port
                            );

            return baseUrl;
        }
#endregion

    }
}