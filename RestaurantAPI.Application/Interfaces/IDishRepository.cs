using System.Collections.Generic;
using RestaurantAPI.Domain.Entities;

namespace RestaurantAPI.Application.Interfaces
{
    public interface IDishRepository
    {
        void AddDishToDbContext(Dish dish);
        void RemoveDishToDbContext(Dish dish);
        void RemoveRangeDishesToDbContext(IEnumerable<Dish> dishes);
    }
}
