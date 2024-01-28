using FluentValidation.Results;

namespace Planor.Domain.Exceptions;

public class ValidationException : Exception
{
    public ValidationException()
        : base("Bir veya birden fazla validasyon hatası meydana geldi.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    public IDictionary<string, string[]> Errors { get; }
}