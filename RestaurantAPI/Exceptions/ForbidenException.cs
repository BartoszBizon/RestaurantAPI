using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Exceptions
{
    public class ForbidenException : Exception
    {
        public ForbidenException(string message) : base(message)
        {
            
        }
    }
}