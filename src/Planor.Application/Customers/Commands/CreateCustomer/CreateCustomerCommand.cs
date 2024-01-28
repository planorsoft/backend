using System.Globalization;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Application.Tags.Commands.CreateTag;
using Planor.Domain.Entities;
using Planor.Domain.Events;
using Planor.Domain.Exceptions;

namespace Planor.Application.Customers.Commands.CreateCustomer;

public class CreateCustomerCommand : IRequest<int>
{
    public string Name { get; set; } = null!;

    public bool IsCompany { get; set; } = false;

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? District { get; set; }

    public string? PostCode { get; set; }

    public string? Country { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Website { get; set; }

    public string? GovernmentId { get; set; }

    public string? CurrencyCode { get; set; }

    public bool IsPotantial { get; set; }

    public IList<string?> Contacts { get; set; } = new List<string?>();
}

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly CultureInfo _culture;
    
    public CreateCustomerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
        _culture = new CultureInfo("en-US");
    }

    public async Task<int> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = new Customer()
        {
            Name = _culture.TextInfo.ToTitleCase(request.Name),
            IsCompany = request.IsCompany,
            Address = request.Address,
            City = request.City,
            District = request.District,
            PostCode = request.PostCode,
            Country = request.Country,
            PhoneNumber = request.PhoneNumber,
            Website = request.Website,
            GovernmentId = request.GovernmentId,
            IsPotantial = request.IsPotantial,
        };

        var currency = await _context
            .Currencies
            .FirstOrDefaultAsync(x => x.Code == request.CurrencyCode, cancellationToken);

        if (currency is null)
        {
            currency = await _context
                .Currencies
                .FirstOrDefaultAsync(x => x.IsDefault, cancellationToken);

            if (currency is null) throw new NotFoundException(nameof(Currency));
        }

        entity.Currency = currency;

        _context.Customers.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}