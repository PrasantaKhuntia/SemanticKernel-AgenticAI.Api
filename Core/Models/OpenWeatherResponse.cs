using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticKernel_AgenticAI.Api.Core.Models
{
    public class OpenWeatherResponse
    {
        public MainInfo Main { get; set; }
        public List<WeatherInfo> Weather { get; set; }
        public string Name { get; set; }
    }

    public class MainInfo
    {
        public double Temp { get; set; }
    }

    public class WeatherInfo
    {
        public string Main { get; set; }
    }
}
