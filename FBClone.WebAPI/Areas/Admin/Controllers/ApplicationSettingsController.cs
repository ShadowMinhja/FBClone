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

namespace FBClone.WebAPI.Areas.Admin.Controllers
{
    [CustomAuthorize()]
    public class ApplicationSettingsController : BaseController
    {
        private readonly IApplicationSettingService applicationSettingService;

        public ApplicationSettingsController() {
        }

        public ApplicationSettingsController(IApplicationSettingService applicationSettingService)
        {
            this.applicationSettingService = applicationSettingService;
        }

        [ResponseType(typeof(ApplicationSetting))]
        public IHttpActionResult Get()
        {
            try {
                var applicationSetting = applicationSettingService.GetById(userId);
                if (applicationSetting == null)
                    return Ok<ApplicationSetting>(null);
                else
                    return Ok(applicationSetting);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
