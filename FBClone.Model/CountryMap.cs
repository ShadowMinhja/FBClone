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
    internal partial class CountryMap : EntityTypeConfiguration<Country>
    {
        public CountryMap()
            : this("dbo")
        {
        }
 
        public CountryMap(string schema)
        {
            ToTable(schema + ".Country");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.AllowsBilling).HasColumnName("AllowsBilling").IsRequired().HasColumnType("bit");
            Property(x => x.AllowsShipping).HasColumnName("AllowsShipping").IsRequired().HasColumnType("bit");
            Property(x => x.TwoLetterIsoCode).HasColumnName("TwoLetterIsoCode").IsOptional().HasColumnType("nvarchar").HasMaxLength(2);
            Property(x => x.ThreeLetterIsoCode).HasColumnName("ThreeLetterIsoCode").IsOptional().HasColumnType("nvarchar").HasMaxLength(3);
            Property(x => x.NumericIsoCode).HasColumnName("NumericIsoCode").IsRequired().HasColumnType("int");
            Property(x => x.SubjectToVat).HasColumnName("SubjectToVat").IsRequired().HasColumnType("bit");
            Property(x => x.Published).HasColumnName("Published").IsRequired().HasColumnType("bit");
            Property(x => x.DisplayOrder).HasColumnName("DisplayOrder").IsRequired().HasColumnType("int");
            Property(x => x.LimitedToStores).HasColumnName("LimitedToStores").IsRequired().HasColumnType("bit");
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
