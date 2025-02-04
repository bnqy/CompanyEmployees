using CompanyEmployees.Extensions;
using Contracts;
using Microsoft.AspNetCore.HttpOverrides;
using NLog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), @"\nlog.config"));

builder.Services.ConfigCors();
builder.Services.ConfigIISIntegration();
builder.Services.ConfigLoggerService();
builder.Services.ConfigRepositoryManager();
builder.Services.ConfigServiceManager();
builder.Services.ConfigSqlContext(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddControllers(config =>
{
	config.RespectBrowserAcceptHeader = true;
})
	.AddXmlDataContractSerializerFormatters()
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
