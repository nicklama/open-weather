using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using open_weather.Functions.Middleware;

var host = new HostBuilder()
	.ConfigureFunctionsWebApplication(workerApplication =>
	{
		// Register custom middlewares with the worker
		workerApplication.UseMiddleware<ApiKeyMiddleware>();
		workerApplication.UseMiddleware<RateLimitMiddleware>();
	})
	.ConfigureServices(services =>
	{
		services.AddApplicationInsightsTelemetryWorkerService();
		services.ConfigureFunctionsApplicationInsights();
		services.AddHttpClient();
	})
	.Build();

host.Run();
