﻿using Application.DTOs;
using FluentValidation;

namespace Application.Validations.Player
{
    public class EditPlayerValidation : AbstractValidator<EditPlayerDTO>
    {
        public EditPlayerValidation()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Age).NotEmpty().ExclusiveBetween(10, 70);
        }
    }
}
