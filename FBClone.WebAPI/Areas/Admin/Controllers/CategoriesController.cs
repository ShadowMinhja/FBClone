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
    public class CategoriesController : BaseController
    {
        private readonly ICategoryService categoryService;

        public CategoriesController() {
        }

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        // GET: api/StaffMembers
        [EnableQuery()]
        [ResponseType(typeof(Category))]
        public IHttpActionResult Get()
        {
            try {
                var categories = categoryService.Query(x => x.UserId == userId);
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ResponseType(typeof(Category))]
        public IHttpActionResult Get(string id)
        {
            try {
                var category = categoryService.GetById(id);
                if (category == null)
                    return NotFound();
                else
                    return Ok(category);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ResponseType(typeof(Category))]
        public IHttpActionResult Post([FromBody]Category category)
        {
            try
            {
                if (category == null)
                    return BadRequest("Category cannot be null");
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                category.Id = Guid.NewGuid().ToString();
                var newCategory = categoryService.Add(category);
                if (newCategory == null)
                    return Conflict();
                return Created<Category>(Request.RequestUri + category.Id.ToString(), category);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ResponseType(typeof(Category))]
        public IHttpActionResult Put(string id, [FromBody]Category category)
        {
            try {
                if (category == null)
                    return BadRequest("Category cannot be null");
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var updatedCategory = categoryService.Update(category);
                if (updatedCategory == null)
                    return NotFound();
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ResponseType(typeof(Category))]
        public void Delete(string id)
        {
            categoryService.Delete(id);
        }
    }
}
