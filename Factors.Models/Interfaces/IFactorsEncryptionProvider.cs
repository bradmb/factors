namespace Factors.Models.Interfaces
{
    public interface IFactorsEncryptionProvider
    {
        /// <summary>
        /// Hashes a string
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        string HashData(string text);

        /// <summary>
        /// Verifies that the passed value matches the hash
        /// </summary>
        /// <param name="text"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        bool VerifyHash(string text, string hash);
    }
}
