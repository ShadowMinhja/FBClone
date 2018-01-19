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
    // Subscriptions
    public partial class Subscription
    {
        public int Id { get; set; } // Id (Primary key)
        public DateTime? Start { get; set; } // Start
        public DateTime? End { get; set; } // End
        public DateTime? TrialStart { get; set; } // TrialStart
        public DateTime? TrialEnd { get; set; } // TrialEnd
        public string SubscriptionPlanId { get; set; } // SubscriptionPlanId
        public string UserId { get; set; } // UserId
        public string StripeId { get; set; } // StripeId
        public string Status { get; set; } // Status
        public decimal TaxPercent { get; set; } // TaxPercent
        public string ReasonToCancel { get; set; } // ReasonToCancel

        // Foreign keys
        public virtual SubscriptionPlan SubscriptionPlan { get; set; } // FK_dbo.Subscriptions_dbo.SubscriptionPlans_SubscriptionPlanId
        
        public Subscription()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
