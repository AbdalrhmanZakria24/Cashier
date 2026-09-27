namespace Cashier.Application.Interface
{
    public interface IEmailSeder
    {
       public Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
