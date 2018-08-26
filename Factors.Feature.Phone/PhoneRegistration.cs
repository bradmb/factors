using Factors.Feature.Phone.Models;

namespace Factors.Feature.Phone
{
    public static partial class EmailRegistration
    {
        private static PhoneProvider Instance;

        /// <summary>
        /// Registered the Email Two-Factor registration service with Factors
        /// </summary>
        /// <param name="registration"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static FactorsRegistration UseEmailFactor(this FactorsRegistration registration, PhoneConfiguration configuration)
        {
            Instance = new PhoneProvider(configuration);

            registration.RegisterFeature<PhoneFeatureType>(Instance);
            return registration;
        }
    }
}