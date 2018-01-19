using FBClone.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FBClone.WebAPI.Areas.Admin
{
    public class LocationViewModel
    {
        public Location Location {get; set;}
        public Survey Survey { get; set; }
        public ICollection<FBClone.Model.Menu> Menus { get; set; }
    }
}