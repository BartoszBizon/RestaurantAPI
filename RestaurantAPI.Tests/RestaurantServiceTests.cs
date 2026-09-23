using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using RestaurantAPI;
using RestaurantAPI.Domain.Entities;
using RestaurantAPI.Entities;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace Restaurant.Tests;

public class RestaurantServiceTests
{
    [Fact]
    public void GetAllRestaurants_ForMatchingSearchPhrase_ReturnsOnlyMatchingRestaurant()
    {
        // ARRANGE

        // 1. Baza InMemory - unikalna nazwa na test, żeby dane się nie mieszały między testami
        var options = new DbContextOptionsBuilder<RestaurantDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var dbContext = new RestaurantDbContext(options);

        // 2. Dane testowe - dwie restauracje, tylko jedna powinna pasować do wyszukiwania "Pizza"
        // Address jest relacją WYMAGANĄ (AddressId to int, nie int?) - bez
        // przypisanego Address, RestaurantService.GetAllRestaurants (który
        // robi .Include(x => x.Address)) po cichu zgubi taką restaurację.
        dbContext.Restaurants.AddRange(
            new RestaurantAPI.Domain.Entities.Restaurant()
            {
                Name = "Pizza Hut",
                Description = "Najlepsza pizza w mieście",
                Category = "Fast Food",
                Address = new Address() { City = "Kraków", Street = "Testowa 1", PostalCode = "30-001" }
            },
            new RestaurantAPI.Domain.Entities.Restaurant()
            {
                Name = "KFC",
                Description = "Kurczak",
                Category = "Fast Food",
                Address = new Address() { City = "Warszawa", Street = "Testowa 2", PostalCode = "00-001" }
            }
        );
        dbContext.SaveChanges();

        // 3. Prawdziwy mapper, zbudowany z tego samego profilu co aplikacja
        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<RestaurantMappingProfile>());
        var mapper = mapperConfig.CreateMapper();

        // 4. Zależności nieużywane przez GetAllRestaurants - najprostsze możliwe atrapy
        var logger = NullLogger<RestaurantService>.Instance;
        var authorizationServiceMock = new Mock<IAuthorizationService>();
        var userContextServiceMock = new Mock<IUserContextService>();

        var service = new RestaurantService(
            dbContext,
            mapper,
            logger,
            authorizationServiceMock.Object,
            userContextServiceMock.Object);

        var query = new RestaurantQuery()
        {
            SearchPhrase = "Pizza",
            PageNumber = 1,
            PageSize = 10
        };

        // ACT
        var result = service.GetAllRestaurants(query);

        // ASSERT
        Assert.Equal(1, result.TotalItemsCount);
        Assert.Single(result.Items);
        Assert.Equal("Pizza Hut", result.Items.First().Name);
    }
}
