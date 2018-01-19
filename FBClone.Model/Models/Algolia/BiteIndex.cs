using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBClone.Model
{
    public class BiteIndex
    {
        public ObjectId Id { get; set; }
        public string UserId { get; set; }
        public MenuItem MenuItem { get; set; }
        public Survey Survey { get; set; }
    }
}
