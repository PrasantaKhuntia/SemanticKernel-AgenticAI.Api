namespace SemanticKernel_AgenticAI.Api.Core.RAG
{
    public class InMemoryVectorStore
    {
        private readonly List<string> _documents = new()
        {
            "Delhi has extreme summers with temperatures above 40°C.",
            "Pune has moderate climate with less humidity.",
            "Mumbai is humid due to coastal location."
        };

        public List<string> GetAll() => _documents;
    }
}
