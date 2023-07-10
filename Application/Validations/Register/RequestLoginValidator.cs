using Application.DTOs;
using FluentValidation;

namespace Application.Validations.Register
{
    public class RequestLoginValidator : AbstractValidator<LoginRequestDTO>
    {
        public RequestLoginValidator() 
        {
            RuleFor(dto => dto.UserName)
           .NotEmpty().WithMessage("UserName is required.")
           .EmailAddress().WithMessage("UserName should be a valid email address.");

            RuleFor(dto => dto.Password)
                .NotEmpty().WithMessage("Password is required.");

            
        }
    }
}
