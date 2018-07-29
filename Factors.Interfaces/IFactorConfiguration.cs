namespace Factors.Interfaces
{
    public interface IFactorConfiguration
    {
        /// <summary>
        /// The database to use for storage of user factors data
        /// </summary>
        IFactorsDatabase StorageDatabase { get; set; }

        /// <summary>
        /// The encryption to use for securing user information
        /// </summary>
        IFactorsEncryption EncryptionProvider { get; set; }

        /// <summary>
        /// The token generator to use for sending out two-factor tokens
        /// </summary>
        ITokenProvider TokenProvider { get; set; }
    }
}
