using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticKernel_AgenticAI.Api.Core.Models
{
    public class WeatherResult
    {
        public double Temperature { get; set; }
        public string Condition { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
    }
}
