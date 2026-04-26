using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using SemanticKernel_AgenticAI.Api.Core.Memory;
using SemanticKernel_AgenticAI.Api.Core.RAG;
using Microsoft.Extensions.Logging;

namespace SemanticKernel_AgenticAI.Api.Core.Planner
{   

    public class AgentPlanner
    {
        private readonly Kernel _kernel;
        private readonly ChatContextBuilder _chatContextBuilder;
        private readonly IRetriever _retriever;
        private readonly ILogger<AgentPlanner> _logger;

        public AgentPlanner(
            Kernel kernel,
            ChatContextBuilder chatContextBuilder,
            IRetriever retriever,
            ILogger<AgentPlanner> logger)
        {
            _kernel = kernel;
            _chatContextBuilder = chatContextBuilder;
            _retriever = retriever;
            _logger = logger;
        }

        public async Task<string> ExecuteAsync(string input)
        {
            var chatService = _kernel.GetRequiredService<IChatCompletionService>();

            //User query
            _logger.LogInformation("User Query: {Input}", input);

            // 1. Retrieve context
            var contextDocs = await _retriever.RetrieveAsync(input);

            var ragUsed = contextDocs.Any();
            var context = ragUsed
                ? string.Join("\n", contextDocs)
                : "NO_CONTEXT";

            _logger.LogInformation("RAG Used: {RagUsed}", ragUsed);

            if (ragUsed)
            {
                _logger.LogInformation("RAG Context: {Context}", context);
            }
            else
            {
                _logger.LogWarning("No RAG context retrieved");
            }

            // 2. Build chat with context
            var history = _chatContextBuilder.Build(input, context);

            // 3. Call LLM with tools enabled
            var response = await chatService.GetChatMessageContentAsync(
                history,
                new OpenAIPromptExecutionSettings
                {
                    ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
                },
                _kernel
            );

            // 4. Store memory
            _chatContextBuilder.AddAssistantMessage(response.Content);

            return response.Content;
        }
    }
}
