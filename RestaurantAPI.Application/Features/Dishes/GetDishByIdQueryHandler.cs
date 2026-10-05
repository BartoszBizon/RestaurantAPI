using AutoMapper;
using RestaurantAPI.Application.Interfaces;
using RestaurantAPI.Application.Mediator;
using RestaurantAPI.Models;

namespace RestaurantAPI.Application.Features.Dishes
{
    public class GetDishByIdQueryHandler : IRequestHandler<GetDishByIdQuery, DishDto>
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMapper _mapper;

        public GetDishByIdQueryHandler(IRestaurantRepository restaurantRepository, IMapper mapper)
        {
            _restaurantRepository = restaurantRepository;
            _mapper = mapper;
        }

        public Task<DishDto> Handle(GetDishByIdQuery request, CancellationToken cancellationToken)
        {
            var restaurant = DishLookup.GetRestaurantOrThrow(_restaurantRepository, request.RestaurantId);
            var dish = DishLookup.GetDishOrThrow(restaurant, request.DishId);
            return Task.FromResult(_mapper.Map<DishDto>(dish));
        }
    }
}
