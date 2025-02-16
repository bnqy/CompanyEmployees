﻿using AspNetCoreRateLimit;
using Contracts;
using LoggerService;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
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
				.AllowAnyHeader() // WithHeaders("accept", "content-type") for specific headers.
				.WithExposedHeaders("X-Pagination"); // Eneble client app to read new header.

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

	// CSV formatter register
	public static IMvcBuilder AddCustomCsvFormatter(this IMvcBuilder mvcBuilder)
	{
		return mvcBuilder.AddMvcOptions(config =>
		config.OutputFormatters.Add(new CsvOutputFormatter()));
	}

	// Custom media type
	public static void AddCustomMediaTypes(this IServiceCollection services)
	{
		services.Configure<MvcOptions>(config =>
		{
			var systemTextJsonOutputFormatter = config.OutputFormatters
			.OfType<SystemTextJsonOutputFormatter>()?.FirstOrDefault();

			if (systemTextJsonOutputFormatter != null)
			{
				systemTextJsonOutputFormatter.SupportedMediaTypes
				.Add("application/vnd.beneq.hateoas+json");

				systemTextJsonOutputFormatter.SupportedMediaTypes
				.Add("application/vnd.beneq.apiroot+json");
			}

			var xmlOutputFormatter = config.OutputFormatters
			.OfType<XmlDataContractSerializerOutputFormatter>()?.FirstOrDefault();

			if (xmlOutputFormatter != null)
			{
				xmlOutputFormatter.SupportedMediaTypes
				.Add("application/vnd.beneq.hateoas+xml");


				xmlOutputFormatter.SupportedMediaTypes
				.Add("application/vnd.beneq.apiroot+xml");
			}
		});
	}

	// Api versioning.
	public static void ConfigVersioning(this IServiceCollection services)
	{
		services.AddApiVersioning(opt =>
		{
			opt.ReportApiVersions = true; // Adds api version to responce header.
			opt.AssumeDefaultVersionWhenUnspecified = true;
			opt.DefaultApiVersion = new ApiVersion(1, 0);
			opt.ApiVersionReader = new HeaderApiVersionReader("api-version"); // Get version from header.
		});
	}

	// Response Caching
	public static void ConfigResponseCaching(this IServiceCollection services)
	{
		services.AddResponseCaching();
	}

	// Caching response headers
	public static void ConfigHttpCacheHeaders(this IServiceCollection services)
	{
		services.AddHttpCacheHeaders(
			(expirationOpt) =>
			{
				expirationOpt.MaxAge = 100;
				expirationOpt.CacheLocation = CacheLocation.Private;
			},
			(validationOpt) =>
			{
				validationOpt.MustRevalidate = true;
			});
	}

	// Config rate limiting
	public static void ConfigRateLimiting(this IServiceCollection services)
	{
		var rateLimitRules = new List<RateLimitRule>
		{
			new RateLimitRule
			{
				Endpoint = "*",
				Limit = 3,
				Period = "5m"
			}
		};

		services.Configure<IpRateLimitOptions>(opt =>
		{
			opt.GeneralRules = rateLimitRules;
		});

		services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
		services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
		services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
		services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
	}
}
