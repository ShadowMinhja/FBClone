using Microsoft.WindowsAzure.Mobile.Service;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBClone.DataObjects
{
    public class Question : EntityData
    {
        public string DraftId { get; set; }
        public string SurveyId { get; set; }
        public string CategoryId { get; set; }
        public string QuestionType { get; set; }
        public bool IsOptional { get; set; }
        public string QuestionText { get; set; }
        public double QuestionFactor { get; set; }
        public bool Active { get; set; }
        public bool AdminOnly { get; set; }
        public int Sequence { get; set; }
        public ICollection<Answer> Answers { get; set; }
        public string UserId { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        [ForeignKey("DraftId")]
        public virtual QuestionDraft QuestionDraft { get; set; }
        [ForeignKey("SurveyId")]
        public virtual Survey Survey { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}