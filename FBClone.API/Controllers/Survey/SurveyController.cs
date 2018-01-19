using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.WindowsAzure.Mobile.Service;
using FBClone.DataObjects;
using FBClone.Models;

namespace FBClone.Controllers
{
    public class SurveyController : TableController<Survey>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Survey>(context, Request, Services);
        }

        // GET tables/Survey
        public IQueryable<Survey> GetAllSurveys()
        {
            return Query();
        }

        // GET tables/Survey/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Survey> GetSurvey(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Survey/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Survey> PatchSurvey(string id, Delta<Survey> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/Survey
        public async Task<IHttpActionResult> PostSurvey(Survey item)
        {
            Survey current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Survey/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteSurvey(string id)
        {
            return DeleteAsync(id);
        }
    }
}