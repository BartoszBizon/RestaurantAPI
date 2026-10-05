using RestaurantAPI.Application.Mediator;

namespace RestaurantAPI.Application.Features.Dishes
{
    public record RemoveDishByIdCommand(int RestaurantId, int DishId) : IRequest<Unit>;
}
