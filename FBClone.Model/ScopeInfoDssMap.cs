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
    // scope_info_dss
    internal partial class ScopeInfoDssMap : EntityTypeConfiguration<ScopeInfoDss>
    {
        public ScopeInfoDssMap()
            : this("DataSync")
        {
        }
 
        public ScopeInfoDssMap(string schema)
        {
            ToTable(schema + ".scope_info_dss");
            HasKey(x => x.SyncScopeName);

            Property(x => x.ScopeLocalId).HasColumnName("scope_local_id").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.ScopeId).HasColumnName("scope_id").IsRequired().HasColumnType("uniqueidentifier");
            Property(x => x.SyncScopeName).HasColumnName("sync_scope_name").IsRequired().HasColumnType("nvarchar").HasMaxLength(100).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.ScopeSyncKnowledge).HasColumnName("scope_sync_knowledge").IsOptional().HasColumnType("varbinary");
            Property(x => x.ScopeTombstoneCleanupKnowledge).HasColumnName("scope_tombstone_cleanup_knowledge").IsOptional().HasColumnType("varbinary");
            Property(x => x.ScopeTimestamp).HasColumnName("scope_timestamp").IsOptional().HasColumnType("timestamp").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.ScopeConfigId).HasColumnName("scope_config_id").IsOptional().HasColumnType("uniqueidentifier");
            Property(x => x.ScopeRestoreCount).HasColumnName("scope_restore_count").IsRequired().HasColumnType("int");
            Property(x => x.ScopeUserComment).HasColumnName("scope_user_comment").IsOptional().HasColumnType("nvarchar");
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
