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
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;

namespace FBClone.WebAPI.Areas.Admin.Controllers
{
    [CustomAuthorize()]
    public class MenuController : BaseController
    {
        private readonly IMenuService menuService;
        private readonly ILocationService locationService;

        public MenuController() {
        }

        public MenuController(IMenuService menuService, ILocationService locationService)
        {
            this.menuService = menuService;
            this.locationService = locationService;
        }

        // GET: api/Menus
        [EnableQuery()]
        [ResponseType(typeof(FBClone.Model.Menu))]
        public IHttpActionResult Get()
        {
            try {
                var menus = menuService.AllIncluding(x => x.UserId == userId)
                    .OrderBy(x => x.Description);
                return Ok(menus);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ResponseType(typeof(FBClone.Model.Menu))]
        public IHttpActionResult Get(string id)
        {
            try {
                var menu = menuService.GetById(id);
                if (menu == null)
                    return NotFound();
                else
                    return Ok(menu);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ResponseType(typeof(FBClone.Model.Menu))]
        public IHttpActionResult Post([FromBody]FBClone.Model.Menu menu)
        {
            try
            {
                if (menu == null)
                    return BadRequest("Menu cannot be null");
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                menu.Id = Guid.NewGuid().ToString();
                //Save Locations To Update Many to Many
                var locations = menu.Locations;
                //Null out objects to prevent reinsert of child objects
                menu.Locations = null;
                var newMenu = menuService.Add(menu);
                if (newMenu == null)
                    return Conflict();
                else
                {
                    //Update corresponding Location many to many
                    foreach (var location in locations)
                    {
                        locationService.UpdateMenuAssociation(location.Id, newMenu.Id);
                    }
                }
                return Created<FBClone.Model.Menu>(Request.RequestUri + menu.Description.ToString(), menu);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ResponseType(typeof(FBClone.Model.Menu))]
        public IHttpActionResult Put(string id, [FromBody]FBClone.Model.Menu menu)
        {
            try {
                if (menu == null)
                    return BadRequest("Menu cannot be null");
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var updatedMenu = menuService.Update(menu);
                if (updatedMenu == null)
                    return NotFound();
                else
                {
                    //Update corresponding Location many to many
                    var locationsWithMenu = menu.Locations.ToList();
                    foreach (var location in locationsWithMenu)
                    {
                        locationService.UpdateMenuAssociation(location.Id, updatedMenu.Id);
                    }
                    //Remove Menu From Locations that Previously Had it
                    var priorLocationsWithMenu = locationService.GetAll().Where(x => x.Menus.Contains(updatedMenu)).ToList();
                    foreach (var location in priorLocationsWithMenu)
                    {
                        if (!locationsWithMenu.Contains(location))
                        {
                            locationService.DeleteMenuAssociation(location.Id, updatedMenu.Id);
                        }
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/Menu/ImportMenu")]
        [ResponseType(typeof(FBClone.Model.Menu))]
        public async Task<HttpResponseMessage> Post(string menuId)
        {
            try
            {
                // Check if the request contains multipart/form-data.
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                string root = HttpContext.Current.Server.MapPath("~/App_Data");
                var provider = new MultipartFormDataStreamProvider(root);

                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                var file = provider.FileData[0];
                if (file != null && file.Headers.ContentType.MediaType.Contains("excel"))
                {
                    FBClone.Model.Menu targetMenu = menuService.GetById(menuId);
                    var inputStream = File.OpenRead(file.LocalFileName);
                    var reader = new StreamReader(inputStream);
                    var headerRow = reader.ReadLine();
                    bool errorFlag = false;
                    while (!reader.EndOfStream && errorFlag == false)
                    {
                        var line = reader.ReadLine();
                        Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                        var values = CSVParser.Split(line);
                        // clean up the fields (remove " and leading spaces)
                        for (int i = 0; i < values.Length; i++)
                        {
                            values[i] = values[i].TrimStart(' ', '"');
                            values[i] = values[i].TrimEnd('"');
                        }
                        string MenuSectionName = String.Empty;
                        string MenuItemName = String.Empty;
                        string Price = String.Empty;
                        string Sequence = String.Empty;
                        string MenuName = values[0];
                        if(MenuName == targetMenu.Description)
                        {
                            MenuSectionName = values[1];
                            var targetMenuSection = targetMenu.MenuSections.Where(x => x.SectionTitle == MenuSectionName).FirstOrDefault();
                            if (targetMenuSection == null) //Create menu section
                            {
                                targetMenu.MenuSections.Add(new MenuSection
                                {
                                    MenuId = targetMenu.Id,
                                    SectionTitle = MenuSectionName,
                                    Active = "Active",
                                    Sequence = targetMenu.MenuSections.Count + 1,
                                    UserId = userId,
                                    CreatedBy = "User File Import",
                                    UpdatedBy = "User File Import"
                                });
                                targetMenuSection = targetMenu.MenuSections.Where(x => x.SectionTitle == MenuSectionName).FirstOrDefault();
                            }
                            //Add Menu Items
                            MenuItemName = values[2];
                            var targetMenuItem = targetMenuSection.MenuItems.Where(x => x.ItemText.Contains(MenuItemName)).FirstOrDefault();
                            Price = values[3];
                            Sequence = values[4];
                            if (targetMenuItem == null) //Create menu item
                            {
                                targetMenuSection.MenuItems.Add(new MenuItem
                                {
                                    Required = true,
                                    ItemText = "<p>" + MenuItemName + "</p>",
                                    Active = "Active",
                                    Price = Convert.ToDouble(Price),
                                    Sequence = Sequence != String.Empty ? Convert.ToInt32(Sequence) : targetMenuSection.MenuItems.Count + 1
                                });
                            }
                            else //Update Existing Item
                            {
                                targetMenuItem.Price = Convert.ToDouble(Price);
                                if(targetMenuItem.Sequence > -1) //Already existing sequence
                                    targetMenuItem.Sequence = Sequence != String.Empty ? Convert.ToInt32(Sequence) : targetMenuItem.Sequence;
                                else
                                    targetMenuItem.Sequence = Sequence != String.Empty ? Convert.ToInt32(Sequence) : targetMenuSection.MenuItems.Count + 1;
                            }                           
                        }                        
                    }
                    //Update Final Object
                    menuService.Update(targetMenu);

                    inputStream.Close();
                    return Request.CreateResponse<FBClone.Model.Menu>(HttpStatusCode.OK, targetMenu);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new FBClone.Model.Menu());
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<String>(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [ResponseType(typeof(FBClone.Model.Menu))]
        public void Delete(string id)
        {
            menuService.Delete(id);
        }
    }
}
