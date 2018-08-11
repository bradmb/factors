using Factors.Feature.Email.Interfaces;
using Factors.Feature.Email.Models;
using PostmarkDotNet;
using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Factors.Feature.Email.Postmark
{
    public class EmailPostmarkProvider : IMailProvider
    {
        private PostmarkClient _client;

        public EmailPostmarkProvider(string serverToken)
        {
            if (String.IsNullOrWhiteSpace(serverToken))
            {
                throw new ArgumentException("In order to use Postmark, you must pass the server token to the 'serverToken' parameter.");
            }

            _client = new PostmarkClient(serverToken);
        }

        public EmailSendResult Send(MailMessage message)
        {
            var sendTask = Task.Run(async () =>
            {
                return await this.SendAsync(message);
            });

            sendTask.Wait();
            return sendTask.Result;
        }

        public async Task<EmailSendResult> SendAsync(MailMessage message)
        {
            var recipients = String.Join(";", message.To.Select(msg => msg.Address));
            var senderEmail = $"{message.From.DisplayName} <{message.From.Address}>";
            
            var postmarkMessage = new PostmarkMessage(
                senderEmail,
                recipients,
                message.Subject,
                message.IsBodyHtml ? String.Empty : message.Body,
                message.IsBodyHtml ? message.Body : String.Empty);

            var sendResult = await _client.SendMessageAsync(postmarkMessage);

            return new EmailSendResult
            {
                IsSuccess = sendResult.Status == PostmarkStatus.Success,
                Message = sendResult.Message,
                TrackingIdentifier = sendResult.MessageID.ToString()
            };
        }
    }
}
