using Factors.Helpers;
using Factors.Models.Interfaces;
using System;
using System.Text.RegularExpressions;

namespace Factors.Token.Alphanumeric
{
    public class Provider : IFactorsTokenProvider
    {
        private readonly int _tokenLength;
        private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        /// <summary>
        /// Initalizes the token number provider. Defaults to
        /// a six-character alphanumeric token
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
            var tokenValue = String.Empty;

            //
            // If we don't have an alphanumeric string, then
            // we're going to keep looping through until we do
            //
            while (!Regex.IsMatch(tokenValue, @"^[a-zA-Z0-9]+$"))
            {
                for (int i = 0; i < stringChars.Length; i++)
                {
                    var character = NumberGenerator.GenerateRandomInteger(0, _chars.Length);
                    stringChars[i] = _chars[character];
                }

                tokenValue = new String(stringChars);
            }

            return tokenValue;
        }
    }
}
