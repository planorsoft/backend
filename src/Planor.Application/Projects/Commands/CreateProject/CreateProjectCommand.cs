using MediatR;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;
using Planor.Domain.Events;
using Planor.Domain.Exceptions;

namespace Planor.Application.Projects.Commands.CreateProject;

public class CreateProjectCommand : IRequest<int>
{
    public string Title { get; set; } = null!;
    public bool IsCompleted { get; set; }
    public int CustomerId { get; set; }
    public int? CurrencyId { get; set; }
    public string? Description { get; set; }
    public double Price { get; set; }
}

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateProjectCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == request.CustomerId, cancellationToken: cancellationToken);
        if (customer is null) throw new NotFoundException(nameof(Customer), request.CustomerId);

        if (customer.IsPotantial)
        {
            customer.IsPotantial = false;
            _context.Customers.Update(customer);
        }

        var entity = new Project
        {
            Title = request.Title,
            IsCompleted = request.IsCompleted,
            Customer = customer,
            Description = request.Description,
            Price = request.Price
        };

        if (request.CurrencyId.HasValue)
        {
            var currency = await _context.Currencies.FirstOrDefaultAsync(x => x.Id == request.CurrencyId.Value, cancellationToken);
            if (currency is null) throw new NotFoundException(nameof(Currency), request.CurrencyId);

            entity.Currency = currency;
        }
        
        if (entity.Currency is null && request.Price > 0)
        {
            var currency = await _context.Currencies.FirstOrDefaultAsync(x => x.IsDefault, cancellationToken);
            if (currency is null) throw new NotFoundException(nameof(Currency), "Default");

            entity.Currency = currency;
        }
        
        _context.Projects.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}