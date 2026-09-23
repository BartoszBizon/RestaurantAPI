using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Application.Interfaces;
using RestaurantAPI.Domain.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Infrastructure
{
    public class RestaurantRepository : IRestaurantRepository
    {

        private readonly RestaurantDbContext _dbContext;

        public RestaurantRepository(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Restaurant GetById(int id)
        {
            var restaurant = _dbContext.Restaurants.Where(x => x.Id == id)
            .Include(x => x.Address)
            .Include(x => x.Dishes)
            .FirstOrDefault();

            return restaurant;
        }

        public void AddRestaurantToDbContext(Restaurant restaurant)
        {
            _dbContext.Restaurants.Add(restaurant);
            _dbContext.SaveChanges();
        }

        public void RemoveRestaurantToDbContext(Restaurant restaurant)
        {
            _dbContext.Restaurants.Remove(restaurant);
            _dbContext.SaveChanges();
        }

        public void SaveChangesOnDbContext()
        {
            _dbContext.SaveChanges();
        }

        public (List<Restaurant> Items, int TotalCount) GetAllMatching(RestaurantQuery query)
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

            return (restaurants, totalItemsCount);
        }

        public IAsyncEnumerable<Restaurant> GetStreamAllRestaurants()
        {
            return (IAsyncEnumerable<Restaurant>)_dbContext.Restaurants;
        }
    }
}