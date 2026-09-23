using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using RestaurantAPI.Domain.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI
{
    public class RestaurantMappingProfile : Profile
    {
        public RestaurantMappingProfile()
        {
            CreateMap<Restaurant, RestaurantDto>()
            .ForMember(x => x.City, x => x.MapFrom(s => s.Address.City))
            .ForMember(x => x.Street, x => x.MapFrom(s => s.Address.Street))
            .ForMember(x => x.PostalCode, x => x.MapFrom(s => s.Address.PostalCode));

            CreateMap<Dish, DishDto>();

            CreateMap<CreateRestaurantDto, Restaurant>()
            .ForMember(x => x.Address, x => x.MapFrom(dto => new Address()
            {
                City = dto.City,
                PostalCode = dto.PostalCode,
                Street = dto.Street
            }));

            CreateMap<CreateDishDto, Dish>();


        }
    }
}