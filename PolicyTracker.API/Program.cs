using Microsoft.EntityFrameworkCore;
using PolicyTracker.API.Data;

var builder = WebApplication.CreateBuilder(args);

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