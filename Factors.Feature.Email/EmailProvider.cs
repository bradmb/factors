using Factors.Feature.Email.Models;

namespace Factors.Feature.Email
{
    public static partial class EmailProvider
    {
        public static EmailInstance Instance;

        public static FactorsInstance InitializeEmailFactor(this FactorsInstance instance, EmailConfiguration configuration)
        {
            Instance = new EmailInstance(instance, configuration);
            return instance;
        }
    }
}