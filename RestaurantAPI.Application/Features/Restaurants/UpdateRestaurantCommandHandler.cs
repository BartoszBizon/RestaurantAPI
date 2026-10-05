using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Application.Interfaces;
using RestaurantAPI.Application.Mediator;
using RestaurantAPI.Authorization;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Interfaces;

namespace RestaurantAPI.Application.Features.Restaurants
{
    public class UpdateRestaurantCommandHandler : IRequestHandler<UpdateRestaurantCommand, Unit>
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;

        public UpdateRestaurantCommandHandler(IRestaurantRepository restaurantRepository, IAuthorizationService authorizationService,
            IUserContextService userContextService)
        {
            _restaurantRepository = restaurantRepository;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
        }

        public async Task<Unit> Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
        {
            var restaurant = _restaurantRepository.GetById(request.Id);
            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            var authorizationResult = await _authorizationService.AuthorizeAsync(
                _userContextService.User, restaurant, new ResourceOperationRequirement(ResourceOperation.Update));

            if (!authorizationResult.Succeeded)
                throw new ForbidenException("Athorization Fault");

            restaurant.Name = request.Dto.Name;
            restaurant.Description = request.Dto.Description;
            restaurant.HasDelivery = request.Dto.HasDelivery;
            _restaurantRepository.SaveChangesOnDbContext();
            return Unit.Value;
        }
    }
}
