// ReSharper disable RedundantUsingDirective
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable RedundantNameQualifier
// TargetFrameworkVersion = 4.51
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data.Entity.ModelConfiguration;
using System.Threading;
using System.Threading.Tasks;
using DatabaseGeneratedOption = System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption;

namespace FBClone.Model
{
    public partial class FBCloneContext : DbContext, IFBCloneContext
    {
        public IDbSet<Answer> Answers { get; set; } // Answers
        public IDbSet<AnswerDraft> AnswerDrafts { get; set; } // AnswerDrafts
        public IDbSet<ApplicationSetting> ApplicationSettings { get; set; } // ApplicationSettings
        public IDbSet<AspNetRole> AspNetRoles { get; set; } // AspNetRoles
        public IDbSet<AspNetUser> AspNetUsers { get; set; } // AspNetUsers
        public IDbSet<AspNetUserClaim> AspNetUserClaims { get; set; } // AspNetUserClaims
        public IDbSet<AspNetUserLogin> AspNetUserLogins { get; set; } // AspNetUserLogins
        public IDbSet<AspNetUserRole> AspNetUserRoles { get; set; } // AspNetUserRoles
        public IDbSet<Category> Categories { get; set; } // Categories
        public IDbSet<City> Cities { get; set; } // Cities
        public IDbSet<Country> Countries { get; set; } // Country
        public IDbSet<CreditCard> CreditCards { get; set; } // CreditCards
        public IDbSet<HubConnection> HubConnections { get; set; } // HubConnections
        public IDbSet<Invoice> Invoices { get; set; } // Invoices
        public IDbSet<LineItem> LineItems { get; set; } // LineItems
        public IDbSet<Location> Locations { get; set; } // Locations
        public IDbSet<Menu> Menus { get; set; } // Menus
        public IDbSet<MenuItem> MenuItems { get; set; } // MenuItems
        public IDbSet<MenuQrCode> MenuQrCodes { get; set; } // MenuQRCodes
        public IDbSet<MenuSection> MenuSections { get; set; } // MenuSections
        public IDbSet<Order> Orders { get; set; } // Orders
        public IDbSet<OrderDetail> OrderDetails { get; set; } // OrderDetails
        public IDbSet<Question> Questions { get; set; } // Questions
        public IDbSet<QuestionDraft> QuestionDrafts { get; set; } // QuestionDrafts
        public IDbSet<QuestionResponse> QuestionResponses { get; set; } // QuestionResponses
        public IDbSet<QuestionResponseSet> QuestionResponseSets { get; set; } // QuestionResponseSets
        public IDbSet<StaffMember> StaffMembers { get; set; } // StaffMembers
        public IDbSet<StateProvince> StateProvinces { get; set; } // StateProvince
        public IDbSet<Subscription> Subscriptions { get; set; } // Subscriptions
        public IDbSet<SubscriptionPlan> SubscriptionPlans { get; set; } // SubscriptionPlans
        public IDbSet<SubscriptionPlanProperty> SubscriptionPlanProperties { get; set; } // SubscriptionPlanProperties
        public IDbSet<Survey> Surveys { get; set; } // Surveys
        
        static FBCloneContext()
        {
            System.Data.Entity.Database.SetInitializer<FBCloneContext>(null);
        }

        public FBCloneContext()
            : base("Name=FBCloneContext")
        {
            InitializePartial();
        }

        public FBCloneContext(string connectionString) : base(connectionString)
        {
            InitializePartial();
        }

