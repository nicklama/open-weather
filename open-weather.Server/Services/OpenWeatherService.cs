using open_weather.Server.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace open_weather.Server.Services
{
	public class OpenWeatherService
	{
		private readonly HttpClient _httpClient;
		private readonly IConfiguration _configuration;
		private readonly JsonSerializerOptions _jsonOptions;

		public OpenWeatherService(HttpClient httpClient, IConfiguration configuration, JsonSerializerOptions jsonOptions)
		{
			_httpClient = httpClient;
			_configuration = configuration;
			_jsonOptions = jsonOptions;
		}
		public async Task<Weather> GetOpenWeatherAsync(string city, string country)
		{
			string? apiKey = _configuration["OpenWeather:ApiKey"];
			string url = $"https://api.openweathermap.org/data/2.5/weather?q={city},{country}&appid={apiKey}";
			
			var response = await _httpClient.GetAsync(url);

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception($"Failed to get data from the Open Weather API: {response.ReasonPhrase}");
			}

			var data = await response.Content.ReadAsStringAsync();

			var fullResponse = JsonSerializer.Deserialize<OpenWeatherData>(data, _jsonOptions);

			return new Weather
			{
				Description = fullResponse?.Weather[0].Description ?? "Error"
			};
		}
	}
}
