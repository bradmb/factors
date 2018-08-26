using Factors.Models.Interfaces;

namespace Factors.Models
{
    public class FactorsConfiguration : IFactorsConfiguration
    {
        /// <summary>
        /// The name of the application using Factors
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// The database to use for storage of user factors data
        /// </summary>
        public IFactorsDatabaseProvider StorageDatabase { get; set; }

        /// <summary>
        /// The encryption to use for securing user information
        /// </summary>
        public IFactorsEncryptionProvider EncryptionProvider { get; set; }

        /// <summary>
        /// The token generator to use for sending out two-factor tokens
        /// </summary>
        public IFactorsTokenProvider TokenProvider { get; set; }
    }
}
