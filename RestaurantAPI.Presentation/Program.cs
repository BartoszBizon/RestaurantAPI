//Create WebHost
using Microsoft.EntityFrameworkCore;
using RestaurantAPI;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Services;
using NLog.Web;
using RestaurantAPI.Presentation.ExceptionHandlers;
using Microsoft.AspNetCore.Identity;
using FluentValidation.AspNetCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RestaurantAPI.Authorization;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Npgsql;
using RestaurantAPI.Repositories;
using RestaurantAPI.Domain.Entities;
using RestaurantAPI.Infrastructure;
using RestaurantAPI.Application.Interfaces;
using RestaurantAPI.Application;
using RestaurantAPI.Presentation.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// NLog: Setup NLog for Dependency injection
builder.Logging.ClearProviders();
builder.Host.UseNLog();

// Add services to the container. // Dependency Injection

var authenticationSettings = new AuthenticationSettings();


builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);
builder.Services.AddSingleton(authenticationSettings);
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";

}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = authenticationSettings.JwtIssuer,
        ValidAudience = authenticationSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
    };
});

builder.Services.AddControllers();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("HasNationality", builder => builder.RequireClaim("Nationality", "Polish"));
    options.AddPolicy("Atleast20", builder => builder.AddRequirements(new MinimumAgeRequirement(20)));
});


builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);


builder.Services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
builder.Services.AddExceptionHandler<BadRequestExceptionHandler>();
builder.Services.AddExceptionHandler<ForbiddenExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddSwaggerGen();



builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontEndClient", builder =>
    builder
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowAnyOrigin());
});





var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>();
    seeder.Seed();
}



// Configure the HTTP request pipeline.
// Middleware


app.UseCors("FrontEndClient");
app.UseExceptionHandler();
app.UseAuthentication();
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestaurantAPI"));
}

app.UseAuthorization();

app.MapControllers();
app.MapUserEndpoints();

app.Run();


