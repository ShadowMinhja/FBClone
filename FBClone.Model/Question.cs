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
    // Questions
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.14.3.0")]
    public partial class Question
    {
        public string Id { get; set; } // Id (Primary key)
        public string DraftId { get; set; } // DraftId
        public string SurveyId { get; set; } // SurveyId
        public string CategoryId { get; set; } // CategoryId
        public string QuestionType { get; set; } // QuestionType
        public bool IsOptional { get; set; } // IsOptional
        public string QuestionText { get; set; } // QuestionText
        public double QuestionFactor { get; set; } // QuestionFactor
        public bool Active { get; set; } // Active
        public bool AdminOnly { get; set; } // AdminOnly
        public int Sequence { get; set; } // Sequence
        public string UserId { get; set; } // UserId
        public string CreatedBy { get; set; } // CreatedBy
        public string UpdatedBy { get; set; } // UpdatedBy
        public byte[] Version { get; set; } // Version
        public DateTimeOffset CreatedAt { get; set; } // CreatedAt
        public DateTimeOffset? UpdatedAt { get; set; } // UpdatedAt
        public bool Deleted { get; set; } // Deleted

        // Reverse navigation
        public virtual ICollection<Answer> Answers { get; set; } // Answers.FK_FBClone.Answers_FBClone.Questions_QuestionId
        public virtual ICollection<QuestionResponse> QuestionResponses { get; set; } // QuestionResponses.FK_FBClone.QuestionResponses_FBClone.Questions_QuestionId

        // Foreign keys
        public virtual Category Category { get; set; } // FK_FBClone.Questions_FBClone.Categories_CategoryId
        public virtual QuestionDraft QuestionDraft { get; set; } // FK_FBClone.Questions_FBClone.QuestionDrafts_DraftId
        public virtual Survey Survey { get; set; } // FK_FBClone.Questions_FBClone.Surveys_SurveyId
        
        public Question()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = System.DateTimeOffset.UtcNow;
            Answers = new List<Answer>();
            QuestionResponses = new List<QuestionResponse>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
