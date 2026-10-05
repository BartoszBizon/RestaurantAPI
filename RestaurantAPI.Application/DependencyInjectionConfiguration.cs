using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RestaurantAPI.Application.Features.Dishes;
using RestaurantAPI.Application.Features.Restaurants;
using RestaurantAPI.Application.Features.Users;
using RestaurantAPI.Application.Mediator;
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
            services.AddScoped<IMediator, Mediator.Mediator>();

            services.AddScoped<IRequestHandler<GetAllRestaurantsQuery, PageResult<RestaurantDto>>, GetAllRestaurantsQueryHandler>();
            services.AddScoped<IRequestHandler<GetRestaurantByIdQuery, RestaurantDto>, GetRestaurantByIdQueryHandler>();
            services.AddScoped<IRequestHandler<CreateRestaurantCommand, int>, CreateRestaurantCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateRestaurantCommand, Unit>, UpdateRestaurantCommandHandler>();
            services.AddScoped<IRequestHandler<DeleteRestaurantCommand, Unit>, DeleteRestaurantCommandHandler>();

            services.AddScoped<IRequestHandler<CreateDishCommand, int>, CreateDishCommandHandler>();
            services.AddScoped<IRequestHandler<GetDishByIdQuery, DishDto>, GetDishByIdQueryHandler>();
            services.AddScoped<IRequestHandler<GetAllDishesQuery, List<DishDto>>, GetAllDishesQueryHandler>();
            services.AddScoped<IRequestHandler<RemoveAllDishesCommand, Unit>, RemoveAllDishesCommandHandler>();
            services.AddScoped<IRequestHandler<RemoveDishByIdCommand, Unit>, RemoveDishByIdCommandHandler>();

            services.AddScoped<IRequestHandler<RegisterUserCommand, Unit>, RegisterUserCommandHandler>();
            services.AddScoped<IRequestHandler<LoginCommand, string>, LoginCommandHandler>();

            services.AddScoped<IRestaurantService, RestaurantService>();
            services.AddAutoMapper(typeof(RestaurantMappingProfile));
            services.AddScoped<IValidator<CreateUserDto>, CreateUserDtoValidator>();
            services.AddScoped<IValidator<RestaurantQuery>, RestaurantQueryValidator>();

            return services;
        }
    }
}
