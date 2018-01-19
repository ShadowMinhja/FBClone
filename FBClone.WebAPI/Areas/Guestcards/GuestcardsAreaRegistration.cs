using System.Web.Mvc;

namespace FBClone.WebAPI.Areas.Guestcards
{
    public class GuestcardsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Guestcards";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Guestcards_default",
                "Guestcards/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}