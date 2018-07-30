using Factors.Models;
using Factors.Models.Exception;
using Factors.Models.Interfaces;

namespace Factors
{
    public class Factors
    {
        internal static FactorsApplication Instance;
        public static FactorsRegistration Registration;

        public static FactorsApplication ForUser(string userAccountId)
        {
            Instance.UserAccount = userAccountId;
            return Instance;
        }

        /// <summary>
        /// Initalizes the FactorsClient instance
        /// </summary>
        /// <param name="configuration"></param>
        public static FactorsRegistration Initalize(FactorsConfiguration configuration)
        {
            Registration = new FactorsRegistration();
            Instance = new FactorsApplication(configuration);

            return Registration;
        }

        public static void Dispose()
        {
            Instance.Configuration.StorageDatabase.Dispose();
            Instance = null;
        }
    }
}