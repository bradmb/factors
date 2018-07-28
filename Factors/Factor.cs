using Factors.Models;

namespace Factors
{
    public class Factor
    {
        private static FactorsInstance _instance;

        public static FactorsInstance ForUser(string userAccountId)
        {
            _instance.UserAccount = userAccountId;
            return _instance;
        }

        /// <summary>
        /// Initalizes the FactorsClient instance
        /// </summary>
        /// <param name="configuration"></param>
        public static FactorsInstance Initalize(FactorsConfiguration configuration)
        {
            _instance = new FactorsInstance(configuration);
            return _instance;
        }

        public static void Dispose()
        {
            _instance._configuration.StorageDatabase.Dispose();
            _instance = null;
        }
    }
}