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
    internal partial class StateProvinceMap : EntityTypeConfiguration<StateProvince>
    {
        public StateProvinceMap()
            : this("dbo")
        {
        }
 
        public StateProvinceMap(string schema)
        {
            ToTable(schema + ".StateProvince");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.CountryId).HasColumnName("CountryId").IsRequired().HasColumnType("int");
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Abbreviation).HasColumnName("Abbreviation").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Published).HasColumnName("Published").IsRequired().HasColumnType("bit");
            Property(x => x.DisplayOrder).HasColumnName("DisplayOrder").IsRequired().HasColumnType("int");

            // Foreign keys
            HasRequired(a => a.Country).WithMany(b => b.StateProvinces).HasForeignKey(c => c.CountryId); // FK_FBClone.StateProvinces_FBClone.Countries_CountryId
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
