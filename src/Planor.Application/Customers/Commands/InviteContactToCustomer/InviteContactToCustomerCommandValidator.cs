using FluentValidation;

namespace Planor.Application.Customers.Commands.InviteContactToCustomer;

public class InviteContactToCustomerCommandValidator : AbstractValidator<InviteContactToCustomerCommand>
{
    public InviteContactToCustomerCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .GreaterThanOrEqualTo(0)
            .NotEmpty();
        
        RuleFor(x => x.Email)
            .MaximumLength(64)
            .EmailAddress()
            .NotEmpty();
    }
}