using Factors.Feature.Email.Models;
using Factors.Models.Interfaces;

namespace Factors.Feature.Email
{
    public partial class EmailInstance : IFactorFeature
    {
        private EmailConfiguration _configuration;
        private IFeatureType _featureType;

        public EmailInstance(EmailConfiguration configuration)
        {
            _configuration = configuration;
            _featureType = new EmailFeatureType();

            this.ValidateConfiguration();
        }
    }
}
