using System.Collections.Generic;
using RestaurantAPI.Application.Interfaces;
using RestaurantAPI.Domain.Entities;

namespace RestaurantAPI.Infrastructure
{
    public class DishRepository : IDishRepository
    {
        private readonly RestaurantDbContext _dbContext;

        public DishRepository(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddDishToDbContext(Dish dish)
        {
            _dbContext.Dishes.Add(dish);
            _dbContext.SaveChanges();
        }

        public void RemoveDishToDbContext(Dish dish)
        {
            _dbContext.Dishes.Remove(dish);
            _dbContext.SaveChanges();
        }

        public void RemoveRangeDishesToDbContext(IEnumerable<Dish> dishes)
        {
            _dbContext.Dishes.RemoveRange(dishes);
            _dbContext.SaveChanges();
        }
    }
}
