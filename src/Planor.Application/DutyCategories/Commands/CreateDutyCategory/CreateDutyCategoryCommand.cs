using MediatR;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.DutyCategories.Commands.CreateDutyCategory;

public class CreateDutyCategoryCommand : IRequest<int>
{
    public string Title { get; set; } = null!;
}

public class CreateDutyCategoryCommandHandler : IRequestHandler<CreateDutyCategoryCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateDutyCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateDutyCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = new DutyCategory
        {
            Title = request.Title,
        };

        _context.DutyCategories.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}