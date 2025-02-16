using AspNetCoreRateLimit;
using CompanyEmployees.Extensions;
using CompanyEmployees.Presentation.ActionFilters;
using CompanyEmployees.Utility;
using Contracts;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using NLog;
using Service.DataShaping;
using Shared.DataTransferObjects;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), @"\nlog.config"));

builder.Services.ConfigCors();
builder.Services.ConfigIISIntegration();
builder.Services.ConfigLoggerService();

builder.Services.AddMemoryCache(); // AspNetCoreRateLimit uses a memory cache to store its rules and counters.
builder.Services.ConfigRateLimiting();
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigResponseCaching();
builder.Services.ConfigHttpCacheHeaders();
builder.Services.ConfigVersioning();
builder.Services.AddScoped<IDataShaper<EmployeeDto>, DataShaper<EmployeeDto>>();
builder.Services.AddScoped<IEmployeeLinks, EmployeeLinks>();

builder.Services.ConfigRepositoryManager();
builder.Services.ConfigServiceManager();
builder.Services.ConfigSqlContext(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
	options.SuppressModelStateInvalidFilter = true; // Enable custome responces from actions.
});

builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.AddScoped<ValidateMediaTypeAttribute>();

builder.Services.AddControllers(config =>
{
	config.RespectBrowserAcceptHeader = true;
	config.ReturnHttpNotAcceptable = true; // 406 if media type supported
	config.InputFormatters.Insert(0, GetJsonPatchInputFormatter()); // JSON patch requests
	config.CacheProfiles.Add("60SecondsDuration", new CacheProfile { Duration = 60});
})
	.AddXmlDataContractSerializerFormatters()
	.AddCustomCsvFormatter() // Custom CSV formatter
	.AddApplicationPart(typeof(CompanyEmployees.Presentation.AssemblyReference).Assembly); // To use Controllers in Presentation project. (From main to presentation)

builder.Services.AddCustomMediaTypes(); // links.

builder.Services.AddAuthentication();
builder.Services.ConfigIdentity();

var app = builder.Build();

// Configure the HTTP request pipeline.
var logger = app.Services.GetRequiredService<ILoggerManager>();
app.ConfigExceptionHandler(logger);

/*if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}
else
{
	app.UseHsts();
}*/

if (app.Environment.IsProduction())
{
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
	ForwardedHeaders = ForwardedHeaders.All
});

app.UseIpRateLimiting();
app.UseCors("CorsPolicy");
app.UseResponseCaching(); // recommended after CORS
app.UseHttpCacheHeaders();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter() =>
	new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson()
	.Services.BuildServiceProvider()
	.GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters
	.OfType<NewtonsoftJsonPatchInputFormatter>().First();
