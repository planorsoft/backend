namespace Planor.Domain.Exceptions;

public class UnauthorizedException: Exception
{
    public UnauthorizedException(string message) : base(message) { }

    public UnauthorizedException() : base() { }
}