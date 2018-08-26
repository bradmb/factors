using Factors.Feature.Phone.Interfaces;
using Factors.Feature.Phone.Models;
using System;
using System.Threading.Tasks;

namespace Factors.Feature.Phone.NullRoute
{
    public class PhoneNullRouteProvider : IMessagingProvider
    {
        public MessageSendResult SendPhoneCall(string phoneNumber, Uri inboundEndpoint)
        {
            return new MessageSendResult
            {
                IsSuccess = true,
                Message = "Message discarded, I am a null route",
                TrackingIdentifier = Guid.NewGuid().ToString()
            };
        }

        public Task<MessageSendResult> SendPhoneCallAsync(string phoneNumber, Uri inboundEndpoint)
        {
            return new Task<MessageSendResult>(() =>
            {
                return new MessageSendResult
                {
                    IsSuccess = true,
                    Message = "Message discarded, I am a null route",
                    TrackingIdentifier = Guid.NewGuid().ToString()
                };
            });
        }

        public MessageSendResult SendTextMessage(string phoneNumber, string messageText)
        {
            return new MessageSendResult
            {
                IsSuccess = true,
                Message = "Message discarded, I am a null route",
                TrackingIdentifier = Guid.NewGuid().ToString()
            };
        }

        public Task<MessageSendResult> SendTextMessageAsync(string phoneNumber, string messageText)
        {
            return new Task<MessageSendResult>(() =>
            {
                return new MessageSendResult
                {
                    IsSuccess = true,
                    Message = "Message discarded, I am a null route",
                    TrackingIdentifier = Guid.NewGuid().ToString()
                };
            });
        }
    }
}
