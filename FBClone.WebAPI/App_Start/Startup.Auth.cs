using System;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using FBClone.WebAPI.Providers;
using FBClone.WebAPI.Models;
using System.Configuration;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using FBClone.WebAPI.Results.Providers;
using FBClone.Service;

namespace FBClone.WebAPI
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }
        public static GoogleOAuth2AuthenticationOptions googleAuthOptions { get; private set; }
        public static FacebookAuthenticationOptions facebookAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }
        public static string Environment { get; private set; }
        public static bool AllowInsecureHttp { get; private set; }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Configure the application for OAuth based flow
            PublicClientId = "fbClone";
            if(GlobalConstants.ENVIRONMENT == "PROD")
            {
                AllowInsecureHttp = true; //TODO: Change this later
            }
            else
            {
                AllowInsecureHttp = true;
            }

            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                // Set Project to Run With SSL
                AllowInsecureHttp = AllowInsecureHttp
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //    consumerKey: "",
            //    consumerSecret: "");

            //Configure Facebook External Login
            facebookAuthOptions = new FacebookAuthenticationOptions()
            {
                AppId = ConfigurationManager.AppSettings["FACEBOOK_APP_ID"],
                AppSecret = ConfigurationManager.AppSettings["FACEBOOK_SECRET"],
                Provider = new FacebookAuthProvider()
            };
            facebookAuthOptions.Scope.Add("email");
            facebookAuthOptions.Scope.Add("public_profile");
            facebookAuthOptions.Scope.Add("user_friends");
            app.UseFacebookAuthentication(facebookAuthOptions);
            
            //Configure Google External Login
            googleAuthOptions = new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = ConfigurationManager.AppSettings["GOOGLE_CLIENT_ID"],
                ClientSecret = ConfigurationManager.AppSettings["GOOGLE_SECRET"],
                Provider = new GoogleAuthProvider()
            };

            app.UseGoogleAuthentication(googleAuthOptions);
        }
    }
}
