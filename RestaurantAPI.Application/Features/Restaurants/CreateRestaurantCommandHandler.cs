using AutoMapper;
using RestaurantAPI.Application.Interfaces;
using RestaurantAPI.Application.Mediator;
using RestaurantAPI.Domain.Entities;
using RestaurantAPI.Interfaces;

namespace RestaurantAPI.Application.Features.Restaurants
{
    public class CreateRestaurantCommandHandler : IRequestHandler<CreateRestaurantCommand, int>
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;

        public CreateRestaurantCommandHandler(IRestaurantRepository restaurantRepository, IMapper mapper, IUserContextService userContextService)
        {
            _restaurantRepository = restaurantRepository;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
        {
            var restaurant = _mapper.Map<Restaurant>(request.Dto);
            restaurant.CreatedById = _userContextService.GetUserId;
            _restaurantRepository.AddRestaurantToDbContext(restaurant);
            return Task.FromResult(restaurant.Id);
        }
    }
}
