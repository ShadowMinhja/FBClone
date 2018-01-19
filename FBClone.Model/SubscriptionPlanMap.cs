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
    internal partial class SubscriptionPlanMap : EntityTypeConfiguration<SubscriptionPlan>
    {
        public SubscriptionPlanMap()
            : this("dbo")
        {
        }
 
        public SubscriptionPlanMap(string schema)
        {
            ToTable(schema + ".SubscriptionPlans");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasColumnType("nvarchar").HasMaxLength(128).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasColumnType("nvarchar");
            Property(x => x.Price).HasColumnName("Price").IsRequired().HasColumnType("float");
            Property(x => x.Currency).HasColumnName("Currency").IsOptional().HasColumnType("nvarchar");
            Property(x => x.Interval).HasColumnName("Interval").IsRequired().HasColumnType("int");
            Property(x => x.TrialPeriodInDays).HasColumnName("TrialPeriodInDays").IsRequired().HasColumnType("int");
            Property(x => x.Disabled).HasColumnName("Disabled").IsRequired().HasColumnType("bit");
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
