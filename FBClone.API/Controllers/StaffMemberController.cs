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
    public class StaffMemberController : TableController<StaffMember>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<StaffMember>(context, Request, Services);
        }

        // GET tables/StaffMember
        public IQueryable<StaffMember> GetAllStaffMembers()
        {
            return Query();
        }

        // GET tables/StaffMember/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<StaffMember> GetStaffMember(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/StaffMember/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<StaffMember> PatchStaffMember(string id, Delta<StaffMember> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/StaffMember
        public async Task<IHttpActionResult> PostStaffMember(StaffMember item)
        {
            StaffMember current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/StaffMember/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteStaffMember(string id)
        {
            return DeleteAsync(id);
        }
    }
}