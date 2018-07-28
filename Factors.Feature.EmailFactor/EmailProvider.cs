using Factors.Feature.Email.Models;

namespace Factors.Feature.Email
{
    public static partial class EmailProvider
    {
        private static EmailConfiguration _configuration;

        public static FeatureType FeatureType { get; private set; }

        public static FactorsInstance InitializeEmailFactor(this FactorsInstance instance, EmailConfiguration configuration)
        {
            _configuration = configuration;
            FeatureType = new FeatureType();

            return instance;
        }
    }
}