using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.Products.Commands;
using ProductService.Core.Interfaces;
using ProductService.Infrastructure.Data;
using ProductService.Infrastructure.Messaging;
using ProductService.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
var builder = WebApplication.CreateBuilder(args);

// DbContext (PostgreSQL)
builder.Services.AddDbContext<ProductDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("ProductDb")));

// MediatR (Application assembly)
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<CreateProductCommand>());

// DI
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// RabbitMQ Publisher (basit kayıt)
builder.Services.AddSingleton<IMessagePublisher>(sp =>
{
    var cfg = builder.Configuration.GetSection("RabbitMq");
    return new RabbitMqPublisher(
        hostName: cfg["HostName"] ?? "localhost",
        exchange: cfg["Exchange"] ?? "product_exchange",
        userName: cfg["UserName"] ?? "guest",
        password: cfg["Password"] ?? "guest"
    );
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; // development için
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],       // AuthService ile aynı
            ValidAudience = builder.Configuration["Jwt:Audience"],   // AuthService ile aynı
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            ),
            RoleClaimType = "userType" // userType claim’i üzerinden rol doğrulaması yapacak
        };
    });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireClaim("userType", "admin"));
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
