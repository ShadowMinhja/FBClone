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
using System.Web;
using System.IO;
using System.Collections.ObjectModel;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Configuration;
using Newtonsoft.Json;

namespace FBClone.WebAPI.Areas.Admin.Controllers
{

    [CustomAuthorize()]
    public class BitesController : BaseController
    {
        private readonly IBiteService biteService;
        private readonly IAspNetUserService aspNetUserService;
        private readonly IQuestionResponseSetService questionResponseSetService;
        private readonly ILocationService locationService;
        private readonly IMenuService menuService;
        private readonly IS3BucketService s3BucketService;
        private CloudBlobContainer container;

        public BitesController() {
        }

        public BitesController(IBiteService biteService, IAspNetUserService aspNetUserService, IQuestionResponseSetService questionResponseSetService, 
            ILocationService locationService, IMenuService menuService, IS3BucketService s3BucketService)
        {
            this.biteService = biteService;
            this.aspNetUserService = aspNetUserService;
            this.questionResponseSetService = questionResponseSetService;
            this.locationService = locationService;
            this.menuService = menuService;      
            this.s3BucketService = s3BucketService;
            this.container = AzureUtil.InitializeStorage();
        }

        // GET: api/Bites
        [EnableQuery()]
        [ResponseType(typeof(Bite))]
        public async Task<IHttpActionResult> Get(string userName, string lastId)
        {
            this.biteService.SetUserId(userName, this.userId);
            if(!String.IsNullOrEmpty(this.userId))
            {
                try
                {
                    var results = await this.biteService.GetAll(lastId);
                    return Ok(results);
                }
                catch(Exception ex)
                {
                    return BadRequest("An unknown error has occurred.");
                }
            }
            else
            {
                return BadRequest("The logged in user could not be determined.");
            }
        }

        [Route("api/Bites/GetBiteById")]
        [ResponseType(typeof(Bite))]
        public IHttpActionResult GetBiteById(string biteId)
        {
            var result = this.biteService.GetById(biteId);
            return Ok(result);
        }

        [ResponseType(typeof(Bite))]
        public async Task<IHttpActionResult> Post()
        {
            QuestionResponseSet questionResponseSet = new QuestionResponseSet();
            this.biteService.SetUserId(null, this.userId);
            Bite bite = new Bite();
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
                foreach (var key in provider.FormData.AllKeys)
                {
                    foreach (var val in provider.FormData.GetValues(key))
                    {
                        switch (key)
                        {
                            case "comment":
                                bite.Comment = val;
                                break;
                            case "foodType":
                                bite.FoodType = val;
                                break;
                            case "venue":
                                Location venue = JsonConvert.DeserializeObject<Location>(val);
                                bite.Venue= venue;
                                break;
                            case "menuItem":
                                MenuItem menuItem = JsonConvert.DeserializeObject<MenuItem>(val);
                                bite.MenuItem = menuItem;
                                break;
                            case "foodTags":
                                bite.FoodTags = val == String.Empty ? null : val.Split(',').ToList();
                                break;
                            case "allergenTags":
                                bite.AllergenTags = val == String.Empty ? null : val.Split(',').ToList();
                                break;
                            case "surveyQuestionResponseSet":
                                questionResponseSet = await postQuestionResponseSet(val, bite);
                                if(questionResponseSet != null)
                                    bite.QuestionResponseSetId = GuidEncoder.Encode(questionResponseSet.Id);
                                break;
                            default:
                                break;
                        }
                    }
                }
                var files = provider.FileData;
                List<string> storedImages = await UploadS3(files, GuidEncoder.Encode(userId));
                bite.Images = storedImages;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            Bite newBite = await biteService.AddActivity(bite, true);
            if (bite.Images.Any())
            {
                bite.Images = bite.Images.Select(x => String.Format("{0}/{1}", GlobalConstants.IMGIXURL, x)).ToList();
            }
            PremakeFBShareImage(newBite);
            bite.QuestionResponseSet = questionResponseSet;
            return Ok(newBite);
        }

        /// <summary>
        /// Used for fixing missing GetStream Records
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="lastId"></param>
        /// <returns></returns>
        [Route("api/Bites/InsertGetStream")]
        public async Task<IHttpActionResult> InsertGetStream(string userName, string biteComment, string mongoObjectId)
        {
            AspNetUser user = aspNetUserService.GetByName(userName);
            this.biteService.SetUserId(userName, user.Id);
            Bite bite = new Bite();
            bite.Comment = biteComment;
            bite.Id = new MongoDB.Bson.ObjectId(mongoObjectId);
            await biteService.AddActivity(bite, false);
            return Ok();
        }

