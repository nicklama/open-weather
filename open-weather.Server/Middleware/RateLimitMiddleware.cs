using System.Collections.Concurrent;
using System.Net.NetworkInformation;
using System.Text.Json;

namespace open_weather.Server.Middleware
{
	public class RateLimitMiddleware
	{
		private static readonly ConcurrentDictionary<string, RequestInfo> _requestDictionary = new ConcurrentDictionary<string, RequestInfo>();
		private readonly RequestDelegate _requestDelegate;
		private readonly int _maxRequests;
		private readonly TimeSpan _requestWindow;

		public RateLimitMiddleware(RequestDelegate requestDelegate, int maxRequests, TimeSpan requestWindow)
		{
			_requestDelegate = requestDelegate;
			_maxRequests = maxRequests;
			_requestWindow = requestWindow;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			// Get API key from the request
			string inputApiKey = context.Request.Headers["ApiKey"].FirstOrDefault() ?? "";

			// Get the current number of requests for the provided API key
			var requestInfo = _requestDictionary.GetOrAdd(inputApiKey, new RequestInfo());

			// Filter out older requests to only check those that are within the request window
			requestInfo.Requests = requestInfo.Requests.Where(r => r > DateTime.UtcNow - _requestWindow).ToList();

			if (requestInfo.Requests.Count >= _maxRequests)
			{
				// Calculate the next time that endpoint can be called
				var nextAvailableRequest = requestInfo.Requests.First() + _requestWindow;
				// Return status code 429 due to too many requests
				context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
				var response = new
				{
					error = true,
					message = $"Too many requests. Please try again after {nextAvailableRequest.ToLocalTime()}."
				};
				await context.Response.WriteAsync(JsonSerializer.Serialize(response));
				return;
			}

			// Track the current request time
			requestInfo.Requests.Add(DateTime.UtcNow);

			// Proceed to the controller as the key has not exceeded its usage allowance
			await _requestDelegate(context);
		}

		private class RequestInfo
		{
			public List<DateTime> Requests { get; set; } = new List<DateTime>();
		}
	}
}
