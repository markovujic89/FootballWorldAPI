using Application.DTOs.Player;
using FluentValidation;

namespace Application.Validations.Player
{
    public class CreatePlayerValidator : AbstractValidator<CreatePlayerDTO>
    {
        public CreatePlayerValidator()
        {
            RuleFor(x=>x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x=>x.LastName).NotEmpty().MaximumLength(50);
            RuleFor(x=>x.Age).NotEmpty().ExclusiveBetween(10, 70);
        }
    }
}
