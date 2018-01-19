using Microsoft.WindowsAzure.Mobile.Service;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FBClone.DataObjects
{
    public class Menu : EntityData
    {
        public string Description { get; set; }
        public string Active { get; set; }
        public ICollection<MenuSection> MenuSections { get; set; }
        public string UserId { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        [JsonIgnore]
        public virtual ICollection<Location> Locations { get; set; } // Many to many mapping
    }
}