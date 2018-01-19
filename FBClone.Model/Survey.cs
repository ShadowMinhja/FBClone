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
    // Surveys
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.14.3.0")]
    public partial class Survey
    {
        public string Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name
        public string Description { get; set; } // Description
        public bool Active { get; set; } // Active
        public string UserId { get; set; } // UserId
        public string CreatedBy { get; set; } // CreatedBy
        public string UpdatedBy { get; set; } // UpdatedBy
        public byte[] Version { get; set; } // Version
        public DateTimeOffset CreatedAt { get; set; } // CreatedAt
        public DateTimeOffset? UpdatedAt { get; set; } // UpdatedAt
        public bool Deleted { get; set; } // Deleted

        // Reverse navigation
        public virtual ICollection<Question> Questions { get; set; } // Questions.FK_FBClone.Questions_FBClone.Surveys_SurveyId
        public virtual ICollection<QuestionDraft> QuestionDrafts { get; set; } // QuestionDrafts.FK_FBClone.QuestionDrafts_FBClone.Surveys_SurveyId
        public virtual ICollection<QuestionResponseSet> QuestionResponseSets { get; set; } // QuestionResponseSets.FK_FBClone.QuestionResponseSets_FBClone.Surveys_SurveyId
        
        public Survey()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = System.DateTimeOffset.UtcNow;
            QuestionDrafts = new List<QuestionDraft>();
            QuestionResponseSets = new List<QuestionResponseSet>();
            Questions = new List<Question>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
