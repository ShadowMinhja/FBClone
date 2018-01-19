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
    // HubConnections
    internal partial class HubConnectionMap : EntityTypeConfiguration<HubConnection>
    {
        public HubConnectionMap()
            : this("dbo")
        {
        }
 
        public HubConnectionMap(string schema)
        {
            ToTable(schema + ".HubConnections");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.ConnectionId).HasColumnName("ConnectionId").IsRequired().HasColumnType("uniqueidentifier");
            Property(x => x.UserAgent).HasColumnName("UserAgent").IsOptional().HasColumnType("nvarchar").HasMaxLength(512);
            Property(x => x.Connected).HasColumnName("Connected").IsRequired().HasColumnType("bit");
            Property(x => x.HubType).HasColumnName("HubType").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.HubRole).HasColumnName("HubRole").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.UserId).HasColumnName("UserId").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsOptional().HasColumnType("nvarchar");
            Property(x => x.UpdatedBy).HasColumnName("UpdatedBy").IsOptional().HasColumnType("nvarchar");
            Property(x => x.Version).HasColumnName("Version").IsRequired().IsFixedLength().HasColumnType("timestamp").HasMaxLength(8).IsRowVersion().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedAt).HasColumnName("CreatedAt").IsRequired().HasColumnType("datetimeoffset");
            Property(x => x.UpdatedAt).HasColumnName("UpdatedAt").IsOptional().HasColumnType("datetimeoffset");
            Property(x => x.Deleted).HasColumnName("Deleted").IsRequired().HasColumnType("bit");
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
