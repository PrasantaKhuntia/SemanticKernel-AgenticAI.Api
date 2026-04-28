using SemanticKernel_AgenticAI.Api.Core.Interfaces;
using SemanticKernel_AgenticAI.Api.Core.Models;
using SemanticKernel_AgenticAI.Api.Core.Services;
using SemanticKernel_AgenticAI.Api.Core.Planner;
using SemanticKernel_AgenticAI.Api.Core.Memory;
using SemanticKernel_AgenticAI.Api.Infrastructure;
using SemanticKernel_AgenticAI.Api.Plugins;
using SemanticKernel_AgenticAI.Api.Core.RAG;
using Microsoft.SemanticKernel;
using Microsoft.Extensions.AI;
using SemanticKernel_AgenticAI.Api.Core.VectorDB;

var builder = WebApplication.CreateBuilder(args);

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

// Load settings
var settings = builder.Configuration.GetSection("OpenAI").Get<OpenAISettings>();

// Kernel (Singleton)
builder.Services.AddSingleton(sp =>
{
    var kernel = KernelFactory.CreateKernel(settings);

    kernel.Plugins.AddFromObject(new WeatherPlugin());
    kernel.Plugins.AddFromObject(new ComparisonPlugin());

    return kernel;
});

// Memory (VERY IMPORTANT)
builder.Services.AddScoped<ChatContextBuilder>();

// Planner
builder.Services.AddScoped<AgentPlanner>();

// Agent
builder.Services.AddScoped<IAgentService, AgentService>();

// In-Memory Store
builder.Services.AddScoped<IRetriever, Retriever>();

// Vector Store
builder.Services.AddHttpClient<ChromaClient>(c =>
{
    c.BaseAddress = new Uri("http://localhost:8000");
});

builder.Services.AddScoped<DocumentIndexer>();

var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var indexer = scope.ServiceProvider.GetRequiredService<DocumentIndexer>();
//    await indexer.IndexAsync();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();