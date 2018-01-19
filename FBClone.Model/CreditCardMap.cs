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
    // CreditCards
    internal partial class CreditCardMap : EntityTypeConfiguration<CreditCard>
    {
        public CreditCardMap()
            : this("dbo")
        {
        }
 
        public CreditCardMap(string schema)
        {
            ToTable(schema + ".CreditCards");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.StripeId).HasColumnName("StripeId").IsOptional().HasColumnType("nvarchar");
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasColumnType("nvarchar");
            Property(x => x.Last4).HasColumnName("Last4").IsOptional().HasColumnType("nvarchar");
            Property(x => x.Type).HasColumnName("Type").IsOptional().HasColumnType("nvarchar");
            Property(x => x.Fingerprint).HasColumnName("Fingerprint").IsOptional().HasColumnType("nvarchar");
            Property(x => x.AddressCity).HasColumnName("AddressCity").IsRequired().HasColumnType("nvarchar");
            Property(x => x.AddressCountry).HasColumnName("AddressCountry").IsRequired().HasColumnType("nvarchar");
            Property(x => x.AddressLine1).HasColumnName("AddressLine1").IsRequired().HasColumnType("nvarchar");
            Property(x => x.AddressLine2).HasColumnName("AddressLine2").IsOptional().HasColumnType("nvarchar");
            Property(x => x.AddressState).HasColumnName("AddressState").IsOptional().HasColumnType("nvarchar");
            Property(x => x.AddressZip).HasColumnName("AddressZip").IsRequired().HasColumnType("nvarchar");
            Property(x => x.Cvc).HasColumnName("Cvc").IsRequired().HasColumnType("nvarchar").HasMaxLength(4);
            Property(x => x.ExpirationMonth).HasColumnName("ExpirationMonth").IsRequired().HasColumnType("nvarchar");
            Property(x => x.ExpirationYear).HasColumnName("ExpirationYear").IsRequired().HasColumnType("nvarchar");
            Property(x => x.CardCountry).HasColumnName("CardCountry").IsOptional().HasColumnType("nvarchar");
            Property(x => x.SaasEcomUserId).HasColumnName("SaasEcomUserId").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
