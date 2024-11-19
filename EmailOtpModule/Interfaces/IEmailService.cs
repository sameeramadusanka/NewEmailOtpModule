public interface IEmailService
{
    bool ValidateUserEmail(string? userEmail);
    bool SendEmail(string email, string otp);
    void EmailSentOut(string email, string emailBody);
}