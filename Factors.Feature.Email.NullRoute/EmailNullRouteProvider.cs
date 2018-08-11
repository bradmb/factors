using Factors.Feature.Email.Interfaces;
using Factors.Feature.Email.Models;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Factors.Feature.Email.NullRoute
{
    public class EmailNullRouteProvider : IMailProvider
    {
        public EmailSendResult Send(MailMessage message)
        {
            return new EmailSendResult
            {
                IsSuccess = true,
                Message = "Null route, no message was sent."
            };
        }

        public Task<EmailSendResult> SendAsync(MailMessage message)
        {
            return new Task<EmailSendResult>(() =>
            {
                return new EmailSendResult
                {
                    IsSuccess = true,
                    Message = "Null route, no message was sent."
                };
            });
        }
    }
}
