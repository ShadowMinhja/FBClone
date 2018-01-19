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
    //TODO: Retire this controller. Should save response in one call within QuestionResponseSetController
    public class QuestionResponseController : TableController<QuestionResponse>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<QuestionResponse>(context, Request, Services);
        }

        // GET tables/QuestionResponse
        public IQueryable<QuestionResponse> GetAllQuestionResponses()
        {
            return Query();
        }

        // GET tables/QuestionResponse/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<QuestionResponse> GetQuestionResponse(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/QuestionResponse/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<QuestionResponse> PatchQuestionResponse(string id, Delta<QuestionResponse> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/QuestionResponse
        public async Task<IHttpActionResult> PostQuestionResponse(QuestionResponse item)
        {
            QuestionResponse current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/QuestionResponse/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteQuestionResponse(string id)
        {
            return DeleteAsync(id);
        }
    }
}