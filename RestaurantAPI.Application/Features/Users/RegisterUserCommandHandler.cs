using Microsoft.AspNetCore.Identity;
using RestaurantAPI.Application.Interfaces;
using RestaurantAPI.Application.Mediator;
using RestaurantAPI.Domain.Entities;

namespace RestaurantAPI.Application.Features.Users
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IMessagePublisher _messagePublisher;

        public RegisterUserCommandHandler(IUserRepository userRepository, IPasswordHasher<User> passwordHasher, IMessagePublisher messagePublisher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _messagePublisher = messagePublisher;
        }

        public async Task<Unit> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            var user = new User()
            {
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth,
                Nationality = dto.Nationality,
                RoleId = dto.RoleId,
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
            _userRepository.AddUserToDbContext(user);

            await _messagePublisher.PublishAsync(new UserRegisteredEvent(user.Email), QueueNames.UserRegistered);
            return Unit.Value;
        }
    }
}
