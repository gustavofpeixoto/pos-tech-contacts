using FluentValidation;
using PosTech.Contacts.ApplicationCore.DTOs.Requests;
using System.Text.RegularExpressions;

namespace PosTech.Contacts.ApplicationCore.Validators
{
    public class CreateAndUpdateContactValidator : AbstractValidator<CreateAndUpdateContactRequestDto>
    {
        public CreateAndUpdateContactValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("É necessário informar um nome")
                .MinimumLength(2)
                .WithMessage("Necessário informar um nome com pelo menos 2 caracteres");

            RuleFor(c => c.Surname)
                .NotEmpty()
                .WithMessage("É necessário informar um sobrenome")
                .MinimumLength(5)
                .WithMessage("Necessário informar um sobrenome com pelo menos 5 caracteres");

            RuleFor(x => x.Ddd)
                .Must(ddd => CommonValidators.ValidateDdd(ddd))
                .WithMessage("O DDD informado deve ser válido. Exemplo: 11 (São Paulo capital)");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("É necessário informar um endereço de e-mail")
                .EmailAddress()
                .WithMessage("O e-mail informado deve ser válido");

            RuleFor(x => x.Phone)
                .NotEmpty()
                .WithMessage("É necessário informar um telefone")
                .Must(phone => CommonValidators.ValidatePhone(phone))
                .WithMessage("O telefone informado informado deve ser válido. Exemplo: 91234-5678 (celular) ou 3456-7890 (fixo)");
        }
    }
}
