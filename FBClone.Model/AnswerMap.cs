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
    // Answers
    internal partial class AnswerMap : EntityTypeConfiguration<Answer>
    {
        public AnswerMap()
            : this("FBClone")
        {
        }
 
        public AnswerMap(string schema)
        {
            ToTable(schema + ".Answers");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasColumnType("nvarchar").HasMaxLength(128).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.DraftId).HasColumnName("DraftId").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.QuestionId).HasColumnName("QuestionId").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.AnswerText).HasColumnName("AnswerText").IsOptional().HasColumnType("nvarchar");
            Property(x => x.AnswerFactor).HasColumnName("AnswerFactor").IsRequired().HasColumnType("float");
            Property(x => x.Active).HasColumnName("Active").IsRequired().HasColumnType("bit");
            Property(x => x.Sequence).HasColumnName("Sequence").IsRequired().HasColumnType("int");
            Property(x => x.UserId).HasColumnName("UserId").IsOptional().HasColumnType("nvarchar");
            Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsOptional().HasColumnType("nvarchar");
            Property(x => x.UpdatedBy).HasColumnName("UpdatedBy").IsOptional().HasColumnType("nvarchar");
            Property(x => x.Version).HasColumnName("Version").IsRequired().IsFixedLength().HasColumnType("timestamp").HasMaxLength(8).IsRowVersion().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedAt).HasColumnName("CreatedAt").IsRequired().HasColumnType("datetimeoffset");
            Property(x => x.UpdatedAt).HasColumnName("UpdatedAt").IsOptional().HasColumnType("datetimeoffset");
            Property(x => x.Deleted).HasColumnName("Deleted").IsRequired().HasColumnType("bit");

            // Foreign keys
            HasOptional(a => a.AnswerDraft).WithMany(b => b.Answers).HasForeignKey(c => c.DraftId); // FK_FBClone.Answers_FBClone.AnswerDrafts_DraftId
            HasOptional(a => a.Question).WithMany(b => b.Answers).HasForeignKey(c => c.QuestionId); // FK_FBClone.Answers_FBClone.Questions_QuestionId
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
