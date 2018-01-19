using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using FBClone.Service;
using FBClone.Model;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Infrastructure;

namespace FBClone.WebAPI.Hubs
{
    [HubName("ordersHub")]
    //[Authorize] //Right now anyone can order
    public class OrdersHub : Hub
    {
        private readonly IOrderService orderService;
        private readonly IHubConnectionService hubConnectionService;
        private readonly IAspNetUserService aspNetUserService;
        private readonly ILocationService locationService;

        public OrdersHub(IOrderService orderService, IHubConnectionService hubConnectionService, IAspNetUserService aspNetUserService, ILocationService locationService)
        {
            this.orderService = orderService;
            this.hubConnectionService = hubConnectionService;
            this.aspNetUserService = aspNetUserService;
            this.locationService = locationService;
        }

        public void SendOrder(Order newOrder)
        {
            try
            {
                var order = orderService.Add(newOrder);
                if (order != null)
                {
                    Location merchantLocation = locationService.Query(x => x.Id == order.LocationId).FirstOrDefault();
                    var merchantUserId = merchantLocation.UserId;
                    //var hubConnection = hubConnectionService.GetByUserName(merchantUserId);
                    //if(hubConnection != null)
                    //    Clients.Client(hubConnection.ConnectionId.ToString()).receiveOrder(newOrder);

                    //For Demo Purposes, Just Send All Orders to One Client.
                    var hubConnection = hubConnectionService.Query(x => x.UserId != null).OrderByDescending(x => x.CreatedAt).FirstOrDefault();
                    Clients.Client(hubConnection.ConnectionId.ToString()).receiveOrder(newOrder);

                    //Return to caller
                    Clients.Caller.acceptOrder(order);
                }
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
        }

        public void CompleteOrder(Order order)
        {
            try
            {
                //Clients.Others.notifyOrderComplete(order);
                //Notify Customer that Order Has Completed
                Clients.Client(order.HubConnectionId.ToString()).notifyOrderComplete(order);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
        }

        public override Task OnConnected()
        {
            string name = String.Empty;
            if (Context.User != null)
                name = Context.User.Identity.Name;
            var userAgent = Context.Request.Headers["User-Agent"];
            string userAgentString = String.Empty;
            if (userAgent != null)
                userAgentString = userAgent.ToString();
            var refererUrl = Context.Request.Headers["Referer"];
            string refererUrlString = String.Empty;
            if (refererUrl != null)
                refererUrlString = refererUrl.ToString();
            if (refererUrlString.Contains("/app")) //This is the merchant (receiver)
            {
                AspNetUser aspNetUser = null;
                if(name != String.Empty)
                    aspNetUser = aspNetUserService.GetByName(name);
                HubConnection merchantHubConnection = new HubConnection
                {
                    Id = Guid.NewGuid(),
                    ConnectionId = new Guid(Context.ConnectionId),
                    UserAgent = userAgentString,
                    Connected = true,
                    HubType = "Orders",
                    HubRole = "Receiver",
                    UserId = aspNetUser != null ? aspNetUser.Id : null
                    //actionContext.RequestContext.Principal.Identity.GetUserId();
                };
                hubConnectionService.Add(merchantHubConnection);
            }
            else //This is the customer (sender)
            {
                HubConnection customerHubConnection = new HubConnection
                {
                    Id = Guid.NewGuid(),
                    ConnectionId = new Guid(Context.ConnectionId),
                    UserAgent = userAgentString,
                    Connected = true,
                    HubType = "Orders",
                    HubRole = "Sender",
                    UserId = null
                };
                hubConnectionService.Add(customerHubConnection);
            }
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            hubConnectionService.Delete(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }
    }
}