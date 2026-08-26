using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantAPI.Models;

namespace RestaurantAPI.Interfaces
{
    public interface IUserService
    {
        public void RegisterUser(CreateUserDto dto);
        public string GenerateJwt(LoginDto dto);
    }
}