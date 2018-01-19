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
    // CreditCards
    public partial class CreditCard
    {
        public int Id { get; set; } // Id (Primary key)
        public string StripeId { get; set; } // StripeId
        public string Name { get; set; } // Name
        public string Last4 { get; set; } // Last4
        public string Type { get; set; } // Type
        public string Fingerprint { get; set; } // Fingerprint
        public string AddressCity { get; set; } // AddressCity
        public string AddressCountry { get; set; } // AddressCountry
        public string AddressLine1 { get; set; } // AddressLine1
        public string AddressLine2 { get; set; } // AddressLine2
        public string AddressState { get; set; } // AddressState
        public string AddressZip { get; set; } // AddressZip
        public string Cvc { get; set; } // Cvc
        public string ExpirationMonth { get; set; } // ExpirationMonth
        public string ExpirationYear { get; set; } // ExpirationYear
        public string CardCountry { get; set; } // CardCountry
        public string SaasEcomUserId { get; set; } // SaasEcomUserId
        
        public CreditCard()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
