using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Tables;
using FBClone.DataObjects;

namespace FBClone.Models
{

    public class MobileServiceContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to alter your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
        //
        // To enable Entity Framework migrations in the cloud, please ensure that the 
        // service name, set by the 'MS_MobileServiceName' AppSettings in the local 
        // Web.config, is the same as the service name when hosted in Azure.

        private const string connectionStringName = "Name=MS_TableConnectionString";

        public MobileServiceContext() : base(connectionStringName)
        {
        }

        //public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<ApplicationSetting> ApplicationSettings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionResponse> QuestionResponses { get; set; }
        public DbSet<QuestionResponseSet> QuestionResponseSets { get; set; }
        public DbSet<StaffMember> StaffMembers { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuSection> MenuSections { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            string schema = "FBClone"; //ServiceSettingsDictionary.GetSchemaName(); --> Used for dynamic schema.
            if (!string.IsNullOrEmpty(schema))
            {
                modelBuilder.HasDefaultSchema(schema);
            }

            modelBuilder.Conventions.Add(
                new AttributeToColumnAnnotationConvention<TableColumnAttribute, string>(
                    "ServiceTableColumn", (property, attributes) => attributes.Single().ColumnType.ToString()));

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }

}
