using Factors.Feature.Email.Models;

namespace Factors.Feature.Email
{
    public static partial class EmailRegistration
    {
        private static EmailProvider Instance;

        /// <summary>
        /// Registered the Email Two-Factor registration service with Factors
        /// </summary>
        /// <param name="registration"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static FactorsRegistration UseEmailFactor(this FactorsRegistration registration, EmailConfiguration configuration)
        {
            Instance = new EmailProvider(configuration);

            registration.RegisterFeature<EmailFeatureType>(Instance);
            return registration;
        }
    }
}