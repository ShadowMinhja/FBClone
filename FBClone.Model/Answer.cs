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
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.14.3.0")]
    public partial class Answer
    {
        public string Id { get; set; } // Id (Primary key)
        public string DraftId { get; set; } // DraftId
        public string QuestionId { get; set; } // QuestionId
        public string AnswerText { get; set; } // AnswerText
        public double AnswerFactor { get; set; } // AnswerFactor
        public bool Active { get; set; } // Active
        public int Sequence { get; set; } // Sequence
        public string UserId { get; set; } // UserId
        public string CreatedBy { get; set; } // CreatedBy
        public string UpdatedBy { get; set; } // UpdatedBy
        public byte[] Version { get; set; } // Version
        public DateTimeOffset CreatedAt { get; set; } // CreatedAt
        public DateTimeOffset? UpdatedAt { get; set; } // UpdatedAt
        public bool Deleted { get; set; } // Deleted

        // Reverse navigation
        public virtual ICollection<QuestionResponse> QuestionResponses { get; set; } // QuestionResponses.FK_FBClone.QuestionResponses_FBClone.Answers_AnswerId

        // Foreign keys
        public virtual AnswerDraft AnswerDraft { get; set; } // FK_FBClone.Answers_FBClone.AnswerDrafts_DraftId
        public virtual Question Question { get; set; } // FK_FBClone.Answers_FBClone.Questions_QuestionId
        
        public Answer()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = System.DateTimeOffset.UtcNow;
            QuestionResponses = new List<QuestionResponse>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
