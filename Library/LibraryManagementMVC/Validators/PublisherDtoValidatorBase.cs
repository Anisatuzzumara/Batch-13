using FluentValidation;

namespace FluentValidationMVC.Validators
{
    /// <summary>
    /// Kelas validator dasar untuk DTO Publisher.
    /// </summary>
    public abstract class PublisherDtoValidatorBase<T> : AbstractValidator<T> where T : class
    {
         protected void ConfigureBaseRules()
        {
            RuleFor(x => (string)GetPropValue(x, "Name"))
                .NotEmpty().WithMessage("Nama penerbit tidak boleh kosong.")
                .MaximumLength(150).WithMessage("Nama penerbit tidak boleh lebih dari 150 karakter.");
            
            RuleFor(x => (string)GetPropValue(x, "Country"))
                .MaximumLength(100).WithMessage("Nama negara tidak boleh lebih dari 100 karakter.");

            RuleFor(x => (string)GetPropValue(x, "City"))
                .MaximumLength(100).WithMessage("Nama kota tidak boleh lebih dari 100 karakter.");
            
            RuleFor(x => (string)GetPropValue(x, "Email"))
                .EmailAddress().WithMessage("Format email tidak valid.")
                .MaximumLength(100).WithMessage("Email tidak boleh lebih dari 100 karakter.")
                .When(x => !string.IsNullOrEmpty((string)GetPropValue(x, "Email"))); // Validasi email hanya jika diisi.
        }

        private object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName)?.GetValue(src, null) ?? default!;
        }
    }
}
