using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FBClone.Model
{
    [MetadataType(typeof(StaffMemberMetadata))]
    public partial class StaffMember
    {
        private sealed class StaffMemberMetadata
        {
            [Required(ErrorMessage = "*")]
            [Display(Name = "Gender")]
            public string Gender { get; set; }
            [Required(ErrorMessage = "*")]
            [Display(Name = "Staff Member Name")]
            public string Name { get; set; }
            public string UserId { get; set; }
            //[Required(ErrorMessage = "*")]
            [Display(Name = "Passcode")]
            public string Passcode { get; set; }
            //[Required(ErrorMessage = "*")]
            //public string userType { get; set; }
            [JsonIgnore]
            public ICollection<QuestionResponseSet> QuestionResponseSets { get; set; }
        }
    }
}