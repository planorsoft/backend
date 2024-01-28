using MediatR;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Duties.Commands.UpdateDutyOrders;

public class UpdateDutyOrdersCommand : IRequest
{
    public IList<UpdateDutyOrdersDto> DutyOrders { get; set; }

    public UpdateDutyOrdersCommand(IList<UpdateDutyOrdersDto> dutyOrders)
    {
        DutyOrders = dutyOrders;
    }
}

public class UpdateDutyOrdersCommandHandler : IRequestHandler<UpdateDutyOrdersCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateDutyOrdersCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateDutyOrdersCommand request, CancellationToken cancellationToken)
    {
        foreach (var dutyOrder in request.DutyOrders)
        {
            var duty = await _context.Duties.FirstOrDefaultAsync(x => x.Id == dutyOrder.Id, cancellationToken);
            if (duty is null) throw new NotFoundException(nameof(Duty), dutyOrder.Id.ToString());

            if (duty.Order != dutyOrder.Order)
            {
                duty.Order = dutyOrder.Order;
            }

            if (duty.CategoryId != dutyOrder.CategoryId)
            {
                duty.CategoryId = dutyOrder.CategoryId;
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}