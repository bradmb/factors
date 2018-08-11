using Factors.Feature.Email.Interfaces;
using Factors.Feature.Email.Models;
using PostmarkDotNet;
using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Factors.Feature.Email.Postmark
{
    public class EmailPostmarkTemplateProvider<TSendGridTemplateModel> : IMailProvider
    {
        private PostmarkClient _client;

        private string _emailTemplateId;
        private TSendGridTemplateModel _emailTemplate;

        public EmailPostmarkTemplateProvider(string serverToken, string emailTemplateId, TSendGridTemplateModel emailTemplate)
        {
            if (String.IsNullOrWhiteSpace(serverToken))
            {
                throw new ArgumentException("In order to use Postmark, you must pass the server token to the 'serverToken' parameter.");
            }
            else if (String.IsNullOrWhiteSpace(emailTemplateId) || emailTemplate == null)
            {
                throw new ArgumentException("To use Postmark's templating system, you must pass both a template object (via 'emailTemplate') and the template id (via 'emailTemplateId').");
            }

            _emailTemplateId = emailTemplateId;
            _emailTemplate = emailTemplate;

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

            var templateSendResult = await _client.SendEmailWithTemplateAsync<TSendGridTemplateModel>
                (_emailTemplateId,
                _emailTemplate,
                recipients,
                senderEmail);

            return new EmailSendResult
            {
                IsSuccess = templateSendResult.Status == PostmarkStatus.Success,
                Message = templateSendResult.Message,
                TrackingIdentifier = templateSendResult.MessageID.ToString()
            };
        }
    }

}
