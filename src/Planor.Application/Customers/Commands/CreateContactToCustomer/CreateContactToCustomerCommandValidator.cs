using FluentValidation;

namespace Planor.Application.Customers.Commands.CreateContactToCustomer;

public class CreateContactToCustomerCommandValidator : AbstractValidator<CreateContactToCustomerCommand>
{
    public CreateContactToCustomerCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .GreaterThanOrEqualTo(0)
            .NotEmpty();

        RuleFor(x => x.Name)
            .MaximumLength(64)
            .NotEmpty();
        
        RuleFor(x => x.Username)
            .MaximumLength(64)
            .NotEmpty();
        
        RuleFor(x => x.Email)
            .MaximumLength(64)
            .EmailAddress()
            .NotEmpty();

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(32);
    }
}