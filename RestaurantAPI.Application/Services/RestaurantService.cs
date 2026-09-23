using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using RestaurantAPI.Application.Interfaces;
using RestaurantAPI.Authorization;
using RestaurantAPI.Domain.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;
        private readonly IAuthorizationService _authorizationService;

        private readonly IUserContextService _userContextService;
        private readonly IRestaurantRepository _restaurantRepository;

        public RestaurantService(IRestaurantRepository restaurantRepository,
         IMapper mapper, ILogger<RestaurantService> logger,
        IAuthorizationService authorizationService, IUserContextService userContextService)
        {
            _restaurantRepository = restaurantRepository;
            _mapper = mapper;
            _logger = logger;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
        }

        public PageResult<RestaurantDto> GetAllRestaurants(RestaurantQuery query)
        {
            var result = _restaurantRepository.GetAllMatching(query);
            var matchingRestaurants = result.Items;
            var totalItemsCount = result.TotalCount;
            var restaurantsDto = _mapper.Map<List<RestaurantDto>>(matchingRestaurants);
            var pageResult = new PageResult<RestaurantDto>(restaurantsDto, totalItemsCount, query.PageSize, query.PageNumber);
            return pageResult;
        }

        public RestaurantDto GetRestaurantWithID(int id)
        {
            var restaurant = _restaurantRepository.GetById(id);
            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);

            return restaurantDto;
        }


        public int CreateRestaurant(CreateRestaurantDto dto)
        {
            var restaurant = _mapper.Map<Restaurant>(dto);
            restaurant.CreatedById = _userContextService.GetUserId;
            _restaurantRepository.AddRestaurantToDbContext(restaurant);

            return restaurant.Id;
        }

        public void DeleteRestaurant(int id)
        {
            _logger.LogError($"Restaurant with id {id} invoked");

            var restaurant = _restaurantRepository.GetById(id);
            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            var user = _userContextService.User;
            var authorizationResult = _authorizationService
            .AuthorizeAsync(user, restaurant, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidenException("Athorization Fault");
            }

            _restaurantRepository.RemoveRestaurantToDbContext(restaurant);
        }

        public void UpdateRestaurant(int id, UpdateRestaurantDto dto)
        {
            var restaurant = _restaurantRepository.GetById(id);
            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            var user = _userContextService.User;
            var authorizationResult = _authorizationService
            .AuthorizeAsync(user, restaurant, new ResourceOperationRequirement(ResourceOperation.Update)).Result; ;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidenException("Athorization Fault");
            }

            restaurant.Name = dto.Name;
            restaurant.Description = dto.Description;
            restaurant.HasDelivery = dto.HasDelivery;
            _restaurantRepository.SaveChangesOnDbContext();
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