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
    public class MenuSectionsController : BaseController
    {
        private readonly IMenuService menuService;

        public MenuSectionsController() {
        }

        public MenuSectionsController(IMenuService menuService)
        {
            this.menuService = menuService;
        }

        [ResponseType(typeof(MenuSection))]
        public IHttpActionResult Post([FromBody]MenuSection menuSection)
        {
            try
            {
                if (menuSection == null)
                    return BadRequest("MenuSection cannot be null");
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                menuSection.Id = Guid.NewGuid().ToString();
                var newMenuSection = menuService.AddMenuSection(menuSection);
                if (newMenuSection == null)
                    return Conflict();
                return Created<MenuSection>(Request.RequestUri + menuSection.SectionTitle.ToString(), menuSection);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ResponseType(typeof(MenuSection))]
        public IHttpActionResult Put(string id, [FromBody]MenuSection menuSection)
        {
            try {
                if (menuSection == null)
                    return BadRequest("MenuSection cannot be null");
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var updatedMenuSection = menuService.UpdateMenuSection(menuSection);
                if (updatedMenuSection == null)
                    return NotFound();
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ResponseType(typeof(MenuSection))]
        public void Delete(string id)
        {
            menuService.DeleteMenuSection(id);
        }
    }
}
