using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FBClone.API
{
    public class PreflightRequestHandler : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += (sender, args) =>
            {
                var app = (HttpApplication)sender;

                if (app.Request.HttpMethod == "OPTIONS")
                {
                    app.Response.StatusCode = 200;
                    app.Response.AddHeader("Access-Control-Allow-Headers", "content-type, x-zumo-application, x-zumo-installation-id, x-zumo-version");
                    app.Response.AddHeader("Access-Control-Allow-Origin", "*");
                    app.Response.AddHeader("Access-Control-Allow-Credentials", "true");
                    app.Response.AddHeader("Access-Control-Allow-Methods", "POST,GET,PUT,DELETE,OPTIONS");
                    app.Response.AddHeader("Content-Type", "*");
                    //app.Response.End();
                    //use ApplicationInstance.CompleteRequest to prevent thread disposing error
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            };
        }

        public void Dispose()
        {
        }
    }
}