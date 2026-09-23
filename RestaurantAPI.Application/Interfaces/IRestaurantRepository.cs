using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantAPI.Domain.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Application.Interfaces
{
    public interface IRestaurantRepository
    {
        Restaurant GetById(int id);
        void AddRestaurantToDbContext(Restaurant restaurant);
        void RemoveRestaurantToDbContext(Restaurant restaurant);
        public void SaveChangesOnDbContext();
        (List<Restaurant> Items, int TotalCount) GetAllMatching(RestaurantQuery query);
        IAsyncEnumerable<Restaurant> GetStreamAllRestaurants();
    }
}