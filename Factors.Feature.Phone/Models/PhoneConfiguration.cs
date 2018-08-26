using Factors.Feature.Phone.Interfaces;
using System;

namespace Factors.Feature.Phone.Models
{
    public class PhoneConfiguration
    {
        /// <summary>
        /// If enabled, will allow outbound phone calls with
        /// verification tokens. This requires your application
        /// to supply an endpoint that can provide the messaging
        /// provider with the text to say on the phone call
        /// </summary>
        public bool EnablePhoneCallSupport { get; set; }

        /// <summary>
        /// This is the URL that will be called by the phone call
        /// messaging provider when a phone call is connected, which
        /// will then provide the phone call text that will be read
        /// to the end user
        /// </summary>
        public Uri PhoneCallInboundEndpoint { get; set; }

        /// <summary>
        /// The template that is used on outbound text messages. Can include
        /// "@APPNAME@" to auto-populate the application name. Must include
        /// "@APPCODE@", as that is what is replaced with the verification token.
        /// Defaults to: "Your @APPNAME@ verification code is @APPCODE@"
        /// </summary>
        public string TextMessageTemplate { get; set; } = "Your @APPNAME@ verification code is: @APPCODE@";

        /// <summary>
        /// The message delivery service you want to use
        /// for sending out text messages and phone calls
        /// </summary>
        public IMessagingProvider MessagingProvider { get; set; }

        /// <summary>
        /// The amount of time before an email token
        /// will expire and become unuseable
        /// </summary>
        public TimeSpan TokenExpirationTime { get; set; }
    }
}
