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
    // scope_config_dss
    internal partial class ScopeConfigDssMap : EntityTypeConfiguration<ScopeConfigDss>
    {
        public ScopeConfigDssMap()
            : this("DataSync")
        {
        }
 
        public ScopeConfigDssMap(string schema)
        {
            ToTable(schema + ".scope_config_dss");
            HasKey(x => x.ConfigId);

            Property(x => x.ConfigId).HasColumnName("config_id").IsRequired().HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.ConfigData).HasColumnName("config_data").IsRequired().HasColumnType("xml");
            Property(x => x.ScopeStatus).HasColumnName("scope_status").IsOptional().IsFixedLength().IsUnicode(false).HasColumnType("char").HasMaxLength(1);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
