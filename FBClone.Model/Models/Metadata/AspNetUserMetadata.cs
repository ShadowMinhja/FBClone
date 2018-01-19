using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FBClone.Model
{
    [MetadataType(typeof(AspNetUserMetadata))]
    public partial class AspNetUser
    {
        private sealed class AspNetUserMetadata
        {
            //Hidden for Security Reasons
            [JsonIgnore]
            public string Id { get; set; } // Id (Primary key)
            public string FirstName { get; set; } // FirstName
            public string LastName { get; set; } // LastName
            public string OrganizationName { get; set; } // OrganizationName
            [JsonIgnore]
            public DateTime RegistrationDate { get; set; } // RegistrationDate
            [JsonIgnore]
            public DateTime LastLoginTime { get; set; } // LastLoginTime
            [JsonIgnore]
            public string StripeCustomerId { get; set; } // StripeCustomerId
            [JsonIgnore]
            public string IpAddress { get; set; } // IPAddress
            [JsonIgnore]
            public string IpAddressCountry { get; set; } // IPAddressCountry
            [JsonIgnore]
            public bool Delinquent { get; set; } // Delinquent
            [JsonIgnore]
            public decimal LifetimeValue { get; set; } // LifetimeValue
            [JsonIgnore]
            public string Email { get; set; } // Email
            [JsonIgnore]
            public bool EmailConfirmed { get; set; } // EmailConfirmed
            [JsonIgnore]
            public string PasswordHash { get; set; } // PasswordHash
            [JsonIgnore]
            public string SecurityStamp { get; set; } // SecurityStamp
            [JsonIgnore]
            public string PhoneNumber { get; set; } // PhoneNumber
            [JsonIgnore]
            public bool PhoneNumberConfirmed { get; set; } // PhoneNumberConfirmed
            [JsonIgnore]
            public bool TwoFactorEnabled { get; set; } // TwoFactorEnabled
            [JsonIgnore]
            public DateTime? LockoutEndDateUtc { get; set; } // LockoutEndDateUtc
            [JsonIgnore]
            public bool LockoutEnabled { get; set; } // LockoutEnabled
            [JsonIgnore]
            public int AccessFailedCount { get; set; } // AccessFailedCount
            [JsonIgnore]
            public string UserName { get; set; } // UserName
        }
    }
}