using Factors.Models;
using Factors.Models.Exception;
using Factors.Models.Interfaces;

namespace Factors
{
    public class Factors
    {
        internal static FactorsInstance Instance;
        public static FactorsRegistration Registration;

        public static FactorsInstance ForUser(string userAccountId)
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
            Instance = new FactorsInstance(configuration);

            return Registration;
        }

        public static void Dispose()
        {
            Instance.Configuration.StorageDatabase.Dispose();
            Instance = null;
        }
    }
}