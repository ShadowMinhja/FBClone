using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FBClone.Model
{
    [MetadataType(typeof(MenuSectionMetadata))]
    public partial class MenuSection
    {
        private sealed class MenuSectionMetadata
        {
            [JsonIgnore]
            public Menu Menu { get; set; }
        }
    }
}