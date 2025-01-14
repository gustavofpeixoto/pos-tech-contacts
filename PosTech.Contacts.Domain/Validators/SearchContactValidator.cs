using FluentValidation;
using PosTech.Contacts.ApplicationCore.DTOs.Requests;

namespace PosTech.Contacts.ApplicationCore.Validators
{
    public class SearchContactValidator : AbstractValidator<SearchContactRequestDto>
    {
        public SearchContactValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage("É necessário informar um nome")
                .MinimumLength(2)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage("Necessário informar um nome com pelo menos 2 caracteres");

            RuleFor(c => c.Surname)
                .NotEmpty()
                .WithMessage("É necessário informar um sobrenome")
                .When(x => !string.IsNullOrEmpty(x.Surname))
                .MinimumLength(5)
                .When(x => !string.IsNullOrEmpty(x.Surname))
                .WithMessage("Necessário informar um sobrenome com pelo menos 5 caracteres");

            RuleFor(x => x.Ddd)
                .Must(ddd => CommonValidators.ValidateDdd(ddd))
                .When(x => x.Ddd != default)
                .WithMessage("O DDD informado deve ser válido. Exemplo: 11 (São Paulo capital)");

            RuleFor(x => x.Email)
                .NotEmpty()
                .When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage("É necessário informar um endereço de e-mail")
                .EmailAddress()
                .When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage("O e-mail informado deve ser válido");

            RuleFor(x => x.Phone)
                .NotEmpty()
                .When(x => !string.IsNullOrEmpty(x.Phone))
                .WithMessage("É necessário informar um telefone")
                .Must(phone => CommonValidators.ValidatePhone(phone))
                .When(x => !string.IsNullOrEmpty(x.Phone))
                .WithMessage("O telefone informado informado deve ser válido. Exemplo: 91234-5678 (celular) ou 3456-7890 (fixo)");
        }
    }
}
