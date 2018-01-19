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
    // ApplicationSettings
    public partial class ApplicationSetting
    {
        public string Id { get; set; } // Id (Primary key)
        public bool StaffSetup { get; set; } // StaffSetup
        public bool SurveySetup { get; set; } // SurveySetup
        public bool PromotionSetup { get; set; } // PromotionSetup
        public bool BrandingSetup { get; set; } // BrandingSetup
        public bool SurveyIsLooping { get; set; } // SurveyIsLooping
        public string BrandingLogoUrl { get; set; } // BrandingLogoURL
        public string PromoLogoUrl { get; set; } // PromoLogoURL
        public string UserId { get; set; } // UserId
        public string CreatedBy { get; set; } // CreatedBy
        public string UpdatedBy { get; set; } // UpdatedBy
        public byte[] Version { get; set; } // Version
        public DateTimeOffset CreatedAt { get; set; } // CreatedAt
        public DateTimeOffset? UpdatedAt { get; set; } // UpdatedAt
        public bool Deleted { get; set; } // Deleted
        
        public ApplicationSetting()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = System.DateTimeOffset.UtcNow;
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
