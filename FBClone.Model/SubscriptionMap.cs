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
    internal partial class SubscriptionMap : EntityTypeConfiguration<Subscription>
    {
        public SubscriptionMap()
            : this("dbo")
        {
        }
 
        public SubscriptionMap(string schema)
        {
            ToTable(schema + ".Subscriptions");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Start).HasColumnName("Start").IsOptional().HasColumnType("datetime");
            Property(x => x.End).HasColumnName("End").IsOptional().HasColumnType("datetime");
            Property(x => x.TrialStart).HasColumnName("TrialStart").IsOptional().HasColumnType("datetime");
            Property(x => x.TrialEnd).HasColumnName("TrialEnd").IsOptional().HasColumnType("datetime");
            Property(x => x.SubscriptionPlanId).HasColumnName("SubscriptionPlanId").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.UserId).HasColumnName("UserId").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.StripeId).HasColumnName("StripeId").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.Status).HasColumnName("Status").IsOptional().HasColumnType("nvarchar");
            Property(x => x.TaxPercent).HasColumnName("TaxPercent").IsRequired().HasColumnType("decimal").HasPrecision(18,2);
            Property(x => x.ReasonToCancel).HasColumnName("ReasonToCancel").IsOptional().HasColumnType("nvarchar");

            // Foreign keys
            HasOptional(a => a.SubscriptionPlan).WithMany(b => b.Subscriptions).HasForeignKey(c => c.SubscriptionPlanId); // FK_dbo.Subscriptions_dbo.SubscriptionPlans_SubscriptionPlanId
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
