using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantAPI.Models;

namespace RestaurantAPI.Interfaces
{
    public interface IDishService
    {
        public int Create(int id, CreateDishDto dto);
        public DishDto GetById(int restaurantId, int dishId);
        public List<DishDto> GetAll(int restaurantId);
        public void RemoveAll(int restaurantId);
        public void RemoveById(int restaurantId, int dishId);
    }
}