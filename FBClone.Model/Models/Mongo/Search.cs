using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBClone.Model
{
    public class Search
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string SearchText { get; set; }
        public string UserId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
