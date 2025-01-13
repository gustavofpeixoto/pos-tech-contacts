using FluentValidation;
using PosTech.Contacts.ApplicationCore.Commands;
using System.Text.RegularExpressions;

namespace PosTech.Contacts.ApplicationCore.Validators
{
    public class CreateContactCommandValidator : AbstractValidator<CreateContactCommand>
    {
        public CreateContactCommandValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("É necessário informar um nome")
                .MinimumLength(2)
                .WithMessage("Necessário informar um nome com pelo menos 2 caracteres");

            RuleFor(c => c.Surname)
                .NotEmpty()
                .WithMessage("É necessário informar um sobrenome").MinimumLength(5)
                .WithMessage("Necessário informar um sobrenome com pelo menos 5 caracteres");

            RuleFor(x => x.Ddd)
                .Must(ddd => ValidateDdd(ddd))
                .WithMessage("O DDD informado deve ser válido. Exemplo: 11 (São Paulo capital)");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("É necessário informar um endereço de e-mail")
                .EmailAddress()
                .WithMessage("O e-mail informado deve ser válido");

            RuleFor(x => x.Phone)
                .NotEmpty()
                .WithMessage("É necessário informar um telefone")
                .Must(phone => ValidatePhone(phone))
                .WithMessage("O telefone informado informado deve ser válido. Exemplo: 91234-5678 (celular) ou 3456-7890 (fixo)");
        }

        private static bool ValidatePhone(string phone)
        {
            var phonepattern = @"^(9\d{4}-\d{4}|\d{4}-\d{4})$";
            var regex = new Regex(phonepattern);

            return regex.IsMatch(phone);
        }

        private static bool ValidateDdd(int x)
        {
            int[] validDdd = { 11, 12, 13, 14, 15, 16, 17, 18, 19, 21, 22, 24, 27, 28, 31, 
                32, 33, 34, 35, 37, 38, 41, 42, 43, 44, 45, 46, 47, 48, 49, 51, 53, 54, 
                55, 61, 62, 63, 64, 65, 66, 67, 68, 69, 71, 73, 74, 75, 77, 79, 81, 
                82, 83, 84, 85, 86, 87, 88, 89, 91, 92, 93, 94, 95, 96, 97, 98, 99 };
        
            return validDdd.Contains(x);
        }
    }
}
