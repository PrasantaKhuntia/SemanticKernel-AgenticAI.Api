using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;

namespace SemanticKernel_AgenticAI.Api.Plugins
{
    public class ComparisonPlugin
    {
        [KernelFunction]
        [Description("Compare temperatures of two cities and determine which is hotter")]
        public string CompareWeather(
            double temp1,
            double temp2,
            string city1,
            string city2,
            string condition1,
            string condition2)
        {
            var diff = Math.Abs(temp1 - temp2);

            var result = $"{city1}: {temp1:F1}°C ({condition1})\n" +
                         $"{city2}: {temp2:F1}°C ({condition2})\n\n";

            if (temp1 > temp2)
                result += $"{city1} is hotter by {diff:F1}°C";
            else if (temp2 > temp1)
                result += $"{city2} is hotter by {diff:F1}°C";
            else
                result += "Both cities have the same temperature";

            return result;
        }
    }
}
