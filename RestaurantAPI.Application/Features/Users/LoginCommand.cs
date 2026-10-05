using RestaurantAPI.Application.Mediator;
using RestaurantAPI.Models;

namespace RestaurantAPI.Application.Features.Users
{
    public record LoginCommand(LoginDto Dto) : IRequest<string>;
}
