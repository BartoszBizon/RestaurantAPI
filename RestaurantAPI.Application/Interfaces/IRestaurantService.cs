using RestaurantAPI.Models;

namespace RestaurantAPI.Interfaces
{
    public interface IRestaurantService
    {
        IAsyncEnumerable<RestaurantDto> GetRestaurantsByStream();
    }
}
