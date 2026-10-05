using RestaurantAPI.Application.Mediator;
using RestaurantAPI.Models;

namespace RestaurantAPI.Application.Features.Dishes
{
    public record CreateDishCommand(int RestaurantId, CreateDishDto Dto) : IRequest<int>;
}
