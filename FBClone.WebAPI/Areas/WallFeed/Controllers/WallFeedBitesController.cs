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
using System.Data.Entity.Validation;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using FBClone.WebAPI.Common;
using System.Threading.Tasks;

namespace FBClone.WebAPI.Areas.Admin.Controllers
{

    [CustomAuthorize()]
    public class WallFeedBitesController : BaseController
    {
        private readonly IBiteService biteService;

        public WallFeedBitesController() {
        }

        public WallFeedBitesController(IBiteService biteService)
        {
            this.biteService = biteService;
        }

        // GET: api/WallFeedBites
        [EnableQuery()]
        [ResponseType(typeof(Bite))]
        public async Task<IHttpActionResult> Get(string lastId)
        {
            this.biteService.SetUserId(null, this.userId);
            if (!String.IsNullOrEmpty(this.userId))
            {
                var results = await this.biteService.GetAllForWallFeed(lastId);
                return Ok(results);
            }
            else
            {
                return BadRequest("The logged in user could not be determined.");
            }
        }

        [Route("api/WallFeedBites/GetBiteById")]
        [ResponseType(typeof(Bite))]
        public IHttpActionResult GetBiteById(string id)
        {
            return Ok(new Bite());
        }
        
    }
}
