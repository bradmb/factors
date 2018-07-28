using Factors.Feature.Email.Models;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Factors.Feature.Email
{
    public static partial class EmailProvider
    {
        internal static EmailSendResult SendTokenMessage(string emailAddress, string token)
        {
            return SendTokenMessageAsync(emailAddress, token, false).GetAwaiter().GetResult();
        }

        internal static Task<EmailSendResult> SendTokenMessageAsync(string emailAddress, string token)
        {
            return SendTokenMessageAsync(emailAddress, token, true);
        }

        private static async Task<EmailSendResult> SendTokenMessageAsync(string emailAddress, string token, bool runAsAsync)
        {
            var message = new MailMessage();
            message.From = new MailAddress(_configuration.FromAddress, _configuration.FromName);
            message.To.Add(emailAddress);
            message.Body = token;

            var sendResult = runAsAsync
                ? await _configuration.MailProvider.SendAsync(message).ConfigureAwait(false)
                : _configuration.MailProvider.Send(message);

            return sendResult;
        }
    }
}
