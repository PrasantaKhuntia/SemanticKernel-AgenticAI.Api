using System.Net.Http.Json;
using System.Text.Json;

namespace SemanticKernel_AgenticAI.Api.Core.VectorDB
{   

    public class ChromaClient
    {
        private readonly HttpClient _http;

        public ChromaClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<string> GetOrCreateCollectionAsync(string name)
        {
            var collections = await GetCollectionsAsync();

            var existing = collections.FirstOrDefault(c => c.Name == name);

            if (existing != null)
            {
                return existing.Id;
            }

            // create new
            return await CreateCollectionAsync(name);
        }

        public async Task<List<ChromaCollectionResponse>> GetCollectionsAsync()
        {
            var response = await _http.GetAsync(
                "/api/v2/tenants/default_tenant/databases/default_database/collections");

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception(content);

            var result = System.Text.Json.JsonSerializer.Deserialize<List<ChromaCollectionResponse>>(
                            content,
                            new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });

            return result;
        }

        public async Task<string> CreateCollectionAsync(string name)
        {
            var response = await _http.PostAsJsonAsync(
                "/api/v2/tenants/default_tenant/databases/default_database/collections",
                new { name });

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception(content);

            var result = System.Text.Json.JsonSerializer.Deserialize<ChromaCollectionResponse>(content);

            return result.Id;
        }

        public async Task AddAsync(string collectionId, List<string> documents, List<float[]> embeddings)
        {
            var ids = documents.Select(d => d.GetHashCode().ToString()).ToList();

            var response = await _http.PostAsJsonAsync(
                $"/api/v2/tenants/default_tenant/databases/default_database/collections/{collectionId}/add",
                new
                {
                    ids = ids,
                    documents = documents,
                    embeddings = embeddings
                });

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception(content);
        }

        public async Task<List<string>> QueryAsync(string collectionId, float[] queryEmbedding)
        {
            var response = await _http.PostAsJsonAsync(
                $"/api/v2/tenants/default_tenant/databases/default_database/collections/{collectionId}/query",
                new
                {
                    query_embeddings = new[] { queryEmbedding },
                    n_results = 3,
                    include = new[] { "documents" }
                });

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception(content);

            var result = System.Text.Json.JsonSerializer.Deserialize<ChromaQueryResponse>(
                content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result?.documents?.FirstOrDefault() ?? new List<string>();
        }
    }

    public class ChromaCollectionResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class ChromaQueryResponse
    {
        public List<List<string>> documents { get; set; }
    }
}
