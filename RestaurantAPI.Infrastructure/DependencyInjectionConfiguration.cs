using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using RestaurantAPI.Application.Interfaces;
using RestaurantAPI.Domain.Entities;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Repositories;
using RestaurantAPI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Infrastructure.Messaging;

namespace RestaurantAPI.Infrastructure
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMq"));
            services.AddHostedService<WelcomeEmailConsumer>();
            services.AddScoped<IMessagePublisher, RabbitMqPublisher>();
            services.AddScoped<IRestaurantRepository, RestaurantRepository>();
            services.AddScoped<IDishRepository, DishRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRestaurantSeeder, RestaurantSeeder>();
            services.AddScoped<IUserContextService, UserContextService>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<RestaurantDapperRepository>();
            services.AddScoped<IDbConnection>(sp =>
            new NpgsqlConnection(configuration.GetConnectionString("RestaurantDbConnecton")));

            services.AddDbContext<RestaurantDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("RestaurantDbConnecton")));
            services.AddHttpContextAccessor();
            return services;
        }
    }
}