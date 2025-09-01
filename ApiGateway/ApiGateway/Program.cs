using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.HttpLogging;
using Yarp.ReverseProxy;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// YARP
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// Rate Limiting (global politika)
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = 5;                // 5 istek
        opt.Window = TimeSpan.FromSeconds(10); // 10 sn pencerede
        opt.QueueLimit = 0;                 // kuyruk yok
    });

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

// (Opsiyonel) HTTP logging
builder.Services.AddHttpLogging(o =>
{
    o.LoggingFields = HttpLoggingFields.RequestPath
                    | HttpLoggingFields.RequestMethod
                    | HttpLoggingFields.ResponseStatusCode
                    | HttpLoggingFields.RequestHeaders
                    | HttpLoggingFields.ResponseHeaders;
    // Not: Body loglamak istersen geliştirme dışında önerilmez
});

// (Opsiyonel) CORS – frontend varsa aç
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("dev", p =>
        p.AllowAnyOrigin()
         .AllowAnyHeader()
         .AllowAnyMethod());
});

var app = builder.Build();

// middleware sırası
app.UseHttpLogging();    // opsiyonel
app.UseCors("dev");      // opsiyonel
app.UseRateLimiter();    // rate limiter aktif

// Health endpoint
app.MapGet("/healthz", () => Results.Ok(new { status = "ok" }));

// YARP – tüm rotalara rate limiting uygula
app.MapReverseProxy()
   .RequireRateLimiting("fixed");

app.Run();
