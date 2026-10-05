using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using RestaurantAPI.Application.Interfaces;
using RestaurantAPI.Application.Mediator;
using RestaurantAPI.Authorization;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Interfaces;

namespace RestaurantAPI.Application.Features.Restaurants
{
    public class DeleteRestaurantCommandHandler : IRequestHandler<DeleteRestaurantCommand, Unit>
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;
        private readonly ILogger<DeleteRestaurantCommandHandler> _logger;

        public DeleteRestaurantCommandHandler(IRestaurantRepository restaurantRepository, IAuthorizationService authorizationService,
            IUserContextService userContextService, ILogger<DeleteRestaurantCommandHandler> logger)
        {
            _restaurantRepository = restaurantRepository;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
        {
            _logger.LogError($"Restaurant with id {request.Id} invoked");

            var restaurant = _restaurantRepository.GetById(request.Id);
            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            var authorizationResult = await _authorizationService.AuthorizeAsync(
                _userContextService.User, restaurant, new ResourceOperationRequirement(ResourceOperation.Delete));

            if (!authorizationResult.Succeeded)
                throw new ForbidenException("Athorization Fault");

            _restaurantRepository.RemoveRestaurantToDbContext(restaurant);
            return Unit.Value;
        }
    }
}
