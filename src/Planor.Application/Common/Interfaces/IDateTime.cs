namespace Planor.Application.Common.Interfaces;

public interface IDateTime
{
    DateTime Now { get; }
    long Unix => DateTimeOffset.Now.ToUnixTimeSeconds();
}