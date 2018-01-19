// ReSharper disable RedundantUsingDirective
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable RedundantNameQualifier
// TargetFrameworkVersion = 4.51
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data.Entity.ModelConfiguration;
using System.Threading;
using System.Threading.Tasks;
using DatabaseGeneratedOption = System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption;

namespace FBClone.Model
{
    // Locations
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.14.3.0")]
    public partial class Location
    {
        public string Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name
        public string Description { get; set; } // Description
        public string Address { get; set; } // Address
        public string Address1 { get; set; } // Address1
        public string Address2 { get; set; } // Address2
        public string Locality { get; set; } // Locality
        public string Region { get; set; } // Region
        public string PostalCode { get; set; } // PostalCode
        public string Country { get; set; } // Country
        public double? GeoLat { get; set; } // GeoLat
        public double? GeoLng { get; set; } // GeoLng
        public string PlaceId { get; set; } // PlaceId
        public string Source { get; set; } // Source
        public string LocationImageUrl { get; set; } // LocationImageUrl
        public string WebsiteUrl { get; set; } // WebsiteUrl
        public double? AmbienceTotalScore { get; set; } // AmbienceTotalScore
        public int? AmbienceTotalReviews { get; set; } // AmbienceTotalReviews
        public double? ServiceTotalScore { get; set; } // ServiceTotalScore
        public int? ServiceTotalReviews { get; set; } // ServiceTotalReviews
        public string UserId { get; set; } // UserId
        public string CreatedBy { get; set; } // CreatedBy
        public string UpdatedBy { get; set; } // UpdatedBy
        public byte[] Version { get; set; } // Version
        public DateTimeOffset CreatedAt { get; set; } // CreatedAt
        public DateTimeOffset? UpdatedAt { get; set; } // UpdatedAt
        public bool Deleted { get; set; } // Deleted

        // Reverse navigation
        public virtual ICollection<Menu> Menus { get; set; } // Many to many mapping
        public virtual ICollection<MenuQrCode> MenuQrCodes { get; set; } // MenuQRCodes.FK_dbo.MenuQRCodes_FBClone.Locations_LocationId
        public virtual ICollection<Order> Orders { get; set; } // Orders.FK_dbo.Orders_FBClone.Locations_LocationId
        public virtual ICollection<QuestionResponseSet> QuestionResponseSets { get; set; } // QuestionResponseSets.FK_FBClone.QuestionResponseSets_FBClone.Locations_LocationId
        
        public Location()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = System.DateTimeOffset.UtcNow;
            MenuQrCodes = new List<MenuQrCode>();
            Orders = new List<Order>();
            QuestionResponseSets = new List<QuestionResponseSet>();
            Menus = new List<Menu>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
