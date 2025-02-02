using Contracts;
using LoggerService;
using Microsoft.EntityFrameworkCore;
using Repository;
using Service;
using Service.Contracts;

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


	// To register Repository manager.
	public static void ConfigRepositoryManager(this IServiceCollection services) =>
		services.AddScoped<IRepositoryManager, RepositoryManager>();

	// To register Service manager.
	public static void ConfigServiceManager(this IServiceCollection services) =>
		services.AddScoped<IServiceManager, ServiceManager>();

	// Register RepositoryContext at a Runtime.
	public static void ConfigSqlContext(this IServiceCollection services, IConfiguration configuration) =>
		services.AddDbContext<RepositoryContext>(opts =>
		opts.UseSqlServer(configuration.GetConnectionString("sqlConnection")));
}
