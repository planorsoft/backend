using FluentValidation;

namespace Planor.Application.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(64)
            .NotEmpty();

        RuleFor(x => x.Address)
            .MaximumLength(128);

        RuleFor(x => x.City)
            .MaximumLength(64);

        RuleFor(x => x.District)
            .MaximumLength(64);
        
        RuleFor(x => x.PostCode)
            .MaximumLength(64);
        
        RuleFor(x => x.Country)
            .MaximumLength(64);
        
        RuleFor(x => x.PhoneNumber)
            .MaximumLength(64);
        
        RuleFor(x => x.Website)
            .MaximumLength(64);

        RuleFor(x => x.GovernmentId)
            .MaximumLength(64);
        
        RuleFor(x => x.GovernmentId)
            .Length(10).When(x => x.IsCompany && !string.IsNullOrEmpty(x.GovernmentId));
        
        RuleFor(x => x.GovernmentId)
            .Length(11).When(x => !x.IsCompany && !string.IsNullOrEmpty(x.GovernmentId));
        
        RuleFor(x => x.CurrencyCode)
            .MaximumLength(6);

    }
}