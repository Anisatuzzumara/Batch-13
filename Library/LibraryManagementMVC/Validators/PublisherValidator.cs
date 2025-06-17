using FluentValidation;
using LibraryManagementMVC.Models; 


namespace LibraryManagementMVC.Validator
{
    public class PublisherValidator : AbstractValidator<Publisher>
    {
        public PublisherValidator()
        {
            
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Publisher name is required.")
                .MaximumLength(150).WithMessage("Publisher name cannot be longer than 150 characters.");

            
            RuleFor(p => p.Country)
                .MaximumLength(100).WithMessage("Country name cannot be longer than 100 characters.");
        }
    }
}
