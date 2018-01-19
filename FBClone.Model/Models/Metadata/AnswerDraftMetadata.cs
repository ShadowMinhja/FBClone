using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FBClone.Model
{
    [MetadataType(typeof(AnswerDraftMetadata))]
    public partial class AnswerDraft
    {
        private sealed class AnswerDraftMetadata
        {
            [Required(ErrorMessage = "Answer Text Required")]
            [Display(Name = "Answer Text")]
            public string AnswerText { get; set; }
            [Display(Name = "Answer Factor")]
            public string AnswerFactor { get; set; }
            public string UserId { get; set; }
            [JsonIgnore]
            public ICollection<Answer> Answers { get; set; }
            [JsonIgnore]
            public QuestionDraft QuestionDraft { get; set; }
        }

        public AnswerDraft(string id, string questionId, string answerText, double answerFactor, int sequence, string userid, DateTimeOffset createdAt, string createdBy, DateTimeOffset updatedAt, byte[] version)
        {
            this.Id = id;
            this.QuestionId = questionId;
            this.AnswerText = answerText;
            this.AnswerFactor = answerFactor;
            this.Sequence = sequence;
            this.UserId = userid;
            this.CreatedBy = createdBy;
            this.CreatedAt = createdAt;
            this.Version = version;
        }

        public void UpdateFrom(AnswerDraft answerDraft)
        {
            this.QuestionId = answerDraft.QuestionId;
            this.AnswerText = answerDraft.AnswerText;
            this.AnswerFactor = answerDraft.AnswerFactor;
            this.Sequence = answerDraft.Sequence;
            this.UserId = answerDraft.UserId;
            this.CreatedAt = answerDraft.CreatedAt;
            this.CreatedBy = answerDraft.CreatedBy;
            this.UpdatedAt = answerDraft.UpdatedAt;
            this.UpdatedBy = answerDraft.UpdatedBy;
            this.Version = answerDraft.Version;
        }
    }
}