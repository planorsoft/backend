using MediatR;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;
using Planor.Domain.Events;

namespace Planor.Application.Tags.Commands.CreateTag;

public class CreateTagCommand : IRequest<int>
{
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
}

public class CreateTodoItemCommandHandler : IRequestHandler<CreateTagCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateTodoItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateTagCommand request, CancellationToken cancellationToken)
    {
        var entity = new Tag
        {
            Name = request.Name,
            Slug = request.Slug
        };

        entity.AddDomainEvent(new TagCreatedEvent(entity));

        _context.Tags.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}