using Microsoft.AspNetCore.Mvc;
using RAGDataService.Services;
using RAGDataService.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RAGDataService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RAGController : ControllerBase
    {
        private readonly EmbeddingService _embedService;
        private readonly MongoService _mongoService;

        public RAGController(EmbeddingService embedService, MongoService mongoService)
        {
            _embedService = embedService;
            _mongoService = mongoService;
        }

        [HttpPost("datasync")]
        public async Task<IActionResult> DataSync([FromBody] string text)
        {
            var embedding = await _embedService.GetEmbeddingAsync(text);
            _mongoService.Insert(new TextData { Text = text, Embedding = embedding });
            return Ok("Data synced successfully");
        }

        [HttpPost("datasync/bulk")]
        public async Task<IActionResult> DataSyncBulk([FromBody] List<string> texts)
        {
            foreach (var text in texts)
            {
                var embedding = await _embedService.GetEmbeddingAsync(text);
                _mongoService.Insert(new TextData { Text = text, Embedding = embedding });
            }

            return Ok(new { message = $"{texts.Count} entries added successfully" });
        }

        [HttpPost("dataquery")]
        public async Task<IActionResult> DataQuery([FromBody] string query)
        {
            var queryEmb = await _embedService.GetEmbeddingAsync(query);
            var docs = _mongoService.GetAll();

            var results = docs.Select(d => new QueryResult
            {
                Text = d.Text,
                Score = MongoService.CosineSimilarity(d.Embedding, queryEmb)
            })
            .OrderByDescending(r => r.Score)
            .Take(3)
            .ToList();

            return Ok(results);
        }
    }
}
