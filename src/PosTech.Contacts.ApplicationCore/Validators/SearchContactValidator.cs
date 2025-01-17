using FluentValidation;
using PosTech.Contacts.ApplicationCore.DTOs.Requests;
using PosTech.Contacts.ApplicationCore.Resources;

namespace PosTech.Contacts.ApplicationCore.Validators
{
    public class SearchContactValidator : AbstractValidator<SearchContactRequestDto>
    {
        public SearchContactValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(Messages.NameRequired)
                .MinimumLength(2)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(Messages.MinLengthForName);

            RuleFor(c => c.Surname)
                .NotEmpty()
                .WithMessage(Messages.SurnameRequired)
                .When(x => !string.IsNullOrEmpty(x.Surname))
                .MinimumLength(5)
                .When(x => !string.IsNullOrEmpty(x.Surname))
                .WithMessage(Messages.MinLengthForSurname);

            RuleFor(x => x.Ddd)
                .Must(ddd => CommonValidators.ValidateDdd(ddd))
                .When(x => x.Ddd != default)
                .WithMessage(Messages.DddInvalid);

            RuleFor(x => x.Email)
                .NotEmpty()
                .When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage(Messages.EmailRequired)
                .EmailAddress()
                .When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage(Messages.EmailInvalid);

            RuleFor(x => x.Phone)
                .NotEmpty()
                .When(x => !string.IsNullOrEmpty(x.Phone))
                .WithMessage(Messages.PhoneRequired)
                .Must(phone => CommonValidators.ValidatePhone(phone))
                .When(x => !string.IsNullOrEmpty(x.Phone))
                .WithMessage(Messages.PhoneInvalid);
        }
    }
}
