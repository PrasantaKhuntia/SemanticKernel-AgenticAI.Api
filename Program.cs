using SemanticKernel_AgenticAI.Api.Core.Interfaces;
using SemanticKernel_AgenticAI.Api.Core.Models;
using SemanticKernel_AgenticAI.Api.Core.Services;
using SemanticKernel_AgenticAI.Api.Core.Planner;
using SemanticKernel_AgenticAI.Api.Core.Memory;
using SemanticKernel_AgenticAI.Api.Infrastructure;
using SemanticKernel_AgenticAI.Api.Plugins;
using Microsoft.SemanticKernel;

var builder = WebApplication.CreateBuilder(args);

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();