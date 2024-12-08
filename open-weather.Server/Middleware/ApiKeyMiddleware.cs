using System.Text.Json;

namespace open_weather.Server.Middleware
{
	public class ApiKeyMiddleware
	{
		private readonly RequestDelegate _requestDelegate;
		private readonly IConfiguration _configuration;

		public ApiKeyMiddleware(RequestDelegate requestDelegate, IConfiguration configuration)
		{
			_requestDelegate = requestDelegate;
			_configuration = configuration;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			// Get API key from the request
			string inputApiKey = context.Request.Headers["ApiKey"].FirstOrDefault() ?? "";

			// Get the valid keys from the appsettings config
			var validKeys = _configuration.GetSection("ApiKeys").Get<List<string>>() ?? [];

			// Check the inputted API key
			if (!string.IsNullOrEmpty(inputApiKey) && validKeys.Contains(inputApiKey))
			{
				// If the key is valid then proceed to the rate limiter check
				await _requestDelegate(context);
			}
			else
			{
				// Otherwise return a 401 unauthorised response if the key is invalid
				context.Response.StatusCode = StatusCodes.Status401Unauthorized;
				var response = new
				{
					error = true,
					message = $"Unauthorised: The API Key you have provided is not valid ({inputApiKey})"
				};
				await context.Response.WriteAsync(JsonSerializer.Serialize(response));
				return;
			}
			
		}
	}
}
