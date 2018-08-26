using Factors.Models.Interfaces;
using System;

namespace Factors.Feature.Phone
{
    public partial class PhoneProvider : IFactorsFeatureProvider
    {
        /// <summary>
        /// Verifies that the initial configuration is correct
        /// </summary>
        private void ValidateConfiguration()
        {
            if (_configuration.MessagingProvider == null)
            {
                throw new ArgumentException("PhoneFactor: MessagingProvider must be configured at initialization");
            }

            if (String.IsNullOrWhiteSpace(_configuration.TextMessageTemplate))
            {
                throw new ArgumentException("PhoneFactor: TextMessageTemplate must be configured at initialization");
            }

            if (!_configuration.TextMessageTemplate.Contains("@APPCODE@"))
            {
                throw new ArgumentException("PhoneFactor: TextMessageTemplate must contain the '@APPCODE@'template parameter");
            }

            if (_configuration.EnablePhoneCallSupport && _configuration.PhoneCallInboundEndpoint == null)
            {
                throw new ArgumentException("PhoneFactor: PhoneCallInboundEndpoint must be configured if EnablePhoneCallSupport is enabled");
            }

            if (_configuration.TokenExpirationTime.TotalSeconds <= 0)
            {
                throw new ArgumentException("PhoneFactor: TokenExpirationTime must be a positive number greater than zero seconds");
            }
        }
    }
}
