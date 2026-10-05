using RestaurantAPI.Application.Mediator;
using RestaurantAPI.Models;

namespace RestaurantAPI.Application.Features.Users
{
    public record RegisterUserCommand(CreateUserDto Dto) : IRequest<Unit>;
}
