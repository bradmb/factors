using Factors.Models.Interfaces;

namespace Factors.Encryption.BCryptStandard
{
    public class Provider : IFactorsEncryptionProvider
    {
        public string HashData(string text)
        {
            text = text.ToLower();
            return BCrypt.Net.BCrypt.HashString(text);
        }

        public bool VerifyHash(string text, string hash)
        {
            text = text.ToLower();
            return BCrypt.Net.BCrypt.Verify(text, hash);
        }
    }
}