        public FBCloneContext(string connectionString, System.Data.Entity.Infrastructure.DbCompiledModel model) : base(connectionString, model)
        {
            InitializePartial();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new AnswerMap());
            modelBuilder.Configurations.Add(new AnswerDraftMap());
            modelBuilder.Configurations.Add(new ApplicationSettingMap());
            modelBuilder.Configurations.Add(new AspNetRoleMap());
            modelBuilder.Configurations.Add(new AspNetUserMap());
            modelBuilder.Configurations.Add(new AspNetUserClaimMap());
            modelBuilder.Configurations.Add(new AspNetUserLoginMap());
            modelBuilder.Configurations.Add(new AspNetUserRoleMap());
            modelBuilder.Configurations.Add(new CategoryMap());
            modelBuilder.Configurations.Add(new CityMap());
            modelBuilder.Configurations.Add(new CountryMap());
            modelBuilder.Configurations.Add(new CreditCardMap());
            modelBuilder.Configurations.Add(new HubConnectionMap());
            modelBuilder.Configurations.Add(new InvoiceMap());
            modelBuilder.Configurations.Add(new LineItemMap());
            modelBuilder.Configurations.Add(new LocationMap());
            modelBuilder.Configurations.Add(new MenuMap());
            modelBuilder.Configurations.Add(new MenuItemMap());
            modelBuilder.Configurations.Add(new MenuQrCodeMap());
            modelBuilder.Configurations.Add(new MenuSectionMap());
            modelBuilder.Configurations.Add(new OrderMap());
            modelBuilder.Configurations.Add(new OrderDetailMap());
            modelBuilder.Configurations.Add(new QuestionMap());
            modelBuilder.Configurations.Add(new QuestionDraftMap());
            modelBuilder.Configurations.Add(new QuestionResponseMap());
            modelBuilder.Configurations.Add(new QuestionResponseSetMap());
            modelBuilder.Configurations.Add(new StaffMemberMap());
            modelBuilder.Configurations.Add(new StateProvinceMap());
            modelBuilder.Configurations.Add(new SubscriptionMap());
            modelBuilder.Configurations.Add(new SubscriptionPlanMap());
            modelBuilder.Configurations.Add(new SubscriptionPlanPropertyMap());
            modelBuilder.Configurations.Add(new SurveyMap());

            OnModelCreatingPartial(modelBuilder);
        }

        public static DbModelBuilder CreateModel(DbModelBuilder modelBuilder, string schema)
        {
            modelBuilder.Configurations.Add(new AnswerMap(schema));
            modelBuilder.Configurations.Add(new AnswerDraftMap(schema));
            modelBuilder.Configurations.Add(new ApplicationSettingMap(schema));
            modelBuilder.Configurations.Add(new AspNetRoleMap(schema));
            modelBuilder.Configurations.Add(new AspNetUserMap(schema));
            modelBuilder.Configurations.Add(new AspNetUserClaimMap(schema));
            modelBuilder.Configurations.Add(new AspNetUserLoginMap(schema));
            modelBuilder.Configurations.Add(new AspNetUserRoleMap(schema));
            modelBuilder.Configurations.Add(new CategoryMap(schema));
            modelBuilder.Configurations.Add(new CityMap(schema));
            modelBuilder.Configurations.Add(new CountryMap(schema));
            modelBuilder.Configurations.Add(new CreditCardMap(schema));
            modelBuilder.Configurations.Add(new HubConnectionMap(schema));
            modelBuilder.Configurations.Add(new InvoiceMap(schema));
            modelBuilder.Configurations.Add(new LineItemMap(schema));
            modelBuilder.Configurations.Add(new LocationMap(schema));
            modelBuilder.Configurations.Add(new MenuMap(schema));
            modelBuilder.Configurations.Add(new MenuItemMap(schema));
            modelBuilder.Configurations.Add(new MenuQrCodeMap(schema));
            modelBuilder.Configurations.Add(new MenuSectionMap(schema));
            modelBuilder.Configurations.Add(new OrderMap(schema));
            modelBuilder.Configurations.Add(new OrderDetailMap(schema));
            modelBuilder.Configurations.Add(new QuestionMap(schema));
            modelBuilder.Configurations.Add(new QuestionDraftMap(schema));
            modelBuilder.Configurations.Add(new QuestionResponseMap(schema));
            modelBuilder.Configurations.Add(new QuestionResponseSetMap(schema));
            modelBuilder.Configurations.Add(new StaffMemberMap(schema));
            modelBuilder.Configurations.Add(new StateProvinceMap(schema));
            modelBuilder.Configurations.Add(new SubscriptionMap(schema));
            modelBuilder.Configurations.Add(new SubscriptionPlanMap(schema));
            modelBuilder.Configurations.Add(new SubscriptionPlanPropertyMap(schema));
            modelBuilder.Configurations.Add(new SurveyMap(schema));
            return modelBuilder;
        }

        partial void InitializePartial();
        partial void OnModelCreatingPartial(DbModelBuilder modelBuilder);
        
        // Stored Procedures
        public List<RspQuestionResponseSetAvgScoreReturnModel> RspQuestionResponseSetAvgScore(Guid? userId, DateTime? beginDate, DateTime? endDate)
        {
            int procResult;
            return RspQuestionResponseSetAvgScore(userId, beginDate, endDate, out procResult);
        }

