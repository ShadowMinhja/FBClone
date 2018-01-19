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
    // StateProvince
    public partial class StateProvince
    {
        public int Id { get; set; } // Id (Primary key)
        public int CountryId { get; set; } // CountryId
        public string Name { get; set; } // Name
        public string Abbreviation { get; set; } // Abbreviation
        public bool Published { get; set; } // Published
        public int DisplayOrder { get; set; } // DisplayOrder

        // Foreign keys
        public virtual Country Country { get; set; } // FK_FBClone.StateProvinces_FBClone.Countries_CountryId
        
        public StateProvince()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
