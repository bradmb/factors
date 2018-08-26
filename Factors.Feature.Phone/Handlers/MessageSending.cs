using Factors.Feature.Phone.Models;
using Factors.Models.Interfaces;
using System.Threading.Tasks;

namespace Factors.Feature.Phone
{
    public partial class PhoneProvider : IFactorsFeatureProvider
    {
        private MessageSendResult SendTokenMessage(string phoneNumber, string tokenMessage, bool sendAsPhoneCall)
        {
            return SendTokenMessageAsync(phoneNumber, tokenMessage, sendAsPhoneCall, false).GetAwaiter().GetResult();
        }

        private Task<MessageSendResult> SendTokenMessageAsync(string phoneNumber, string tokenMessage, bool sendAsPhoneCall)
        {
            return SendTokenMessageAsync(phoneNumber, tokenMessage, sendAsPhoneCall, true);
        }

        private async Task<MessageSendResult> SendTokenMessageAsync(string phoneNumber, string tokenMessage, bool sendAsPhoneCall, bool runAsAsync)
        {
            if (sendAsPhoneCall && runAsAsync)
            {
                return await _configuration.MessagingProvider.SendPhoneCallAsync(phoneNumber, _configuration.PhoneCallInboundEndpoint);
            }
            else if (sendAsPhoneCall)
            {
                return _configuration.MessagingProvider.SendPhoneCall(phoneNumber, _configuration.PhoneCallInboundEndpoint);
            }

            if (!sendAsPhoneCall && runAsAsync)
            {
                return await _configuration.MessagingProvider.SendTextMessageAsync(phoneNumber, tokenMessage);
            }
            else if (!sendAsPhoneCall)
            {
                return _configuration.MessagingProvider.SendTextMessage(phoneNumber, tokenMessage);
            }

            return null;
        }
    }
}
