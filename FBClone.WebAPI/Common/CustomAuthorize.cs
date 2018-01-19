using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using FBClone.WebAPI.Controllers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Security.Claims;

namespace FBClone.WebAPI.Common
{
    public class CustomAuthorize : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //string validCustomAuthorizeControllers = ConfigurationManager.AppSettings["ValidCustomAuthorizeControllers"];
            if (actionContext.RequestContext.Principal.Identity.IsAuthenticated)
            {
                string userId = actionContext.RequestContext.Principal.Identity.GetUserId();
                if(userId == null) //May be external login
                {
                    var identity = actionContext.RequestContext.Principal.Identity as ClaimsIdentity;
                    userId = identity.FindFirstValue("userId");
                }
                ((BaseController)actionContext.ControllerContext.Controller).userId = userId;
            }
        }
    }
}