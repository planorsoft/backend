using FluentValidation;
using Planor.Application.Customers.Queries.GetCustomersWithPagination;

namespace Planor.Application.Projects.Queries.GetProjectsWithPagination;

public class GetProjectsWithPaginationQueryValidator: AbstractValidator<GetProjectsWithPaginationQuery>
{
    public GetProjectsWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(0);
    }
}