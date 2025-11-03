using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RAGDataService.Services
{
    public class EmbeddingService
    {
        private readonly HttpClient _client;

        public EmbeddingService(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<float>> GetEmbeddingAsync(string text)
        {
            var payload = JsonSerializer.Serialize(new { texts = new[] { text } });
            var response = await _client.PostAsync("http://localhost:8000/embed",
                new StringContent(payload, Encoding.UTF8, "application/json"));
            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            var array = doc.RootElement.GetProperty("embeddings")[0]
                .EnumerateArray();
            var result = new List<float>();
            foreach (var val in array)
                result.Add(val.GetSingle());
            return result;
        }
    }
}
