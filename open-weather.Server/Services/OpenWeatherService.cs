using open_weather.Server.Models;
using System.Text.Json;

namespace open_weather.Server.Services
{
	public class OpenWeatherService
	{
		private readonly HttpClient _httpClient;
		private readonly string _apiKey = "8b7535b42fe1c551f18028f64e8688f7";

		public OpenWeatherService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}
		public async Task<Weather> GetOpenWeatherAsync(string location)
		{
			string url = $"https://api.openweathermap.org/data/2.5/weather?q={location}&appid={_apiKey}";
			
			var response = await _httpClient.GetAsync(url);

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception("Failed to get weather data");
			}

			var data = await response.Content.ReadAsStringAsync();

			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			};
			var fullResponse = JsonSerializer.Deserialize<OpenWeatherData>(data,options);

			return new Weather
			{
				Description = fullResponse?.Weather[0].Description ?? "Error"
			};
		}
	}
}
