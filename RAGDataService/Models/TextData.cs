using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace RAGDataService.Models
{
    public class TextData
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? Text { get; set; }
        public List<float>? Embedding { get; set; }
    }
}
