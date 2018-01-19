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
    // SubscriptionPlanProperties
    public partial class SubscriptionPlanProperty
    {
        public int Id { get; set; } // Id (Primary key)
        public string Key { get; set; } // Key
        public string Value { get; set; } // Value
        public string SubscriptionPlanId { get; set; } // SubscriptionPlan_Id

        // Foreign keys
        public virtual SubscriptionPlan SubscriptionPlan { get; set; } // FK_dbo.SubscriptionPlanProperties_dbo.SubscriptionPlans_SubscriptionPlan_Id
        
        public SubscriptionPlanProperty()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
