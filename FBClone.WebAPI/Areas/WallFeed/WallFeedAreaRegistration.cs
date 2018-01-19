using System.Web.Mvc;

namespace FBClone.WebAPI.Areas.WallFeed
{
    public class WallFeedAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "WallFeed";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "WallFeed_default",
                "WallFeed/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}