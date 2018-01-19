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
    // AspNetRoles
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.14.3.0")]
    public partial class AspNetRole
    {
        public string Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name

        // Reverse navigation
        public virtual ICollection<AspNetUserRole> AspNetUserRoles { get; set; } // Many to many mapping
        
        public AspNetRole()
        {
            AspNetUserRoles = new List<AspNetUserRole>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
