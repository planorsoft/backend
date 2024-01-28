using System.Globalization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Finances.Commands.UpdateFinance;

public class UpdateFinanceCommand : IRequest
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string? Description { get; set; }
    public float Amount { get; set; }
    public long Date { get; set; }
    
    public int? CustomerId { get; set; }

    public int? ProjectId { get; set; }
}

public class UpdateFinanceCommandHandler : IRequestHandler<UpdateFinanceCommand>
{
    private readonly IApplicationDbContext _context;


    public UpdateFinanceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateFinanceCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Finances
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Finance), request.Id.ToString());
        }
        
        var category = await _context
            .FinanceCategories
            .FirstOrDefaultAsync(x => x.Id == request.CategoryId, cancellationToken);

        if (category is null) throw new NotFoundException(nameof(FinanceCategory), request.CategoryId);

        entity.Category = category;
        
        if (request.CustomerId.HasValue)
        {
            var customer = await _context
                .Customers
                .FirstOrDefaultAsync(x => x.Id == request.CustomerId.Value, cancellationToken);

            if (customer is null) throw new NotFoundException(nameof(Customer), request.CustomerId.Value);
            entity.Customer = customer;
        }
        
        if (request.ProjectId.HasValue)
        {
            var project = await _context
                .Projects
                .FirstOrDefaultAsync(x => x.Id == request.ProjectId.Value, cancellationToken);

            if (project is null) throw new NotFoundException(nameof(Project), request.ProjectId.Value);
            
            if (request.CustomerId.HasValue && project.CustomerId != request.CustomerId.Value)
            {
                throw new BadRequestException("Verilen proje, verilen müşterinin projesi değil.");
            }
            
            entity.Project = project;
        }
        
        if (request.Amount >= 0)
        {
            entity.IsIncome = true;
        }

        entity.Description = request.Description;
        entity.Amount = request.Amount;
        entity.Date = request.Date;

        await _context.SaveChangesAsync(cancellationToken);
    }
}