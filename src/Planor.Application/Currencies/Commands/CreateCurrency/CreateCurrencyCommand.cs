using MediatR;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Application.MailTemplates.Commands.CreateMailTemplate;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Currencies.Commands.CreateCurrency;

public class CreateCurrencyCommand : IRequest<int>
{
    public string Code { get; set; } = null!;
    public string? Symbol { get; set; }
    public double Rate { get; set; }
    public bool IsDefault { get; set; }
}

public class CreateCurrencyCommandHandler : IRequestHandler<CreateCurrencyCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateCurrencyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateCurrencyCommand request, CancellationToken cancellationToken)
    {
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
        
        var entity = new Currency
        {
            Code = request.Code,
            Symbol = request.Symbol,
            Rate = request.Rate,
            IsDefault = request.IsDefault
        };

        _context.Currencies.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}