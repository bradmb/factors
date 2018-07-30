namespace Factors.Models.Interfaces
{
    public interface IFactorsConfiguration
    {
        /// <summary>
        /// The database to use for storage of user factors data
        /// </summary>
        IFactorsDatabaseProvider StorageDatabase { get; set; }

        /// <summary>
        /// The encryption to use for securing user information
        /// </summary>
        IFactorsEncryptionProvider EncryptionProvider { get; set; }

        /// <summary>
        /// The token generator to use for sending out two-factor tokens
        /// </summary>
        IFactorsTokenProvider TokenProvider { get; set; }
    }
}
