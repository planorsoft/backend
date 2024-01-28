using FluentValidation;

namespace Planor.Application.Tags.Queries.GetTagsWithPagination;

public class GetTagsWithPaginationQueryValidator : AbstractValidator<GetTagsWithPaginationQuery>
{
    public GetTagsWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(0);
    }
}