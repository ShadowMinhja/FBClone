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
    // AspNetUserRoles
    internal partial class AspNetUserRoleMap : EntityTypeConfiguration<AspNetUserRole>
    {
        public AspNetUserRoleMap()
            : this("dbo")
        {
        }
 
        public AspNetUserRoleMap(string schema)
        {
            ToTable(schema + ".AspNetUserRoles");
            HasKey(x => new { x.UserId, x.RoleId });

            Property(x => x.UserId).HasColumnName("UserId").IsRequired().HasColumnType("nvarchar").HasMaxLength(128).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.RoleId).HasColumnName("RoleId").IsRequired().HasColumnType("nvarchar").HasMaxLength(128).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.SaasEcomUserId).HasColumnName("SaasEcomUser_Id").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);

            // Foreign keys
            HasRequired(a => a.AspNetRole).WithMany(b => b.AspNetUserRoles).HasForeignKey(c => c.RoleId); // FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
