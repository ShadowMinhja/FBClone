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
    public class MenuController : TableController<Menu>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Menu>(context, Request, Services);
        }

        // GET tables/Menu
        public IQueryable<Menu> GetAllMenus()
        {
            return Query();
        }

        // GET tables/Menu/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Menu> GetMenu(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Menu/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Menu> PatchMenu(string id, Delta<Menu> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/Menu
        public async Task<IHttpActionResult> PostMenu(Menu item)
        {
            Menu current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Menu/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteMenu(string id)
        {
            return DeleteAsync(id);
        }
    }
}