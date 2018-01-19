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
    // SubscriptionPlans
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.14.3.0")]
    public partial class SubscriptionPlan
    {
        public string Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name
        public double Price { get; set; } // Price
        public string Currency { get; set; } // Currency
        public int Interval { get; set; } // Interval
        public int TrialPeriodInDays { get; set; } // TrialPeriodInDays
        public bool Disabled { get; set; } // Disabled

        // Reverse navigation
        public virtual ICollection<Subscription> Subscriptions { get; set; } // Subscriptions.FK_dbo.Subscriptions_dbo.SubscriptionPlans_SubscriptionPlanId
        public virtual ICollection<SubscriptionPlanProperty> SubscriptionPlanProperties { get; set; } // SubscriptionPlanProperties.FK_dbo.SubscriptionPlanProperties_dbo.SubscriptionPlans_SubscriptionPlan_Id
        
        public SubscriptionPlan()
        {
            SubscriptionPlanProperties = new List<SubscriptionPlanProperty>();
            Subscriptions = new List<Subscription>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
