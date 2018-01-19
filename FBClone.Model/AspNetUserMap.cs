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
    // AspNetUsers
    internal partial class AspNetUserMap : EntityTypeConfiguration<AspNetUser>
    {
        public AspNetUserMap()
            : this("dbo")
        {
        }
 
        public AspNetUserMap(string schema)
        {
            ToTable(schema + ".AspNetUsers");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasColumnType("nvarchar").HasMaxLength(128).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.FirstName).HasColumnName("FirstName").IsOptional().HasColumnType("nvarchar");
            Property(x => x.LastName).HasColumnName("LastName").IsOptional().HasColumnType("nvarchar");
            Property(x => x.OrganizationName).HasColumnName("OrganizationName").IsOptional().HasColumnType("nvarchar");
            Property(x => x.RegistrationDate).HasColumnName("RegistrationDate").IsRequired().HasColumnType("datetime");
            Property(x => x.LastLoginTime).HasColumnName("LastLoginTime").IsRequired().HasColumnType("datetime");
            Property(x => x.StripeCustomerId).HasColumnName("StripeCustomerId").IsOptional().HasColumnType("nvarchar");
            Property(x => x.IpAddress).HasColumnName("IPAddress").IsOptional().HasColumnType("nvarchar");
            Property(x => x.IpAddressCountry).HasColumnName("IPAddressCountry").IsOptional().HasColumnType("nvarchar");
            Property(x => x.Delinquent).HasColumnName("Delinquent").IsRequired().HasColumnType("bit");
            Property(x => x.LifetimeValue).HasColumnName("LifetimeValue").IsRequired().HasColumnType("decimal").HasPrecision(18,2);
            Property(x => x.Email).HasColumnName("Email").IsOptional().HasColumnType("nvarchar").HasMaxLength(256);
            Property(x => x.EmailConfirmed).HasColumnName("EmailConfirmed").IsRequired().HasColumnType("bit");
            Property(x => x.PasswordHash).HasColumnName("PasswordHash").IsOptional().HasColumnType("nvarchar");
            Property(x => x.SecurityStamp).HasColumnName("SecurityStamp").IsOptional().HasColumnType("nvarchar");
            Property(x => x.PhoneNumber).HasColumnName("PhoneNumber").IsOptional().HasColumnType("nvarchar");
            Property(x => x.PhoneNumberConfirmed).HasColumnName("PhoneNumberConfirmed").IsRequired().HasColumnType("bit");
            Property(x => x.TwoFactorEnabled).HasColumnName("TwoFactorEnabled").IsRequired().HasColumnType("bit");
            Property(x => x.LockoutEndDateUtc).HasColumnName("LockoutEndDateUtc").IsOptional().HasColumnType("datetime");
            Property(x => x.LockoutEnabled).HasColumnName("LockoutEnabled").IsRequired().HasColumnType("bit");
            Property(x => x.AccessFailedCount).HasColumnName("AccessFailedCount").IsRequired().HasColumnType("int");
            Property(x => x.UserName).HasColumnName("UserName").IsRequired().HasColumnType("nvarchar").HasMaxLength(256);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
