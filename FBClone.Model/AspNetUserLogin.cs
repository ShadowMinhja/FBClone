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
    // AspNetUserLogins
    public partial class AspNetUserLogin
    {
        public string LoginProvider { get; set; } // LoginProvider (Primary key)
        public string ProviderKey { get; set; } // ProviderKey (Primary key)
        public string UserId { get; set; } // UserId (Primary key)
        public string SaasEcomUserId { get; set; } // SaasEcomUser_Id
        
        public AspNetUserLogin()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
