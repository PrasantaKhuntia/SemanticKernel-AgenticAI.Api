using Microsoft.AspNetCore.Mvc;
using SemanticKernel_AgenticAI.Api.Core.Interfaces;
using SemanticKernel_AgenticAI.Api.Core.Models;
using System.Diagnostics;

namespace SemanticKernel_AgenticAI.Api.Controllers
{   

    [ApiController]
    [Route("api/agent")]
    public class AgentController : ControllerBase
    {
        private readonly IAgentService _agent;

        public AgentController(IAgentService agent)
        {
            _agent = agent;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] AgentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Query))
                return BadRequest("Query is required");

            var sw = Stopwatch.StartNew();

            var result = await _agent.AskAsync(request.Query);

            sw.Stop();

            return Ok(new AgentResponse
            {
                Response = result,
                ExecutionTimeMs = sw.ElapsedMilliseconds
            });
        }
    }
}
