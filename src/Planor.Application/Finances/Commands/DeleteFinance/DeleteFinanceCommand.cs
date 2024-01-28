using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Finances.Commands.DeleteFinance;

public class DeleteFinanceCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteFinanceCommandHandler : IRequestHandler<DeleteFinanceCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteFinanceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteFinanceCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Finances
            .Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity is null)
        {
            throw new NotFoundException(nameof(Finance), request.Id);
        }

        _context.Finances.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}