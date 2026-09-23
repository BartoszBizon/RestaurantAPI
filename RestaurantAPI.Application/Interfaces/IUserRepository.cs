using RestaurantAPI.Domain.Entities;

namespace RestaurantAPI.Application.Interfaces
{
    public interface IUserRepository
    {
        void AddUserToDbContext(User user);
        User GetByEmailWithRole(string email);
    }
}
