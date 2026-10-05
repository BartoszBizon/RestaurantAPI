using FluentValidation;
using RestaurantAPI.Application.Features.Users;
using RestaurantAPI.Application.Mediator;
using RestaurantAPI.Models;

namespace RestaurantAPI.Presentation.Endpoints
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/accounts");

            group.MapPost("/register", async (IValidator<CreateUserDto> validator, IMediator mediator, CreateUserDto dto) =>
            {
                var result = validator.Validate(dto);
                if (!result.IsValid) return Results.ValidationProblem(result.ToDictionary());

                await mediator.Send(new RegisterUserCommand(dto));
                return Results.Created();
            });

            group.MapPost("/login", async (IMediator mediator, LoginDto dto) =>
            {
                var token = await mediator.Send(new LoginCommand(dto));
                return Results.Ok(token);
            });
        }
    }
}
