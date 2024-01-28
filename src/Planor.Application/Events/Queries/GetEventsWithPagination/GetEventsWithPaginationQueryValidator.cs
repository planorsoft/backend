using FluentValidation;

namespace Planor.Application.Events.Queries.GetEventsWithPagination;

public class GetEventsWithPaginationQueryValidator : AbstractValidator<GetEventsWithPaginationQuery>
{
    public GetEventsWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(0);
    }
}