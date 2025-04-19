using Microsoft.AspNetCore.Components;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using System.Collections.Concurrent;
using System.Net.NetworkInformation;
using System.Text.Json;

namespace open_weather.Functions.Middleware
{
	public class RateLimitMiddleware : IFunctionsWorkerMiddleware
	{
		private static readonly ConcurrentDictionary<string, RequestInfo> _requestDictionary = new ConcurrentDictionary<string, RequestInfo>();
		private readonly int _maxRequests = 5;
		private readonly TimeSpan _requestWindow = TimeSpan.FromHours(1);

		public RateLimitMiddleware(/*int maxRequests, TimeSpan requestWindow*/)
		{
			//_maxRequests = maxRequests;
			//_requestWindow = requestWindow;
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

			// Get the current number of requests for the provided API key
			var requestInfo = _requestDictionary.GetOrAdd(inputApiKey, new RequestInfo());

			// Filter out older requests to only check those that are within the request window
			requestInfo.Requests = requestInfo.Requests.Where(r => r > DateTime.UtcNow - _requestWindow).ToList();

			if (requestInfo.Requests.Count >= _maxRequests)
			{
				// Calculate the next time that endpoint can be called
				var nextAvailableRequest = requestInfo.Requests.First() + _requestWindow;
				var timeRemaining = nextAvailableRequest - DateTime.UtcNow;
				// Return status code 429 due to too many requests
				var res = httpRequest.CreateResponse(System.Net.HttpStatusCode.TooManyRequests);
				await res.WriteAsJsonAsync(new
				{
					error = true,
					message = $"Geez I'm exhausted! Please try again in {timeRemaining.Minutes} minutes."
				});
				var invocationResult = context.GetInvocationResult();
				invocationResult.Value = res;
				return;
			}

			// Track the current request time
			requestInfo.Requests.Add(DateTime.UtcNow);

			// Proceed to the controller as the key has not exceeded its usage allowance
			await next(context);
		}

		private class RequestInfo
		{
			public List<DateTime> Requests { get; set; } = new List<DateTime>();
		}
	}
}
