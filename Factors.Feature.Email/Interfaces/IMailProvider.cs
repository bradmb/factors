using Factors.Feature.Email.Models;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Factors.Feature.Email.Interfaces
{
    public interface IMailProvider
    {
        /// <summary>
        /// Sends out the generated email through the selected mail provider
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        EmailSendResult Send(MailMessage message);

        /// <summary>
        /// Sends out the generated email through the selected mail provider
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task<EmailSendResult> SendAsync(MailMessage message);
    }
}