        public List<RspQuestionResponseSetAvgScoreReturnModel> RspQuestionResponseSetAvgScore(Guid? userId, DateTime? beginDate, DateTime? endDate, out int procResult)
        {
            var userIdParam = new SqlParameter { ParameterName = "@UserId", SqlDbType = SqlDbType.UniqueIdentifier, Direction = ParameterDirection.Input, Value = userId.GetValueOrDefault() };
            if (!(userIdParam.Value is ValueType) && userIdParam.Value == null)
                userIdParam.Value = DBNull.Value;

            var beginDateParam = new SqlParameter { ParameterName = "@BeginDate", SqlDbType = SqlDbType.Date, Direction = ParameterDirection.Input, Value = beginDate.GetValueOrDefault() };
            if (!(beginDateParam.Value is ValueType) && beginDateParam.Value == null)
                beginDateParam.Value = DBNull.Value;

            var endDateParam = new SqlParameter { ParameterName = "@EndDate", SqlDbType = SqlDbType.Date, Direction = ParameterDirection.Input, Value = endDate.GetValueOrDefault() };
            if (!(endDateParam.Value is ValueType) && endDateParam.Value == null)
                endDateParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<RspQuestionResponseSetAvgScoreReturnModel>("EXEC @procResult = [dbo].[rspQuestionResponseSetAvgScore] @UserId, @BeginDate, @EndDate", userIdParam, beginDateParam, endDateParam, procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public List<RspQuestionResponseSetAvgScoreByStaffMemberReturnModel> RspQuestionResponseSetAvgScoreByStaffMember(Guid? userId, DateTime? beginDate, DateTime? endDate)
        {
            int procResult;
            return RspQuestionResponseSetAvgScoreByStaffMember(userId, beginDate, endDate, out procResult);
        }

        public List<RspQuestionResponseSetAvgScoreByStaffMemberReturnModel> RspQuestionResponseSetAvgScoreByStaffMember(Guid? userId, DateTime? beginDate, DateTime? endDate, out int procResult)
        {
            var userIdParam = new SqlParameter { ParameterName = "@UserId", SqlDbType = SqlDbType.UniqueIdentifier, Direction = ParameterDirection.Input, Value = userId.GetValueOrDefault() };
            if (!(userIdParam.Value is ValueType) && userIdParam.Value == null)
                userIdParam.Value = DBNull.Value;

            var beginDateParam = new SqlParameter { ParameterName = "@BeginDate", SqlDbType = SqlDbType.Date, Direction = ParameterDirection.Input, Value = beginDate.GetValueOrDefault() };
            if (!(beginDateParam.Value is ValueType) && beginDateParam.Value == null)
                beginDateParam.Value = DBNull.Value;

            var endDateParam = new SqlParameter { ParameterName = "@EndDate", SqlDbType = SqlDbType.Date, Direction = ParameterDirection.Input, Value = endDate.GetValueOrDefault() };
            if (!(endDateParam.Value is ValueType) && endDateParam.Value == null)
                endDateParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<RspQuestionResponseSetAvgScoreByStaffMemberReturnModel>("EXEC @procResult = [dbo].[rspQuestionResponseSetAvgScoreByStaffMember] @UserId, @BeginDate, @EndDate", userIdParam, beginDateParam, endDateParam, procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public List<RspQuestionResponseSetPositivitySpreadReturnModel> RspQuestionResponseSetPositivitySpread(Guid? userId, DateTime? beginDate, DateTime? endDate)
        {
            int procResult;
            return RspQuestionResponseSetPositivitySpread(userId, beginDate, endDate, out procResult);
        }

        public List<RspQuestionResponseSetPositivitySpreadReturnModel> RspQuestionResponseSetPositivitySpread(Guid? userId, DateTime? beginDate, DateTime? endDate, out int procResult)
        {
            var userIdParam = new SqlParameter { ParameterName = "@UserId", SqlDbType = SqlDbType.UniqueIdentifier, Direction = ParameterDirection.Input, Value = userId.GetValueOrDefault() };
            if (!(userIdParam.Value is ValueType) && userIdParam.Value == null)
                userIdParam.Value = DBNull.Value;

            var beginDateParam = new SqlParameter { ParameterName = "@BeginDate", SqlDbType = SqlDbType.Date, Direction = ParameterDirection.Input, Value = beginDate.GetValueOrDefault() };
            if (!(beginDateParam.Value is ValueType) && beginDateParam.Value == null)
                beginDateParam.Value = DBNull.Value;

            var endDateParam = new SqlParameter { ParameterName = "@EndDate", SqlDbType = SqlDbType.Date, Direction = ParameterDirection.Input, Value = endDate.GetValueOrDefault() };
            if (!(endDateParam.Value is ValueType) && endDateParam.Value == null)
                endDateParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<RspQuestionResponseSetPositivitySpreadReturnModel>("EXEC @procResult = [dbo].[rspQuestionResponseSetPositivitySpread] @UserId, @BeginDate, @EndDate", userIdParam, beginDateParam, endDateParam, procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public List<RspQuestionTextAvgScoreReturnModel> RspQuestionTextAvgScore(Guid? userId, DateTime? beginDate, DateTime? endDate)
        {
            int procResult;
            return RspQuestionTextAvgScore(userId, beginDate, endDate, out procResult);
        }

        public List<RspQuestionTextAvgScoreReturnModel> RspQuestionTextAvgScore(Guid? userId, DateTime? beginDate, DateTime? endDate, out int procResult)
        {
            var userIdParam = new SqlParameter { ParameterName = "@UserId", SqlDbType = SqlDbType.UniqueIdentifier, Direction = ParameterDirection.Input, Value = userId.GetValueOrDefault() };
            if (!(userIdParam.Value is ValueType) && userIdParam.Value == null)
                userIdParam.Value = DBNull.Value;

            var beginDateParam = new SqlParameter { ParameterName = "@BeginDate", SqlDbType = SqlDbType.Date, Direction = ParameterDirection.Input, Value = beginDate.GetValueOrDefault() };
            if (!(beginDateParam.Value is ValueType) && beginDateParam.Value == null)
                beginDateParam.Value = DBNull.Value;

            var endDateParam = new SqlParameter { ParameterName = "@EndDate", SqlDbType = SqlDbType.Date, Direction = ParameterDirection.Input, Value = endDate.GetValueOrDefault() };
            if (!(endDateParam.Value is ValueType) && endDateParam.Value == null)
                endDateParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<RspQuestionTextAvgScoreReturnModel>("EXEC @procResult = [dbo].[rspQuestionTextAvgScore] @UserId, @BeginDate, @EndDate", userIdParam, beginDateParam, endDateParam, procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public int SpClearUserTestData(string email)
        {
            var emailParam = new SqlParameter { ParameterName = "@email", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = email, Size = 256 };
            if (!(emailParam.Value is ValueType) && emailParam.Value == null)
                emailParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[spClearUserTestData] @email", emailParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int SpCreateUser(string userid)
        {
            var useridParam = new SqlParameter { ParameterName = "@userid", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = userid, Size = 128 };
            if (!(useridParam.Value is ValueType) && useridParam.Value == null)
                useridParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[spCreateUser] @userid", useridParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int SpDeleteUser(string email)
        {
            var emailParam = new SqlParameter { ParameterName = "@email", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = email, Size = 256 };
            if (!(emailParam.Value is ValueType) && emailParam.Value == null)
                emailParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[spDeleteUser] @email", emailParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public List<SpGetAnswersByQuestionIdReturnModel> SpGetAnswersByQuestionId(Guid? questionId, Guid? userid, bool? isDraft)
        {
            int procResult;
            return SpGetAnswersByQuestionId(questionId, userid, isDraft, out procResult);
        }

        public List<SpGetAnswersByQuestionIdReturnModel> SpGetAnswersByQuestionId(Guid? questionId, Guid? userid, bool? isDraft, out int procResult)
        {
            var questionIdParam = new SqlParameter { ParameterName = "@questionId", SqlDbType = SqlDbType.UniqueIdentifier, Direction = ParameterDirection.Input, Value = questionId.GetValueOrDefault() };
            if (!(questionIdParam.Value is ValueType) && questionIdParam.Value == null)
                questionIdParam.Value = DBNull.Value;

            var useridParam = new SqlParameter { ParameterName = "@userid", SqlDbType = SqlDbType.UniqueIdentifier, Direction = ParameterDirection.Input, Value = userid.GetValueOrDefault() };
            if (!(useridParam.Value is ValueType) && useridParam.Value == null)
                useridParam.Value = DBNull.Value;

            var isDraftParam = new SqlParameter { ParameterName = "@isDraft", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Input, Value = isDraft.GetValueOrDefault() };
            if (!(isDraftParam.Value is ValueType) && isDraftParam.Value == null)
                isDraftParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<SpGetAnswersByQuestionIdReturnModel>("EXEC @procResult = [dbo].[spGetAnswersByQuestionId] @questionId, @userid, @isDraft", questionIdParam, useridParam, isDraftParam, procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public List<SpGetLocationReviewsReturnModel> SpGetLocationReviews(Guid? locationId)
        {
            int procResult;
            return SpGetLocationReviews(locationId, out procResult);
        }

        public List<SpGetLocationReviewsReturnModel> SpGetLocationReviews(Guid? locationId, out int procResult)
        {
            var locationIdParam = new SqlParameter { ParameterName = "@LocationId", SqlDbType = SqlDbType.UniqueIdentifier, Direction = ParameterDirection.Input, Value = locationId.GetValueOrDefault() };
            if (!(locationIdParam.Value is ValueType) && locationIdParam.Value == null)
                locationIdParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<SpGetLocationReviewsReturnModel>("EXEC @procResult = [dbo].[spGetLocationReviews] @LocationId", locationIdParam, procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public List<SpGetMenuItemReviewsReturnModel> SpGetMenuItemReviews(Guid? menuItemId)
        {
            int procResult;
            return SpGetMenuItemReviews(menuItemId, out procResult);
        }

        public List<SpGetMenuItemReviewsReturnModel> SpGetMenuItemReviews(Guid? menuItemId, out int procResult)
        {
            var menuItemIdParam = new SqlParameter { ParameterName = "@MenuItemId", SqlDbType = SqlDbType.UniqueIdentifier, Direction = ParameterDirection.Input, Value = menuItemId.GetValueOrDefault() };
            if (!(menuItemIdParam.Value is ValueType) && menuItemIdParam.Value == null)
                menuItemIdParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<SpGetMenuItemReviewsReturnModel>("EXEC @procResult = [dbo].[spGetMenuItemReviews] @MenuItemId", menuItemIdParam, procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public int SpGetQuestionsBySurveyId(string surveyId, Guid? userid, bool? isDraft)
        {
            var surveyIdParam = new SqlParameter { ParameterName = "@surveyId", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = surveyId, Size = 128 };
            if (!(surveyIdParam.Value is ValueType) && surveyIdParam.Value == null)
                surveyIdParam.Value = DBNull.Value;

            var useridParam = new SqlParameter { ParameterName = "@userid", SqlDbType = SqlDbType.UniqueIdentifier, Direction = ParameterDirection.Input, Value = userid.GetValueOrDefault() };
            if (!(useridParam.Value is ValueType) && useridParam.Value == null)
                useridParam.Value = DBNull.Value;

            var isDraftParam = new SqlParameter { ParameterName = "@isDraft", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Input, Value = isDraft.GetValueOrDefault() };
            if (!(isDraftParam.Value is ValueType) && isDraftParam.Value == null)
                isDraftParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[spGetQuestionsBySurveyId] @surveyId, @userid, @isDraft", surveyIdParam, useridParam, isDraftParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int SpPublishSurvey(Guid? surveyId)
        {
            var surveyIdParam = new SqlParameter { ParameterName = "@surveyId", SqlDbType = SqlDbType.UniqueIdentifier, Direction = ParameterDirection.Input, Value = surveyId.GetValueOrDefault() };
            if (!(surveyIdParam.Value is ValueType) && surveyIdParam.Value == null)
                surveyIdParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[spPublishSurvey] @surveyId", surveyIdParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int SpUserClaimLocationAndMenus(Guid? locationId, Guid? userId)
        {
            var locationIdParam = new SqlParameter { ParameterName = "@locationId", SqlDbType = SqlDbType.UniqueIdentifier, Direction = ParameterDirection.Input, Value = locationId.GetValueOrDefault() };
            if (!(locationIdParam.Value is ValueType) && locationIdParam.Value == null)
                locationIdParam.Value = DBNull.Value;

            var userIdParam = new SqlParameter { ParameterName = "@userId", SqlDbType = SqlDbType.UniqueIdentifier, Direction = ParameterDirection.Input, Value = userId.GetValueOrDefault() };
            if (!(userIdParam.Value is ValueType) && userIdParam.Value == null)
                userIdParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [dbo].[spUserClaimLocationAndMenus] @locationId, @userId", locationIdParam, userIdParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

    }
}
