using System;
using FluentValidation;
using FluentValidationMVC.Validators;
using LibraryManagementMVC.DTOs;

namespace LibraryManagementMVC.Validators;

public class PublisherCreateDtoValidator : PublisherDtoValidatorBase<PublisherCreateDto>
    {
        public PublisherCreateDtoValidator()
        {
            ConfigureBaseRules();
        }
        
    }