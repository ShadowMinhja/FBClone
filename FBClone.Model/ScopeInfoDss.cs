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
    public partial class ScopeInfoDss
    {
        public int ScopeLocalId { get; set; } // scope_local_id
        public Guid ScopeId { get; set; } // scope_id
        public string SyncScopeName { get; set; } // sync_scope_name (Primary key)
        public byte[] ScopeSyncKnowledge { get; set; } // scope_sync_knowledge
        public byte[] ScopeTombstoneCleanupKnowledge { get; set; } // scope_tombstone_cleanup_knowledge
        public byte[] ScopeTimestamp { get; set; } // scope_timestamp
        public Guid? ScopeConfigId { get; set; } // scope_config_id
        public int ScopeRestoreCount { get; set; } // scope_restore_count
        public string ScopeUserComment { get; set; } // scope_user_comment
        
        public ScopeInfoDss()
        {
            ScopeId = System.Guid.NewGuid();
            ScopeRestoreCount = 0;
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
