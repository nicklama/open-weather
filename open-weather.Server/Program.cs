using open_weather.Server.Middleware;
using open_weather.Server.Services;
using System.Text.Json;

namespace open_weather.Server
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			DotNetEnv.Env.Load();

			// Add services to the container.
			builder.Services.AddHttpClient<OpenWeatherService>(); // Register the OpenWeatherService
			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddSingleton(new JsonSerializerOptions
			{
				// Register JsonSerializerOptions and allow case insensitive json fields
				PropertyNameCaseInsensitive = true
			});
			builder.Configuration.AddEnvironmentVariables(); // Read .env vars

			var app = builder.Build();

			app.UseDefaultFiles();
			app.UseStaticFiles();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();
			app.UseMiddleware<ApiKeyMiddleware>(); // Register the API key authorisation Middleware
			app.UseMiddleware<RateLimitMiddleware>(5, TimeSpan.FromHours(1)); // Register the rate limiter middleware and restrict to 5 requests per hour

			app.MapControllers();

			app.MapFallbackToFile("/index.html");

			app.Run();
		}
	}
}
