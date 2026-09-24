using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using RestaurantAPI.Application.Interfaces;


namespace RestaurantAPI.Models.Validators
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        private readonly IUserRepository _userRepository;
        public CreateUserDtoValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public CreateUserDtoValidator()
        {
            RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

            RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6);

            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password);

            RuleFor(x => x.Email)
            .Custom((value, context) =>
            {
                var emailInUse = _userRepository.EmailExists(value);
                if (emailInUse)
                {
                    context.AddFailure("Email", "That email is taken");
                }
            });
        }
    }
}