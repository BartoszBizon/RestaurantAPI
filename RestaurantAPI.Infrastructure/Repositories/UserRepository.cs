using System.Linq;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Application.Interfaces;
using RestaurantAPI.Domain.Entities;

namespace RestaurantAPI.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly RestaurantDbContext _dbContext;

        public UserRepository(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddUserToDbContext(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }

        public User GetByEmailWithRole(string email)
        {
            return _dbContext.Users
                .Include(x => x.Role)
                .FirstOrDefault(x => x.Email.Equals(email));
        }
    }
}
