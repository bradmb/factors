using Factors.Feature.Email.Models;
using Factors.Models.Interfaces;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Factors.Feature.Email
{
    public partial class EmailInstance : IFactorFeature
    {
        private EmailSendResult SendTokenMessage(string emailAddress, string token)
        {
            return SendTokenMessageAsync(emailAddress, token, false).GetAwaiter().GetResult();
        }

        private Task<EmailSendResult> SendTokenMessageAsync(string emailAddress, string token)
        {
            return SendTokenMessageAsync(emailAddress, token, true);
        }

        private async Task<EmailSendResult> SendTokenMessageAsync(string emailAddress, string token, bool runAsAsync)
        {
            //
            // Configure the base message object
            //
            var message = new MailMessage
            {
                From = new MailAddress(_configuration.FromAddress, _configuration.FromName)
            };

            message.To.Add(emailAddress);

            //
            // Setup our subject line
            //
            var subjectLine = _configuration.SubjectLineTemplate;
            if (String.IsNullOrWhiteSpace(subjectLine)) {
                subjectLine = $"Your {_configuration.FromName} Verification Code";
            }

            message.Subject = subjectLine;

            //
            // Populate the message body
            //
            var messageBody = _configuration.MessageBodyTemplate;
            if (String.IsNullOrWhiteSpace(messageBody))
            {
                messageBody = "Your verification code is: {{verification-code}}";
            } else
            {
                message.IsBodyHtml = _configuration.MessageBodyTemplateIsHTML;
            }

            messageBody = messageBody.Replace("{{verification-code}}", token);
            message.Body = messageBody;

            //
            // And finally send the message out
            //
            var sendResult = runAsAsync
                ? await _configuration.MailProvider.SendAsync(message).ConfigureAwait(false)
                : _configuration.MailProvider.Send(message);

            return sendResult;
        }
    }
}
