using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FBClone.Model
{
    [MetadataType(typeof(LocationMetadata))]
    public partial class Location
    {
        private sealed class LocationMetadata
        {
            [Required(ErrorMessage = "*")]
            [Display(Name = "City")]
            public string Locality { get; set; }

            [Required(ErrorMessage = "*")]
            [Display(Name = "Street")]
            public string Address1 { get; set; }

            [Required(ErrorMessage = "*")]
            [Display(Name = "State")]
            public string Region { get; set; }

            [Required(ErrorMessage = "*")]
            [Display(Name = "Zip Code")]
            public string PostalCode { get; set; }

            public string UserId { get; set; }

            [JsonIgnore]
            public ICollection<Menu> Menus { get; set; } // Many to many mapping
            [JsonIgnore]
            public ICollection<MenuQrCode> MenuQrCodes { get; set; } // MenuQRCodes.FK_dbo.MenuQRCodes_FBClone.Locations_LocationId
            [JsonIgnore]
            public ICollection<Order> Orders { get; set; } // Orders.FK_dbo.Orders_FBClone.Locations_LocationId
            [JsonIgnore]
            public ICollection<QuestionResponseSet> QuestionResponseSets { get; set; } // QuestionResponseSets.FK_FBClone.QuestionResponseSets_FBClone.Locations_LocationId

        }

        public Location(string locality, string address1, string region, string postalCode, string userid)
        {
            this.Locality = locality;
            this.Address1 = address1;
            this.Region = region;
            this.PostalCode = postalCode;
            this.UserId = userid;
        }
    }
}