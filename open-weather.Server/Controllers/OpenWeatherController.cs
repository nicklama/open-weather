using Microsoft.AspNetCore.Mvc;
using open_weather.Server.Services;

namespace open_weather.Server.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class OpenWeatherController : ControllerBase
	{
		private readonly OpenWeatherService _openWeatherService;

		public OpenWeatherController(OpenWeatherService openWeatherService)
		{
			_openWeatherService = openWeatherService;
		}

		[HttpGet(Name = "GetOpenWeather")]
		public async Task<IActionResult> GetWeather(string city, string country)
		{
			try
			{
				var weather = await _openWeatherService.GetOpenWeatherAsync(city, country);
				return Ok(weather);
			}
			catch (Exception ex)
			{
				var errorResponse = new
				{
					error = true,
					message = "An unexpected server error has occurred, please try again later.",
					details = ex.Message
				};
				// Return status code 500 for a generic internal server error
				return StatusCode(500, errorResponse);
			}
		}
	}
}
