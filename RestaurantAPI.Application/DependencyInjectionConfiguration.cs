using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Models;
using RestaurantAPI.Models.Validators;
using RestaurantAPI.Services;

namespace RestaurantAPI.Application
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IRestaurantService, RestaurantService>();
            services.AddScoped<IDishService, DishService>();
            services.AddScoped<IUserService, UserService>();
            services.AddAutoMapper(typeof(RestaurantMappingProfile));
            services.AddScoped<IValidator<CreateUserDto>, CreateUserDtoValidator>();
            services.AddScoped<IValidator<RestaurantQuery>, RestaurantQueryValidator>();

            return services;
        }
    }
}