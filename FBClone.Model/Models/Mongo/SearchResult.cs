using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBClone.Model
{
    public class SearchResult
    {
        public string SearchText { get; set; }
        public string ResultText { get; set; }
        public string ResultUrl { get; set; }
        public bool Historical { get; set; }
    }
}
