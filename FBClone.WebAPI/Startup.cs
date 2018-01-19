using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(FBClone.WebAPI.Startup))]

namespace FBClone.WebAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            var config = new HubConfiguration
            {
                // You can enable JSONP by uncommenting line below.
                // JSONP requests are insecure but some older browsers (and some
                // versions of IE) require JSONP to work cross domain
                EnableJSONP = true,
            };
            app.MapSignalR(config);           
        }        
    }
}
