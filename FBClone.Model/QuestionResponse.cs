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
    public partial class QuestionResponse
    {
        public string Id { get; set; } // Id (Primary key)
        public string QuestionId { get; set; } // QuestionId
        public string QuestionResponseSetId { get; set; } // QuestionResponseSetId
        public string AnswerId { get; set; } // AnswerId
        public string Comments { get; set; } // Comments
        public bool Skipped { get; set; } // Skipped
        public double QuestionScore { get; set; } // QuestionScore
        public string UserId { get; set; } // UserId
        public string CreatedBy { get; set; } // CreatedBy
        public string UpdatedBy { get; set; } // UpdatedBy
        public byte[] Version { get; set; } // Version
        public DateTimeOffset CreatedAt { get; set; } // CreatedAt
        public DateTimeOffset? UpdatedAt { get; set; } // UpdatedAt
        public bool Deleted { get; set; } // Deleted

        // Foreign keys
        public virtual Answer Answer { get; set; } // FK_FBClone.QuestionResponses_FBClone.Answers_AnswerId
        public virtual Question Question { get; set; } // FK_FBClone.QuestionResponses_FBClone.Questions_QuestionId
        public virtual QuestionResponseSet QuestionResponseSet { get; set; } // FK_FBClone.QuestionResponses_FBClone.QuestionResponseSets_QuestionResponseSetId
        
        public QuestionResponse()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = System.DateTimeOffset.UtcNow;
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
