namespace MotorCycleRentail.Domain.Validations;

public class MotorcycleValidation : AbstractValidator<Motorcycle>
{
    public MotorcycleValidation()
    {
        ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("pt-BR");

        RuleFor(x => x.Identifier)
            .NotEmpty().WithMessage("O campo Identificador é obrigatório.")
            .MaximumLength(100).WithMessage("O campo Identificador deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Year)
            .NotEmpty().WithMessage("O campo Ano é obrigatório.")
            .GreaterThan(1920).WithMessage("O campo Ano deve ser maior que 1920.")
            .LessThan(9999).WithMessage("O campo Ano deve ser menor que 9999.");

        RuleFor(x => x.Model)
            .NotEmpty().WithMessage("O campo Modelo é obrigatório.")
            .MaximumLength(50).WithMessage("O campo Modelo deve ter no máximo 50 caracteres.");

        RuleFor(x => x.LicensePlate)
            .NotEmpty().WithMessage("O campo Placa é obrigatório.")
            .MaximumLength(7).WithMessage("O campo Placa deve ter no máximo 10 caracteres.");
    }
}
