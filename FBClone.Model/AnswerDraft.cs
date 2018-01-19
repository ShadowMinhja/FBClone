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
    // AnswerDrafts
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.14.3.0")]
    public partial class AnswerDraft
    {
        public string Id { get; set; } // Id (Primary key)
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
        public virtual ICollection<Answer> Answers { get; set; } // Answers.FK_FBClone.Answers_FBClone.AnswerDrafts_DraftId

        // Foreign keys
        public virtual QuestionDraft QuestionDraft { get; set; } // FK_FBClone.AnswerDrafts_FBClone.QuestionDrafts_QuestionId
        
        public AnswerDraft()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = System.DateTimeOffset.UtcNow;
            Answers = new List<Answer>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
