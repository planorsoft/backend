using FluentValidation;

namespace Planor.Application.Customers.Commands.UploadCustomerImage;

public class UploadCustomerImageCommandValidator: AbstractValidator<UploadCustomerImageCommand>
{
    public UploadCustomerImageCommandValidator()
    {
        RuleFor(v => v.File)
            .NotNull();
        
        RuleFor(x => x.File.ContentType)
            .NotNull()
            .Must(x => x.Equals("image/jpeg") || x.Equals("image/jpg") || x.Equals("image/png"))
            .WithMessage("jpeg veya png dosya kullanabilirsiniz.");
    }
}