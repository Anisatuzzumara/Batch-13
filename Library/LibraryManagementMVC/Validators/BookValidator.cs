using System;
using FluentValidation;
using LibraryManagementMVC.Models; 

namespace LibraryManagementMVC.Validator;

public class BookValidator : AbstractValidator<Book>
{
    public BookValidator()
        {
            
            RuleFor(book => book.Title)
                .NotEmpty().WithMessage("Book title is required.")
                .MaximumLength(200).WithMessage("Title cannot be longer than 200 characters.");

            RuleFor(book => book.Author)
                .NotEmpty().WithMessage("Author name is required.")
                .MaximumLength(150).WithMessage("Author name cannot be longer than 150 characters.");

            RuleFor(book => book.ISBN)
                .NotEmpty().WithMessage("ISBN is required.")
                .Length(10, 17).WithMessage("A valid ISBN must be between 10 and 17 characters (including hyphens).")
                
                .Matches(@"^(?:ISBN(?:-1[03])?:? )?(?=[0-9X]{10}$|(?=(?:[0-9]+-){3})[0-9X]{13}$)[0-9]{1,5}-[0-9]+-[0-9]+-[0-9X]$")
                .WithMessage("Please enter a valid ISBN-10 or ISBN-13 format.");
            
            RuleFor(book => book.PublishedYear)
                .NotEmpty().WithMessage("Published year is required.")
                .InclusiveBetween(1400, DateTime.Now.Year) 
                .WithMessage($"Year must be between 1400 and the current year ({DateTime.Now.Year}).");

            RuleFor(book => book.PubId)
                .GreaterThan(0).WithMessage("You must select a publisher.");
        }
}
