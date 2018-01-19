using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FBClone.Model
{
    [MetadataType(typeof(AnswerMetadata))]
    public partial class Answer
    {
        private sealed class AnswerMetadata
        {
            [JsonIgnore]
            public ICollection<QuestionResponse> QuestionResponses { get; set; } // QuestionResponses.FK_FBClone.QuestionResponses_FBClone.Answers_AnswerId
            [JsonIgnore]
            public AnswerDraft AnswerDraft { get; set; } // FK_FBClone.Answers_FBClone.AnswerDrafts_DraftId
            [JsonIgnore]
            public Question Question { get; set; } // FK_FBClone.Answers_FBClone.Questions_QuestionId
        }

        public Answer(string draftId, string questionId, string answerText, double answerFactor, bool active, int sequence, string userid, DateTimeOffset createdAt, string createdBy, DateTimeOffset updatedAt, string updatedBy, byte[] version)
        {
            this.DraftId = draftId;
            this.QuestionId = questionId;
            this.AnswerText = answerText;
            this.AnswerFactor = answerFactor;
            this.Active = active;
            this.Sequence = sequence;
            this.UserId = userid;
            this.CreatedAt = createdAt;
            this.CreatedBy = createdBy;
            this.UpdatedAt = updatedAt;
            this.UpdatedBy = updatedBy;
            this.Version = version;
        }
    }
}