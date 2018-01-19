using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using FBClone.WebAPI.Models;
using FBClone.WebAPI.Providers;
using FBClone.WebAPI.Results;
using System.Data.SqlClient;
using FBClone.Model;
using SaasEcom.Core.Infrastructure.Facades;
using SaasEcom.Core.DataServices.Storage;
using System.Configuration;
using SaasEcom.Core.Infrastructure.PaymentProcessor.Stripe;
using FBClone.Service;
using FBClone.WebAPI.Common;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Net;
using Facebook;
using System.Web.Http.Description;

namespace FBClone.WebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : BaseController
    {
        #region Member Vars
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;
        //private AuthRepository authRepository;
        private SubscriptionsFacade _subscriptionsFacade;
        private FBCloneContext dbContext;
        private SubscriptionsFacade SubscriptionsFacade
        {
            get
            {
                return _subscriptionsFacade ?? (_subscriptionsFacade = new SubscriptionsFacade(
                    new SubscriptionDataService<ApplicationDbContext, ApplicationUser>
                        (HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>()),
                    new SubscriptionProvider(ConfigurationManager.AppSettings["StripeApiSecretKey"]),
                    new CardProvider(ConfigurationManager.AppSettings["StripeApiSecretKey"],
                        new CardDataService<ApplicationDbContext, ApplicationUser>(Request.GetOwinContext().Get<ApplicationDbContext>())),
                    new CardDataService<ApplicationDbContext, ApplicationUser>(Request.GetOwinContext().Get<ApplicationDbContext>()),
                    new CustomerProvider(ConfigurationManager.AppSettings["StripeApiSecretKey"]),
                    new SubscriptionPlanDataService<ApplicationDbContext, ApplicationUser>(Request.GetOwinContext().Get<ApplicationDbContext>()),
                    new ChargeProvider(ConfigurationManager.AppSettings["StripeApiSecretKey"])));
            }
        }
        #endregion

        private IAspNetUserService aspNetUserService;
        private IAlgoliaService algoliaService;
        private readonly IProfileService profileService;

        public AccountController()
        {
            this.dbContext = new FBCloneContext();
            //this.authRepository = new AuthRepository();
        }

        public AccountController(IAspNetUserService aspNetUserService, IAlgoliaService algoliaService, IProfileService profileService)
        {
            this.dbContext = new FBCloneContext();
            //this.authRepository = new AuthRepository();
            this.aspNetUserService = aspNetUserService;
            this.algoliaService = algoliaService;
            this.profileService = profileService;
        }
        
        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            this.dbContext = new FBCloneContext();
            //this.authRepository = new AuthRepository();
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? Request.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new UserInfoViewModel
            {
                Email = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }

        // POST api/Account/Logout
        //[OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [Route("ManageInfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
            {
                return null;
            }

            List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

            foreach (IdentityUserLogin linkedAccount in user.Logins)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName,
                });
            }

            return new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                Email = user.UserName,
                Logins = logins,
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
            };
        }

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);
            
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/AddExternalLogin
        [Route("AddExternalLogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return BadRequest("External login failure.");
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                return BadRequest("The external login is already associated with an account.");
            }

            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/RemoveLogin
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            string redirectUri = String.Empty;
            if (error != null)
            {
                //return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
                return Redirect(Url.Content("~/Scripts/closeWindow.html"));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            var redirectUriValidationResult = ValidateClientAndRedirectUri(this.Request, ref redirectUri);
            if (!string.IsNullOrWhiteSpace(redirectUriValidationResult))
            {
                return BadRequest(redirectUriValidationResult);
            }

            ClaimsIdentity identity = User.Identity as ClaimsIdentity;
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(identity);
            if (externalLogin == null)
            {
                return BadRequest("User cannot be found");
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider, externalLogin.ProviderKey));

            //Have user confirm email account before logging in
            bool isConfirmed = false;
            if (user != null)
            {
                isConfirmed = await UserManager.IsEmailConfirmedAsync(user.Id);
            }

            bool hasRegistered = user != null;
            if (hasRegistered)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                //Create Local Bearer Token and Sign In
                ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName, user.Id, user.OrganizationName);
                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
                return RedirectOAuthPopup(redirectUri, externalLogin, hasRegistered, isConfirmed, user.Id);
            }
            else
            {
                IEnumerable<Claim> claims = externalLogin.GetClaims();
                identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType); //Create Local Bearer Token and Sign In
                Authentication.SignIn(identity);
                return RedirectOAuthPopup(redirectUri, externalLogin, hasRegistered, isConfirmed, user == null ? null : user.Id);
            }
        }

        // GET api/Account/ExternalLoginMobile
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLoginMobile", Name = "ExternalLoginMobile")]
        [ResponseType(typeof(ExternalLoginMobile))]
        public async Task<IHttpActionResult> GetExternalLoginMobile(string loginProvider, string providerKey, string externalAccessToken)
        {
            ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(loginProvider, providerKey));

            //Have user confirm email account before logging in
            bool isConfirmed = false;
            if (user != null)
            {
                isConfirmed = await UserManager.IsEmailConfirmedAsync(user.Id);
            }

            bool hasRegistered = user != null;
            if (hasRegistered)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                //Create Local Bearer Token and Sign In
                ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName, user.Id, user.OrganizationName);
                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            }
            return Ok(new ExternalLoginMobile
            {
                UserId = user == null ? null : user.Id,
                HasRegistered = hasRegistered,
                IsConfirmed = isConfirmed,
                LoginProvider = loginProvider,
                ExternalAccessToken = externalAccessToken
            });
        }

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (AuthenticationDescription description in descriptions)
            {
                ExternalLoginViewModel login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state = state
                    }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { Errors = ModelState.Errors() });
            }

            //Check that username has not been taken            
            var aspUser = this.aspNetUserService.GetByNameExactMatch(model.UserName);
            if (aspUser != null)
            {
                Dictionary<String, String> dict = new Dictionary<String, String>();
                dict.Add("Key", "model.UserName");
                dict.Add("Value", "User Name already taken");
                return Json(new { Errors = new List<Dictionary<String, String>>() { dict } });
            }
            //var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };
            var newUser = new ApplicationUser()
            {
                UserName = model.UserName ?? model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                OrganizationName = model.OrganizationName,
                Email = model.Email,
                RegistrationDate = DateTime.UtcNow,
                LastLoginTime = DateTime.UtcNow
            };

            try
            {
                IdentityResult result = await UserManager.CreateAsync(newUser, model.Password);
                await GenerateEmailConfirmation(newUser, result.Succeeded);
                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }
            }
            catch (Exception ex)
            {

            }
            return Ok(newUser);
        }

        // POST api/Account/RegisterExternal
        [AllowAnonymous]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { Errors = ModelState.Errors() });
            }

            //Check that username has not been taken
            var aspUser = this.aspNetUserService.GetByNameExactMatch(model.UserName);
            if (aspUser != null)
            {
                Dictionary<String, String> dict = new Dictionary<String, String>();
                dict.Add("Key", "model.UserName");
                dict.Add("Value", "User Name already taken");
                return Json(new { Errors = new List<Dictionary<String, String>>() { dict } });
            }

            var verifiedAccessToken = await VerifyExternalAccessToken(model.Provider, model.ExternalAccessToken);
            if (verifiedAccessToken == null)
            {
                return BadRequest("Invalid Provider or External Access Token");
            }

            //Check if already registered
            ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(model.Provider, model.UserName));
            bool hasRegistered = user != null;
            if (hasRegistered)
            {
                return BadRequest("External user is already registered");
            }

            //Create User Without Password
            var newUser = new ApplicationUser() {
                UserName = model.UserName ?? model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                OrganizationName = model.OrganizationName,
                Email = model.Email,
                RegistrationDate = DateTime.UtcNow,
                LastLoginTime = DateTime.UtcNow
            };

            var info = await Authentication.GetExternalLoginInfoAsync();
            try
            {
                IdentityResult result = await UserManager.CreateAsync(newUser);
                await GenerateEmailConfirmation(newUser, result.Succeeded, model.Provider, model.ExternalAccessToken);
                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }
                else
                {
                    //Associate User to Login
                    result = await UserManager.AddLoginAsync(newUser.Id, info.Login);
                    if (!result.Succeeded)
                    {
                        return GetErrorResult(result);
                    }
                    else
                    {
                        //Add to Algolia Index
                        await AddToAlgoliaIndex(newUser);
                        //External Users Would Never Automatically be Subscribed
                        //// Register user to Stripe
                        //if (model.SubscriptionPlan != "free")
                        //{
                        //    await SubscriptionsFacade.SubscribeUserAsync(newUser, model.SubscriptionPlan);
                        //    await UserManager.UpdateAsync(newUser);
                        //    //Insert First Time User Questions
                        //    SetUpFirstTimeUser(newUser);
                        //}
                    }

                    //generate access token response
                    var accessTokenResponse = GenerateLocalAccessTokenResponse(model.UserName, newUser.Id, model.OrganizationName);
                    return Ok(accessTokenResponse);
                }
            }
            catch (Exception ex)
            {

            }
            return Ok();
        }

        // POST api/Account/RegisterBusiness
        [CustomAuthorize]
        [Route("RegisterBusiness")]
        public async Task<IHttpActionResult> RegisterBusiness(RegisterBusinessBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { Errors = ModelState.Errors() });
            }

            ApplicationUser applicationUser = UserManager.FindById(this.userId);
            applicationUser.OrganizationName = model.OrganizationName;
            applicationUser.RegistrationDate = DateTime.UtcNow;
            applicationUser.LastLoginTime = DateTime.UtcNow;

            try
            {
                // Register user to Stripe
                if (model.SubscriptionPlan != "free")
                {
                    await SubscriptionsFacade.SubscribeUserAsync(applicationUser, model.SubscriptionPlan);
                }
                await UserManager.UpdateAsync(applicationUser);
                //Insert First Time User Questions
                SetUpFirstTimeUser(applicationUser);
            }
            catch (Exception ex)
            {

            }
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ObtainLocalAccessToken")]
        public async Task<IHttpActionResult> ObtainLocalAccessToken(string provider, string externalAccessToken)
        {
            if (string.IsNullOrWhiteSpace(provider) || string.IsNullOrWhiteSpace(externalAccessToken))
            {
                //Provider or external access token is not sent
                return BadRequest("Invalid information was provided. Please try again.");
            }

            var verifiedAccessToken = await VerifyExternalAccessToken(provider, externalAccessToken);
            if (verifiedAccessToken == null)
            {
                //Invalid Provider or External Access Token
                return BadRequest("Invalid information was provided. Please try again.");
            }

            ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(provider, verifiedAccessToken.user_id));

            bool hasRegistered = user != null;
            if (!hasRegistered)
            {
                //External user is not registered
                return BadRequest("User is not registered");
            }

            //generate access token response
            var accessTokenResponse = GenerateLocalAccessTokenResponse(user.UserName, user.Id, user.OrganizationName);
            return Ok(accessTokenResponse);
        }

        // GET: api/Account/ConfirmEmail
        [AllowAnonymous]
        [HttpGet]
        [Route("ConfirmEmail")]
        public async Task<IHttpActionResult> ConfirmEmail(string userId, string code)
        {
            code = code.Replace(' ', '+');
            if (userId == "undefined" || code == "undefined")
            {
                return BadRequest("Missing user or confirmation code.");
            }
            IdentityResult result;
            try
            {
                result = await UserManager.ConfirmEmailAsync(userId, code);
            }
            catch (InvalidOperationException ioe)
            {
                // ConfirmEmailAsync throws when the userId is not found.
                return BadRequest(ioe.Message);
            }

            if (result.Succeeded)
            {
                var user = UserManager.FindById(userId);
                await FollowFBCloneStream(user);
                if (user.PasswordHash != null) //Normal Login
                {
                    return Ok();
                }
                else //External Login
                {
                    var accessTokenResponse = GenerateLocalAccessTokenResponse(user.UserName, user.Id, user.OrganizationName);
                    return Ok(accessTokenResponse);
                }
            }

            // If we got this far, something failed.
            return BadRequest("Email code is invalid or has expired.");
        }

        // GET: api/Account/ResendConfirmationEmail
        [AllowAnonymous]
        [HttpGet]
        [Route("ResendConfirmationEmail")]
        public async Task<IHttpActionResult> ResendConfirmationEmail(string userId)
        {
            var user = UserManager.FindById(userId);
            await GenerateEmailConfirmation(user, true);
            return Ok(user);
        }

        // POST: api/Account/ForgotPassword
        [AllowAnonymous]
        [Route("ForgotPassword")]
        public async Task<IHttpActionResult> ForgotPassword(IdentityUser findUser)
        {
            string email = findUser.Email;
            if (email != null)
            {
                ApplicationUser user = null;
                if (email.Contains("@"))
                {
                    user = await UserManager.FindByEmailAsync(email);
                }
                else
                {
                    user = await UserManager.FindByNameAsync(email);
                }
                if (user == null)
                {
                    // Error message if the user doesn't exist
                    return BadRequest("Invalid Email Address");
                }
                if (user.PasswordHash != null) //Regular Account
                {
                    var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                    string callbackUrlString = String.Format("{0}://{1}/{2}/{3}?userId={4}&code={5}", Request.RequestUri.Scheme, Request.RequestUri.Authority, "Account", "ResetPassword", user.Id, code);
                    var callbackUrl = new Uri(callbackUrlString);
                    await UserManager.SendEmailAsync(
                        user.Id,
                        "ResetPassword",
                        callbackUrl.AbsoluteUri
                    );
                    return Ok();
                }
                else //Social Account
                {
                    return BadRequest("This account uses a social login (Facebook, Google, etc.)");
                }
            }

            // If we got this far, something failed, redisplay form
            return BadRequest("Forgot Password Failed");
        }

        // POST: api/Account/ResetPassword
        [AllowAnonymous]
        [Route("ResetPassword")]
        public async Task<IHttpActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            if (resetPasswordModel.UserId == null || resetPasswordModel.Code == null)
            {
                return BadRequest("Reset link not valid or has expired.");
            }
            resetPasswordModel.Code = resetPasswordModel.Code.Replace(' ', '+');
            var user = await UserManager.FindByIdAsync(resetPasswordModel.UserId);
            if (user == null)
            {
                // If a user type the email that doesn't match the email on records, display error message
                return BadRequest("Reset link not valid or has expired.");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, resetPasswordModel.Code, resetPasswordModel.Password);
            if (result.Succeeded)
            {
                return Ok();
            }
            //Display error if token doesn't matches the email
            return BadRequest("Reset link not valid or has expired.");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers
        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private IHttpActionResult RedirectOAuthPopup(string redirectUri, ExternalLoginData externalLogin, bool hasRegistered, bool isConfirmed, string userId)
        {
            //Redirect to page for account association, use hashbang not queryString for security purposes
            redirectUri = string.Format("{0}#external_access_token={1}&provider={2}&haslocalaccount={3}&external_user_name={4}&email={5}&isConfirmed={6}",
                                        redirectUri,
                                        externalLogin.ExternalAccessToken,
                                        externalLogin.LoginProvider,
                                        hasRegistered.ToString(),
                                        externalLogin.UserName,
                                        externalLogin.Email,
                                        isConfirmed);

            if (!isConfirmed && userId != null)
            {
                redirectUri = String.Concat(redirectUri, "&userId=", userId);
            }
            return Redirect(Url.Content("~/Scripts/loginRedirect.html?redirectUrl=" + redirectUri.Replace("#","<HASHBANG>"))); //Workaround for Safari dropping hash fragment issue
            //return Redirect(redirectUri);
        }

        private async Task GenerateEmailConfirmation(ApplicationUser user, bool result, string loginProvider = null, string externalAccessToken = null)
        {
            if (result)
            {
                var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                string callbackUrlString = String.Format("{0}://{1}/{2}/{3}?userId={4}&code={5}", Request.RequestUri.Scheme, Request.RequestUri.Authority, "Account", "ConfirmEmail", user.Id, code);
                if (loginProvider != null && externalAccessToken != null) {
                    callbackUrlString = String.Concat(callbackUrlString, "&loginProvider=", loginProvider, "&externalAccessToken=", externalAccessToken);
                }
                var callbackUrl = new Uri(callbackUrlString);
                await UserManager.SendEmailAsync(
                    user.Id,
                    "ConfirmAccount",
                    callbackUrl.AbsoluteUri
                );
            }            
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        private async Task AddToAlgoliaIndex(ApplicationUser user)
        {
            AspNetUser aspNetUser = new AspNetUser
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                OrganizationName = user.OrganizationName,
                Email = user.Email,
                UserName = user.UserName
            };
            await algoliaService.AddUserIndex(aspNetUser);
        }

        private void SetUpFirstTimeUser(ApplicationUser user)
        {
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter{
                    ParameterName = "@userid",
                    Value = user.Id
                }
            };
            this.dbContext.Database.ExecuteSqlCommand("EXEC spCreateUser @userid", parameters);
        }

        private string ValidateClientAndRedirectUri(HttpRequestMessage request, ref string redirectUriOutput)
        {
            Uri redirectUri;
            var redirectUriString = GetQueryString(Request, "redirect_uri");
            if (string.IsNullOrWhiteSpace(redirectUriString))
            {
                return "redirect_uri is required";
            }

            bool validUri = Uri.TryCreate(redirectUriString, UriKind.Absolute, out redirectUri);
            if (!validUri)
            {
                return "redirect_uri is invalid";
            }

            var clientId = GetQueryString(Request, "client_id");
            if (string.IsNullOrWhiteSpace(clientId))
            {
                return "client_Id is required";
            }

            //TODO: Implement this later for refresh tokens
            //var client = authRepository.FindClient(clientId);
            //if (client == null)
            //{
            //    return string.Format("Client_id '{0}' is not registered in the system.", clientId);
            //}
            //if (!string.Equals(client.AllowedOrigin, redirectUri.GetLeftPart(UriPartial.Authority), StringComparison.OrdinalIgnoreCase))
            //{
            //    return string.Format("The given URL is not allowed by Client_id '{0}' configuration.", clientId);
            //}
            redirectUriOutput = redirectUri.AbsoluteUri;
            return string.Empty;
        }

        private string GetQueryString(HttpRequestMessage request, string key)
        {
            var queryStrings = request.GetQueryNameValuePairs();
            if (queryStrings == null) return null;
            var match = queryStrings.FirstOrDefault(keyValue => string.Compare(keyValue.Key, key, true) == 0);
            if (string.IsNullOrEmpty(match.Value)) return null;
            return match.Value;
        }

        private async Task<ParsedExternalAccessToken> VerifyExternalAccessToken(string provider, string accessToken)
        {
            ParsedExternalAccessToken parsedToken = null;
            string facebookAppToken = ConfigurationManager.AppSettings["FACEBOOK_APPTOKEN"];
            var verifyTokenEndPoint = "";

            if (provider == "Facebook")
            {
                //You can get it from here: https://developers.facebook.com/tools/accesstoken/
                //More about debug_tokn here: http://stackoverflow.com/questions/16641083/how-does-one-get-the-app-access-token-for-debug-token-inspection-on-facebook                
                verifyTokenEndPoint = string.Format("https://graph.facebook.com/debug_token?input_token={0}&access_token={1}", accessToken, facebookAppToken);
            }
            else if (provider == "Google")
            {
                verifyTokenEndPoint = string.Format("https://www.googleapis.com/oauth2/v1/tokeninfo?access_token={0}", accessToken);
            }
            else
            {
                return null;
            }

            var client = new HttpClient();
            var uri = new Uri(verifyTokenEndPoint);
            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                dynamic jObj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content);

                parsedToken = new ParsedExternalAccessToken();

                if (provider == "Facebook")
                {
                    parsedToken.user_id = jObj["data"]["user_id"];
                    parsedToken.app_id = jObj["data"]["app_id"];

                    if (!string.Equals(Startup.facebookAuthOptions.AppId, parsedToken.app_id, StringComparison.OrdinalIgnoreCase))
                    {
                        return null;
                    }
                }
                else if (provider == "Google")
                {
                    parsedToken.user_id = jObj["user_id"];
                    parsedToken.app_id = jObj["audience"];

                    if (!string.Equals(Startup.googleAuthOptions.ClientId, parsedToken.app_id, StringComparison.OrdinalIgnoreCase))
                    {
                        return null;
                    }
                }
            }

            return parsedToken;
        }

        private JObject GenerateLocalAccessTokenResponse(string userName, string id, string organizationName)
        {
            var tokenExpiration = TimeSpan.FromDays(1);

            ClaimsIdentity identity = new ClaimsIdentity(OAuthDefaults.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, userName));
            identity.AddClaim(new Claim("role", "user"));
            identity.AddClaim(new Claim("userId", id));

            var props = new AuthenticationProperties()
            {
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.Add(tokenExpiration),
            };

            var ticket = new AuthenticationTicket(identity, props);
            var accessToken = Startup.OAuthOptions.AccessTokenFormat.Protect(ticket);

            JObject tokenResponse = new JObject(
                                        new JProperty("userName", userName),
                                        new JProperty("id", id),
                                        new JProperty("guid", GuidEncoder.Encode(id)),
                                        new JProperty("organizationName", organizationName),
                                        new JProperty("access_token", accessToken),
                                        new JProperty("token_type", "bearer"),
                                        new JProperty("expires_in", tokenExpiration.TotalSeconds.ToString()),
                                        new JProperty(".issued", ticket.Properties.IssuedUtc.ToString()),
                                        new JProperty(".expires", ticket.Properties.ExpiresUtc.ToString())
            );

            return tokenResponse;
        }

        private async Task FollowFBCloneStream(ApplicationUser newUser)
        {
            AspNetUser fsUser = aspNetUserService.GetByUserId(new Guid(GlobalConstants.fbClone_USERID));
            //Follow FBClone By Default
            Actor actor = aspNetUserService.ConvertToActor(new AspNetUser
            {
                Id = fsUser.Id,
                Email = fsUser.Email,
                FirstName = fsUser.FirstName,
                LastName = fsUser.LastName,
                UserName = fsUser.UserName
            });
            await profileService.FollowFeed(new FollowContainer
            {
                Source = GuidEncoder.Encode(fsUser.Id),
                Target = GuidEncoder.Encode(GlobalConstants.fbClone_USERID),
                IsFollowing = false,
                Actor = actor
            }, GuidEncoder.Encode(newUser.Id));
        }
        #endregion
    }
}
