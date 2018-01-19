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
    public class ApplicationSettingController : TableController<ApplicationSetting>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<ApplicationSetting>(context, Request, Services);
        }

        // GET tables/ApplicationSetting
        public IQueryable<ApplicationSetting> GetAllApplicationSettings()
        {
            return Query();
        }

        // GET tables/ApplicationSetting/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<ApplicationSetting> GetApplicationSetting(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/ApplicationSetting/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<ApplicationSetting> PatchApplicationSetting(string id, Delta<ApplicationSetting> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/ApplicationSetting
        public async Task<IHttpActionResult> PostApplicationSetting(ApplicationSetting item)
        {
            ApplicationSetting current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/ApplicationSetting/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteApplicationSetting(string id)
        {
            return DeleteAsync(id);
        }
    }
}