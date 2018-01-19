using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBClone.Model.ViewModels
{
    [NotMapped]
    public class QuestionDraftViewModel : QuestionDraft
    {
        public List<AnswerDraft> SelectedAnswers { get; set; }
        public List<AnswerDraft> AvailableAnswerChoices { get; set; }

        public QuestionDraft ToDomainModel()
        {
            return Mapper.Map<QuestionDraft>(this);
        }
    }
}
