using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FBClone.Model
{
    [MetadataType(typeof(MenuItemMetadata))]
    public partial class MenuItem
    {
        private sealed class MenuItemMetadata
        {
            [JsonIgnore]
            public MenuSection MenuSection { get; set; }
        }
    }
}