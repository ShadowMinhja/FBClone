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
using Stream;

namespace FBClone.WebAPI.Areas.Admin.Controllers
{

    [CustomAuthorize()]
    public class ProfilesController : BaseController
    {
        private readonly IProfileService profileService;

        public ProfilesController() {
        }

        public ProfilesController(IProfileService profileService)
        {
            this.profileService = profileService;
        }

        // GET: api/Profiles
        [EnableQuery()]
        [ResponseType(typeof(FollowContainer))]
        public async Task<IHttpActionResult> Get(string targetUserName)
        {
            if (String.IsNullOrEmpty(this.userId)) //Not logged in
            {
                return Unauthorized();
            }
            else
            {
                if (this.profileService.IsValidUserName(targetUserName))
                {
                    this.profileService.SetUserId(null, this.userId);
                    FollowContainer followContainer = await this.profileService.CheckIfFollowing(targetUserName);
                    return Ok(followContainer);
                }
                else
                {
                    return NotFound();
                }
            }
        }

        [Route("api/Profiles/FollowProfile")]
        [ResponseType(typeof(FollowContainer))]
        public async Task<IHttpActionResult> FollowProfile([FromBody]FollowContainer followContainer)
        {
            this.profileService.SetUserId(null, this.userId);
            var result = await this.profileService.FollowFeed(followContainer);
            return Ok(result);
        }

        [Route("api/Profiles/UnfollowProfile")]
        [ResponseType(typeof(FollowContainer))]
        public async Task<IHttpActionResult> UnfollowProfile([FromBody]FollowContainer followContainer)
        {
            this.profileService.SetUserId(null, this.userId);
            var result = await this.profileService.UnfollowFeed(followContainer);
            return Ok(result);
        }
    }
}
