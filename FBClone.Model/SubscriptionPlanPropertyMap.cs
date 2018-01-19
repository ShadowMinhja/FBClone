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
    internal partial class SubscriptionPlanPropertyMap : EntityTypeConfiguration<SubscriptionPlanProperty>
    {
        public SubscriptionPlanPropertyMap()
            : this("dbo")
        {
        }
 
        public SubscriptionPlanPropertyMap(string schema)
        {
            ToTable(schema + ".SubscriptionPlanProperties");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Key).HasColumnName("Key").IsOptional().HasColumnType("nvarchar");
            Property(x => x.Value).HasColumnName("Value").IsOptional().HasColumnType("nvarchar");
            Property(x => x.SubscriptionPlanId).HasColumnName("SubscriptionPlan_Id").IsRequired().HasColumnType("nvarchar").HasMaxLength(128);

            // Foreign keys
            HasRequired(a => a.SubscriptionPlan).WithMany(b => b.SubscriptionPlanProperties).HasForeignKey(c => c.SubscriptionPlanId); // FK_dbo.SubscriptionPlanProperties_dbo.SubscriptionPlans_SubscriptionPlan_Id
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
