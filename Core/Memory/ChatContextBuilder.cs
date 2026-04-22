using Microsoft.SemanticKernel.ChatCompletion;
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

        public ChatHistory Build(string input)
        {
            if (_history.Count == 0)
            {
                _history.AddSystemMessage(@"
                You are an AI assistant.

                Rules:
                - For ANY weather-related query, you MUST call available tools
                - NEVER answer from your own knowledge for weather
                - If tools fail, say you cannot fetch data

                Be precise and concise.
                ");
            }

            _history.AddUserMessage(input);

            return _history;
        }

        public void AddAssistantMessage(string response)
        {
            _history.AddAssistantMessage(response);
        }
    }
}
