using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Application
{
    public record UserRegisteredEvent
    {
        public string Email { get; init; }

        public UserRegisteredEvent(string email)
        {
            Email = email;
        }
    }

}