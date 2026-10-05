using RestaurantAPI.Application.Mediator;

namespace RestaurantAPI.Application.Features.Restaurants
{
    public record DeleteRestaurantCommand(int Id) : IRequest<Unit>;
}
