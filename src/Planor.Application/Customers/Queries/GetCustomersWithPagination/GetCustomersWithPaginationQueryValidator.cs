using FluentValidation;
using Planor.Application.Customers.Queries.GetCustomerById;

namespace Planor.Application.Customers.Queries.GetCustomersWithPagination;

public class GetCustomersWithPaginationQueryValidator : AbstractValidator<GetCustomersWithPaginationQuery>
{
    public GetCustomersWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(0);
    }
}