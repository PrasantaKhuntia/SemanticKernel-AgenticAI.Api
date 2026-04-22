namespace SemanticKernel_AgenticAI.Api.Core.Models
{
    public class AgentResponse
    {
        public string Response { get; set; } = string.Empty;
        public long ExecutionTimeMs { get; set; }
    }
}
