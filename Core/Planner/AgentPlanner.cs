using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SemanticKernel.ChatCompletion;
using SemanticKernel_AgenticAI.Api.Core.Memory;

namespace SemanticKernel_AgenticAI.Api.Core.Planner
{
    public class AgentPlanner
    {
        private readonly Kernel _kernel;
        private readonly ChatContextBuilder _chatContextBuilder;
        public AgentPlanner(Kernel kernel, ChatContextBuilder chatContextBuilder)
        {
            _kernel = kernel;
            _chatContextBuilder = chatContextBuilder;
        }

        public async Task<string> ExecuteAsync(string input)
        {
            var chatService = _kernel.GetRequiredService<IChatCompletionService>();

            var history = _chatContextBuilder.Build(input);

            var response = await chatService.GetChatMessageContentAsync(
                history,
                new OpenAIPromptExecutionSettings
                {
                    ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
                },
                _kernel
            );

            _chatContextBuilder.AddAssistantMessage(response.Content);

            return response.Content;
        }
    }
}
