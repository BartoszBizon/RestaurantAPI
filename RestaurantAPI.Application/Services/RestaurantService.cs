using AutoMapper;
using RestaurantAPI.Application.Interfaces;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMapper _mapper;

        public RestaurantService(IRestaurantRepository restaurantRepository, IMapper mapper)
        {
            _restaurantRepository = restaurantRepository;
            _mapper = mapper;
        }

        public async IAsyncEnumerable<RestaurantDto> GetRestaurantsByStream()
        {
            await foreach (var restaurant in _restaurantRepository.GetStreamAllRestaurants())
            {
                var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);
                await Task.Delay(1000);
                yield return restaurantDto;
            }
        }
    }
}
