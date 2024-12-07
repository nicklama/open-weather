namespace open_weather.Server.Models
{
	public class OpenWeatherData
	{
		public required List<Weather> Weather { get; set; }	
	}
	public class Weather
	{
		public required string Description { get; set; }
	}
}
