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
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.14.3.0")]
    public partial class QuestionResponseSet
    {
        public string Id { get; set; } // Id (Primary key)
        public string SurveyId { get; set; } // SurveyId
        public string LocationId { get; set; } // LocationId
        public long OrderId { get; set; } // OrderId
        public string StaffMemberId { get; set; } // StaffMemberId
        public string CustomerName { get; set; } // CustomerName
        public string CustomerEmail { get; set; } // CustomerEmail
        public bool IsSubscribe { get; set; } // IsSubscribe
        public string Positivity { get; set; } // Positivity
        public string TableNumber { get; set; } // TableNumber
        public double TotalScore { get; set; } // TotalScore
        public double SessionDuration { get; set; } // SessionDuration
        public string UserId { get; set; } // UserId
        public string CreatedBy { get; set; } // CreatedBy
        public string UpdatedBy { get; set; } // UpdatedBy
        public byte[] Version { get; set; } // Version
        public DateTimeOffset CreatedAt { get; set; } // CreatedAt
        public DateTimeOffset? UpdatedAt { get; set; } // UpdatedAt
        public bool Deleted { get; set; } // Deleted

        // Reverse navigation
        public virtual ICollection<QuestionResponse> QuestionResponses { get; set; } // QuestionResponses.FK_FBClone.QuestionResponses_FBClone.QuestionResponseSets_QuestionResponseSetId

        // Foreign keys
        public virtual Location Location { get; set; } // FK_FBClone.QuestionResponseSets_FBClone.Locations_LocationId
        public virtual StaffMember StaffMember { get; set; } // FK_FBClone.QuestionResponseSets_FBClone.StaffMembers_StaffMemberId
        public virtual Survey Survey { get; set; } // FK_FBClone.QuestionResponseSets_FBClone.Surveys_SurveyId
        
        public QuestionResponseSet()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = System.DateTimeOffset.UtcNow;
            QuestionResponses = new List<QuestionResponse>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
