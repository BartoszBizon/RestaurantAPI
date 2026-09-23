using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using RestaurantAPI.Application.Interfaces;
using RestaurantAPI.Domain.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public class DishService : IDishService
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IDishRepository _dishRepository;
        private readonly IMapper _mapper;

        public DishService(IRestaurantRepository restaurantRepository, IDishRepository dishRepository, IMapper mapper)
        {
            _restaurantRepository = restaurantRepository;
            _dishRepository = dishRepository;
            _mapper = mapper;
        }

        public int Create(int restaurantId, CreateDishDto dto)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var newDish = _mapper.Map<Dish>(dto);
            newDish.RestaurantId = restaurantId;
            _dishRepository.AddDishToDbContext(newDish);
            return newDish.Id;
        }

        public List<DishDto> GetAll(int restaurantId)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var dishDtos = _mapper.Map<List<DishDto>>(restaurant.Dishes);

            return dishDtos;
        }

        public DishDto GetById(int restaurantId, int dishId)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var dish = restaurant.Dishes.FirstOrDefault(x => x.Id == dishId);
            if (dish is null)
                throw new NotFoundException("Dish not found");

            var dishDto = _mapper.Map<DishDto>(dish);

            return dishDto;
        }

        public void RemoveAll(int restaurantId)
        {
            var restaurant = GetRestaurantById(restaurantId);
            _dishRepository.RemoveRangeDishesToDbContext(restaurant.Dishes);
        }

        public void RemoveById(int restaurantId, int dishId)
        {
            var restaurant = GetRestaurantById(restaurantId);
            var dishes = restaurant.Dishes;
            var dish = dishes.FirstOrDefault(x => x.Id == dishId);
            if (dish is null)
                throw new NotFoundException("Dish not found");

            _dishRepository.RemoveDishToDbContext(dish);
        }

        private Restaurant GetRestaurantById(int restaurantId)
        {
            var restaurant = _restaurantRepository.GetById(restaurantId);
            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            return restaurant;
        }
    }
}
