using System.Threading.RateLimiting;
using AuctoValue.Backend.Services;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Load the appsettings into the application
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Add services to the container
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register application services
builder.Services.AddScoped<IAuctionCalculatorService, AuctionCalculatorService>();

// Add controllers so MapControllers works
builder.Services.AddControllers();

// Configure CORS for the Vue.js frontend
var corsOrigins = builder.Configuration.GetSection("CorsOrigins").Get<string[]>() ?? throw new InvalidOperationException("CorsOrigins not configured in appsettings.json");
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueFrontend", policy =>
    {
        policy.WithOrigins(corsOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configure rate limiting
builder.Services.AddRateLimiter(options =>
{
    // Fixed window rate limiter: 20 requests per minute per IP address
    options.AddFixedWindowLimiter("fixed", limiterOptions =>
    {
        limiterOptions.PermitLimit = 200;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 10;
    });

    // Return 429 Too Many Requests when the rate limit is exceeded
    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await context.HttpContext.Response.WriteAsJsonAsync(
            new { error = "Too many requests. Please try again later." },
            cancellationToken: cancellationToken
        );
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowVueFrontend");

app.UseRateLimiter();

app.UseAuthorization();

app.MapControllers();

app.Run();