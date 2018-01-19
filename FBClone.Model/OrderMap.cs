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
    internal partial class OrderMap : EntityTypeConfiguration<Order>
    {
        public OrderMap()
            : this("dbo")
        {
        }
 
        public OrderMap(string schema)
        {
            ToTable(schema + ".Orders");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasColumnType("bigint").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.LocationId).HasColumnName("LocationId").IsRequired().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.TableId).HasColumnName("TableId").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.OrderNumber).HasColumnName("OrderNumber").IsOptional().HasColumnType("nvarchar").HasMaxLength(10);
            Property(x => x.Status).HasColumnName("Status").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.HubConnectionId).HasColumnName("HubConnectionId").IsOptional().HasColumnType("uniqueidentifier");
            Property(x => x.UserId).HasColumnName("UserId").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsOptional().HasColumnType("nvarchar");
            Property(x => x.UpdatedBy).HasColumnName("UpdatedBy").IsOptional().HasColumnType("nvarchar");
            Property(x => x.Version).HasColumnName("Version").IsRequired().IsFixedLength().HasColumnType("timestamp").HasMaxLength(8).IsRowVersion().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedAt).HasColumnName("CreatedAt").IsRequired().HasColumnType("datetimeoffset");
            Property(x => x.UpdatedAt).HasColumnName("UpdatedAt").IsOptional().HasColumnType("datetimeoffset");
            Property(x => x.Deleted).HasColumnName("Deleted").IsRequired().HasColumnType("bit");

            // Foreign keys
            HasRequired(a => a.Location).WithMany(b => b.Orders).HasForeignKey(c => c.LocationId); // FK_dbo.Orders_FBClone.Locations_LocationId
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
