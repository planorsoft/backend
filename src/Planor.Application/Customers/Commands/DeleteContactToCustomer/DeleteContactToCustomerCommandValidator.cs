using FluentValidation;

namespace Planor.Application.Customers.Commands.DeleteContactToCustomer;

public class DeleteContactToCustomerCommandValidator : AbstractValidator<DeleteContactToCustomerCommand>
{
    public DeleteContactToCustomerCommandValidator()
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