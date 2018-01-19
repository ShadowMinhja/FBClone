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
    // provision_marker_dss
    internal partial class ProvisionMarkerDssMap : EntityTypeConfiguration<ProvisionMarkerDss>
    {
        public ProvisionMarkerDssMap()
            : this("DataSync")
        {
        }
 
        public ProvisionMarkerDssMap(string schema)
        {
            ToTable(schema + ".provision_marker_dss");
            HasKey(x => new { x.OwnerScopeLocalId, x.ObjectId });

            Property(x => x.ObjectId).HasColumnName("object_id").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.OwnerScopeLocalId).HasColumnName("owner_scope_local_id").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.ProvisionScopeLocalId).HasColumnName("provision_scope_local_id").IsOptional().HasColumnType("int");
            Property(x => x.ProvisionTimestamp).HasColumnName("provision_timestamp").IsRequired().HasColumnType("bigint");
            Property(x => x.ProvisionLocalPeerKey).HasColumnName("provision_local_peer_key").IsRequired().HasColumnType("int");
            Property(x => x.ProvisionScopePeerKey).HasColumnName("provision_scope_peer_key").IsOptional().HasColumnType("int");
            Property(x => x.ProvisionScopePeerTimestamp).HasColumnName("provision_scope_peer_timestamp").IsOptional().HasColumnType("bigint");
            Property(x => x.ProvisionDatetime).HasColumnName("provision_datetime").IsOptional().HasColumnType("datetime");
            Property(x => x.State).HasColumnName("state").IsOptional().HasColumnType("int");
            Property(x => x.Version).HasColumnName("version").IsRequired().IsFixedLength().HasColumnType("timestamp").HasMaxLength(8).IsRowVersion().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
