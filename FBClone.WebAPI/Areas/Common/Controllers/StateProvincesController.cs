using FBClone.Model;
using FBClone.Service;
using FBClone.WebAPI.Common;
using FBClone.WebAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.OData;

namespace FBClone.WebAPI.Areas.Common.Controllers
{
    //[CustomAuthorize()]
    public class StateProvincesController : BaseController
    {
        private readonly IStateProvinceService stateProvinceService;

        public StateProvincesController()
        {
        }

        public StateProvincesController(IStateProvinceService stateProvinceService)
        {
            this.stateProvinceService = stateProvinceService;
        }

        // GET: api/StaffMembers
        [EnableQuery()]
        [ResponseType(typeof(StateProvince))]
        public IHttpActionResult Get()
        {
            try
            {
                var stateProvinces = stateProvinceService.Query(x => x.Country.Name.Equals("United States", StringComparison.OrdinalIgnoreCase));
                return Ok(stateProvinces);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
