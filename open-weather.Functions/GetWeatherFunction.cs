using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using open_weather.Server.Models;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Web;

namespace open_weather.Functions
{
    public class GetWeatherFunction
    {
		private readonly HttpClient _httpClient;
		private readonly IConfiguration _configuration;
		private readonly JsonSerializerOptions _jsonOptions;
		private readonly ILogger<GetWeatherFunction> _logger;

        public GetWeatherFunction(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<GetWeatherFunction> logger)
        {
			_httpClient = httpClientFactory.CreateClient();
			_configuration = configuration;
			_jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
			_logger = logger;
        }

        [Function("GetWeather")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "weather")] HttpRequestData req)
        {
			// Parse the http query
			var query = HttpUtility.ParseQueryString(req.Url.Query);
			string? city = query["city"];
			string? country = query["country"];

			var response = req.CreateResponse();

			if (string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(country))
			{
				response.StatusCode = HttpStatusCode.BadRequest;
				await response.WriteAsJsonAsync(new
				{
					error = true,
					message = "Missing city or country query parameters."
				});
				return response;
			}

			try
			{
				// Call the OpenWeather API
				string apiKey = _configuration["OpenWeatherApiKey"] ?? "";
				string url = $"https://api.openweathermap.org/data/2.5/weather?q={city},{country}&appid={apiKey}";
				var apiResponse = await _httpClient.GetAsync(url);

				if (!apiResponse.IsSuccessStatusCode)
				{
					if (apiResponse.StatusCode == HttpStatusCode.NotFound)
					{
						response.StatusCode = HttpStatusCode.NotFound;
						await response.WriteAsJsonAsync(new
						{
							error = true,
							message = "Sorry I could not find that location. Please check your spelling and try again."
						});
						return response;
					}
					response.StatusCode = HttpStatusCode.BadGateway;
					await response.WriteAsJsonAsync(new
					{
						error = true,
						message = $"Error from OpenWeather: {apiResponse.ReasonPhrase}"
					});
					return response;
				}

				// Deserialize the json response into an OpenWeatherData object
				var jsonData = await apiResponse.Content.ReadAsStringAsync();
				var weatherData = JsonSerializer.Deserialize<OpenWeatherData>(jsonData, _jsonOptions);

				var result = new Weather
				{
					Description = weatherData?.Weather[0].Description ?? "Unavailable"
				};

				response.StatusCode = HttpStatusCode.OK;
				await response.WriteAsJsonAsync(result);
				return response;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error fetching weather data");
				response.StatusCode = HttpStatusCode.InternalServerError;
				await response.WriteAsJsonAsync(new
				{
					error = true,
					message = "Something went wrong while processing your request."
				});
				return response;
			}
        }
    }
}
