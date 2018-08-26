using System;
using System.Threading.Tasks;
using Factors.Feature.Phone.Interfaces;
using Factors.Feature.Phone.Models;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Factors.Feature.Phone.Twilio
{
    public class PhoneTwilioProvider : IMessagingProvider
    {
        private readonly string _sid;
        private readonly string _token;
        private readonly PhoneNumber _phoneNumber;

        public PhoneTwilioProvider(string accountSid, string authToken, string phoneNumber)
        {
            _sid = accountSid;
            _token = authToken;
            _phoneNumber = new PhoneNumber(phoneNumber);

            TwilioClient.Init(accountSid, authToken);
        }

        /// <summary>
        /// Sends a text message out through Twilio
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="messageText"></param>
        /// <returns></returns>
        public MessageSendResult SendTextMessage(string phoneNumber, string messageText)
        {
            var sendResult = MessageResource.Create(
                from: _phoneNumber,
                to: new PhoneNumber(phoneNumber),
                body: messageText
            );

            return new MessageSendResult
            {
                IsSuccess = sendResult.Status != MessageResource.StatusEnum.Failed,
                Message = sendResult.Status != MessageResource.StatusEnum.Failed
                    ? "Message sent to Twilio for processing and delivery"
                    : sendResult.ErrorMessage,
                TrackingIdentifier = sendResult.Sid
            };
        }

        /// <summary>
        /// Sends a text message out through Twilio
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="messageText"></param>
        /// <returns></returns>
        public async Task<MessageSendResult> SendTextMessageAsync(string phoneNumber, string messageText)
        {
            var sendResult = await MessageResource.CreateAsync(
                from: _phoneNumber,
                to: new PhoneNumber(phoneNumber),
                body: messageText
            );

            return new MessageSendResult
            {
                IsSuccess = sendResult.Status != MessageResource.StatusEnum.Failed,
                Message = sendResult.Status != MessageResource.StatusEnum.Failed
                    ? "Message sent to Twilio for processing and delivery"
                    : sendResult.ErrorMessage,
                TrackingIdentifier = sendResult.Sid
            };
        }

        public MessageSendResult SendPhoneCall(string phoneNumber, Uri messageUrl)
        {
            var sendResult = CallResource.Create(
                from: _phoneNumber,
                to: new PhoneNumber(phoneNumber),
                url: messageUrl
            );

            return new MessageSendResult
            {
                IsSuccess = sendResult.Status != MessageResource.StatusEnum.Failed,
                Message = sendResult.Status != MessageResource.StatusEnum.Failed
                    ? "Outbound phone call initiated"
                    : "There was an issue starting the phone call, please check your Twilio logs",
                TrackingIdentifier = sendResult.Sid
            };
        }

        public async Task<MessageSendResult> SendPhoneCallAsync(string phoneNumber, Uri messageUrl)
        {
            var sendResult = await CallResource.CreateAsync(
                from: _phoneNumber,
                to: new PhoneNumber(phoneNumber),
                url: messageUrl
            );

            return new MessageSendResult
            {
                IsSuccess = sendResult.Status != MessageResource.StatusEnum.Failed,
                Message = sendResult.Status != MessageResource.StatusEnum.Failed
                    ? "Outbound phone call initiated"
                    : "There was an issue starting the phone call, please check your Twilio logs",
                TrackingIdentifier = sendResult.Sid
            };
        }
    }
}
