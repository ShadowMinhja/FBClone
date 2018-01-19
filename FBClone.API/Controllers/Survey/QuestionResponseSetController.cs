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
    public class QuestionResponseSetController : TableController<QuestionResponseSet>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<QuestionResponseSet>(context, Request, Services);
        }

        //// GET tables/QuestionResponseSet
        //public IQueryable<QuestionResponseSet> GetAllQuestionResponseSets()
        //{
        //    return Query();
        //}

        //// GET tables/QuestionResponseSet/48D68C86-6EA6-4C25-AA33-223FC9A27959
        //public SingleResult<QuestionResponseSet> GetQuestionResponseSet(string id)
        //{
        //    return Lookup(id);
        //}

        // PATCH tables/QuestionResponseSet/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<QuestionResponseSet> PatchQuestionResponseSet(string id, Delta<QuestionResponseSet> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/QuestionResponseSet
        public async Task<IHttpActionResult> PostQuestionResponseSet(QuestionResponseSet item)
        {
            QuestionResponseSet current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/QuestionResponseSet/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteQuestionResponseSet(string id)
        {
            return DeleteAsync(id);
        }
    }
}