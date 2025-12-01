using FluentValidation;
using APIUsuarios.Application.DTOs;

namespace APIUsuarios.Application.Validators;

public class UsuarioUpdateDtoValidator : AbstractValidator<UsuarioUpdateDto>
{
    public UsuarioUpdateDtoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .Length(2, 100).WithMessage("Nome deve ter entre 2 e 100 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório.")
            .EmailAddress().WithMessage("Email deve ser um endereço válido.");

        RuleFor(x => x.DataNascimento)
            .NotEmpty().WithMessage("Data de nascimento é obrigatória.")
            .LessThan(DateTime.Now).WithMessage("Data de nascimento deve ser no passado.");

        RuleFor(x => x.Telefone)
            .Matches(@"^\d{10,11}$").WithMessage("Telefone deve conter 10 ou 11 dígitos.")
            .When(x => !string.IsNullOrEmpty(x.Telefone));

        RuleFor(x => x.Ativo)
            .NotNull().WithMessage("Ativo é obrigatório.");
    }
}
