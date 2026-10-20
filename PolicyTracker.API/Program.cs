using Microsoft.EntityFrameworkCore;
using PolicyTracker.API.Data;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// ---- LOGGING ----

var splunkToken = builder.Configuration["Splunk:Token"];

var loggerConfig = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "PolicyTracker.API")
    .WriteTo.Console();

if (!string.IsNullOrEmpty(splunkToken))
{
    loggerConfig.WriteTo.EventCollector(
        "http://localhost:8088/services/collector",
        splunkToken);
}

Log.Logger = loggerConfig.CreateLogger();
builder.Host.UseSerilog();

// ---- SERVICES ----

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PolicyTrackerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PolicyTrackerDb")));

var app = builder.Build();

// ---- MIDDLEWARE ----

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();