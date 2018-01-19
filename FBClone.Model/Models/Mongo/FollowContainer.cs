using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBClone.Model
{
    public class FollowContainer
    {
        public string Source { get; set; }
        public string Target { get; set; }
        public bool IsFollowing { get; set; }
        public Actor Actor { get; set; }
    }
}
