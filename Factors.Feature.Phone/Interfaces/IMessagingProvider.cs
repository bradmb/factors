using Factors.Feature.Phone.Models;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Factors.Feature.Phone.Interfaces
{
    public interface IMessagingProvider
    {
        /// <summary>
        /// Sends out the generated text message through the selected messaging provider
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        MessageSendResult SendTextMessage(string phoneNumber, string messageText);

        /// <summary>
        /// Sends out the generated text message through the selected messaging provider
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task<MessageSendResult> SendTextMessageAsync(string phoneNumber, string messageText);

        /// <summary>
        /// Sends out the generated text phone call through the selected messaging provider
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        MessageSendResult SendPhoneCall(string phoneNumber, Uri inboundEndpoint);

        /// <summary>
        /// Sends out the generated text phone call through the selected messaging provider
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task<MessageSendResult> SendPhoneCallAsync(string phoneNumber, Uri inboundEndpoint);
    }
}
