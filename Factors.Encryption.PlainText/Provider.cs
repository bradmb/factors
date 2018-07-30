using Factors.Models.Interfaces;

namespace Factors.Encryption.PlainText
{
    public class Provider : IFactorsEncryptionProvider
    {
        public string DecryptData(string encryptedText)
        {
            return encryptedText;
        }

        public string EncryptData(string text)
        {
            return text;
        }
    }
}
