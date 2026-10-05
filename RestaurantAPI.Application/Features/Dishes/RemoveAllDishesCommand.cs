using RestaurantAPI.Application.Mediator;

namespace RestaurantAPI.Application.Features.Dishes
{
    public record RemoveAllDishesCommand(int RestaurantId) : IRequest<Unit>;
}
