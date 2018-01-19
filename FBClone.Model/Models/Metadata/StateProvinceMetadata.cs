using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FBClone.Model
{
    [MetadataType(typeof(StateProvinceMetadata))]
    public partial class StateProvince
    {
        private sealed class StateProvinceMetadata
        {
            [JsonIgnore]
            public Country Country { get; set; } // FK_FBClone.StateProvinces_FBClone.Countries_CountryId
        }
    }
}