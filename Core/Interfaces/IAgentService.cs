using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticKernel_AgenticAI.Api.Core.Interfaces
{
    public interface IAgentService
    {
        Task<string> AskAsync(string input);
    }
}
