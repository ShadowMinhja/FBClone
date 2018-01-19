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
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.14.3.0")]
    public partial class Invoice
    {
        public int Id { get; set; } // Id (Primary key)
        public string StripeId { get; set; } // StripeId
        public string StripeCustomerId { get; set; } // StripeCustomerId
        public DateTime? Date { get; set; } // Date
        public DateTime? PeriodStart { get; set; } // PeriodStart
        public DateTime? PeriodEnd { get; set; } // PeriodEnd
        public int? Subtotal { get; set; } // Subtotal
        public int? Total { get; set; } // Total
        public bool? Attempted { get; set; } // Attempted
        public bool? Closed { get; set; } // Closed
        public bool? Paid { get; set; } // Paid
        public int? AttemptCount { get; set; } // AttemptCount
        public int? AmountDue { get; set; } // AmountDue
        public int? StartingBalance { get; set; } // StartingBalance
        public int? EndingBalance { get; set; } // EndingBalance
        public DateTime? NextPaymentAttempt { get; set; } // NextPaymentAttempt
        public int? ApplicationFee { get; set; } // ApplicationFee
        public int? Tax { get; set; } // Tax
        public decimal? TaxPercent { get; set; } // TaxPercent
        public string Currency { get; set; } // Currency
        public string BillingAddressName { get; set; } // BillingAddress_Name
        public string BillingAddressAddressLine1 { get; set; } // BillingAddress_AddressLine1
        public string BillingAddressAddressLine2 { get; set; } // BillingAddress_AddressLine2
        public string BillingAddressCity { get; set; } // BillingAddress_City
        public string BillingAddressState { get; set; } // BillingAddress_State
        public string BillingAddressZipCode { get; set; } // BillingAddress_ZipCode
        public string BillingAddressCountry { get; set; } // BillingAddress_Country
        public string BillingAddressVat { get; set; } // BillingAddress_Vat
        public string Description { get; set; } // Description
        public string StatementDescriptor { get; set; } // StatementDescriptor
        public string ReceiptNumber { get; set; } // ReceiptNumber
        public bool Forgiven { get; set; } // Forgiven
        public string CustomerId { get; set; } // Customer_Id

        // Reverse navigation
        public virtual ICollection<LineItem> LineItems { get; set; } // LineItems.FK_dbo.LineItems_dbo.Invoices_Invoice_Id
        
        public Invoice()
        {
            LineItems = new List<LineItem>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
