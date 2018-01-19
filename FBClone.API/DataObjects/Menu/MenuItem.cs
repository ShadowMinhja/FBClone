using Microsoft.WindowsAzure.Mobile.Service;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBClone.DataObjects
{
    public class MenuItem : EntityData
    {
        public string MenuSectionId { get; set; }
        public string ItemType { get; set; }
        public bool Required { get; set; }
        public string ItemText { get; set; }
        public string ItemDesc { get; set; }
        public double Price { get; set; }
        public string Active { get; set; }
        public string DaysOfWeek { get; set; }
        public int Sequence { get; set; }
        public string ItemImageURL { get; set; }
        public double? CL { get; set; }
        public double? ImageCL { get; set; }
        public string Source { get; set; }
        public string SourceType { get; set; }
        public double? FoodTotalScore { get; set; }
        public int? FoodTotalReviews { get; set; }
        public string UserId { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        [JsonIgnore]
        [ForeignKey("MenuSectionId")]
        public virtual MenuSection MenuSection { get; set; }
    }
}