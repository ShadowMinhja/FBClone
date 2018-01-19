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
    // AspNetUserClaims
    public partial class AspNetUserClaim
    {
        public int Id { get; set; } // Id (Primary key)
        public string UserId { get; set; } // UserId
        public string ClaimType { get; set; } // ClaimType
        public string ClaimValue { get; set; } // ClaimValue
        public string SaasEcomUserId { get; set; } // SaasEcomUser_Id
        
        public AspNetUserClaim()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
