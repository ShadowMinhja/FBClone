using Autofac;
using Microsoft.AspNet.Identity;
using FBClone.WebAPI.Mappers;
using FBClone.Data.Infrastructure;
using FBClone.Data.Repositories;
using FBClone.Model;
//using FBClone.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.Identity.EntityFramework;
using FBClone.Service;
using FBClone.WebAPI.Common;

namespace FBClone.WebAPI
{
    public class Bootstrapper
    {
        public static void Configure()
        {
            ConfigureAutofacContainer();
            AutoMapperConfiguration.Configure();
        }

        public static void ConfigureAutofacContainer()
        {
            var webApiContainerBuilder = new ContainerBuilder();
            ConfigureWebApiContainer(webApiContainerBuilder);
        }

        public static void ConfigureWebApiContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().AsImplementedInterfaces().InstancePerRequest();
            containerBuilder.RegisterType<UnitOfWork>().As<IUnitOfWork>().AsImplementedInterfaces().InstancePerRequest();
            containerBuilder.RegisterType<StaffMemberRepository>().As<IStaffMemberRepository>().InstancePerRequest();
            containerBuilder.RegisterType<StaffMemberService>().As<IStaffMemberService>().InstancePerRequest();
            containerBuilder.RegisterType<LocationRepository>().As<ILocationRepository>().InstancePerRequest();
            containerBuilder.RegisterType<LocationService>().As<ILocationService>().InstancePerRequest();
            containerBuilder.RegisterType<SurveyRepository>().As<ISurveyRepository>().InstancePerRequest();
            containerBuilder.RegisterType<SurveyService>().As<ISurveyService>().InstancePerRequest();
            containerBuilder.RegisterType<QuestionDraftRepository>().As<IQuestionDraftRepository>().InstancePerRequest();
            containerBuilder.RegisterType<QuestionDraftService>().As<IQuestionDraftService>().InstancePerRequest();
            containerBuilder.RegisterType<AnswerDraftRepository>().As<IAnswerDraftRepository>().InstancePerRequest();
            containerBuilder.RegisterType<AnswerDraftService>().As<IAnswerDraftService>().InstancePerRequest();
            containerBuilder.RegisterType<QuestionRepository>().As<IQuestionRepository>().InstancePerRequest();
            containerBuilder.RegisterType<QuestionService>().As<IQuestionService>().InstancePerRequest();
            containerBuilder.RegisterType<AnswerRepository>().As<IAnswerRepository>().InstancePerRequest();
            containerBuilder.RegisterType<AnswerService>().As<IAnswerService>().InstancePerRequest();
            containerBuilder.RegisterType<CategoryRepository>().As<ICategoryRepository>().InstancePerRequest();
            containerBuilder.RegisterType<CategoryService>().As<ICategoryService>().InstancePerRequest();
            containerBuilder.RegisterType<BlobStorageService>().As<IBlobStorageService>().InstancePerRequest();            
            containerBuilder.RegisterType<ApplicationSettingService>().As<IApplicationSettingService>().InstancePerRequest();
            containerBuilder.RegisterType<StateProvinceRepository>().As<IStateProvinceRepository>().InstancePerRequest();
            containerBuilder.RegisterType<StateProvinceService>().As<IStateProvinceService>().InstancePerRequest();
            containerBuilder.RegisterType<QuestionResponseSetRepository>().As<IQuestionResponseSetRepository>().InstancePerRequest();
            containerBuilder.RegisterType<QuestionResponseSetService>().As<IQuestionResponseSetService>().InstancePerRequest();
            containerBuilder.RegisterType<EntityDBService>().As<IEntityDBService>().InstancePerRequest();
            containerBuilder.RegisterType<MenuRepository>().As<IMenuRepository>().InstancePerRequest();
            containerBuilder.RegisterType<MenuSectionRepository>().As<IMenuSectionRepository>().InstancePerRequest();
            containerBuilder.RegisterType<MenuItemRepository>().As<IMenuItemRepository>().InstancePerRequest();
            containerBuilder.RegisterType<MenuService>().As<IMenuService>().InstancePerRequest();
            containerBuilder.RegisterType<MenuQrCodeRepository>().As<IMenuQrCodeRepository>().InstancePerRequest();
            containerBuilder.RegisterType<MenuQrCodeService>().As<IMenuQrCodeService>().InstancePerRequest();
            containerBuilder.RegisterType<OrderRepository>().As<IOrderRepository>().InstancePerRequest();
            containerBuilder.RegisterType<OrderDetailRepository>().As<IOrderDetailRepository>().InstancePerRequest();
            containerBuilder.RegisterType<OrderService>().As<IOrderService>().InstancePerRequest();

            //containerBuilder.Register(c => new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new MentorMatchEntities())
            //{
            //    /*Avoids UserStore invoking SaveChanges on every actions.*/
            //    //AutoSaveChanges = false
            //})).As<UserManager<ApplicationUser>>().InstancePerRequest();

            containerBuilder.RegisterApiControllers(System.Reflection.Assembly.GetExecutingAssembly());
            IContainer container = containerBuilder.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

    }
}