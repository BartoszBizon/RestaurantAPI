using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Models;

namespace RestaurantAPI.Presentation.Endpoints
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/accounts");

            group.MapPost("/register", async (IValidator<CreateUserDto> validator, IUserService userService, CreateUserDto dto) =>
            {
                var result = validator.Validate(dto);
                if (!result.IsValid) return Results.ValidationProblem(result.ToDictionary());

                await userService.RegisterUserAsync(dto);
                return Results.Created();
            });

            group.MapPost("/login", (IUserService userService, LoginDto dto) =>
            {
                string token = userService.GenerateJwt(dto);
                return Results.Ok(token);
            });
        }
    }
}