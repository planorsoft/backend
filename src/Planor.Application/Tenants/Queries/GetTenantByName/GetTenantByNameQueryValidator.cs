using FluentValidation;
using Planor.Application.Projects.Queries.GetProjectById;

namespace Planor.Application.Tenants.Queries.GetTenantByName;

public class GetTenantByNameQueryValidator: AbstractValidator<GetTenantByNameQuery>
{
    public GetTenantByNameQueryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();
    }
}