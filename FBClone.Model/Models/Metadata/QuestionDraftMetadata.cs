using AutoMapper;
using FBClone.Model.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FBClone.Model
{
    [MetadataType(typeof(QuestionDraftMetadata))]
    public partial class QuestionDraft
    {
        private sealed class QuestionDraftMetadata
        {
            [Required(ErrorMessage = "*")]
            public string Id { get; set; }
            [Required(ErrorMessage = "*")]
            [Display(Name = "Form Id")]
            public string SurveyId { get; set; }
            [Display(Name = "Question Type")]
            [Required(ErrorMessage = "*")]
            public string QuestionType { get; set; }
            [Display(Name = "Is Optional?")]
            [Required(ErrorMessage = "*")]
            public bool IsOptional { get; set; }
            [Display(Name = "Question Text")]
            [Required(ErrorMessage = "*")]
            //[MaxLength(40)]
            public string QuestionText { get; set; }
            [Display(Name = "Category")]
            [Required(ErrorMessage = "*")]
            public string CategoryId { get; set; }
            public bool AdminOnly { get; set; }
            [Display(Name = "Question Factor")]
            [Required(ErrorMessage = "*")]
            public string QuestionFactor { get; set; }
            [Display(Name = "Sequence")]
            [Required(ErrorMessage = "*")]
            public int Sequence { get; set; }
            public string UserId { get; set; }
            public System.DateTimeOffset CreatedAt { get; set; }
            public string CreatedBy { get; set; }
            public System.DateTimeOffset UpdatedAt { get; set; }
            public string UpdatedBy { get; set; }
            public byte[] Version { get; set; }
            [JsonIgnore]
            public ICollection<Question> Questions { get; set; } // Questions.FK_FBClone.Questions_FBClone.QuestionDrafts_DraftId
            [JsonIgnore]
            public  Survey Survey { get; set; } // FK_FBClone.QuestionDrafts_FBClone.Surveys_SurveyId
        }

        public QuestionDraftViewModel ToViewModel()
        {
            return Mapper.Map<QuestionDraftViewModel>(this);
        }

        public QuestionDraft(string surveyId, string questionType, bool isOptional, string questiontext, double questionFactor, String cId, bool adminOnly, int sequence, string userid,
            DateTimeOffset CreatedAt, string CreatedBy, DateTimeOffset UpdatedAt, string UpdatedBy, byte[] Version)
        {
            this.SurveyId = surveyId;
            this.QuestionType = questionType;
            this.IsOptional = isOptional;
            this.QuestionText = questiontext;
            this.QuestionFactor = questionFactor;
            this.CategoryId = cId;
            this.AdminOnly = adminOnly;
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