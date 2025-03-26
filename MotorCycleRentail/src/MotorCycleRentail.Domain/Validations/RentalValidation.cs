namespace MotorCycleRentail.Domain.Validations;

public class RentalValidation : AbstractValidator<Rental>
{
    public RentalValidation()
    {
        ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("pt-BR");

        RuleFor(x => x.PlanDays)
            .NotEmpty().WithMessage("O campo PlanDays é obrigatório.")
            .GreaterThan(0).WithMessage("O campo PlanDays deve ser maior que 0.");

        RuleFor(x => x.CourierIdentifier)
            .NotEmpty().WithMessage("O campo CourierIdentifier é obrigatório.");

        RuleFor(x => x.MotorcycleIdentifier)
            .NotEmpty().WithMessage("O campo MotorcycleIdentifier é obrigatório.");

        var tomorrow = DateTime.UtcNow.Date.AddDays(1);

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("O campo StartDate é obrigatório.")
            .Must(date => date.Date >= tomorrow).WithMessage("O campo StartDate deve ser maior do que o dia atual.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("O campo EndDate é obrigatório.")
            .Must(date => date.Date >= tomorrow).WithMessage("O campo EndDate deve ser maior do que o dia atual.");

        RuleFor(x => x.ExpectedEndDate)
            .NotEmpty().WithMessage("O campo ExpectedEndDate é obrigatório.")
            .Must(date => date.Date >= tomorrow).WithMessage("O campo ExpectedEndDate deve ser maior do que o dia atual.");

        RuleFor(x => x.ReturnDate)
            .Must(date => date == null || date.Value.Date >= tomorrow)
            .WithMessage("O campo ReturnDate deve ser maior do que o dia atual.")
            .When(x => x.ReturnDate.HasValue);
    }
}
