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
    // QuestionResponseSets
    internal partial class QuestionResponseSetMap : EntityTypeConfiguration<QuestionResponseSet>
    {
        public QuestionResponseSetMap()
            : this("FBClone")
        {
        }
 
        public QuestionResponseSetMap(string schema)
        {
            ToTable(schema + ".QuestionResponseSets");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasColumnType("nvarchar").HasMaxLength(128).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.SurveyId).HasColumnName("SurveyId").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.LocationId).HasColumnName("LocationId").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.OrderId).HasColumnName("OrderId").IsRequired().HasColumnType("bigint");
            Property(x => x.StaffMemberId).HasColumnName("StaffMemberId").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.CustomerName).HasColumnName("CustomerName").IsOptional().HasColumnType("nvarchar");
            Property(x => x.CustomerEmail).HasColumnName("CustomerEmail").IsOptional().HasColumnType("nvarchar");
            Property(x => x.IsSubscribe).HasColumnName("IsSubscribe").IsRequired().HasColumnType("bit");
            Property(x => x.Positivity).HasColumnName("Positivity").IsOptional().HasColumnType("nvarchar");
            Property(x => x.TableNumber).HasColumnName("TableNumber").IsOptional().HasColumnType("nvarchar");
            Property(x => x.TotalScore).HasColumnName("TotalScore").IsRequired().HasColumnType("float");
            Property(x => x.SessionDuration).HasColumnName("SessionDuration").IsRequired().HasColumnType("float");
            Property(x => x.UserId).HasColumnName("UserId").IsOptional().HasColumnType("nvarchar");
            Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsOptional().HasColumnType("nvarchar");
            Property(x => x.UpdatedBy).HasColumnName("UpdatedBy").IsOptional().HasColumnType("nvarchar");
            Property(x => x.Version).HasColumnName("Version").IsRequired().IsFixedLength().HasColumnType("timestamp").HasMaxLength(8).IsRowVersion().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedAt).HasColumnName("CreatedAt").IsRequired().HasColumnType("datetimeoffset");
            Property(x => x.UpdatedAt).HasColumnName("UpdatedAt").IsOptional().HasColumnType("datetimeoffset");
            Property(x => x.Deleted).HasColumnName("Deleted").IsRequired().HasColumnType("bit");

            // Foreign keys
            HasOptional(a => a.Location).WithMany(b => b.QuestionResponseSets).HasForeignKey(c => c.LocationId); // FK_FBClone.QuestionResponseSets_FBClone.Locations_LocationId
            HasOptional(a => a.StaffMember).WithMany(b => b.QuestionResponseSets).HasForeignKey(c => c.StaffMemberId); // FK_FBClone.QuestionResponseSets_FBClone.StaffMembers_StaffMemberId
            HasOptional(a => a.Survey).WithMany(b => b.QuestionResponseSets).HasForeignKey(c => c.SurveyId); // FK_FBClone.QuestionResponseSets_FBClone.Surveys_SurveyId
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
