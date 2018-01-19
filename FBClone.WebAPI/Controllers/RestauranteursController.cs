using FBClone.Service;
using FBClone.WebAPI.Common;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FBClone.WebAPI.Controllers
{
    public class RestauranteursController : Controller
    {
        // GET: Restauranteurs
        public ActionResult Index()
        {
            ViewBag.Environment = GlobalConstants.ENVIRONMENT;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Environment = GlobalConstants.ENVIRONMENT;
            return View();
        }

        public ActionResult Services()
        {
            ViewBag.Environment = GlobalConstants.ENVIRONMENT;
            return View();
        }

        public ActionResult FAQ()
        {
            ViewBag.Environment = GlobalConstants.ENVIRONMENT;
            return View();
        }

        public ActionResult Testimonials()
        {
            ViewBag.Environment = GlobalConstants.ENVIRONMENT;
            return View();
        }

        public ActionResult Pricing()
        {
            ViewBag.Environment = GlobalConstants.ENVIRONMENT;
            return View();
        }

        public ActionResult Support()
        {
            ViewBag.Environment = GlobalConstants.ENVIRONMENT;
            return View();
        }

        public ActionResult Blog()
        {
            ViewBag.Environment = GlobalConstants.ENVIRONMENT; 
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Environment = GlobalConstants.ENVIRONMENT; 
            return View();
        }

        [HttpPost]
        public ActionResult Contact(FormCollection collection)
        {
            try
            {
                MailMessage emailer = new MailMessage("ContactForm");
                emailer.ReceiveContactFormMessage(
                    collection["name"],
                    collection["email"],
                    collection["phone"],
                    collection["subject"],
                    collection["message"]
                );
                TempData["MessageSent"] = true;
                return RedirectToAction("Contact");
            }
            catch
            {
                TempData["MessageSent"] = false;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Redirect("/");
        }

        private IAuthenticationManager Authentication
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
    }
}