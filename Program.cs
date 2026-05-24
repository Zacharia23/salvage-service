using System.Text.Json.Serialization;
using Asp.Versioning;
using EntityFramework.Exceptions.SqlServer;
using Hangfire;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.FeatureManagement;
using SalvageCore.Configuration;
using SalvageCore.Data;
using SalvageCore.Extensions;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Sinks.Grafana.Loki;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi("v1", options => { options.AddDocumentTransformer<BearerSecuritySchemeTransformer>(); });
builder.Services.AddGlobalErrorHandling();

var labelOption = new[]
{
    new LokiLabel { Key = "service", Value = "SmartSalvage" },
    new LokiLabel { Key = "environment", Value = builder.Environment.EnvironmentName }
};

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.WithProperty("Service", "SmartSalvage")
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.GrafanaLoki("http://144.91.108.100:3100", labelOption)
    .CreateLogger();

builder.Services.AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; });

builder.Services.AddDbContextPool<ApplicationDbContext>(option =>
{
    option
        .UseNpgsql(builder.Configuration.GetConnectionString("MainConnection"))
        .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning))
        .UseExceptionProcessor();
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
    options.InstanceName = builder.Configuration["RedisInstanceName"];
});

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = ApiVersion.Default;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader());
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddFeatureManagement();
builder.Services.AddProblemDetails();

builder.Services.AddCors(options =>
{
    options.AddPolicy("GEN_SERVER", policyBuilder =>
    {
        policyBuilder.WithOrigins("https://office.smartsalvagetz.com", "http://localhost:3000", "https://smartsalvagetz.com");
        policyBuilder.AllowAnyHeader();
        policyBuilder.AllowAnyMethod();
        policyBuilder.AllowCredentials();
    });
});

builder.Services.Configure<ForwardedHeadersOptions>(options => { options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost; });

builder.Host.UseSerilog();

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient();

var app = builder.Build();

await SeedData.EnsureSeedData(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Salvage Core APIs");
        options.AddPreferredSecuritySchemes("BearerAuth")
            .AddHttpAuthentication("Bearer", auth => { auth.Token = ""; }).EnablePersistentAuthentication();
    });
}

// Middlewares Section
app.UseSerilogRequestLogging();
app.UseStatusCodePages();
app.UseGlobalErrorHandling();
app.UseExceptionHandler();

app.UseCors("GEN_SERVER");

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseHangfireDashboard();
app.MapHangfireDashboard("/hangfire");

app.Run();