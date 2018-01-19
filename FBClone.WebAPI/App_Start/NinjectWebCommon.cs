using FBClone.WebAPI.App_Start;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(NinjectWebCommon), "Stop")]

namespace FBClone.WebAPI.App_Start
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using System.Reflection;
    using Data.Infrastructure;
    using Data.Repositories;
    using Service;
    using System.Web.Http;
    using Hubs;
    using Microsoft.AspNet.SignalR;
    using Common;
    using Newtonsoft.Json;
    using Ninject.Activation;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            RegisterServices(kernel);

            // Install our Ninject-based IDependencyResolver into the Web API config
            GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);

            // Install our Ninject-based IDependencyResolver into SignalR            
            GlobalHost.DependencyResolver = new NinjectSignalRDependencyResolver(kernel);
            //Register CamelCase Serializer
            //http://stackoverflow.com/questions/30005575/signalr-use-camel-case
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new SignalRContractResolver();
            var serializer = JsonSerializer.Create(settings);
            kernel.Bind<JsonSerializer>().ToConstant(serializer);

            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IEntityDBService>().To<EntityDBService>();
            kernel.Bind<IDatabaseFactory>().To<DatabaseFactory>().InThreadScope();
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>();
            //SQL
            kernel.Bind<IAspNetUserRepository>().To<AspNetUserRepository>();
            kernel.Bind<IAspNetUserService>().To<AspNetUserService>();
            kernel.Bind<IStaffMemberRepository>().To<StaffMemberRepository>();
            kernel.Bind<IStaffMemberService>().To<StaffMemberService>();
            kernel.Bind<ILocationRepository>().To<LocationRepository>();
            kernel.Bind<ILocationService>().To<LocationService>();
            kernel.Bind<ISurveyRepository>().To<SurveyRepository>();
            kernel.Bind<ISurveyService>().To<SurveyService>();
            kernel.Bind<IQuestionDraftRepository>().To<QuestionDraftRepository>();
            kernel.Bind<IQuestionDraftService>().To<QuestionDraftService>();
            kernel.Bind<IAnswerDraftRepository>().To<AnswerDraftRepository>();
            kernel.Bind<IAnswerDraftService>().To<AnswerDraftService>();
            kernel.Bind<IQuestionRepository>().To<QuestionRepository>();
            kernel.Bind<IQuestionService>().To<QuestionService>();
            kernel.Bind<IAnswerRepository>().To<AnswerRepository>();
            kernel.Bind<IAnswerService>().To<AnswerService>();
            kernel.Bind<ICategoryRepository>().To<CategoryRepository>();
            kernel.Bind<ICategoryService>().To<CategoryService>();            
            kernel.Bind<IApplicationSettingService>().To<ApplicationSettingService>();
            kernel.Bind<IStateProvinceRepository>().To<StateProvinceRepository>();
            kernel.Bind<IStateProvinceService>().To<StateProvinceService>();
            kernel.Bind<IQuestionResponseSetRepository>().To<QuestionResponseSetRepository>();
            kernel.Bind<IQuestionResponseSetService>().To<QuestionResponseSetService>();            
            kernel.Bind<IQuestionResponseRepository>().To<QuestionResponseRepository>();
            kernel.Bind<IMenuRepository>().To<MenuRepository>();
            kernel.Bind<IMenuSectionRepository>().To<MenuSectionRepository>();
            kernel.Bind<IMenuItemRepository>().To<MenuItemRepository>();
            kernel.Bind<IMenuService>().To<MenuService>();
            kernel.Bind<IMenuQrCodeRepository>().To<MenuQrCodeRepository>();
            kernel.Bind<IMenuQrCodeService>().To<MenuQrCodeService>();
            kernel.Bind<IOrderRepository>().To<OrderRepository>();
            kernel.Bind<IOrderDetailRepository>().To<OrderDetailRepository>();
            kernel.Bind<IOrderService>().To<OrderService>();
            //Signal R
            kernel.Bind<IHubConnectionRepository>().To<HubConnectionRepository>();
            kernel.Bind<IHubConnectionService>().To<HubConnectionService>();
            //Cloud Storage
            kernel.Bind<IBlobStorageService>().To<BlobStorageService>();            
            kernel.Bind<IS3BucketService>().To<S3BucketService>();
            //Mongo DB / GetStream.IO
            kernel.Bind<IMongoService>().To<MongoService>().InSingletonScope();
            kernel.Bind<IBiteService>().To<BiteService>();
            kernel.Bind<ISearchService>().To<SearchService>();
            kernel.Bind<IProfileService>().To<ProfileService>();
            //Algolia
            kernel.Bind<IAlgoliaService>().To<AlgoliaService>().InSingletonScope();            

        }

    }
}