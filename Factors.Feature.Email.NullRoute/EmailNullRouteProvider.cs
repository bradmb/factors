using Factors.Feature.Email.Interfaces;
using Factors.Feature.Email.Models;
using System;
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
                Message = "Message discarded, I am a null route",
                TrackingIdentifier = Guid.NewGuid().ToString()
            };
        }

        public Task<EmailSendResult> SendAsync(MailMessage message)
        {
            return new Task<EmailSendResult>(() =>
            {
                return new EmailSendResult
                {
                    IsSuccess = true,
                    Message = "Message discarded, I am a null route",
                    TrackingIdentifier = Guid.NewGuid().ToString()
                };
            });
        }
    }
}
