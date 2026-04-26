namespace SemanticKernel_AgenticAI.Api.Core.RAG
{
    public interface IRetriever
    {
        Task<List<string>> RetrieveAsync(string query);
    }
}
