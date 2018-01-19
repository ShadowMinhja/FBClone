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
    // MenuItems
    public partial class MenuItem
    {
        public string Id { get; set; } // Id (Primary key)
        public string MenuSectionId { get; set; } // MenuSectionId
        public string ItemType { get; set; } // ItemType
        public bool Required { get; set; } // Required
        public string ItemText { get; set; } // ItemText
        public string ItemDesc { get; set; } // ItemDesc
        public double Price { get; set; } // Price
        public string Active { get; set; } // Active
        public string DaysOfWeek { get; set; } // DaysOfWeek
        public int Sequence { get; set; } // Sequence
        public string ItemImageUrl { get; set; } // ItemImageURL
        public double? Cl { get; set; } // CL
        public double? ImageCl { get; set; } // ImageCL
        public string Source { get; set; } // Source
        public string SourceType { get; set; } // SourceType
        public double? FoodTotalScore { get; set; } // FoodTotalScore
        public int? FoodTotalReviews { get; set; } // FoodTotalReviews
        public string UserId { get; set; } // UserId
        public string CreatedBy { get; set; } // CreatedBy
        public string UpdatedBy { get; set; } // UpdatedBy
        public byte[] Version { get; set; } // Version
        public DateTimeOffset CreatedAt { get; set; } // CreatedAt
        public DateTimeOffset? UpdatedAt { get; set; } // UpdatedAt
        public bool Deleted { get; set; } // Deleted

        // Foreign keys
        public virtual MenuSection MenuSection { get; set; } // FK_FBClone.MenuItems_FBClone.MenuSections_MenuSectionId
        
        public MenuItem()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = System.DateTimeOffset.UtcNow;
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
