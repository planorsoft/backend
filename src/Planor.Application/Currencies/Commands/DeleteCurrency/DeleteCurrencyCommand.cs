using MediatR;
using Planor.Application.Common.Interfaces;
using Planor.Application.Tags.Commands.DeleteTag;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Currencies.Commands.DeleteCurrency;

public class DeleteCurrencyCommand: IRequest
{
    public int Id { get; set; }
}

public class DeleteCurrencyCommandHandler : IRequestHandler<DeleteCurrencyCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteCurrencyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteCurrencyCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Currencies
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Currency), request.Id.ToString());
        }

        _context.Currencies.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}