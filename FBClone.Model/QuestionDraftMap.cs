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
    // QuestionDrafts
    internal partial class QuestionDraftMap : EntityTypeConfiguration<QuestionDraft>
    {
        public QuestionDraftMap()
            : this("FBClone")
        {
        }
 
        public QuestionDraftMap(string schema)
        {
            ToTable(schema + ".QuestionDrafts");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasColumnType("nvarchar").HasMaxLength(128).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.SurveyId).HasColumnName("SurveyId").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.CategoryId).HasColumnName("CategoryId").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.QuestionType).HasColumnName("QuestionType").IsOptional().HasColumnType("nvarchar");
            Property(x => x.IsOptional).HasColumnName("IsOptional").IsRequired().HasColumnType("bit");
            Property(x => x.QuestionText).HasColumnName("QuestionText").IsOptional().HasColumnType("nvarchar");
            Property(x => x.QuestionFactor).HasColumnName("QuestionFactor").IsRequired().HasColumnType("float");
            Property(x => x.AdminOnly).HasColumnName("AdminOnly").IsRequired().HasColumnType("bit");
            Property(x => x.Sequence).HasColumnName("Sequence").IsRequired().HasColumnType("int");
            Property(x => x.UserId).HasColumnName("UserId").IsOptional().HasColumnType("nvarchar");
            Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsOptional().HasColumnType("nvarchar");
            Property(x => x.UpdatedBy).HasColumnName("UpdatedBy").IsOptional().HasColumnType("nvarchar");
            Property(x => x.Version).HasColumnName("Version").IsRequired().IsFixedLength().HasColumnType("timestamp").HasMaxLength(8).IsRowVersion().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedAt).HasColumnName("CreatedAt").IsRequired().HasColumnType("datetimeoffset");
            Property(x => x.UpdatedAt).HasColumnName("UpdatedAt").IsOptional().HasColumnType("datetimeoffset");
            Property(x => x.Deleted).HasColumnName("Deleted").IsRequired().HasColumnType("bit");

            // Foreign keys
            HasOptional(a => a.Category).WithMany(b => b.QuestionDrafts).HasForeignKey(c => c.CategoryId); // FK_FBClone.QuestionDrafts_FBClone.Categories_CategoryId
            HasOptional(a => a.Survey).WithMany(b => b.QuestionDrafts).HasForeignKey(c => c.SurveyId); // FK_FBClone.QuestionDrafts_FBClone.Surveys_SurveyId
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
