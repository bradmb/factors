using Factors.Models;

namespace Factors
{
    public class Factor
    {
        private static FactorsInstance Instance;

        public static FactorsInstance ForUser(string userAccountId)
        {
            Instance.UserAccount = userAccountId;
            return Instance;
        }

        /// <summary>
        /// Initalizes the FactorsClient instance
        /// </summary>
        /// <param name="configuration"></param>
        public static FactorsInstance Initalize(FactorsConfiguration configuration)
        {
            Instance = new FactorsInstance(configuration);
            return Instance;
        }

        public static void Dispose()
        {
            Instance.Configuration.StorageDatabase.Dispose();
            Instance = null;
        }
    }
}