using RestaurantAPI.Application.Interfaces;
using RestaurantAPI.Application.Mediator;

namespace RestaurantAPI.Application.Features.Dishes
{
    public class RemoveAllDishesCommandHandler : IRequestHandler<RemoveAllDishesCommand, Unit>
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IDishRepository _dishRepository;

        public RemoveAllDishesCommandHandler(IRestaurantRepository restaurantRepository, IDishRepository dishRepository)
        {
            _restaurantRepository = restaurantRepository;
            _dishRepository = dishRepository;
        }

        public Task<Unit> Handle(RemoveAllDishesCommand request, CancellationToken cancellationToken)
        {
            var restaurant = DishLookup.GetRestaurantOrThrow(_restaurantRepository, request.RestaurantId);
            _dishRepository.RemoveRangeDishesToDbContext(restaurant.Dishes);
            return Task.FromResult(Unit.Value);
        }
    }
}
