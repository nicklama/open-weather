using open_weather.Server.Models;
using System.Text.Json;

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
			// Get API key from the environment vars
			string apiKey = _configuration["OpenWeather:ApiKey"] ?? "";
			string url = $"https://api.openweathermap.org/data/2.5/weather?q={city},{country}&appid={apiKey}";
			
			// Call the OpenWeather API
			var response = await _httpClient.GetAsync(url);

			if (!response.IsSuccessStatusCode)
			{
				// Provide more detail for an invalid location
				if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
				{
					throw new Exception($"Sorry I could not find that location. Please check your spelling and try again.");
				}
				// Otherwise provide the ReasonPhrase to the user
				throw new Exception($"Failed to get data from the OpenWeather API. Reason: {response.ReasonPhrase}");
			}

			var data = await response.Content.ReadAsStringAsync();

			// Deserialize the json response into an OpenWeatherData object
			var fullResponse = JsonSerializer.Deserialize<OpenWeatherData>(data, _jsonOptions);

			return new Weather
			{
				Description = fullResponse?.Weather[0].Description ?? "Error"
			};
		}
	}
}
