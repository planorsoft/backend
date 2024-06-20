using Hangfire;
using Planor.Application;
using Planor.Infrastructure;
using Planor.Infrastructure.Persistence;
using Planor.WebUI;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebServices(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var myAllowedOrigins = "_myAllowedOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowedOrigins, policy =>
    {
        policy
            .SetIsOriginAllowedToAllowWildcardSubdomains()
            .WithOrigins("http://localhost:3030", "http://*.localhost:3030", "https://planorsoft.com", "https://*.planorsoft.com")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .Build();
    });
});

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    // app.UseHangfireDashboard();
    // app.MapHangfireDashboard();
}

app.UseSerilogRequestLogging();

app.UseCors(myAllowedOrigins);

app.InitialiseDatabase();

app.UseAuthorization();

app.MapControllers();

app.UseHealthChecks("/api/health");

app.Run();
