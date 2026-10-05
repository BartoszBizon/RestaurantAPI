using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI;
using RestaurantAPI.Application.Features.Restaurants;
using RestaurantAPI.Domain.Entities;
using RestaurantAPI.Infrastructure;
using RestaurantAPI.Models;

namespace Restaurant.Tests;

public class GetAllRestaurantsQueryHandlerTests
{
    [Fact]
    public async Task Handle_ForMatchingSearchPhrase_ReturnsOnlyMatchingRestaurant()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<RestaurantDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var dbContext = new RestaurantDbContext(options);

        // Address is a required relation: without it, .Include(x => x.Address) silently drops the restaurant.
        dbContext.Restaurants.AddRange(
            new RestaurantAPI.Domain.Entities.Restaurant()
            {
                Name = "Pizza Hut",
                Description = "Najlepsza pizza w mieście",
                Category = "Fast Food",
                ContactEmail = "pizzahut@example.com",
                Address = new Address() { City = "Kraków", Street = "Testowa 1", PostalCode = "30-001" }
            },
            new RestaurantAPI.Domain.Entities.Restaurant()
            {
                Name = "KFC",
                Description = "Kurczak",
                Category = "Fast Food",
                ContactEmail = "kfc@example.com",
                Address = new Address() { City = "Warszawa", Street = "Testowa 2", PostalCode = "00-001" }
            }
        );
        dbContext.SaveChanges();

        var mapper = new MapperConfiguration(cfg => cfg.AddProfile<RestaurantMappingProfile>()).CreateMapper();
        var handler = new GetAllRestaurantsQueryHandler(new RestaurantRepository(dbContext), mapper);

        var query = new RestaurantQuery()
        {
            SearchPhrase = "Pizza",
            PageNumber = 1,
            PageSize = 10
        };

        // Act
        var result = await handler.Handle(new GetAllRestaurantsQuery(query), CancellationToken.None);

        // Assert
        Assert.Equal(1, result.TotalItemsCount);
        Assert.Single(result.Items);
        Assert.Equal("Pizza Hut", result.Items.First().Name);
    }
}
