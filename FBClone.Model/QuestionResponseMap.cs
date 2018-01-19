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
    // QuestionResponses
    internal partial class QuestionResponseMap : EntityTypeConfiguration<QuestionResponse>
    {
        public QuestionResponseMap()
            : this("FBClone")
        {
        }
 
        public QuestionResponseMap(string schema)
        {
            ToTable(schema + ".QuestionResponses");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasColumnType("nvarchar").HasMaxLength(128).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.QuestionId).HasColumnName("QuestionId").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.QuestionResponseSetId).HasColumnName("QuestionResponseSetId").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.AnswerId).HasColumnName("AnswerId").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.Comments).HasColumnName("Comments").IsOptional().HasColumnType("nvarchar");
            Property(x => x.Skipped).HasColumnName("Skipped").IsRequired().HasColumnType("bit");
            Property(x => x.QuestionScore).HasColumnName("QuestionScore").IsRequired().HasColumnType("float");
            Property(x => x.UserId).HasColumnName("UserId").IsOptional().HasColumnType("nvarchar");
            Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsOptional().HasColumnType("nvarchar");
            Property(x => x.UpdatedBy).HasColumnName("UpdatedBy").IsOptional().HasColumnType("nvarchar");
            Property(x => x.Version).HasColumnName("Version").IsRequired().IsFixedLength().HasColumnType("timestamp").HasMaxLength(8).IsRowVersion().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedAt).HasColumnName("CreatedAt").IsRequired().HasColumnType("datetimeoffset");
            Property(x => x.UpdatedAt).HasColumnName("UpdatedAt").IsOptional().HasColumnType("datetimeoffset");
            Property(x => x.Deleted).HasColumnName("Deleted").IsRequired().HasColumnType("bit");

            // Foreign keys
            HasOptional(a => a.Answer).WithMany(b => b.QuestionResponses).HasForeignKey(c => c.AnswerId); // FK_FBClone.QuestionResponses_FBClone.Answers_AnswerId
            HasOptional(a => a.Question).WithMany(b => b.QuestionResponses).HasForeignKey(c => c.QuestionId); // FK_FBClone.QuestionResponses_FBClone.Questions_QuestionId
            HasOptional(a => a.QuestionResponseSet).WithMany(b => b.QuestionResponses).HasForeignKey(c => c.QuestionResponseSetId); // FK_FBClone.QuestionResponses_FBClone.QuestionResponseSets_QuestionResponseSetId
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
