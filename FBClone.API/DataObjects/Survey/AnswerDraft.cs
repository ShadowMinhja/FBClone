using Microsoft.WindowsAzure.Mobile.Service;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBClone.DataObjects
{
    public class AnswerDraft : EntityData
    {
        public string QuestionId { get; set; }
        public string AnswerText { get; set; }
        public double AnswerFactor { get; set; }
        public bool Active { get; set; }
        public int Sequence { get; set; }
        public string UserId { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        [ForeignKey("QuestionId")]
        public virtual QuestionDraft QuestionDraft { get; set; }
    }
}