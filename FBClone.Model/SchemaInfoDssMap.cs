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
    // schema_info_dss
    internal partial class SchemaInfoDssMap : EntityTypeConfiguration<SchemaInfoDss>
    {
        public SchemaInfoDssMap()
            : this("DataSync")
        {
        }
 
        public SchemaInfoDssMap(string schema)
        {
            ToTable(schema + ".schema_info_dss");
            HasKey(x => new { x.SchemaMajorVersion, x.SchemaMinorVersion });

            Property(x => x.SchemaMajorVersion).HasColumnName("schema_major_version").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.SchemaMinorVersion).HasColumnName("schema_minor_version").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.SchemaExtendedInfo).HasColumnName("schema_extended_info").IsRequired().HasColumnType("nvarchar").HasMaxLength(100);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
