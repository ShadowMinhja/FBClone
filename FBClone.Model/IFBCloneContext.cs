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
    public interface IFBCloneContext : IDisposable
    {
        IDbSet<Answer> Answers { get; set; } // Answers
        IDbSet<AnswerDraft> AnswerDrafts { get; set; } // AnswerDrafts
        IDbSet<ApplicationSetting> ApplicationSettings { get; set; } // ApplicationSettings
        IDbSet<AspNetRole> AspNetRoles { get; set; } // AspNetRoles
        IDbSet<AspNetUser> AspNetUsers { get; set; } // AspNetUsers
        IDbSet<AspNetUserClaim> AspNetUserClaims { get; set; } // AspNetUserClaims
        IDbSet<AspNetUserLogin> AspNetUserLogins { get; set; } // AspNetUserLogins
        IDbSet<AspNetUserRole> AspNetUserRoles { get; set; } // AspNetUserRoles
        IDbSet<Category> Categories { get; set; } // Categories
        IDbSet<City> Cities { get; set; } // Cities
        IDbSet<Country> Countries { get; set; } // Country
        IDbSet<CreditCard> CreditCards { get; set; } // CreditCards
        IDbSet<HubConnection> HubConnections { get; set; } // HubConnections
        IDbSet<Invoice> Invoices { get; set; } // Invoices
        IDbSet<LineItem> LineItems { get; set; } // LineItems
        IDbSet<Location> Locations { get; set; } // Locations
        IDbSet<Menu> Menus { get; set; } // Menus
        IDbSet<MenuItem> MenuItems { get; set; } // MenuItems
        IDbSet<MenuQrCode> MenuQrCodes { get; set; } // MenuQRCodes
        IDbSet<MenuSection> MenuSections { get; set; } // MenuSections
        IDbSet<Order> Orders { get; set; } // Orders
        IDbSet<OrderDetail> OrderDetails { get; set; } // OrderDetails
        IDbSet<Question> Questions { get; set; } // Questions
        IDbSet<QuestionDraft> QuestionDrafts { get; set; } // QuestionDrafts
        IDbSet<QuestionResponse> QuestionResponses { get; set; } // QuestionResponses
        IDbSet<QuestionResponseSet> QuestionResponseSets { get; set; } // QuestionResponseSets
        IDbSet<StaffMember> StaffMembers { get; set; } // StaffMembers
        IDbSet<StateProvince> StateProvinces { get; set; } // StateProvince
        IDbSet<Subscription> Subscriptions { get; set; } // Subscriptions
        IDbSet<SubscriptionPlan> SubscriptionPlans { get; set; } // SubscriptionPlans
        IDbSet<SubscriptionPlanProperty> SubscriptionPlanProperties { get; set; } // SubscriptionPlanProperties
        IDbSet<Survey> Surveys { get; set; } // Surveys

        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        
        // Stored Procedures
        List<RspQuestionResponseSetAvgScoreReturnModel> RspQuestionResponseSetAvgScore(Guid? userId, DateTime? beginDate, DateTime? endDate, out int procResult);
        List<RspQuestionResponseSetAvgScoreByStaffMemberReturnModel> RspQuestionResponseSetAvgScoreByStaffMember(Guid? userId, DateTime? beginDate, DateTime? endDate, out int procResult);
        List<RspQuestionResponseSetPositivitySpreadReturnModel> RspQuestionResponseSetPositivitySpread(Guid? userId, DateTime? beginDate, DateTime? endDate, out int procResult);
        List<RspQuestionTextAvgScoreReturnModel> RspQuestionTextAvgScore(Guid? userId, DateTime? beginDate, DateTime? endDate, out int procResult);
        int SpClearUserTestData(string email);
        int SpCreateUser(string userid);
        int SpDeleteUser(string email);
        List<SpGetAnswersByQuestionIdReturnModel> SpGetAnswersByQuestionId(Guid? questionId, Guid? userid, bool? isDraft, out int procResult);
        List<SpGetLocationReviewsReturnModel> SpGetLocationReviews(Guid? locationId, out int procResult);
        List<SpGetMenuItemReviewsReturnModel> SpGetMenuItemReviews(Guid? menuItemId, out int procResult);
        int SpGetQuestionsBySurveyId(string surveyId, Guid? userid, bool? isDraft);
        int SpPublishSurvey(Guid? surveyId);
        int SpUserClaimLocationAndMenus(Guid? locationId, Guid? userId);
    }

}
