using MyRest.Business.Models.Validations.Documentos;
using FluentValidation;

namespace MyRest.Business.Models.Validations
{
    public class SupplierValidation : AbstractValidator<Supplier>
    {
        public SupplierValidation()
        {
            RuleFor(f => f.Name)
                .NotEmpty().WithMessage("The field {PropertyName} needs to be provided.")
                .Length(2, 100)
                .WithMessage("The field {PropertyName} must be between {MinLength} and {MaxLength} characters.");

            When(f => f.SupplierType == SupplierType.PhysicalPerson , () =>
            {
                RuleFor(f => f.Document.Length).Equal(CpfValidacao.CPFSize)
                    .WithMessage("The Document field must have {ComparisonValue} characters and {PropertyValue} was provided.");
                RuleFor(f=> CpfValidacao.Validate(f.Document)).Equal(true)
                    .WithMessage("The document provided is invalid.");
            });

            When(f => f.SupplierType == SupplierType.LegalPerson, () =>
            {
                RuleFor(f => f.Document.Length).Equal(CnpjValidation.CnpjSize)
                    .WithMessage("The Document field must have {ComparisonValue} characters and {PropertyValue} was provided.");
                RuleFor(f => CnpjValidation.Validate(f.Document)).Equal(true)
                    .WithMessage("The document provided is invalid.");
            });
        }
    }
}