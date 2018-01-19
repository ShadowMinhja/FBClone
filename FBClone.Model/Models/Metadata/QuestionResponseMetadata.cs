using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FBClone.Model
{
    [MetadataType(typeof(QuestionResponseMetadata))]
    public partial class QuestionResponse
    {
        private sealed class QuestionResponseMetadata
        {
            [DisplayFormat(DataFormatString = "{0:P}", ApplyFormatInEditMode = true)]
            public double QuestionScore { get; set; }
            [JsonIgnore]
            public QuestionResponseSet QuestionResponseSet { get; set; } // FK_FBClone.QuestionResponses_FBClone.QuestionResponseSets_QuestionResponseSetId
            //[JsonIgnore]
            //public Answer Answer { get; set; } // FK_FBClone.QuestionResponses_FBClone.Answers_AnswerId
        }
    }
}