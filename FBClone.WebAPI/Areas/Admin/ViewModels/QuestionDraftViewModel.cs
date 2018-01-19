using FBClone.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FBClone.WebAPI.Areas.Admin.ViewModels
{
    public class QuestionDraftViewModel : QuestionDraft
    {
        public List<AnswerDraft> SelectedAnswers { get; set; }
        public List<AnswerDraft> AvailableAnswerChoices { get; set; }
    }
}