using Planor.Application.Common.Interfaces;

namespace Planor.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}