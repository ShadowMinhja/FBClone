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
    //TODO: Retire this controller. Answers are included when Question is serialized
    public class AnswerController : TableController<Answer>
    {        
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Answer>(context, Request, Services);
        }

        // GET tables/Answer
        public IQueryable<Answer> GetAllAnswers()
        {
            return Query();
        }

        // GET tables/Answer/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Answer> GetAnswer(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Answer/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Answer> PatchAnswer(string id, Delta<Answer> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/Answer
        public async Task<IHttpActionResult> PostAnswer(Answer item)
        {
            Answer current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Answer/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteAnswer(string id)
        {
            return DeleteAsync(id);
        }
    }
}