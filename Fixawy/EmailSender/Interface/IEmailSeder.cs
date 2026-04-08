namespace Fixawy.EmailSender.Interface
{
    public interface IEmailSeder
    {
       public Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
