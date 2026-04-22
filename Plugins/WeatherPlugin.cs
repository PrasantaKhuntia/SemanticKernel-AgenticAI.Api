using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using SemanticKernel_AgenticAI.Api.Core.Models;

namespace SemanticKernel_AgenticAI.Api.Plugins
{
    public class WeatherPlugin
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiKey = "";

        [KernelFunction]
        [Description("Get current weather including temperature in Celsius and condition")]
        public async Task<WeatherResult?> GetWeather(string city)
        {
            try
            {
                var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={_apiKey}&units=metric";

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return null;

                var data = await response.Content.ReadFromJsonAsync<OpenWeatherResponse>();

                if (data == null)
                    return null;

                return new WeatherResult
                {
                    City = data.Name,
                    Temperature = data.Main.Temp,
                    Condition = data.Weather.FirstOrDefault()?.Main ?? "Unknown"
                };
            }
            catch
            {
                return null;
            }
        }
    }
}
