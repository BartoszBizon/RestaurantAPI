using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Authorization;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;
        private readonly IAuthorizationService _authorizationService;

        private readonly IUserContextService _userContextService;

        public RestaurantService(RestaurantDbContext dbContext, IMapper mapper, ILogger<RestaurantService> logger,
        IAuthorizationService authorizationService, IUserContextService userContextService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
        }

        public PageResult<RestaurantDto> GetAllRestaurants(RestaurantQuery query)
        {
            var baseQuery = _dbContext.Restaurants
            .Include(x => x.Address)
            .Include(x => x.Dishes)
            .Where(x => query.SearchPhrase == null ||
            (x.Name.ToLower().Contains(query.SearchPhrase.ToLower()) ||
            x.Description.ToLower().Contains(query.SearchPhrase.ToLower())));

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var columnsSelector = new Dictionary<string, Expression<Func<Restaurant, object>>>
                {
                  {nameof(Restaurant.Name), x=>x.Name},
                  {nameof(Restaurant.Description), x=>x.Description},
                  {nameof(Restaurant.Category), x=>x.Category},
                };
                
                var selectedColumn = columnsSelector[query.SortBy];

                baseQuery = query.SortDirection == SortDirection.ASC
                ? baseQuery.OrderBy(x => x.Name)
                : baseQuery.OrderByDescending(x => x.Name);
            }

            var totalItemsCount = baseQuery.Count();
            var restaurants = baseQuery
            .Skip(query.PageSize * (query.PageNumber - 1))
            .Take(query.PageSize)
            .ToList();

            var restaurantsDto = _mapper.Map<List<RestaurantDto>>(restaurants);
            var pageResult = new PageResult<RestaurantDto>(restaurantsDto, totalItemsCount, query.PageSize, query.PageNumber);
            return pageResult;
        }

        public RestaurantDto GetRestaurantWithID(int id)
        {
            var restaurant = _dbContext.Restaurants.Where(x => x.Id == id)
            .Include(x => x.Address)
            .Include(x => x.Dishes)
            .FirstOrDefault();

            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);

            return restaurantDto;
        }


        public int CreateRestaurant(CreateRestaurantDto dto)
        {
            var restaurant = _mapper.Map<Restaurant>(dto);
            restaurant.CreatedById = _userContextService.GetUserId;
            _dbContext.Restaurants.Add(restaurant);
            _dbContext.SaveChanges();

            return restaurant.Id;
        }

        public void DeleteRestaurant(int id)
        {
            _logger.LogError($"Restaurant with id {id} invoked");

            var restaurant = _dbContext.Restaurants.FirstOrDefault(x => x.Id == id);
            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            var user = _userContextService.User;
            var authorizationResult = _authorizationService
            .AuthorizeAsync(user, restaurant, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidenException("Athorization Fault");
            }


            _dbContext.Remove(restaurant);
            _dbContext.SaveChanges();
        }

        public void UpdateRestaurant(int id, UpdateRestaurantDto dto)
        {
            var restaurant = _dbContext.Restaurants.FirstOrDefault(x => x.Id == id);
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
            _dbContext.SaveChanges();
        }
    }
}