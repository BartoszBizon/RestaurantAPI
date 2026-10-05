using RestaurantAPI.Application.Interfaces;
using RestaurantAPI.Application.Mediator;

namespace RestaurantAPI.Application.Features.Dishes
{
    public class RemoveDishByIdCommandHandler : IRequestHandler<RemoveDishByIdCommand, Unit>
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IDishRepository _dishRepository;

        public RemoveDishByIdCommandHandler(IRestaurantRepository restaurantRepository, IDishRepository dishRepository)
        {
            _restaurantRepository = restaurantRepository;
            _dishRepository = dishRepository;
        }

        public Task<Unit> Handle(RemoveDishByIdCommand request, CancellationToken cancellationToken)
        {
            var restaurant = DishLookup.GetRestaurantOrThrow(_restaurantRepository, request.RestaurantId);
            var dish = DishLookup.GetDishOrThrow(restaurant, request.DishId);
            _dishRepository.RemoveDishToDbContext(dish);
            return Task.FromResult(Unit.Value);
        }
    }
}
