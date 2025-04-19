using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace open_weather.Functions.Middleware
{
	public class ApiKeyMiddleware : IFunctionsWorkerMiddleware
	{
		private readonly IConfiguration _configuration;

		public ApiKeyMiddleware(IConfiguration configuration)
		{
			_configuration = configuration;
		}

	public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
		{
			// Access request data from the context
			var httpRequest = await context.GetHttpRequestDataAsync();
			if (httpRequest == null)
			{
				await next(context); // Not an HTTP request
				return;
			}
			// Get API key from the request
			string? inputApiKey = httpRequest.Headers.TryGetValues("ApiKey", out var values)
			? values.FirstOrDefault()
			: "";

			// Get the valid phrase from the environment vars and generate 5 valid keys
			var validPhrase = _configuration["ApiKeyPhrase"] ?? "";
			var validKeys = Enumerable.Range(1, 5).Select(i => $"{validPhrase}{i}").ToArray();

			// Check the inputted API key
			if (!string.IsNullOrEmpty(inputApiKey) && validKeys.Contains(inputApiKey))
			{
				// If the key is valid then proceed to the rate limiter check
				await next(context);
			}
			else
			{
				// Otherwise return a 401 unauthorised response if the key is invalid
				var res = httpRequest.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
				await res.WriteAsJsonAsync(new
				{
					error = true,
					message = $"Unauthorised: The API Key you have provided is not valid ({inputApiKey})"
				});
				var invocationResult = context.GetInvocationResult();
				invocationResult.Value = res;
				return;
			}

		}
	}
}
