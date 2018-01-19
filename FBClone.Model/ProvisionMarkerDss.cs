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
    public partial class ProvisionMarkerDss
    {
        public int ObjectId { get; set; } // object_id (Primary key)
        public int OwnerScopeLocalId { get; set; } // owner_scope_local_id (Primary key)
        public int? ProvisionScopeLocalId { get; set; } // provision_scope_local_id
        public long ProvisionTimestamp { get; set; } // provision_timestamp
        public int ProvisionLocalPeerKey { get; set; } // provision_local_peer_key
        public int? ProvisionScopePeerKey { get; set; } // provision_scope_peer_key
        public long? ProvisionScopePeerTimestamp { get; set; } // provision_scope_peer_timestamp
        public DateTime? ProvisionDatetime { get; set; } // provision_datetime
        public int? State { get; set; } // state
        public byte[] Version { get; set; } // version
        
        public ProvisionMarkerDss()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
