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
    // ApplicationSettings
    internal partial class ApplicationSettingMap : EntityTypeConfiguration<ApplicationSetting>
    {
        public ApplicationSettingMap()
            : this("FBClone")
        {
        }
 
        public ApplicationSettingMap(string schema)
        {
            ToTable(schema + ".ApplicationSettings");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasColumnType("nvarchar").HasMaxLength(128).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.StaffSetup).HasColumnName("StaffSetup").IsRequired().HasColumnType("bit");
            Property(x => x.SurveySetup).HasColumnName("SurveySetup").IsRequired().HasColumnType("bit");
            Property(x => x.PromotionSetup).HasColumnName("PromotionSetup").IsRequired().HasColumnType("bit");
            Property(x => x.BrandingSetup).HasColumnName("BrandingSetup").IsRequired().HasColumnType("bit");
            Property(x => x.SurveyIsLooping).HasColumnName("SurveyIsLooping").IsRequired().HasColumnType("bit");
            Property(x => x.BrandingLogoUrl).HasColumnName("BrandingLogoURL").IsOptional().HasColumnType("nvarchar");
            Property(x => x.PromoLogoUrl).HasColumnName("PromoLogoURL").IsOptional().HasColumnType("nvarchar");
            Property(x => x.UserId).HasColumnName("UserId").IsOptional().HasColumnType("nvarchar");
            Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsOptional().HasColumnType("nvarchar");
            Property(x => x.UpdatedBy).HasColumnName("UpdatedBy").IsOptional().HasColumnType("nvarchar");
            Property(x => x.Version).HasColumnName("Version").IsRequired().IsFixedLength().HasColumnType("timestamp").HasMaxLength(8).IsRowVersion().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedAt).HasColumnName("CreatedAt").IsRequired().HasColumnType("datetimeoffset");
            Property(x => x.UpdatedAt).HasColumnName("UpdatedAt").IsOptional().HasColumnType("datetimeoffset");
            Property(x => x.Deleted).HasColumnName("Deleted").IsRequired().HasColumnType("bit");
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
