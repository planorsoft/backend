using FluentValidation;

namespace Planor.Application.Finances.Queries.GetFinanceWithPagination;

public class GetFinancesWithPaginationQueryValidator: AbstractValidator<GetFinancesWithPaginationQuery>
{
    public GetFinancesWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(0);
    }
}