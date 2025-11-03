using MongoDB.Driver;
using RAGDataService.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace RAGDataService.Services
{
    public class MongoService
    {
        private readonly IMongoCollection<TextData> _collection;

        public MongoService(IConfiguration configuration)
        {
            var connectionString = configuration["MongoDB:ConnectionString"];
            var databaseName = configuration["MongoDB:DatabaseName"] ?? "RAG_DB";
            
            var client = new MongoClient(connectionString);
            var db = client.GetDatabase(databaseName);
            _collection = db.GetCollection<TextData>("Texts");
        }

        public void Insert(TextData data) => _collection.InsertOne(data);

        public List<TextData> GetAll() => _collection.Find(_ => true).ToList();

        public static double CosineSimilarity(List<float> a, List<float> b)
        {
            double dot = 0, normA = 0, normB = 0;
            for (int i = 0; i < a.Count; i++)
            {
                dot += a[i] * b[i];
                normA += a[i] * a[i];
                normB += b[i] * b[i];
            }
            return dot / (System.Math.Sqrt(normA) * System.Math.Sqrt(normB));
        }
    }
}
