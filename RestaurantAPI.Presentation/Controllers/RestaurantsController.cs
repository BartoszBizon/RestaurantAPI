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
using RestaurantAPI.Application.Features.Restaurants;
using RestaurantAPI.Application.Mediator;
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
        private IMediator _mediator;

        public RestaurantsController(IRestaurantService restaurantService, RestaurantDapperRepository dapperRepository, IMediator mediator)
        {
            _restaurantService = restaurantService;
            _dapperRepository = dapperRepository;
            _mediator = mediator;
        }

        [HttpGet]
        //[Authorize(Policy = "Atleast20")]
        public async Task<ActionResult<PageResult<RestaurantDto>>> GetAll([FromQuery] RestaurantQuery query)
        {
            var restaurantsDto = await _mediator.Send(new GetAllRestaurantsQuery(query));
            return Ok(restaurantsDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RestaurantDto>> GetRestaurantWithID([FromRoute] int id)
        {
            var restaurantDto = await _mediator.Send(new GetRestaurantByIdQuery(id));
            return Ok(restaurantDto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]

        public async Task<ActionResult> CreateRestaurant([FromBody] CreateRestaurantDto dto)
        {
            var newRestaurantId = await _mediator.Send(new CreateRestaurantCommand(dto));
            return Created($"/api/restaurant/{newRestaurantId}", null);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            await _mediator.Send(new DeleteRestaurantCommand(id));

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateRestaurantDto dto)
        {
            await _mediator.Send(new UpdateRestaurantCommand(id, dto));

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