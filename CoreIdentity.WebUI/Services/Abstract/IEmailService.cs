namespace CoreIdentity.WebUI.Services.Abstract
{
    public interface IEmailService
    {
        Task SendResetPasswordEmail(string resetEmailLink,string ToEmail);
    }
}
