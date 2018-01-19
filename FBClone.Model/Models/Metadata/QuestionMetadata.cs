using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FBClone.Model
{
    [MetadataType(typeof(QuestionMetadata))]
    public partial class Question
    {
        private sealed class QuestionMetadata
        {
            //[JsonIgnore]
            public ICollection<Answer> Answers { get; set; } // Answers.FK_FBClone.Answers_FBClone.Questions_QuestionId
            [JsonIgnore]
            public ICollection<QuestionResponse> QuestionResponses { get; set; } // QuestionResponses.FK_FBClone.QuestionResponses_FBClone.Questions_QuestionId

            // Foreign keys
            //[JsonIgnore]
            public Category Category { get; set; } // FK_FBClone.Questions_FBClone.Categories_CategoryId
            [JsonIgnore]
            public QuestionDraft QuestionDraft { get; set; } // FK_FBClone.Questions_FBClone.QuestionDrafts_DraftId
            [JsonIgnore]
            public Survey Survey { get; set; } // FK_FBClone.Questions_FBClone.Surveys_SurveyId
        }

        public Question(string draftId, string surveyId, string questionType, bool isOptional, string questionText, double questionFactor, bool active, bool adminOnly, string categoryId, int sequence, string userid, 
            DateTimeOffset CreatedAt, string CreatedBy, DateTimeOffset UpdatedAt, string UpdatedBy, byte[] Version)
        {
            this.DraftId = draftId;
            this.SurveyId = surveyId;
            this.QuestionType = questionType;
            this.IsOptional = isOptional;
            this.QuestionText = questionText;
            this.QuestionFactor = questionFactor;
            this.Active = active;
            this.AdminOnly = adminOnly;
            this.CategoryId = categoryId;
            this.Sequence = sequence;
            this.UserId = userid;
            if (CreatedBy != String.Empty)
            {
                this.CreatedAt = CreatedAt;
                this.CreatedBy = CreatedBy;
            }
            this.UpdatedAt = UpdatedAt;
            this.UpdatedAt = UpdatedAt;
            this.Version = Version;
        }
    }
}