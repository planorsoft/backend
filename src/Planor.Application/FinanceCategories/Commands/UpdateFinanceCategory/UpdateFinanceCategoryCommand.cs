﻿using MediatR;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Exceptions;

namespace Planor.Application.FinanceCategories.Commands.UpdateFinanceCategory;

public class UpdateFinanceCategoryCommand : IRequest
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
}

public class UpdateTodoItemCommandHandler : IRequestHandler<UpdateFinanceCategoryCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTodoItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateFinanceCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.FinanceCategories
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(UpdateFinanceCategory), request.Id.ToString());
        }

        entity.Name = request.Name;

        await _context.SaveChangesAsync(cancellationToken);
    }
}