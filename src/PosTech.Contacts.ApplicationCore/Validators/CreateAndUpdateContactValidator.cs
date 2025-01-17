using FluentValidation;
using PosTech.Contacts.ApplicationCore.DTOs.Requests;
using PosTech.Contacts.ApplicationCore.Resources;

namespace PosTech.Contacts.ApplicationCore.Validators
{
    public class CreateAndUpdateContactValidator : AbstractValidator<CreateAndUpdateContactRequestDto>
    {
        public CreateAndUpdateContactValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage(Messages.NameRequired)
                .MinimumLength(2)
                .WithMessage(Messages.MinLengthForName);

            RuleFor(c => c.Surname)
                .NotEmpty()
                .WithMessage(Messages.SurnameRequired)
                .MinimumLength(5)
                .WithMessage(Messages.MinLengthForSurname);

            RuleFor(x => x.Ddd)
                .Must(ddd => CommonValidators.ValidateDdd(ddd))
                .WithMessage(Messages.DddInvalid);

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(Messages.EmailRequired)
                .EmailAddress()
                .WithMessage(Messages.EmailInvalid);

            RuleFor(x => x.Phone)
                .NotEmpty()
                .WithMessage(Messages.PhoneRequired)
                .Must(phone => CommonValidators.ValidatePhone(phone))
                .WithMessage(Messages.PhoneInvalid);
        }
    }
}
