using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantAPI.Application.Mediator;
using RestaurantAPI.Models;

namespace RestaurantAPI.Application.Features.Restaurants
{
    public record GetRestaurantByIdQuery : IRequest<RestaurantDto>
    {
        public int Id { get; }

        public GetRestaurantByIdQuery(int id)
        {
            Id = id;
        }

    }
}