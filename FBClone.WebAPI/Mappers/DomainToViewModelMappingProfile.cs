using AutoMapper;
using FBClone.Model;
using FBClone.WebAPI.Areas.Guestcards;
//using FBClone.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FBClone.WebAPI.Mappers
{
    public class DomainToViewModelMappingProfile : Profile
    {
        protected override void Configure()
        {
            //Mapper.CreateMap<ResourceActivity, ResourceActivityViewModel>()
            //    .ForMember(vm => vm.ActivityDateString, dm => dm.MapFrom(dModel => dModel.ActivityDate.ToLongDateString()));
            //Mapper.CreateMap<QuestionDraft, QuestionDraftViewModel>()
            //    .ForMember(x => x.SelectedAnswers, o => o.MapFrom(s => s.AnswerDrafts.ToList().Where(x => x.Active == true).OrderBy(x => x.Sequence)))
            //    .ForMember(x => x.AvailableAnswerChoices, o => o.MapFrom(s => s.AnswerDrafts.ToList().Where(x => x.Active == true).OrderBy(x => x.Sequence)));
        }
    }
}