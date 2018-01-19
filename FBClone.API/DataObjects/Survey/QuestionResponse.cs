using Microsoft.WindowsAzure.Mobile.Service;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBClone.DataObjects
{
    public class QuestionResponse : EntityData
    {
        public string QuestionId { get; set; }
        public string QuestionResponseSetId { get; set; }
        public string AnswerId { get; set; }
        public string Comments { get; set; }
        public bool Skipped { get; set; }
        public double QuestionScore { get; set; }
        public string UserId { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; }
        [JsonIgnore]
        [ForeignKey("QuestionResponseSetId")]
        public virtual QuestionResponseSet QuestionResponseSet { get; set; }
        [ForeignKey("AnswerId")]
        public virtual Answer Answer { get; set; }
    }
}