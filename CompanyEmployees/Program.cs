using CompanyEmployees.Extensions;
using CompanyEmployees.Presentation.ActionFilters;
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

builder.Services.AddScoped<IDataShaper<EmployeeDto>, DataShaper<EmployeeDto>>();

builder.Services.ConfigRepositoryManager();
builder.Services.ConfigServiceManager();
builder.Services.ConfigSqlContext(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
	options.SuppressModelStateInvalidFilter = true; // Enable custome responces from actions.
});

builder.Services.AddScoped<ValidationFilterAttribute>();

builder.Services.AddControllers(config =>
{
	config.RespectBrowserAcceptHeader = true;
	config.ReturnHttpNotAcceptable = true; // 406 if media type supported
	config.InputFormatters.Insert(0, GetJsonPatchInputFormatter()); // JSON patch requests
})
	.AddXmlDataContractSerializerFormatters()
	.AddCustomCsvFormatter() // Custom CSV formatter
	.AddApplicationPart(typeof(CompanyEmployees.Presentation.AssemblyReference).Assembly); // To use Controllers in Presentation project. (From main to presentation)

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
app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();

NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter() =>
	new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson()
	.Services.BuildServiceProvider()
	.GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters
	.OfType<NewtonsoftJsonPatchInputFormatter>().First();
