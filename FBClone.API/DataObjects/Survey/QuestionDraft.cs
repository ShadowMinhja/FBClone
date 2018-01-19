using Microsoft.WindowsAzure.Mobile.Service;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBClone.DataObjects
{
    public class QuestionDraft : EntityData
    {
        public string SurveyId { get; set; }
        public string CategoryId { get; set; }
        public string QuestionType { get; set; }
        public bool IsOptional { get; set; }
        public string QuestionText { get; set; }
        public double QuestionFactor { get; set; }
        public bool AdminOnly { get; set; }
        public int Sequence { get; set; }
        public string UserId { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        [ForeignKey("SurveyId")]
        public virtual Survey Survey { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}