        private async Task<QuestionResponseSet> postQuestionResponseSet(string val, Bite bite)
        {
            QuestionResponseSet questionResponseSet = JsonConvert.DeserializeObject<QuestionResponseSet>(val);
            if (questionResponseSet.QuestionResponses.Count() > 0)
            {
                questionResponseSet.UserId = this.userId;
                var user = aspNetUserService.GetByUserId(Guid.Parse(this.userId));
                questionResponseSet.CustomerName = $"{user.LastName}, {user.FirstName}";
                questionResponseSet.CustomerEmail = user.Email;
                try
                {
                    var newQuestionResponseSet = questionResponseSetService.Add(questionResponseSet);
                    newQuestionResponseSet.QuestionResponses = questionResponseSet.QuestionResponses;
                    await UpdateCategoryAggregateScores(bite, val);
                    return newQuestionResponseSet;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            return null;
        }

        private Task UpdateCategoryAggregateScores(Bite bite, string questionResponseSetString)
        {
            dynamic dynQuestionResponseSet = JsonConvert.DeserializeObject(questionResponseSetString);
            Dictionary<string, double> categoryScores = new Dictionary<string, double>();
            foreach(var cat in dynQuestionResponseSet.categoryScores)
            {
                categoryScores.Add(cat.category.ToString(), Convert.ToDouble(cat.totalScore));
            }
            //TODO: Handle concurrency
            if (bite.Venue != null)
            {
                var location = locationService.GetById(bite.Venue.Id);
                location.ServiceTotalReviews = location.ServiceTotalReviews == null ? 1 : location.ServiceTotalReviews + 1;
                location.ServiceTotalScore = location.ServiceTotalScore == null ? categoryScores["Service"] : location.ServiceTotalScore + categoryScores["Service"];
                location.AmbienceTotalReviews = location.AmbienceTotalReviews == null ? 1 : location.AmbienceTotalReviews + 1;
                location.AmbienceTotalScore = location.AmbienceTotalScore == null ? categoryScores["Experience"] : location.AmbienceTotalScore + categoryScores["Experience"];
                locationService.Update(location);
            }
            if (bite.MenuItem != null)
            {
                var menuItem = bite.MenuItem;
                menuItem.FoodTotalReviews = menuItem.FoodTotalReviews == null ? 1 : menuItem.FoodTotalReviews + 1;
                menuItem.FoodTotalScore = menuItem.FoodTotalScore == null ? categoryScores["Food"] : menuItem.FoodTotalScore + categoryScores["Food"];
                menuService.UpdateMenuItem(menuItem);
            }
            return Task.FromResult(0);
        }

        private void PremakeFBShareImage(Bite newBite)
        {
            string biteId = newBite.Id.ToString();
            string baseUrl = GetBaseURI();
            string filename = String.Concat("FB_", biteId, ".jpg");
            string fbShareUri = String.Concat(container.Uri, "/", filename);
            if (GlobalConstants.ENVIRONMENT == "PROD") { 
                AzureUtil.InvokeImageWebJob(filename, baseUrl, biteId);
            }
        }

        private async Task<List<string>> UploadS3(Collection<MultipartFileData> files, string userId)
        {
            List<string> storedImages = new List<string>();
            foreach (var file in files)
            {
                if (file != null && file.Headers.ContentType.MediaType.Contains("image"))
                {
                    using (FileStream inputStream = File.OpenRead(file.LocalFileName))
                    {
                        string fileHash = Util.GetMD5Hash(inputStream);
                        string extension = file.Headers.ContentDisposition.FileName.Replace("\"", "").Split('.').Last().ToLower();
                        string uniqueBlobName = String.Format("bite_{0}{1}.{2}", userId, fileHash, extension);
                        await this.s3BucketService.WriteObject(GlobalConstants.S3_BUCKET, uniqueBlobName, inputStream);
                        storedImages.Add(uniqueBlobName);
                    }
                }
            }
            return storedImages;
        }

        private string GetBaseURI()
        {
            string baseUrl = String.Format("{0}://{1}{2}",
                                GlobalConstants.ENVIRONMENT == "PROD" ? "https" : Request.RequestUri.Scheme,
                                Request.RequestUri.Host,
                                Request.RequestUri.Port == 80 ? String.Empty : ":" + Request.RequestUri.Port
                            );

            return baseUrl;
        }
    }
}
