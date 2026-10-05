using RestaurantAPI.Application.Mediator;
using RestaurantAPI.Models;

namespace RestaurantAPI.Application.Features.Dishes
{
    public record GetAllDishesQuery(int RestaurantId) : IRequest<List<DishDto>>;
}
