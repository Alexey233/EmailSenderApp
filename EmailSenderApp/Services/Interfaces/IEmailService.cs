using EmailSenderApplication.Models;

namespace EmailSenderApplication.Services.Interfaces
{
    public interface IEmailService
    {
        public void SendEmail(Email requests);

        public Task<string> ShowSendEmail();
    }
}
