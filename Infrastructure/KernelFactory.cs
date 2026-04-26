using Microsoft.SemanticKernel;
using SemanticKernel_AgenticAI.Api.Core.Models;

namespace SemanticKernel_AgenticAI.Api.Infrastructure
{
    public static class KernelFactory
    {
        public static Kernel CreateKernel(OpenAISettings settings)
        {
            var builder = Kernel.CreateBuilder();

            builder.Services.AddLogging(config =>
            {
                config.AddConsole();
                config.SetMinimumLevel(LogLevel.Information);
            });

            builder.AddOpenAIChatCompletion(
                modelId: settings.Model,
                apiKey: settings.ApiKey
            );

            var kernel = builder.Build();

            return kernel;
        }
    }
}
