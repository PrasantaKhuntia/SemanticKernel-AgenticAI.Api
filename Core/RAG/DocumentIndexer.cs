using Microsoft.SemanticKernel;
using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel.Embeddings;
using SemanticKernel_AgenticAI.Api.Core.VectorDB;

namespace SemanticKernel_AgenticAI.Api.Core.RAG
{
    public class DocumentIndexer
    {
        private readonly Kernel _kernel;
        private readonly ChromaClient _chroma;

        public DocumentIndexer(Kernel kernel, ChromaClient chroma)
        {
            _kernel = kernel;
            _chroma = chroma;
        }

        public async Task IndexAsync()
        {
            var embeddingService = _kernel.GetRequiredService<ITextEmbeddingGenerationService>();

            var collectionName = "weather";

            var collectionId = await _chroma.GetOrCreateCollectionAsync(collectionName);

            var docs = new List<string>
            {
                "Delhi has extreme summers with temperatures above 40°C.",
                "Pune has moderate climate with less humidity.",
                "Mumbai is humid due to coastal location."
            };

            var embeddings = new List<float[]>();

            foreach (var doc in docs)
            {
                var vector = await embeddingService.GenerateEmbeddingAsync(doc);
                embeddings.Add(vector.ToArray());
            }

            await _chroma.AddAsync(collectionId, docs, embeddings);
        }
    }
}
