using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RestaurantAPI.Entities;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Models;
using RestaurantAPI.Repositories;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
    [ApiController]
    [Route("api/restaurant")]
    public class RestaurantsController : ControllerBase
    {
        private IRestaurantService _restaurantService;
        private RestaurantDapperRepository _dapperRepository;

        public RestaurantsController(IRestaurantService restaurantService, RestaurantDapperRepository dapperRepository)
        {
            _restaurantService = restaurantService;
            _dapperRepository = dapperRepository;
        }

        [HttpGet]
        //[Authorize(Policy = "Atleast20")]
        public ActionResult<IEnumerable<RestaurantDto>> GetAll([FromQuery] RestaurantQuery query)
        {
            var restaurantsDto = _restaurantService.GetAllRestaurants(query);
            return Ok(restaurantsDto);
        }

        [HttpGet("{id}")]
        public ActionResult<RestaurantDto> GetRestaurantWithID([FromRoute] int id)
        {
            var restaurantDto = _restaurantService.GetRestaurantWithID(id);

            return Ok(restaurantDto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]

        public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var newRestaurantId = _restaurantService.CreateRestaurant(dto);
            return Created($"/api/restaurant/{newRestaurantId}", null);
        }


        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _restaurantService.DeleteRestaurant(id);

            return NoContent();
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromRoute] int id, [FromBody] UpdateRestaurantDto dto)
        {
            _restaurantService.UpdateRestaurant(id, dto);

            return Ok();
        }

        [HttpGet("dapper-search-vulnearable")]
        public ActionResult DapperSearchVulnearable([FromQuery] string searchPhrase)
        {
            var result = _dapperRepository.SearchVulnearable(searchPhrase);
            return Ok(result);
        }

        [HttpGet("dapper-search-safe")]
        public ActionResult DapperSearchSafe([FromQuery] string searchPhrase)
        {
            var result = _dapperRepository.SearchSafe(searchPhrase);
            return Ok(result);
        }

        [HttpGet("restaurant-stream")]
        public IAsyncEnumerable<RestaurantDto> GetRestaurantStream()
        {
            return _restaurantService.GetRestaurantsByStream();
        }
    }
}