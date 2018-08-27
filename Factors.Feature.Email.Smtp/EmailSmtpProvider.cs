using Factors.Feature.Email.Interfaces;
using Factors.Feature.Email.Models;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Factors.Feature.Email.Smtp
{
    public class EmailSmtpProvider : IMailProvider
    {
        private readonly SmtpClient _client;
        
        public EmailSmtpProvider(SmtpClient smtpClient)
        {
            _client = smtpClient;
        }

        public EmailSendResult Send(MailMessage message)
        {
            return this.SendAsync(message, false).GetAwaiter().GetResult();
        }

        public Task<EmailSendResult> SendAsync(MailMessage message)
        {
            return SendAsync(message, true);
        }

        private async Task<EmailSendResult> SendAsync(MailMessage message, bool runAsAsync)
        {
            try
            {
                if (runAsAsync)
                {
                    await _client.SendMailAsync(message).ConfigureAwait(false);
                }
                else
                {
                    _client.Send(message);
                }

                return new EmailSendResult
                {
                    IsSuccess = true,
                    Message = "Message Sent. There will be no tracking identifier as this message was sent over SMTP."
                };
            }
            catch (Exception ex)
            {
                return new EmailSendResult
                {
                    IsSuccess = false,
                    Message = $"There was an error when trying to send the message: {ex.Message}"
                };
            }
        }
    }
}
