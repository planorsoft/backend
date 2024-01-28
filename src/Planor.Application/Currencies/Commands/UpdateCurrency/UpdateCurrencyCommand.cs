using MediatR;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Application.Tags.Commands.UpdateTag;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Currencies.Commands.UpdateCurrency;

public class UpdateCurrencyCommand : IRequest
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public string? Symbol { get; set; }
    public double Rate { get; set; }
    public bool IsDefault { get; set; }
}

public class UpdateCurrencyCommandHandler : IRequestHandler<UpdateCurrencyCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateCurrencyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateCurrencyCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Currencies
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Currency), request.Id.ToString());
        }
        
        var defaultCurrency = await _context
            .Currencies
            .FirstOrDefaultAsync(x => x.IsDefault, cancellationToken);
        
        if (defaultCurrency is not null)
        {
            if (defaultCurrency.IsDefault && request.IsDefault)
            {
                throw new BadRequestException($"Varsayılan döviz: {defaultCurrency.Code}, 2 adet varsayılan belirlenemez.");
            }
        }

        entity.Code = request.Code;
        entity.Symbol = request.Symbol;
        entity.Rate = request.Rate;
        entity.IsDefault = request.IsDefault;

        await _context.SaveChangesAsync(cancellationToken);
    }
}