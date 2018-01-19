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
    public partial class LineItem
    {
        public int Id { get; set; } // Id (Primary key)
        public string StripeLineItemId { get; set; } // StripeLineItemId
        public string Type { get; set; } // Type
        public int? Amount { get; set; } // Amount
        public string Currency { get; set; } // Currency
        public bool Proration { get; set; } // Proration
        public DateTime? PeriodStart { get; set; } // Period_Start
        public DateTime? PeriodEnd { get; set; } // Period_End
        public int? Quantity { get; set; } // Quantity
        public string PlanStripePlanId { get; set; } // Plan_StripePlanId
        public string PlanInterval { get; set; } // Plan_Interval
        public string PlanName { get; set; } // Plan_Name
        public DateTime? PlanCreated { get; set; } // Plan_Created
        public int? PlanAmountInCents { get; set; } // Plan_AmountInCents
        public string PlanCurrency { get; set; } // Plan_Currency
        public int PlanIntervalCount { get; set; } // Plan_IntervalCount
        public int? PlanTrialPeriodDays { get; set; } // Plan_TrialPeriodDays
        public string PlanStatementDescriptor { get; set; } // Plan_StatementDescriptor
        public int? InvoiceId { get; set; } // Invoice_Id

        // Foreign keys
        public virtual Invoice Invoice { get; set; } // FK_dbo.LineItems_dbo.Invoices_Invoice_Id
        
        public LineItem()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
