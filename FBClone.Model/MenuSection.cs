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
    // MenuSections
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.14.3.0")]
    public partial class MenuSection
    {
        public string Id { get; set; } // Id (Primary key)
        public string MenuId { get; set; } // MenuId
        public string SectionTitle { get; set; } // SectionTitle
        public string SectionSubTitle { get; set; } // SectionSubTitle
        public string Active { get; set; } // Active
        public int Sequence { get; set; } // Sequence
        public string UserId { get; set; } // UserId
        public string CreatedBy { get; set; } // CreatedBy
        public string UpdatedBy { get; set; } // UpdatedBy
        public byte[] Version { get; set; } // Version
        public DateTimeOffset CreatedAt { get; set; } // CreatedAt
        public DateTimeOffset? UpdatedAt { get; set; } // UpdatedAt
        public bool Deleted { get; set; } // Deleted

        // Reverse navigation
        public virtual ICollection<MenuItem> MenuItems { get; set; } // MenuItems.FK_FBClone.MenuItems_FBClone.MenuSections_MenuSectionId

        // Foreign keys
        public virtual Menu Menu { get; set; } // FK_FBClone.MenuSections_FBClone.Menus_MenuId
        
        public MenuSection()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = System.DateTimeOffset.UtcNow;
            MenuItems = new List<MenuItem>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
