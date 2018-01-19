using Microsoft.WindowsAzure.Mobile.Service;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBClone.DataObjects
{
    public class MenuSection : EntityData
    {
        public string MenuId { get; set; }
        public string SectionTitle { get; set; }
        public string SectionSubTitle { get; set; }
        public string Active { get; set; }
        public int Sequence { get; set; }
        public ICollection<MenuItem> MenuItems { get; set; }
        public string UserId { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        [JsonIgnore]
        [ForeignKey("MenuId")]
        public virtual Menu Menu { get; set; }
    }
}