using MediatR;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;

namespace Planor.Application.DutySizes.Commands.CreateDutySize;

public class CreateDutySizeCommand : IRequest<int>
{
    public string Name { get; set; } = null!;
    public int Score { get; set; }
}

public class CreateDutySizeCommandHandler : IRequestHandler<CreateDutySizeCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateDutySizeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateDutySizeCommand request, CancellationToken cancellationToken)
    {
        var entity = new DutySize
        {
            Name = request.Name,
            Score = request.Score
        };

        _context.DutySizes.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}