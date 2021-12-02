using FluentValidation;

namespace MyRest.Business.Models.Validations
{
    public class AddressValidation : AbstractValidator<Address>
    {
        public AddressValidation()
        {
            RuleFor(c => c.Street)
                .NotEmpty().WithMessage("The field {PropertyName} needs to be provided.")
                .Length(2, 200).WithMessage("The field  {PropertyName} must be between {MinLength} and {MaxLength} characters.");

            RuleFor(c => c.Neighborhood)
                .NotEmpty().WithMessage("The field {PropertyName} needs to be provided.")
                .Length(2, 100).WithMessage("The field  {PropertyName} must be between {MinLength} and {MaxLength} characters.");

            RuleFor(c => c.ZipCode)
                .NotEmpty().WithMessage("The field {PropertyName} needs to be provided.")
                .Length(8).WithMessage("O campo {PropertyName} precisa ter {MaxLength} caracteres");

            RuleFor(c => c.City)
                .NotEmpty().WithMessage("A campo {PropertyName} precisa ser fornecida")
                .Length(2, 100).WithMessage("The field  {PropertyName} must be between {MinLength} and {MaxLength} characters.");

            RuleFor(c => c.State)
                .NotEmpty().WithMessage("The field {PropertyName} needs to be provided.")
                .Length(2, 50).WithMessage("The field  {PropertyName} must be between {MinLength} and {MaxLength} characters.");

            RuleFor(c => c.Number)
                .NotEmpty().WithMessage("The field {PropertyName} needs to be provided.")
                .Length(1, 50).WithMessage("The field  {PropertyName} must be between {MinLength} and {MaxLength} characters.");
        }
    }
}