namespace Factors.Models.Interfaces
{
    public interface IFactorsEncryptionProvider
    {
        /// <summary>
        /// Encrypts a string
        /// </summary>
        string EncryptData(string text);

        /// <summary>
        /// Decrypts a string
        /// </summary>
        string DecryptData(string encryptedText);
    }
}
