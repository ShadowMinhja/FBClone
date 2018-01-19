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
    // LineItems
    internal partial class LineItemMap : EntityTypeConfiguration<LineItem>
    {
        public LineItemMap()
            : this("dbo")
        {
        }
 
        public LineItemMap(string schema)
        {
            ToTable(schema + ".LineItems");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.StripeLineItemId).HasColumnName("StripeLineItemId").IsOptional().HasColumnType("nvarchar");
            Property(x => x.Type).HasColumnName("Type").IsOptional().HasColumnType("nvarchar");
            Property(x => x.Amount).HasColumnName("Amount").IsOptional().HasColumnType("int");
            Property(x => x.Currency).HasColumnName("Currency").IsOptional().HasColumnType("nvarchar");
            Property(x => x.Proration).HasColumnName("Proration").IsRequired().HasColumnType("bit");
            Property(x => x.PeriodStart).HasColumnName("Period_Start").IsOptional().HasColumnType("datetime");
            Property(x => x.PeriodEnd).HasColumnName("Period_End").IsOptional().HasColumnType("datetime");
            Property(x => x.Quantity).HasColumnName("Quantity").IsOptional().HasColumnType("int");
            Property(x => x.PlanStripePlanId).HasColumnName("Plan_StripePlanId").IsOptional().HasColumnType("nvarchar");
            Property(x => x.PlanInterval).HasColumnName("Plan_Interval").IsOptional().HasColumnType("nvarchar");
            Property(x => x.PlanName).HasColumnName("Plan_Name").IsOptional().HasColumnType("nvarchar");
            Property(x => x.PlanCreated).HasColumnName("Plan_Created").IsOptional().HasColumnType("datetime");
            Property(x => x.PlanAmountInCents).HasColumnName("Plan_AmountInCents").IsOptional().HasColumnType("int");
            Property(x => x.PlanCurrency).HasColumnName("Plan_Currency").IsOptional().HasColumnType("nvarchar");
            Property(x => x.PlanIntervalCount).HasColumnName("Plan_IntervalCount").IsRequired().HasColumnType("int");
            Property(x => x.PlanTrialPeriodDays).HasColumnName("Plan_TrialPeriodDays").IsOptional().HasColumnType("int");
            Property(x => x.PlanStatementDescriptor).HasColumnName("Plan_StatementDescriptor").IsOptional().HasColumnType("nvarchar");
            Property(x => x.InvoiceId).HasColumnName("Invoice_Id").IsOptional().HasColumnType("int");

            // Foreign keys
            HasOptional(a => a.Invoice).WithMany(b => b.LineItems).HasForeignKey(c => c.InvoiceId); // FK_dbo.LineItems_dbo.Invoices_Invoice_Id
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
