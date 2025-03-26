namespace MotorCycleRentail.Domain.Validations;

public class CourierValidation : AbstractValidator<Courier>
{
    public CourierValidation()
    {
        ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("pt-BR");

        RuleFor(x => x.Identifier)
            .NotEmpty().WithMessage("O campo Identificador é obrigatório.")
            .MaximumLength(100).WithMessage("O campo Identificador deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O campo Nome é obrigatório.")
            .MaximumLength(255).WithMessage("O campo Identificador deve ter no máximo 255 caracteres.");

        RuleFor(x => x.Cnpj)
            .NotEmpty().WithMessage("O campo Cnpj é obrigatório.")
            .MaximumLength(14).WithMessage("O campo Cnpj deve ter no máximo 14 caracteres.");

        RuleFor(x => x.Birthdate)
            .NotEmpty().WithMessage("O campo Aniversário é obrigatório.");

        RuleFor(x => x.CnhNumber)
            .NotEmpty().WithMessage("O campo CnhNumber é obrigatório.")
            .MaximumLength(11).WithMessage("O campo CnhNumber deve ter no máximo 11 caracteres.");

        RuleFor(x => x.CnhType)
            .NotEmpty().WithMessage("O campo CnhType é obrigatório.")
            .Must(value => new[] { "A", "B", "AB" }.Contains(value.ToUpper())).WithMessage("O campo CnhType deve ser 'A', 'B' ou 'AB'.")
            .MaximumLength(2).WithMessage("O campo CnhType deve ter no máximo 2 caracteres.");

        RuleFor(x => x.CnhImageId)
            .NotEmpty().WithMessage("O campo CnhImageId é obrigatório.");
    }
}
