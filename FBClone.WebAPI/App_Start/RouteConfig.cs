using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FBClone.WebAPI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //API Help Pages
            routes.MapRoute(
                name: "API",
                url: "api/{controller}/{id}",
                defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }
            );

            //Bite Area
            routes.MapRoute(
                name: "Grab",
                url: "Grab/{action}/{biteId}",
                defaults: new { controller = "Grab", action = "Bite", biteId = UrlParameter.Optional }
            );

            //Billing Area
            routes.MapRoute(
                name: "Billing",
                url: "Billing/{action}/{id}",
                defaults: new { controller = "Billing", action = "Index", id = UrlParameter.Optional }
            );

            //Restauranteurs Site
            routes.MapRoute(
                name: "Restauranteurs",
                url: "Restauranteurs/{action}/{id}",
                defaults: new { controller = "Restauranteurs", action = "Index", id = UrlParameter.Optional }
            );

            //Stripe WebHooks
            routes.MapRoute(
                name: "StripeWebHooks",
                url: "StripeWebHooks/{action}/{id}",
                defaults: new { controller = "StripeWebHooks", action = "Index", id = UrlParameter.Optional }
            );

            //Angular Catch All Route for SPA
            routes.MapPageRoute("FBClone", "{*anything}", "~/Scripts/app/index.html");

        }
    }
}
