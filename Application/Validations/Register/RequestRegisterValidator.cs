using Application.DTOs;
using FluentValidation;

namespace Application.Validations.Register
{
    public class RequestRegisterValidator : AbstractValidator<RequestRegisterDTO>
    {
        public RequestRegisterValidator() 
        {
            RuleFor(dto => dto.UserName)
           .NotEmpty().WithMessage("UserName is required.")
           .EmailAddress().WithMessage("UserName should be a valid email address.");

            RuleFor(dto => dto.Password)
                .NotEmpty().WithMessage("Password is required.");

            RuleFor(dto => dto.Roles).NotEmpty().WithMessage("Roles should be required");
        }
    }
}
