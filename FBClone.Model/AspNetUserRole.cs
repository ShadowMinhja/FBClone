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
    // AspNetUserRoles
    public partial class AspNetUserRole
    {
        public string UserId { get; set; } // UserId (Primary key)
        public string RoleId { get; set; } // RoleId (Primary key)
        public string SaasEcomUserId { get; set; } // SaasEcomUser_Id

        // Foreign keys
        public virtual AspNetRole AspNetRole { get; set; } // FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId
        
        public AspNetUserRole()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
