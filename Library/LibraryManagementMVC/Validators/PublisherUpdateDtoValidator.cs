using System;
using FluentValidation;
using FluentValidationMVC.Validators;
using LibraryManagementMVC.DTOs;

namespace LibraryManagementMVC.Validators;

public class PublisherUpdateDtoValidator : PublisherDtoValidatorBase<PublisherUpdateDto>
    {
        public PublisherUpdateDtoValidator()
        {
            ConfigureBaseRules();

            RuleFor(dto => dto.PubId)
                .NotEmpty().WithMessage("Publisher ID is required for an update.")
                .GreaterThan(0).WithMessage("Invalid Publisher ID.");
        }
    }