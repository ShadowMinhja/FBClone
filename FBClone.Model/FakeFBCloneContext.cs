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
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.14.3.0")]
    public class FakeFBCloneContext : IFBCloneContext
    {
        public IDbSet<Answer> Answers { get; set; }
        public IDbSet<AnswerDraft> AnswerDrafts { get; set; }
        public IDbSet<ApplicationSetting> ApplicationSettings { get; set; }
        public IDbSet<AspNetRole> AspNetRoles { get; set; }
        public IDbSet<AspNetUser> AspNetUsers { get; set; }
        public IDbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public IDbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public IDbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public IDbSet<Category> Categories { get; set; }
        public IDbSet<City> Cities { get; set; }
        public IDbSet<Country> Countries { get; set; }
        public IDbSet<CreditCard> CreditCards { get; set; }
        public IDbSet<HubConnection> HubConnections { get; set; }
        public IDbSet<Invoice> Invoices { get; set; }
        public IDbSet<LineItem> LineItems { get; set; }
        public IDbSet<Location> Locations { get; set; }
        public IDbSet<Menu> Menus { get; set; }
        public IDbSet<MenuItem> MenuItems { get; set; }
        public IDbSet<MenuQrCode> MenuQrCodes { get; set; }
        public IDbSet<MenuSection> MenuSections { get; set; }
        public IDbSet<Order> Orders { get; set; }
        public IDbSet<OrderDetail> OrderDetails { get; set; }
        public IDbSet<Question> Questions { get; set; }
        public IDbSet<QuestionDraft> QuestionDrafts { get; set; }
        public IDbSet<QuestionResponse> QuestionResponses { get; set; }
        public IDbSet<QuestionResponseSet> QuestionResponseSets { get; set; }
        public IDbSet<StaffMember> StaffMembers { get; set; }
        public IDbSet<StateProvince> StateProvinces { get; set; }
        public IDbSet<Subscription> Subscriptions { get; set; }
        public IDbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
        public IDbSet<SubscriptionPlanProperty> SubscriptionPlanProperties { get; set; }
        public IDbSet<Survey> Surveys { get; set; }

        public FakeFBCloneContext()
        {
            Answers = new FakeDbSet<Answer>();
            AnswerDrafts = new FakeDbSet<AnswerDraft>();
            ApplicationSettings = new FakeDbSet<ApplicationSetting>();
            AspNetRoles = new FakeDbSet<AspNetRole>();
            AspNetUsers = new FakeDbSet<AspNetUser>();
            AspNetUserClaims = new FakeDbSet<AspNetUserClaim>();
            AspNetUserLogins = new FakeDbSet<AspNetUserLogin>();
            AspNetUserRoles = new FakeDbSet<AspNetUserRole>();
            Categories = new FakeDbSet<Category>();
            Cities = new FakeDbSet<City>();
            Countries = new FakeDbSet<Country>();
            CreditCards = new FakeDbSet<CreditCard>();
            HubConnections = new FakeDbSet<HubConnection>();
            Invoices = new FakeDbSet<Invoice>();
            LineItems = new FakeDbSet<LineItem>();
            Locations = new FakeDbSet<Location>();
            Menus = new FakeDbSet<Menu>();
            MenuItems = new FakeDbSet<MenuItem>();
            MenuQrCodes = new FakeDbSet<MenuQrCode>();
            MenuSections = new FakeDbSet<MenuSection>();
            Orders = new FakeDbSet<Order>();
            OrderDetails = new FakeDbSet<OrderDetail>();
            Questions = new FakeDbSet<Question>();
            QuestionDrafts = new FakeDbSet<QuestionDraft>();
            QuestionResponses = new FakeDbSet<QuestionResponse>();
            QuestionResponseSets = new FakeDbSet<QuestionResponseSet>();
            StaffMembers = new FakeDbSet<StaffMember>();
            StateProvinces = new FakeDbSet<StateProvince>();
            Subscriptions = new FakeDbSet<Subscription>();
            SubscriptionPlans = new FakeDbSet<SubscriptionPlan>();
            SubscriptionPlanProperties = new FakeDbSet<SubscriptionPlanProperty>();
            Surveys = new FakeDbSet<Survey>();
        }

        public int SaveChanges()
        {
            return 0;
        }

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing)
        {
        }
        
        public void Dispose()
        {
            Dispose(true);
        }
        
        // Stored Procedures
        public List<RspQuestionResponseSetAvgScoreReturnModel> RspQuestionResponseSetAvgScore(Guid? userId, DateTime? beginDate, DateTime? endDate)
        {
            int procResult;
            return RspQuestionResponseSetAvgScore(userId, beginDate, endDate, out procResult);
        }

        public List<RspQuestionResponseSetAvgScoreReturnModel> RspQuestionResponseSetAvgScore(Guid? userId, DateTime? beginDate, DateTime? endDate, out int procResult)
        {
 
            procResult = 0;
            return new List<RspQuestionResponseSetAvgScoreReturnModel>();
        }

        public List<RspQuestionResponseSetAvgScoreByStaffMemberReturnModel> RspQuestionResponseSetAvgScoreByStaffMember(Guid? userId, DateTime? beginDate, DateTime? endDate)
        {
            int procResult;
            return RspQuestionResponseSetAvgScoreByStaffMember(userId, beginDate, endDate, out procResult);
        }

        public List<RspQuestionResponseSetAvgScoreByStaffMemberReturnModel> RspQuestionResponseSetAvgScoreByStaffMember(Guid? userId, DateTime? beginDate, DateTime? endDate, out int procResult)
        {
 
            procResult = 0;
            return new List<RspQuestionResponseSetAvgScoreByStaffMemberReturnModel>();
        }

        public List<RspQuestionResponseSetPositivitySpreadReturnModel> RspQuestionResponseSetPositivitySpread(Guid? userId, DateTime? beginDate, DateTime? endDate)
        {
            int procResult;
            return RspQuestionResponseSetPositivitySpread(userId, beginDate, endDate, out procResult);
        }

        public List<RspQuestionResponseSetPositivitySpreadReturnModel> RspQuestionResponseSetPositivitySpread(Guid? userId, DateTime? beginDate, DateTime? endDate, out int procResult)
        {
 
            procResult = 0;
            return new List<RspQuestionResponseSetPositivitySpreadReturnModel>();
        }

        public List<RspQuestionTextAvgScoreReturnModel> RspQuestionTextAvgScore(Guid? userId, DateTime? beginDate, DateTime? endDate)
        {
            int procResult;
            return RspQuestionTextAvgScore(userId, beginDate, endDate, out procResult);
        }

        public List<RspQuestionTextAvgScoreReturnModel> RspQuestionTextAvgScore(Guid? userId, DateTime? beginDate, DateTime? endDate, out int procResult)
        {
 
            procResult = 0;
            return new List<RspQuestionTextAvgScoreReturnModel>();
        }

        public int SpClearUserTestData(string email)
        {
 
            return 0;
        }

        public int SpCreateUser(string userid)
        {
 
            return 0;
        }

        public int SpDeleteUser(string email)
        {
 
            return 0;
        }

        public List<SpGetAnswersByQuestionIdReturnModel> SpGetAnswersByQuestionId(Guid? questionId, Guid? userid, bool? isDraft)
        {
            int procResult;
            return SpGetAnswersByQuestionId(questionId, userid, isDraft, out procResult);
        }

        public List<SpGetAnswersByQuestionIdReturnModel> SpGetAnswersByQuestionId(Guid? questionId, Guid? userid, bool? isDraft, out int procResult)
        {
 
            procResult = 0;
            return new List<SpGetAnswersByQuestionIdReturnModel>();
        }

        public List<SpGetLocationReviewsReturnModel> SpGetLocationReviews(Guid? locationId)
        {
            int procResult;
            return SpGetLocationReviews(locationId, out procResult);
        }

        public List<SpGetLocationReviewsReturnModel> SpGetLocationReviews(Guid? locationId, out int procResult)
        {
 
            procResult = 0;
            return new List<SpGetLocationReviewsReturnModel>();
        }

        public List<SpGetMenuItemReviewsReturnModel> SpGetMenuItemReviews(Guid? menuItemId)
        {
            int procResult;
            return SpGetMenuItemReviews(menuItemId, out procResult);
        }

        public List<SpGetMenuItemReviewsReturnModel> SpGetMenuItemReviews(Guid? menuItemId, out int procResult)
        {
 
            procResult = 0;
            return new List<SpGetMenuItemReviewsReturnModel>();
        }

        public int SpGetQuestionsBySurveyId(string surveyId, Guid? userid, bool? isDraft)
        {
 
            return 0;
        }

        public int SpPublishSurvey(Guid? surveyId)
        {
 
            return 0;
        }

        public int SpUserClaimLocationAndMenus(Guid? locationId, Guid? userId)
        {
 
            return 0;
        }

    }
}
