using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;

namespace Planor.Application.Reports.Queries.GetFinanceSummary;

public class GetFinanceSummaryQuery : IRequest<GetFinanceSummaryDto>
{
}

public class GetFinanceSummaryQueryHandler : IRequestHandler<GetFinanceSummaryQuery, GetFinanceSummaryDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetFinanceSummaryQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetFinanceSummaryDto> Handle(GetFinanceSummaryQuery request, CancellationToken cancellationToken)
    {
        var getFinanceSummaryDto = new GetFinanceSummaryDto();
        var sevenMonthsAgo = DateTimeOffset.UtcNow.AddMonths(-7).ToUnixTimeSeconds();

        var finances = await _context
            .Finances
            .Where(x => x.Date > sevenMonthsAgo)
            .ToListAsync(cancellationToken);

        var incomes = finances.Where(x => x.IsIncome == true).AsQueryable();
        var outcomes = finances.Where(x => x.IsIncome == false).AsQueryable();

        for (int i = 6; i >= 0; i--)
        {
            var targetDate = DateTime.UtcNow.AddMonths(-i).Month;
            
            var income = incomes.Where(x => DateTimeOffset.FromUnixTimeSeconds(x.Date).UtcDateTime.Month == targetDate).Sum(x => x.Amount);
            getFinanceSummaryDto.Income.Add(new FinanceSummaryDetailDto
            {
                Month = targetDate,
                Amount = income
            });

            var outcome = outcomes.Where(x => DateTimeOffset.FromUnixTimeSeconds(x.Date).UtcDateTime.Month == targetDate).Sum(x => x.Amount);
            getFinanceSummaryDto.Outcome.Add(new FinanceSummaryDetailDto
            {
                Month = targetDate,
                Amount = Math.Abs(outcome)
            });
        }

        return getFinanceSummaryDto;
    }
}