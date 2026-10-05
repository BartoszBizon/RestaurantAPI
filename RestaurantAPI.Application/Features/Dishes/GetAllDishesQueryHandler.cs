using AutoMapper;
using RestaurantAPI.Application.Interfaces;
using RestaurantAPI.Application.Mediator;
using RestaurantAPI.Models;

namespace RestaurantAPI.Application.Features.Dishes
{
    public class GetAllDishesQueryHandler : IRequestHandler<GetAllDishesQuery, List<DishDto>>
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMapper _mapper;

        public GetAllDishesQueryHandler(IRestaurantRepository restaurantRepository, IMapper mapper)
        {
            _restaurantRepository = restaurantRepository;
            _mapper = mapper;
        }

        public Task<List<DishDto>> Handle(GetAllDishesQuery request, CancellationToken cancellationToken)
        {
            var restaurant = DishLookup.GetRestaurantOrThrow(_restaurantRepository, request.RestaurantId);
            return Task.FromResult(_mapper.Map<List<DishDto>>(restaurant.Dishes));
        }
    }
}
