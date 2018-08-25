using Factors.Helpers;
using Factors.Models.Interfaces;
using System;

namespace Factors.Token.Alphabet
{
    public class Provider : IFactorsTokenProvider
    {
        private readonly int _tokenLength;
        private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// Initalizes the token number provider. Defaults to
        /// a six-character token
        /// </summary>
        /// <param name="tokenLength"></param>
        public Provider(int tokenLength = 6)
        {
            _tokenLength = tokenLength;
        }

        /// <summary>
        /// Generates a token
        /// </summary>
        /// <returns></returns>
        public string GenerateToken()
        {
            var stringChars = new char[_tokenLength];

            for (int i = 0; i < stringChars.Length; i++)
            {
                var character = NumberGenerator.GenerateRandomInteger(0, _chars.Length);
                stringChars[i] = _chars[character];
            }

            var tokenValue = new String(stringChars);
            return tokenValue;
        }
    }
}
