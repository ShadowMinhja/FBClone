using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FBClone.Model
{
    [MetadataType(typeof(CategoryMetadata))]
    public partial class Category
    {
        private sealed class CategoryMetadata
        {
            [Display(Name = "Category")]
            [Required(ErrorMessage = "*")]
            public string Name { get; set; }
            public string UserId { get; set; }
            [JsonIgnore]
            public ICollection<Question> Questions { get; set; } // Questions.FK_FBClone.Questions_FBClone.Categories_CategoryId
            [JsonIgnore]
            public ICollection<QuestionDraft> QuestionDrafts { get; set; } // QuestionDrafts.FK_FBClone.QuestionDrafts_FBClone.Categories_CategoryId
        }

        public Category(string id, string name, string userid, DateTimeOffset createdAt, string createdBy, DateTimeOffset updatedAt, string updatedBy, byte[] version)
        {
            this.Id = id;
            this.Name = name;
            this.UserId = userid;
            this.CreatedAt = createdAt;
            this.CreatedBy = createdBy;
            this.UpdatedAt = updatedAt;
            this.UpdatedBy = updatedBy;
            this.Version = version;
        }

        public void UpdateFrom(Category category)
        {
            this.Name = category.Name;
            this.UserId = category.UserId;
            this.CreatedAt = category.CreatedAt;
            this.CreatedBy = category.CreatedBy;
            this.UpdatedAt = category.UpdatedAt;
            this.UpdatedAt = category.UpdatedAt;
            this.Version = category.Version;
        }
    }
}