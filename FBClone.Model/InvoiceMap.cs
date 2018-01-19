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
    // Invoices
    internal partial class InvoiceMap : EntityTypeConfiguration<Invoice>
    {
        public InvoiceMap()
            : this("dbo")
        {
        }
 
        public InvoiceMap(string schema)
        {
            ToTable(schema + ".Invoices");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.StripeId).HasColumnName("StripeId").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.StripeCustomerId).HasColumnName("StripeCustomerId").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.Date).HasColumnName("Date").IsOptional().HasColumnType("datetime");
            Property(x => x.PeriodStart).HasColumnName("PeriodStart").IsOptional().HasColumnType("datetime");
            Property(x => x.PeriodEnd).HasColumnName("PeriodEnd").IsOptional().HasColumnType("datetime");
            Property(x => x.Subtotal).HasColumnName("Subtotal").IsOptional().HasColumnType("int");
            Property(x => x.Total).HasColumnName("Total").IsOptional().HasColumnType("int");
            Property(x => x.Attempted).HasColumnName("Attempted").IsOptional().HasColumnType("bit");
            Property(x => x.Closed).HasColumnName("Closed").IsOptional().HasColumnType("bit");
            Property(x => x.Paid).HasColumnName("Paid").IsOptional().HasColumnType("bit");
            Property(x => x.AttemptCount).HasColumnName("AttemptCount").IsOptional().HasColumnType("int");
            Property(x => x.AmountDue).HasColumnName("AmountDue").IsOptional().HasColumnType("int");
            Property(x => x.StartingBalance).HasColumnName("StartingBalance").IsOptional().HasColumnType("int");
            Property(x => x.EndingBalance).HasColumnName("EndingBalance").IsOptional().HasColumnType("int");
            Property(x => x.NextPaymentAttempt).HasColumnName("NextPaymentAttempt").IsOptional().HasColumnType("datetime");
            Property(x => x.ApplicationFee).HasColumnName("ApplicationFee").IsOptional().HasColumnType("int");
            Property(x => x.Tax).HasColumnName("Tax").IsOptional().HasColumnType("int");
            Property(x => x.TaxPercent).HasColumnName("TaxPercent").IsOptional().HasColumnType("decimal").HasPrecision(18,2);
            Property(x => x.Currency).HasColumnName("Currency").IsOptional().HasColumnType("nvarchar");
            Property(x => x.BillingAddressName).HasColumnName("BillingAddress_Name").IsOptional().HasColumnType("nvarchar");
            Property(x => x.BillingAddressAddressLine1).HasColumnName("BillingAddress_AddressLine1").IsOptional().HasColumnType("nvarchar");
            Property(x => x.BillingAddressAddressLine2).HasColumnName("BillingAddress_AddressLine2").IsOptional().HasColumnType("nvarchar");
            Property(x => x.BillingAddressCity).HasColumnName("BillingAddress_City").IsOptional().HasColumnType("nvarchar");
            Property(x => x.BillingAddressState).HasColumnName("BillingAddress_State").IsOptional().HasColumnType("nvarchar");
            Property(x => x.BillingAddressZipCode).HasColumnName("BillingAddress_ZipCode").IsOptional().HasColumnType("nvarchar");
            Property(x => x.BillingAddressCountry).HasColumnName("BillingAddress_Country").IsOptional().HasColumnType("nvarchar");
            Property(x => x.BillingAddressVat).HasColumnName("BillingAddress_Vat").IsOptional().HasColumnType("nvarchar");
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasColumnType("nvarchar");
            Property(x => x.StatementDescriptor).HasColumnName("StatementDescriptor").IsOptional().HasColumnType("nvarchar");
            Property(x => x.ReceiptNumber).HasColumnName("ReceiptNumber").IsOptional().HasColumnType("nvarchar");
            Property(x => x.Forgiven).HasColumnName("Forgiven").IsRequired().HasColumnType("bit");
            Property(x => x.CustomerId).HasColumnName("Customer_Id").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
