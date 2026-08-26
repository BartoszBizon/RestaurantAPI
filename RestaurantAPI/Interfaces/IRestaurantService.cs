using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Interfaces
{
    public interface IRestaurantService
    {
        public PageResult<RestaurantDto> GetAllRestaurants(RestaurantQuery query);
        public RestaurantDto GetRestaurantWithID(int id);
        public int CreateRestaurant(CreateRestaurantDto dto);
        public void DeleteRestaurant(int id);
        public void UpdateRestaurant(int id, UpdateRestaurantDto dto);
    }
}