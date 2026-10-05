using AutoMapper;
using RestaurantAPI.Application.Interfaces;
using RestaurantAPI.Application.Mediator;
using RestaurantAPI.Domain.Entities;

namespace RestaurantAPI.Application.Features.Dishes
{
    public class CreateDishCommandHandler : IRequestHandler<CreateDishCommand, int>
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IDishRepository _dishRepository;
        private readonly IMapper _mapper;

        public CreateDishCommandHandler(IRestaurantRepository restaurantRepository, IDishRepository dishRepository, IMapper mapper)
        {
            _restaurantRepository = restaurantRepository;
            _dishRepository = dishRepository;
            _mapper = mapper;
        }

        public Task<int> Handle(CreateDishCommand request, CancellationToken cancellationToken)
        {
            DishLookup.GetRestaurantOrThrow(_restaurantRepository, request.RestaurantId);

            var newDish = _mapper.Map<Dish>(request.Dto);
            newDish.RestaurantId = request.RestaurantId;
            _dishRepository.AddDishToDbContext(newDish);
            return Task.FromResult(newDish.Id);
        }
    }
}
