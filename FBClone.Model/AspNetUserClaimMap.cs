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
    // AspNetUserClaims
    internal partial class AspNetUserClaimMap : EntityTypeConfiguration<AspNetUserClaim>
    {
        public AspNetUserClaimMap()
            : this("dbo")
        {
        }
 
        public AspNetUserClaimMap(string schema)
        {
            ToTable(schema + ".AspNetUserClaims");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.UserId).HasColumnName("UserId").IsRequired().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.ClaimType).HasColumnName("ClaimType").IsOptional().HasColumnType("nvarchar");
            Property(x => x.ClaimValue).HasColumnName("ClaimValue").IsOptional().HasColumnType("nvarchar");
            Property(x => x.SaasEcomUserId).HasColumnName("SaasEcomUser_Id").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
