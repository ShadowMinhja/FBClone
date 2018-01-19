using Microsoft.Owin.Security.Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace FBClone.WebAPI.Results.Providers
{
    public class FacebookAuthProvider : FacebookAuthenticationProvider
    {
        //http://stackoverflow.com/questions/20378043/getting-the-email-from-external-providers-google-and-facebook-during-account-ass
        public override Task Authenticated(FacebookAuthenticatedContext context)
        {
            context.Identity.AddClaim(new Claim("ExternalAccessToken", context.AccessToken));
            return Task.FromResult<object>(null);
        }
    }
}