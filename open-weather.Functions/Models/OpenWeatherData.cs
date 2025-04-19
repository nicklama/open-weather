using System.Text.Json.Serialization;

namespace open_weather.Server.Models
{
	public class OpenWeatherData
	{
		public required List<Weather> Weather { get; set; }	
	}
	public class Weather
	{
		[JsonPropertyName("description")]
		public required string Description { get; set; }
	}
}
