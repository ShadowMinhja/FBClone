using Facebook;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace FBClone.WebAPI.Models
{
    public class ExternalLoginData
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ExternalAccessToken { get; set; }

        public IList<Claim> GetClaims()
        {
            IList<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

            if (UserName != null)
            {
                claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
            }

            return claims;
        }

        public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
        {
            string email = String.Empty;

            if (identity == null)
            {
                return null;
            }

            Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

            switch(providerKeyClaim.Issuer)
            {
                case "Facebook":
                    RetrieveFacebookValues(identity, out email);
                    break;
                default:
                    email = identity.FindFirstValue(ClaimTypes.Email);
                    break;
            }
            
            if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                || String.IsNullOrEmpty(providerKeyClaim.Value))
            {
                return null;
            }

            if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
            {
                return null;
            }

            return new ExternalLoginData
            {
                LoginProvider = providerKeyClaim.Issuer,
                ProviderKey = providerKeyClaim.Value,
                UserName = identity.FindFirstValue(ClaimTypes.Name),
                Email = email,
                ExternalAccessToken = identity.FindFirstValue("ExternalAccessToken")
            };
        }

        private static void RetrieveFacebookValues(ClaimsIdentity identity, out string email)
        {
            // update the facebook client with the access token 
            var fb = new FacebookClient();
            fb.AccessToken = identity.FindFirstValue("ExternalAccessToken");

            // Calling Graph API for user info
            dynamic me = fb.Get("me?fields=friends,name,email");

            email = me.email;
        }
    }
}