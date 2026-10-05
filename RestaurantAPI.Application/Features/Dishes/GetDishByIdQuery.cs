using RestaurantAPI.Application.Mediator;
using RestaurantAPI.Models;

namespace RestaurantAPI.Application.Features.Dishes
{
    public record GetDishByIdQuery(int RestaurantId, int DishId) : IRequest<DishDto>;
}
