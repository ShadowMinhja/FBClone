using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBClone.Model
{
    public class Bite
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string StreamId { get; set; }
        public string UserId { get; set; }
        public string FoodType { get; set; }
        public string Comment { get; set; }
        public List<string> Images { get; set; }
        public Location Venue { get; set; }
        public MenuItem MenuItem { get; set; }
        public QuestionResponseSet QuestionResponseSet { get; set; }
        public string QuestionResponseSetId {get; set;}
        public List<string> FoodTags { get; set; }
        public List<string> AllergenTags { get; set; }
        public string Hashtags { get; set; }
        public Actor Actor { get; set; }
    }
}
