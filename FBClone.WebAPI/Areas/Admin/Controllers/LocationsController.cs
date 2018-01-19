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
using FBClone.WebAPI.Common;
using System.Globalization;
using System.Configuration;

namespace FBClone.WebAPI.Areas.Admin.Controllers
{
    [CustomAuthorize()]
    public class LocationsController : BaseController
    {
        private readonly ILocationService locationService;
        private readonly ISurveyService surveyService;

        public LocationsController() { }

        public LocationsController(ILocationService locationService, ISurveyService surveyService)
        {
            this.locationService = locationService;
            this.surveyService = surveyService;
        }

        // GET: api/Locations
        [EnableQuery()]
        [ResponseType(typeof(Location))]
        public IHttpActionResult Get()
        {
            try {
                var locations = locationService.Query(x => x.UserId == userId);
                return Ok(locations);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ResponseType(typeof(Location))]
        public IHttpActionResult Get(string id)
        {
            try
            {
                var location = locationService.GetById(id);
                if (location == null)
                    return NotFound();
                else
                    return Ok(location);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/Locations/CheckClaimed")]
        [EnableQuery()]
        [AcceptVerbs("GET")]
        [ResponseType(typeof(object))]
        public IHttpActionResult CheckClaimed(string placeId)
        {
            try
            {
                var locations = locationService.Query(x => x.PlaceId == placeId).FirstOrDefault();
                if (locations != null && locations.UserId != ConfigurationManager.AppSettings["fbCloneUserId"])
                    return Ok(new { Claimed = "true" });
                else
                    return Ok(new { Claimed = "false" });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/Locations/SearchLocation")]
        [EnableQuery()]
        [AcceptVerbs("GET")]
        [ResponseType(typeof(LocationViewModel))]
        public IHttpActionResult SearchLocation(string placeId)
        {
            LocationViewModel locationViewModel = new LocationViewModel();
            string fbCloneDefaultUserId = GlobalConstants.fbClone_USERID;
            try
            {
                var locations = locationService.AllIncluding(x => x.PlaceId == placeId);
                List<Survey> surveys = null;
                if (locations.Any())
                {
                    var location = locations.FirstOrDefault();
                    var menus = location.Menus;
                    //TODO: Add support for specific survey to location mapping
                    surveys = surveyService.AllIncluding(x => x.UserId == location.UserId && x.Active == true).ToList();
                    if(!surveys.Any())
                    {
                        surveys = surveyService.AllIncluding(x => x.UserId == fbCloneDefaultUserId && x.Active == true).ToList();
                    }
                    locationViewModel.Location = location;
                    locationViewModel.Menus = menus.Where(x => x.MenuSections.Any()).ToList();
                }       
                else
                {
                    surveys = surveyService.AllIncluding(x => x.UserId == fbCloneDefaultUserId && x.Active == true).ToList();
                }
                locationViewModel.Survey = surveys.FirstOrDefault();
                return Ok(locationViewModel);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //[Route("api/Locations/GetMenuForLocation")]
        //[EnableQuery()]
        //[AcceptVerbs("GET")]
        //[ResponseType(typeof(FBClone.Model.Menu))]
        //public IHttpActionResult GetMenuForLocation(string locationId)
        //{
        //    try
        //    {
        //        var location = locationService.GetById(locationId);
        //        var menus = location.Menus;
        //        return Ok(menus);
        //    }
        //    catch (Exception ex)
        //    {
        //        return InternalServerError(ex);
        //    }
        //}        

        [ResponseType(typeof(Location))]
        public IHttpActionResult Post([FromBody]Location location)
        {
            try {
                if (location == null)
                    return BadRequest("Location cannot be null");
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                if(location.Address == null)
                {
                    location.Address = String.Format("{0}, {1}, {2} {3}", location.Address1, location.Locality, location.Region, location.PostalCode);
                }
                var newLocation = locationService.AddOrUpdate(location, userId);
                if (newLocation == null)
                    return Conflict();
                return Created<Location>(Request.RequestUri + newLocation.Id.ToString(), newLocation);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ResponseType(typeof(Location))]
        public IHttpActionResult Put(string id, [FromBody]Location location)
        {
            try
            {
                if (location == null)
                    return BadRequest("Location cannot be null");
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                if (location.Address == null)
                {
                    location.Address = String.Format("{0}, {1}, {2} {3}", location.Address1, location.Locality, location.Region, location.PostalCode);
                }
                var updatedLocation = locationService.Update(location);
                if (updatedLocation == null)
                    return NotFound();
                return Ok();
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ResponseType(typeof(Location))]
        public void Delete(string id)
        {
            try {
                var location = this.locationService.GetById(id);
                //We No longer Delete Menus. When they are deleted from user, they simply return to fbClone ownership.
                //foreach(var menu in location.Menus.ToList())
                //{
                //    location.Menus.Remove(menu);
                //}
                //locationService.Update(location);
                locationService.Delete(id);
            }
            catch(Exception ex)
            {

            }
        }
    }
}
