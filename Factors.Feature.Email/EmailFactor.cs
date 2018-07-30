using Factors.Feature.Email.Models;
using Factors.Models;

namespace Factors.Feature.Email
{
    public static partial class EmailFactor
    {
        private static EmailInstance Instance;

        public static FactorsRegistration UseEmailFactor(this FactorsRegistration registration, EmailConfiguration configuration)
        {
            Instance = new EmailInstance(configuration);

            registration.RegisterFeature<EmailFeatureType>(Instance);
            return registration;
        }
    }
}