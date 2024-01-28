using MediatR;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;

namespace Planor.Application.FinanceCategories.Commands.CreateFinanceCategory;

public class CreateFinanceCategoryCommand : IRequest<int>
{
    public string Name { get; set; } = null!;
}

public class CreateTodoItemCommandHandler : IRequestHandler<CreateFinanceCategoryCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateTodoItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateFinanceCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = new FinanceCategory
        {
            Name = request.Name,
        };

        _context.FinanceCategories.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}