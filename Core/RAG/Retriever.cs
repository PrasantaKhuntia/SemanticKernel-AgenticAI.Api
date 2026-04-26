namespace SemanticKernel_AgenticAI.Api.Core.RAG
{
    public class Retriever : IRetriever
    {
        private readonly InMemoryVectorStore _store;

        public Retriever(InMemoryVectorStore store)
        {
            _store = store;
        }

        public Task<List<string>> RetrieveAsync(string query)
        {
            var stopWords = new[] { "compare", "weather", "of", "and", "the" };

            var queryWords = query
                .ToLower()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Where(w => !stopWords.Contains(w))
                .ToList();

            var results = _store.GetAll()
                .Select(doc => new
                {
                    Doc = doc,
                    Score = queryWords.Count(word => doc.ToLower().Contains(word))
                })
                .Where(x => x.Score > 0)
                .OrderByDescending(x => x.Score) // 🔥 IMPORTANT
                .Take(3)
                .Select(x => x.Doc)
                .ToList();

            return Task.FromResult(results);
        }
    }
}
