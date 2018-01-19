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
    public class QuestionController : TableController<Question>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Question>(context, Request, Services);
        }

        // GET tables/Question
        public IQueryable<Question> GetAllQuestions()
        {
            return Query();
        }

        // GET tables/Question/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Question> GetQuestion(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Question/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Question> PatchQuestion(string id, Delta<Question> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/Question
        public async Task<IHttpActionResult> PostQuestion(Question item)
        {
            Question current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Question/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteQuestion(string id)
        {
            return DeleteAsync(id);
        }
    }
}