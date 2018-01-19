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
    // Locations
    internal partial class LocationMap : EntityTypeConfiguration<Location>
    {
        public LocationMap()
            : this("FBClone")
        {
        }
 
        public LocationMap(string schema)
        {
            ToTable(schema + ".Locations");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasColumnType("nvarchar").HasMaxLength(128).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Name).HasColumnName("Name").IsOptional().HasColumnType("nvarchar");
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasColumnType("nvarchar");
            Property(x => x.Address).HasColumnName("Address").IsOptional().HasColumnType("nvarchar");
            Property(x => x.Address1).HasColumnName("Address1").IsOptional().HasColumnType("nvarchar");
            Property(x => x.Address2).HasColumnName("Address2").IsOptional().HasColumnType("nvarchar");
            Property(x => x.Locality).HasColumnName("Locality").IsOptional().HasColumnType("nvarchar");
            Property(x => x.Region).HasColumnName("Region").IsOptional().HasColumnType("nvarchar");
            Property(x => x.PostalCode).HasColumnName("PostalCode").IsOptional().HasColumnType("nvarchar");
            Property(x => x.Country).HasColumnName("Country").IsOptional().HasColumnType("nvarchar");
            Property(x => x.GeoLat).HasColumnName("GeoLat").IsOptional().HasColumnType("float");
            Property(x => x.GeoLng).HasColumnName("GeoLng").IsOptional().HasColumnType("float");
            Property(x => x.PlaceId).HasColumnName("PlaceId").IsOptional().HasColumnType("nvarchar");
            Property(x => x.Source).HasColumnName("Source").IsOptional().HasColumnType("nvarchar");
            Property(x => x.LocationImageUrl).HasColumnName("LocationImageUrl").IsOptional().HasColumnType("nvarchar");
            Property(x => x.WebsiteUrl).HasColumnName("WebsiteUrl").IsOptional().HasColumnType("nvarchar");
            Property(x => x.AmbienceTotalScore).HasColumnName("AmbienceTotalScore").IsOptional().HasColumnType("float");
            Property(x => x.AmbienceTotalReviews).HasColumnName("AmbienceTotalReviews").IsOptional().HasColumnType("int");
            Property(x => x.ServiceTotalScore).HasColumnName("ServiceTotalScore").IsOptional().HasColumnType("float");
            Property(x => x.ServiceTotalReviews).HasColumnName("ServiceTotalReviews").IsOptional().HasColumnType("int");
            Property(x => x.UserId).HasColumnName("UserId").IsOptional().HasColumnType("nvarchar");
            Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsOptional().HasColumnType("nvarchar");
            Property(x => x.UpdatedBy).HasColumnName("UpdatedBy").IsOptional().HasColumnType("nvarchar");
            Property(x => x.Version).HasColumnName("Version").IsRequired().IsFixedLength().HasColumnType("timestamp").HasMaxLength(8).IsRowVersion().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedAt).HasColumnName("CreatedAt").IsRequired().HasColumnType("datetimeoffset");
            Property(x => x.UpdatedAt).HasColumnName("UpdatedAt").IsOptional().HasColumnType("datetimeoffset");
            Property(x => x.Deleted).HasColumnName("Deleted").IsRequired().HasColumnType("bit");
            HasMany(t => t.Menus).WithMany(t => t.Locations).Map(m => 
            {
                m.ToTable("MenuLocations", "FBClone");
                m.MapLeftKey("Location_Id");
                m.MapRightKey("Menu_Id");
            });
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
