using RestaurantAPI.Application.Mediator;
using RestaurantAPI.Models;

namespace RestaurantAPI.Application.Features.Restaurants
{
    public record UpdateRestaurantCommand(int Id, UpdateRestaurantDto Dto) : IRequest<Unit>;
}
