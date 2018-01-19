using Microsoft.WindowsAzure.Mobile.Service;
using System.Collections.Generic;

namespace FBClone.DataObjects
{
    public class Location : EntityData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Locality { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public double? GeoLat { get; set; }
        public double? GeoLng { get; set; }
        public string PlaceId { get; set; }
        public string Source { get; set; }
        public string LocationImageUrl { get; set; }
        public string WebsiteUrl { get; set; }
        public double? AmbienceTotalScore { get; set; }
        public int? AmbienceTotalReviews { get; set; }
        public double? ServiceTotalScore { get; set; }
        public int? ServiceTotalReviews { get; set; }
        public string UserId { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public virtual ICollection<Menu> Menus { get; set; } // Many to many mapping
    }
}