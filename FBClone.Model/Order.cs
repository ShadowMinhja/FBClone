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
    // Orders
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.14.3.0")]
    public partial class Order
    {
        public long Id { get; set; } // Id (Primary key)
        public string LocationId { get; set; } // LocationId
        public string TableId { get; set; } // TableId
        public string OrderNumber { get; set; } // OrderNumber
        public string Status { get; set; } // Status
        public Guid? HubConnectionId { get; set; } // HubConnectionId
        public string UserId { get; set; } // UserId
        public string CreatedBy { get; set; } // CreatedBy
        public string UpdatedBy { get; set; } // UpdatedBy
        public byte[] Version { get; set; } // Version
        public DateTimeOffset CreatedAt { get; set; } // CreatedAt
        public DateTimeOffset? UpdatedAt { get; set; } // UpdatedAt
        public bool Deleted { get; set; } // Deleted

        // Reverse navigation
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } // OrderDetails.FK_dbo.OrderDetails_Order_OrderId

        // Foreign keys
        public virtual Location Location { get; set; } // FK_dbo.Orders_FBClone.Locations_LocationId
        
        public Order()
        {
            CreatedAt = System.DateTimeOffset.UtcNow;
            OrderDetails = new List<OrderDetail>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
