using EmailSenderApplication.Models;
using EmailSenderApplication.Services.Interfaces;
using MailKit.Net.Smtp; 
using MailKit.Security;
using MimeKit;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using EmailSenderApplication.Data;

namespace EmailSenderApplication.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly ApplicationContext _applicationContext;
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration, ApplicationContext applicationContext)
        {
            _configuration = configuration;
            _applicationContext = applicationContext;
        }

        public bool IsValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        
        public MimeMessage CreateEmailMessage(Email requests)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(MailboxAddress.Parse(_configuration.GetValue<string>("EmailLogin")));
            emailMessage.Subject = requests.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = requests.Text
            };


            return emailMessage;
        }

        public MailKit.Net.Smtp.SmtpClient CreateSmtp()
        {
            var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(_configuration.GetValue<string>("EmailHost"), 25, SecureSocketOptions.StartTls);
            smtp.Authenticate(_configuration.GetValue<string>("EmailLogin"), _configuration.GetValue<string>("EmailPassword"));


            return smtp;
        }

        public Task SendMessageToOne(MimeMessage emailMessage, MailKit.Net.Smtp.SmtpClient smtpClient, ref Email requests)
        {
            try
            {
                emailMessage.To.Clear();
                emailMessage.To.Add(MailboxAddress.Parse(requests.Recipient));
                smtpClient.Send(emailMessage);
                requests.IsSuccessfulSend = true;
            }
            catch
            {
                requests.IsSuccessfulSend = false;
            }
            return Task.CompletedTask;
        }

        public Task SendMessageToMany(MimeMessage emailMessage, MailKit.Net.Smtp.SmtpClient smtpClient, List<string> carbonCopyRecipients)
        {
            foreach (var email in carbonCopyRecipients)
            {
                try
                {
                    emailMessage.To.Clear();
                    emailMessage.To.Add(MailboxAddress.Parse(email));
                    smtpClient.Send(emailMessage);
                }
                catch { }
            }
            return Task.CompletedTask;
        }
        

        public async void SendEmail(Email requests)
        {
            if (IsValid(requests.Recipient))
            {
                var emailMessage = CreateEmailMessage(requests);
                using var smtp = CreateSmtp();
                

                if (requests.CarbonCopyRecipients.Count == 0)
                {
                    SendMessageToOne(emailMessage, smtp, ref requests);
                }

                else
                {
                    await SendMessageToOne(emailMessage, smtp, ref requests);
                    await SendMessageToMany(emailMessage, smtp, requests.CarbonCopyRecipients);
                }



                _applicationContext.Email.Add(requests);
                _applicationContext.SaveChanges();
                smtp.Disconnect(true);
            }
            else
            {
                requests.IsSuccessfulSend = false;
                _applicationContext.Email.Add(requests);
                _applicationContext.SaveChanges();
            }
        }



        public async Task<string> ShowSendEmail()
        {
            var allEmails = await _applicationContext.Email.ToListAsync();

            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };

            string jsonEmail = JsonConvert.SerializeObject(allEmails, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });

            return jsonEmail;
        }
    }
}
