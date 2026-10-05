using RestaurantAPI.Application.Mediator;
using RestaurantAPI.Models;

namespace RestaurantAPI.Application.Features.Restaurants
{
    public record CreateRestaurantCommand(CreateRestaurantDto Dto) : IRequest<int>;
}
