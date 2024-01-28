using FluentValidation;

namespace Planor.Application.Users.Commands.UploadProfileImage;

public class UploadProfileImageCommandValidator : AbstractValidator<UploadProfileImageCommand>
{
    public UploadProfileImageCommandValidator()
    {
        RuleFor(v => v.File)
            .NotNull();
        
        RuleFor(x => x.File.ContentType)
            .NotNull()
            .Must(x => x.Equals("image/jpeg") || x.Equals("image/jpg") || x.Equals("image/png"))
            .WithMessage("jpeg veya png dosya kullanabilirsiniz.");
    }
}