using Factors.Feature.Email.Models;
using System.Net.Mail;

namespace Factors.Feature.Email
{
    public static partial class EmailProvider
    {
        private static EmailConfiguration _configuration;
        private static SmtpClient _client;

        public static FeatureType FeatureType { get; private set; }

        public static FactorsInstance InitializeEmailFactor(this FactorsInstance instance, EmailConfiguration configuration)
        {
            _configuration = configuration;
            FeatureType = new FeatureType();

            _client = new SmtpClient(_configuration.HostName, configuration.Port)
            {
                EnableSsl = _configuration.UseSSL
            };

            return instance;
        }
    }
}