using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FBClone.Model
{
    [MetadataType(typeof(SurveyMetadata))]
    public partial class Survey
    {
        private sealed class SurveyMetadata
        {            
            [Required(ErrorMessage = "Name is required")]
            public string Name { get; set; }
            [Required(ErrorMessage = "Description is required")]
            public string Description { get; set; }
            //[JsonIgnore]
            public ICollection<Question> Questions { get; set; } // Questions.FK_FBClone.Questions_FBClone.Surveys_SurveyId
            [JsonIgnore]
            public ICollection<QuestionResponseSet> QuestionResponseSets { get; set; } // QuestionResponseSets.FK_FBClone.QuestionResponseSets_FBClone.Surveys_SurveyId             
        }

        public Survey(string id, string name, string description, bool active, string userid, DateTimeOffset CreatedAt, string CreatedBy, DateTimeOffset UpdatedAt, string UpdatedBy, byte[] Version)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Active = active;
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

        public void UpdateFrom(Survey survey)
        {
            this.Name = survey.Name;
            this.Description = survey.Description;
            this.Active = survey.Active;
            this.UserId = survey.UserId;
            this.CreatedAt = survey.CreatedAt;
            this.CreatedBy = survey.CreatedBy;
            this.UpdatedAt = survey.UpdatedAt;
            this.UpdatedAt = survey.UpdatedAt;
            this.Version = survey.Version;
        }
    }
}