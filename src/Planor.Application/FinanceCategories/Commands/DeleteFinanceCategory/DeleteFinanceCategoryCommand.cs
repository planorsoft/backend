using MediatR;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Exceptions;

namespace Planor.Application.FinanceCategories.Commands.DeleteFinanceCategory;

public class DeleteFinanceCategoryCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteTodoItemCommandHandler : IRequestHandler<DeleteFinanceCategoryCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteTodoItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteFinanceCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.FinanceCategories
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(DeleteFinanceCategory), request.Id);
        }

        _context.FinanceCategories.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}