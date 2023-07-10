﻿using Application.DTOs.Club;
using FluentValidation;

namespace Application.Validations.Club
{
    public class EditClubValidator : AbstractValidator<EditClubDTO>
    {
        public EditClubValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.City).NotEmpty().MaximumLength(50);
        }
    }
}
