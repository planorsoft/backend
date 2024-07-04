namespace Planor.Application.Reports.Queries.GetFinanceSummary;

public class GetFinanceSummaryDto
{
    public List<FinanceSummaryDetailDto> Outcome { get; set; } = new List<FinanceSummaryDetailDto>();

    public List<FinanceSummaryDetailDto> Income { get; set; } = new List<FinanceSummaryDetailDto>();
}

public class FinanceSummaryDetailDto
{
    public int Month { get; set; }
    public float Amount { get; set; }
}