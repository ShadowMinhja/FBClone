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
    // MenuQRCodes
    public partial class MenuQrCode
    {
        public long Id { get; set; } // Id (Primary key)
        public string UserId { get; set; } // UserId
        public string LocationId { get; set; } // LocationId
        public string TableId { get; set; } // TableId
        public string QrImageUrl { get; set; } // QRImageUrl
        public string CreatedBy { get; set; } // CreatedBy
        public string UpdatedBy { get; set; } // UpdatedBy
        public byte[] Version { get; set; } // Version
        public DateTimeOffset CreatedAt { get; set; } // CreatedAt
        public DateTimeOffset? UpdatedAt { get; set; } // UpdatedAt
        public bool Deleted { get; set; } // Deleted

        // Foreign keys
        public virtual Location Location { get; set; } // FK_dbo.MenuQRCodes_FBClone.Locations_LocationId
        
        public MenuQrCode()
        {
            CreatedAt = System.DateTimeOffset.UtcNow;
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
