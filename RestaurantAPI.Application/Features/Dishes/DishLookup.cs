using RestaurantAPI.Application.Interfaces;
using RestaurantAPI.Domain.Entities;
using RestaurantAPI.Exceptions;

namespace RestaurantAPI.Application.Features.Dishes
{
    internal static class DishLookup
    {
        public static Restaurant GetRestaurantOrThrow(IRestaurantRepository restaurantRepository, int restaurantId)
        {
            var restaurant = restaurantRepository.GetById(restaurantId);
            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            return restaurant;
        }

        public static Dish GetDishOrThrow(Restaurant restaurant, int dishId)
        {
            var dish = restaurant.Dishes.FirstOrDefault(x => x.Id == dishId);
            if (dish is null)
                throw new NotFoundException("Dish not found");

            return dish;
        }
    }
}
