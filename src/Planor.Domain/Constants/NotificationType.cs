namespace Planor.Domain.Constants;

public abstract class NotificationType
{
    public const string MailConfirmation = nameof(MailConfirmation);
    public const string InviteUser = nameof(InviteUser);
    public const string ForgotMail = nameof(ForgotMail);
    public const string EventTime = nameof(EventTime);
    
    public static readonly string[] List = new[]
    {
        MailConfirmation, InviteUser, ForgotMail, EventTime
    };
}