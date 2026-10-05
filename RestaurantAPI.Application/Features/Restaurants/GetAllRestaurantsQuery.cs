using RestaurantAPI.Application.Mediator;
using RestaurantAPI.Models;

namespace RestaurantAPI.Application.Features.Restaurants
{
    public record GetAllRestaurantsQuery(RestaurantQuery Query) : IRequest<PageResult<RestaurantDto>>;
}
