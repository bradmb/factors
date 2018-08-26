using Factors.Feature.Phone.Models;
using Factors.Models.Interfaces;
using System;
using System.Threading.Tasks;

namespace Factors.Feature.Phone
{
    public partial class PhoneProvider : IFactorsFeatureProvider
    {
        private MessageSendResult SendTokenMessage(string phoneNumber, string tokenMessage, Guid tokenId, bool sendAsPhoneCall)
        {
            return SendTokenMessageAsync(phoneNumber, tokenMessage, tokenId, sendAsPhoneCall, false).GetAwaiter().GetResult();
        }

        private Task<MessageSendResult> SendTokenMessageAsync(string phoneNumber, string tokenMessage, Guid tokenId, bool sendAsPhoneCall)
        {
            return SendTokenMessageAsync(phoneNumber, tokenMessage, tokenId, sendAsPhoneCall, true);
        }

        private async Task<MessageSendResult> SendTokenMessageAsync(string phoneNumber, string tokenMessage, Guid tokenId, bool sendAsPhoneCall, bool runAsAsync)
        {
            if (sendAsPhoneCall)
            {
                var phoneEndpoint = new Uri($"{_configuration.PhoneCallInboundEndpoint}?tokenId={tokenId}");

                return runAsAsync
                    ? await _configuration.MessagingProvider.SendPhoneCallAsync(phoneNumber, phoneEndpoint).ConfigureAwait(false)
                    : _configuration.MessagingProvider.SendPhoneCall(phoneNumber, phoneEndpoint);
            }

            return runAsAsync
                ? await _configuration.MessagingProvider.SendTextMessageAsync(phoneNumber, tokenMessage).ConfigureAwait(false)
                : _configuration.MessagingProvider.SendTextMessage(phoneNumber, tokenMessage);
        }
    }
}
