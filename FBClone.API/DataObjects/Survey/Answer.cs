using Microsoft.WindowsAzure.Mobile.Service;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBClone.DataObjects
{
    public class Answer : EntityData
    {
        public string DraftId { get; set; }
        public string QuestionId { get; set; }
        public string AnswerText { get; set; }
        public double AnswerFactor { get; set; }
        public bool Active { get; set; }
        public int Sequence { get; set; }
        public string UserId { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        [JsonIgnore]
        [ForeignKey("DraftId")]
        public virtual AnswerDraft AnswerDraft { get; set; }
        [JsonIgnore]
        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; }
    }
}