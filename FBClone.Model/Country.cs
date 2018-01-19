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
    // Country
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.14.3.0")]
    public partial class Country
    {
        public int Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name
        public bool AllowsBilling { get; set; } // AllowsBilling
        public bool AllowsShipping { get; set; } // AllowsShipping
        public string TwoLetterIsoCode { get; set; } // TwoLetterIsoCode
        public string ThreeLetterIsoCode { get; set; } // ThreeLetterIsoCode
        public int NumericIsoCode { get; set; } // NumericIsoCode
        public bool SubjectToVat { get; set; } // SubjectToVat
        public bool Published { get; set; } // Published
        public int DisplayOrder { get; set; } // DisplayOrder
        public bool LimitedToStores { get; set; } // LimitedToStores

        // Reverse navigation
        public virtual ICollection<StateProvince> StateProvinces { get; set; } // StateProvince.FK_FBClone.StateProvinces_FBClone.Countries_CountryId
        
        public Country()
        {
            StateProvinces = new List<StateProvince>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
