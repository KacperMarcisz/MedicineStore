
namespace MedicineStore.WEB.Validator
{
    using CORE.ViewModels;
    using FluentValidation;

    public class AddMedicineViewModelValidator : AbstractValidator<AddMedicineViewModel>
    {
        public AddMedicineViewModelValidator()
        {
            RuleFor(t => t.Name).NotEmpty().Length(1, 50)
                .WithMessage("Please specify name");
            RuleFor(t => t.Description).NotEmpty().Length(1, 255)
                .WithMessage("Please specify name");
            RuleFor(t => t.GrossPrice).NotEmpty();
        }
    }
}
