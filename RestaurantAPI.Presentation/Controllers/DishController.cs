using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Application.Features.Dishes;
using RestaurantAPI.Application.Mediator;
using RestaurantAPI.Models;

namespace RestaurantAPI.Controllers
{
    [ApiController]
    [Route("api/{restaurantId}/dish")]
    public class DishController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DishController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromRoute] int restaurantId, [FromBody] CreateDishDto dto)
        {
            await _mediator.Send(new CreateDishCommand(restaurantId, dto));
            return Created("New dish created", null);
        }

        [HttpGet("{dishId}")]
        public async Task<ActionResult<DishDto>> Get([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            var dish = await _mediator.Send(new GetDishByIdQuery(restaurantId, dishId));
            return Ok(dish);
        }

        [HttpGet]
        public async Task<ActionResult<List<DishDto>>> GetAll([FromRoute] int restaurantId)
        {
            var dishDtos = await _mediator.Send(new GetAllDishesQuery(restaurantId));
            return Ok(dishDtos);
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveAll([FromRoute] int restaurantId)
        {
            await _mediator.Send(new RemoveAllDishesCommand(restaurantId));
            return NoContent();
        }

        [HttpDelete("{dishId}")]
        public async Task<ActionResult> RemoveById([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            await _mediator.Send(new RemoveDishByIdCommand(restaurantId, dishId));
            return NoContent();
        }
    }
}
