using Microsoft.SemanticKernel.ChatCompletion;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticKernel_AgenticAI.Api.Core.Memory
{
    public class ChatContextBuilder
    {
        private readonly ChatHistory _history = new();

        public ChatHistory Build(string userInput, string context)
        {
            var history = new ChatHistory();

            history.AddSystemMessage(@"
                You are an AI assistant.

                STRICT RULES:
                1. For real-time weather → ALWAYS use WeatherPlugin
                2. For general knowledge (climate etc.) → MUST use provided context
                3. If context = NO_CONTEXT → say 'I don’t have enough data'
                4. DO NOT answer from your own knowledge
                ");

            if (context != "NO_CONTEXT")
            {
                history.AddSystemMessage($@"Context:{context}");
            }

            history.AddUserMessage(userInput);

            return history;
        }

        public void AddAssistantMessage(string response)
        {
            _history.AddAssistantMessage(response);
        }
    }
}
