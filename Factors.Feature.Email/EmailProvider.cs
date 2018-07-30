using Factors.Feature.Email.Models;
using Factors.Models.Interfaces;

namespace Factors.Feature.Email
{
    public partial class EmailProvider : IFactorsFeatureProvider
    {
        private EmailConfiguration _configuration;
        private IFactorsFeatureType _featureType;

        public EmailProvider(EmailConfiguration configuration)
        {
            _configuration = configuration;
            _featureType = new EmailFeatureType();

            this.ValidateConfiguration();
        }
    }
}
