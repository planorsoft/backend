using MediatR;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Projects.Commands.UpdateProject;

public class UpdateProjectCommand : IRequest
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public bool IsCompleted { get; set; }
    public int? CustomerId { get; set; }
    public int? CurrencyId { get; set; }
    public string? Description { get; set; }
    public double Price { get; set; }
}

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateProjectCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Projects
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Project), request.Id.ToString());
        }
        
        if (request.CustomerId.HasValue)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == request.CustomerId, cancellationToken: cancellationToken);
            if (customer is null) throw new NotFoundException(nameof(Customer), request.CustomerId);

            entity.Customer = customer;
        }
        
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

        entity.Title = request.Title;
        entity.IsCompleted = request.IsCompleted;
        entity.Description = string.IsNullOrEmpty(request.Description) ? entity.Description : request.Description;
        entity.Price = request.Price;

        await _context.SaveChangesAsync(cancellationToken);
    }
}