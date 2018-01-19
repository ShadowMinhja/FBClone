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
using System.Web.Http.Description;
using FBClone.WebAPI.Common;

namespace FBClone.WebAPI.Areas.Admin.Controllers
{
    //[EnableCorsAttribute("http://localhost:55261", "*", "*")]
    [CustomAuthorize()]
    //[EnableCors(origins: "http://localhost:55261", headers: "*", methods: "*", exposedHeaders: "X-Pagination")]
    public class StaffMembersController : BaseController
    {
        private readonly IStaffMemberService staffMemberService;
        private readonly IApplicationSettingService applicationSettingService;

        public StaffMembersController() { }

        public StaffMembersController(IStaffMemberService staffMemberService, IApplicationSettingService applicationSettingService)
        {
            this.staffMemberService = staffMemberService;
            this.applicationSettingService = applicationSettingService;
        }

        // GET: api/StaffMembers
        [EnableQuery()]
        [ResponseType(typeof(StaffMember))]
        public IHttpActionResult Get()
        {
            try {
                var staffMembers = staffMemberService.Query(x => x.UserId==userId);
                return Ok(staffMembers);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        //public IEnumerable<StaffMember> Get()
        //{
        //    var staffMembers = staffMemberService.GetAll();
        //    return staffMembers.ToList();
        //}

        [ResponseType(typeof(StaffMember))]        
        public IHttpActionResult Get(string id)
        {
            try {
                var staffMember = staffMemberService.GetById(id);
                if (staffMember == null)
                    return NotFound();
                else
                    return Ok(staffMember);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ResponseType(typeof(StaffMember))]
        public IHttpActionResult Post([FromBody]StaffMember staffMember)
        {
            try {
                if (staffMember == null)
                    return BadRequest("Staff Member cannot be null");
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                staffMember.Id = Guid.NewGuid().ToString();
                var newStaffMember = staffMemberService.Add(staffMember);
                if (newStaffMember == null)
                    return Conflict();
                var applicationSetting = applicationSettingService.GetById(userId);
                if (applicationSetting.StaffSetup == false)
                {
                    applicationSetting.StaffSetup = true;
                    applicationSettingService.Update(applicationSetting);
                }
                return Created<StaffMember>(Request.RequestUri + staffMember.Id.ToString(), staffMember);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ResponseType(typeof(StaffMember))]
        public IHttpActionResult Put(string id, [FromBody]StaffMember staffMember)
        {
            try {
                if (staffMember == null)
                    return BadRequest("Staff Member cannot be null");
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var updatedStaffMember = staffMemberService.Update(staffMember);
                if (updatedStaffMember == null)
                    return NotFound();
                return Ok();
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ResponseType(typeof(StaffMember))]
        public void Delete(string id)
        {
            staffMemberService.Delete(id);
        }
    }
}
