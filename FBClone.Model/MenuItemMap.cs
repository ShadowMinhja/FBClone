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
    // MenuItems
    internal partial class MenuItemMap : EntityTypeConfiguration<MenuItem>
    {
        public MenuItemMap()
            : this("FBClone")
        {
        }
 
        public MenuItemMap(string schema)
        {
            ToTable(schema + ".MenuItems");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasColumnType("nvarchar").HasMaxLength(128).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.MenuSectionId).HasColumnName("MenuSectionId").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.ItemType).HasColumnName("ItemType").IsOptional().HasColumnType("nvarchar");
            Property(x => x.Required).HasColumnName("Required").IsRequired().HasColumnType("bit");
            Property(x => x.ItemText).HasColumnName("ItemText").IsOptional().HasColumnType("nvarchar");
            Property(x => x.ItemDesc).HasColumnName("ItemDesc").IsOptional().HasColumnType("nvarchar");
            Property(x => x.Price).HasColumnName("Price").IsRequired().HasColumnType("float");
            Property(x => x.Active).HasColumnName("Active").IsOptional().HasColumnType("nvarchar");
            Property(x => x.DaysOfWeek).HasColumnName("DaysOfWeek").IsOptional().HasColumnType("nvarchar");
            Property(x => x.Sequence).HasColumnName("Sequence").IsRequired().HasColumnType("int");
            Property(x => x.ItemImageUrl).HasColumnName("ItemImageURL").IsOptional().HasColumnType("nvarchar");
            Property(x => x.Cl).HasColumnName("CL").IsOptional().HasColumnType("float");
            Property(x => x.ImageCl).HasColumnName("ImageCL").IsOptional().HasColumnType("float");
            Property(x => x.Source).HasColumnName("Source").IsOptional().HasColumnType("nvarchar");
            Property(x => x.SourceType).HasColumnName("SourceType").IsOptional().HasColumnType("nvarchar");
            Property(x => x.FoodTotalScore).HasColumnName("FoodTotalScore").IsOptional().HasColumnType("float");
            Property(x => x.FoodTotalReviews).HasColumnName("FoodTotalReviews").IsOptional().HasColumnType("int");
            Property(x => x.UserId).HasColumnName("UserId").IsOptional().HasColumnType("nvarchar");
            Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsOptional().HasColumnType("nvarchar");
            Property(x => x.UpdatedBy).HasColumnName("UpdatedBy").IsOptional().HasColumnType("nvarchar");
            Property(x => x.Version).HasColumnName("Version").IsRequired().IsFixedLength().HasColumnType("timestamp").HasMaxLength(8).IsRowVersion().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedAt).HasColumnName("CreatedAt").IsRequired().HasColumnType("datetimeoffset");
            Property(x => x.UpdatedAt).HasColumnName("UpdatedAt").IsOptional().HasColumnType("datetimeoffset");
            Property(x => x.Deleted).HasColumnName("Deleted").IsRequired().HasColumnType("bit");

            // Foreign keys
            HasOptional(a => a.MenuSection).WithMany(b => b.MenuItems).HasForeignKey(c => c.MenuSectionId); // FK_FBClone.MenuItems_FBClone.MenuSections_MenuSectionId
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
