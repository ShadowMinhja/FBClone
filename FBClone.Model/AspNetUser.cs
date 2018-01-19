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
    // AspNetUsers
    public partial class AspNetUser
    {
        public string Id { get; set; } // Id (Primary key)
        public string FirstName { get; set; } // FirstName
        public string LastName { get; set; } // LastName
        public string OrganizationName { get; set; } // OrganizationName
        public DateTime RegistrationDate { get; set; } // RegistrationDate
        public DateTime LastLoginTime { get; set; } // LastLoginTime
        public string StripeCustomerId { get; set; } // StripeCustomerId
        public string IpAddress { get; set; } // IPAddress
        public string IpAddressCountry { get; set; } // IPAddressCountry
        public bool Delinquent { get; set; } // Delinquent
        public decimal LifetimeValue { get; set; } // LifetimeValue
        public string Email { get; set; } // Email
        public bool EmailConfirmed { get; set; } // EmailConfirmed
        public string PasswordHash { get; set; } // PasswordHash
        public string SecurityStamp { get; set; } // SecurityStamp
        public string PhoneNumber { get; set; } // PhoneNumber
        public bool PhoneNumberConfirmed { get; set; } // PhoneNumberConfirmed
        public bool TwoFactorEnabled { get; set; } // TwoFactorEnabled
        public DateTime? LockoutEndDateUtc { get; set; } // LockoutEndDateUtc
        public bool LockoutEnabled { get; set; } // LockoutEnabled
        public int AccessFailedCount { get; set; } // AccessFailedCount
        public string UserName { get; set; } // UserName
        
        public AspNetUser()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
