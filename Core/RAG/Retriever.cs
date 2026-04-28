namespace SemanticKernel_AgenticAI.Api.Core.RAG
{
    using Microsoft.SemanticKernel;
    using Microsoft.Extensions.AI;
    using Microsoft.SemanticKernel.Embeddings;
    using SemanticKernel_AgenticAI.Api.Core.VectorDB;

    public class Retriever : IRetriever
    {
        private readonly Kernel _kernel;
        private readonly ChromaClient _chroma;

        private const string COLLECTION_NAME = "weather";

        public Retriever(Kernel kernel, ChromaClient chroma)
        {
            _kernel = kernel;
            _chroma = chroma;
        }

        public async Task<List<string>> RetrieveAsync(string query)
        {
            // 1. Get embedding service
            var embeddingService = _kernel.GetRequiredService<ITextEmbeddingGenerationService>();

            // 2. Convert query → embedding
            var embedding = await embeddingService.GenerateEmbeddingAsync(query);

            // 3. Get collection ID
            var collectionId = await _chroma.GetOrCreateCollectionAsync(COLLECTION_NAME);

            // 4. Query Chroma
            var results = await _chroma.QueryAsync(collectionId, embedding.ToArray());

            return results;
        }
    }
}
