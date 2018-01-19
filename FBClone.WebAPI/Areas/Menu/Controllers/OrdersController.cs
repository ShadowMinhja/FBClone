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

namespace FBClone.WebAPI.Areas.Admin.Controllers
{
    //[CustomAuthorize()]
    public class OrderController : BaseController
    {
        private readonly IOrderService orderService;
        private readonly ILocationService locationService;

        public OrderController() {
        }

        public OrderController(IOrderService orderService, ILocationService locationService)
        {
            this.orderService = orderService;
            this.locationService = locationService;
        }

        // GET: api/Orders
        //[EnableQuery()] //For some reason orders must have ToList, this disables OData
        [ResponseType(typeof(Order))]
        public IHttpActionResult Get()
        {
            try {
                //Get Valid Locations
                var locations = locationService.Query(x => x.UserId == userId).ToList();
                //TODO: Change to get only for user, not all locations
                //var openOrders = orderService.Query(x => x.Status == "Waiting");
                //var orders = (from l in locations
                //                  join o in openOrders on l.Id equals o.LocationId
                //                  select o);
                var orders = orderService.Query(x => x.Status == "Waiting").OrderBy(x => x.OrderNumber).ToList();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ResponseType(typeof(Order))]
        public IHttpActionResult Post(Order order)
        {
            try
            {
                if (order == null)
                    return BadRequest("Order cannot be null");
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var newOrder = orderService.Add(order);
                if (newOrder == null)
                    return Conflict();
                return Created<Order>(Request.RequestUri, new Order());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ResponseType(typeof(Order))]
        public IHttpActionResult Put(Order order)
        {
            try
            {
                if (order == null)
                    return BadRequest("Order cannot be null");
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var updatedOrder = orderService.Update(order);
                if (updatedOrder == null)
                    return NotFound();
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ResponseType(typeof(Order))]
        public void Delete(long id)
        {
            orderService.Delete(id);
        }
    }
}
