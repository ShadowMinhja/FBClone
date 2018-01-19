using Microsoft.WindowsAzure.Mobile.Service;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBClone.DataObjects
{
    public class QuestionResponseSet : EntityData
    {
        public string SurveyId { get; set; }
        public string LocationId { get; set; }
        public long OrderId { get; set; }
        public string StaffMemberId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public bool IsSubscribe { get; set; }
        public string Positivity { get; set; }
        public ICollection<QuestionResponse> QuestionResponses { get; set; }
        public string TableNumber { get; set; }
        public double TotalScore { get; set; }
        public double SessionDuration { get; set; }
        public string UserId { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        [ForeignKey("SurveyId")]
        public virtual Survey Survey { get; set; }
        [ForeignKey("LocationId")]
        public virtual Location Location { get; set; }
        [ForeignKey("StaffMemberId")]
        public virtual StaffMember StaffMember { get; set; }
    }
}