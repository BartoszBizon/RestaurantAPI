using AutoMapper;
using RestaurantAPI.Application.Interfaces;
using RestaurantAPI.Application.Mediator;
using RestaurantAPI.Models;

namespace RestaurantAPI.Application.Features.Restaurants
{
    public class GetAllRestaurantsQueryHandler : IRequestHandler<GetAllRestaurantsQuery, PageResult<RestaurantDto>>
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMapper _mapper;

        public GetAllRestaurantsQueryHandler(IRestaurantRepository restaurantRepository, IMapper mapper)
        {
            _restaurantRepository = restaurantRepository;
            _mapper = mapper;
        }

        public Task<PageResult<RestaurantDto>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
        {
            var query = request.Query;
            var (items, totalCount) = _restaurantRepository.GetAllMatching(query);
            var restaurantsDto = _mapper.Map<List<RestaurantDto>>(items);
            var pageResult = new PageResult<RestaurantDto>(restaurantsDto, totalCount, query.PageSize, query.PageNumber);
            return Task.FromResult(pageResult);
        }
    }
}
