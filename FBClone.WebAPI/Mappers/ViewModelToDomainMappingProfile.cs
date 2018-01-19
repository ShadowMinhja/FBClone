using AutoMapper;
using FBClone.Model;
//using FBClone.WebAPI.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using FBClone.WebAPI.ViewModels;

namespace FBClone.WebAPI.Mappers
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        protected override void Configure()
        {
            //Mapper.CreateMap<ResourceViewModel, Resource>()
            //    .ForMember(resource => resource.Activities, vm => vm.Ignore())
            //    .ForMember(resource => resource.UserId, vm => vm.Ignore());

            //Mapper.CreateMap<QuestionDraftViewModel, QuestionDraft>();
        }
    }
}