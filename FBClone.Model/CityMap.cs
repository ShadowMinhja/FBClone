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
    // Cities
    internal partial class CityMap : EntityTypeConfiguration<City>
    {
        public CityMap()
            : this("dbo")
        {
        }
 
        public CityMap(string schema)
        {
            ToTable(schema + ".Cities");
            HasKey(x => new { x.City_, x.StateCode, x.Zip, x.County });

            Property(x => x.City_).HasColumnName("city").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(50);
            Property(x => x.StateCode).HasColumnName("state_code").IsRequired().IsFixedLength().IsUnicode(false).HasColumnType("char").HasMaxLength(2);
            Property(x => x.Zip).HasColumnName("zip").IsRequired().HasColumnType("nvarchar").HasMaxLength(12);
            Property(x => x.Latitude).HasColumnName("latitude").IsOptional().HasColumnType("float");
            Property(x => x.Longitude).HasColumnName("longitude").IsOptional().HasColumnType("float");
            Property(x => x.County).HasColumnName("county").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(50);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
