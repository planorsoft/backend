using System.Globalization;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Application.Tags.Commands.UpdateTag;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommand : IRequest
{
    public int Id { get; set; }
    
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
}

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly CultureInfo _culture;


    public UpdateCustomerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
        _culture = new CultureInfo("en-US");
    }

    public async Task Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Customers
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Customer), request.Id.ToString());
        }
        
        if (!string.IsNullOrEmpty(request.CurrencyCode))
        {
            var currency = await _context.Currencies.FirstOrDefaultAsync(x => x.Code == request.CurrencyCode, cancellationToken);
            if (currency is null) throw new NotFoundException(nameof(Currency), request.CurrencyCode);
            entity.Currency = currency;
        }

        entity.Name = _culture.TextInfo.ToTitleCase(request.Name);
        entity.IsCompany = request.IsCompany;
        entity.Address = request.Address;
        entity.City = request.City;
        entity.District = request.District;
        entity.PostCode = request.PostCode;
        entity.Country = request.Country;
        entity.PhoneNumber = request.PhoneNumber;
        entity.Website = request.Website;
        entity.GovernmentId = request.GovernmentId;
        entity.IsPotantial = request.IsPotantial;

        await _context.SaveChangesAsync(cancellationToken);
    }
}