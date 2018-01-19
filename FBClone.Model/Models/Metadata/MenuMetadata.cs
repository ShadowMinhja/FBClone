using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FBClone.Model
{
    [MetadataType(typeof(MenuMetadata))]
    public partial class Menu
    {
        private sealed class MenuMetadata
        {
            //[JsonIgnore]
            //public ICollection<Location> Locations { get; set; } // Many to many mapping
        }
    }
}