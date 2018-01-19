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
    // MenuSections
    internal partial class MenuSectionMap : EntityTypeConfiguration<MenuSection>
    {
        public MenuSectionMap()
            : this("FBClone")
        {
        }
 
        public MenuSectionMap(string schema)
        {
            ToTable(schema + ".MenuSections");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasColumnType("nvarchar").HasMaxLength(128).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.MenuId).HasColumnName("MenuId").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.SectionTitle).HasColumnName("SectionTitle").IsOptional().HasColumnType("nvarchar");
            Property(x => x.SectionSubTitle).HasColumnName("SectionSubTitle").IsOptional().HasColumnType("nvarchar");
            Property(x => x.Active).HasColumnName("Active").IsOptional().HasColumnType("nvarchar");
            Property(x => x.Sequence).HasColumnName("Sequence").IsRequired().HasColumnType("int");
            Property(x => x.UserId).HasColumnName("UserId").IsOptional().HasColumnType("nvarchar");
            Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsOptional().HasColumnType("nvarchar");
            Property(x => x.UpdatedBy).HasColumnName("UpdatedBy").IsOptional().HasColumnType("nvarchar");
            Property(x => x.Version).HasColumnName("Version").IsRequired().IsFixedLength().HasColumnType("timestamp").HasMaxLength(8).IsRowVersion().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedAt).HasColumnName("CreatedAt").IsRequired().HasColumnType("datetimeoffset");
            Property(x => x.UpdatedAt).HasColumnName("UpdatedAt").IsOptional().HasColumnType("datetimeoffset");
            Property(x => x.Deleted).HasColumnName("Deleted").IsRequired().HasColumnType("bit");

            // Foreign keys
            HasOptional(a => a.Menu).WithMany(b => b.MenuSections).HasForeignKey(c => c.MenuId); // FK_FBClone.MenuSections_FBClone.Menus_MenuId
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
