using FluentValidation;
using APIUsuarios.Application.DTOs;

namespace APIUsuarios.Application.Validators;

public class UsuarioCreateDtoValidator : AbstractValidator<UsuarioCreateDto>
{
    public UsuarioCreateDtoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .Length(2, 100).WithMessage("Nome deve ter entre 2 e 100 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório.")
            .EmailAddress().WithMessage("Email deve ser um endereço válido.");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("Senha é obrigatória.")
            .MinimumLength(6).WithMessage("Senha deve ter pelo menos 6 caracteres.");

        RuleFor(x => x.DataNascimento)
            .NotEmpty().WithMessage("Data de nascimento é obrigatória.")
            .LessThan(DateTime.Now).WithMessage("Data de nascimento deve ser no passado.")
            .Must(BeAtLeast18YearsOld).WithMessage("Usuário deve ter pelo menos 18 anos.");

        RuleFor(x => x.Telefone)
            .Matches(@"^\d{10,11}$").WithMessage("Telefone deve conter 10 ou 11 dígitos.")
            .When(x => !string.IsNullOrEmpty(x.Telefone));
    }

    private bool BeAtLeast18YearsOld(DateTime dataNascimento)
    {
        var today = DateTime.Today;
        var age = today.Year - dataNascimento.Year;
        if (dataNascimento.Date > today.AddYears(-age)) age--;
        return age >= 18;
    }
}
