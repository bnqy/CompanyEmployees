using Contracts;
using LoggerService;

namespace CompanyEmployees.Extensions;

public static class ServiceExtensions
{
	// CORS - to restrict or give rights to apps from different domains
	public static void ConfigCors(this IServiceCollection services)
	{
		services.AddCors(opts =>
		{
			opts.AddPolicy("CorsPolicy", builder =>
			{
				builder.AllowAnyOrigin() // WithOrigins("https://example.com") for concerete origin
				.AllowAnyMethod() // WithMethods("POST", "GET") for specific methods
				.AllowAnyHeader(); // WithHeaders("accept", "content-type") for specific headers.
			});
		});
	}

	// To host on IIS
	public static void ConfigIISIntegration(this IServiceCollection services) => 
		services.Configure<IISOptions>(opts =>
		{

		});

	// Configuring Logger service for log messages.
	// Every time we want to use a logger service, all we need to do is to inject 
	// it into the constructor of the class that needs it. .NET Core will resolve
	// that service and the logging features will be available.
	public static void ConfigLoggerService(this IServiceCollection services) => 
		services.AddSingleton<ILoggerManager, LoggerManager>();
}
