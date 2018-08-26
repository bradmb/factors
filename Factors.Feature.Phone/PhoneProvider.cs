using Factors.Feature.Phone.Models;
using Factors.Models.Interfaces;

namespace Factors.Feature.Phone
{
    public partial class PhoneProvider : IFactorsFeatureProvider
    {
        private PhoneConfiguration _configuration;
        private IFactorsFeatureType _featureType;

        public PhoneProvider(PhoneConfiguration configuration)
        {
            _configuration = configuration;
            _featureType = new PhoneFeatureType();

            this.ValidateConfiguration();
        }
    }
}
