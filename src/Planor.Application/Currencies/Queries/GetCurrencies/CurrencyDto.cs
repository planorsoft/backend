using Planor.Application.Common.Mappings;
using Planor.Domain.Entities;

namespace Planor.Application.Currencies.Queries.GetCurrencies;

public class CurrencyDto : IMapFrom<Currency>
{
    public int Id { get; set; }
    
    public string Code { get; set; } = null!;
    
    public string? Symbol { get; set; }
    
    public double Rate { get; set; }
    
    public bool IsDefault { get; set; }
}