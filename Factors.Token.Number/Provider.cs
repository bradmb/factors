using Factors.Models.Interfaces;
using System;

namespace Factors.Token.Number
{
    public class Provider : ITokenProvider
    {
        private readonly int _tokenLength;

        /// <summary>
        /// Initalizes the token number provider. Defaults to
        /// a six-digit numerical token
        /// </summary>
        /// <param name="tokenLength"></param>
        public Provider(int tokenLength = 6)
        {
            _tokenLength = tokenLength;
        }

        public string GenerateToken()
        {
            var random = new Random();

            var startNumber = Int32.Parse("1".PadRight(_tokenLength, '0'));
            var stopNumber = Int32.Parse("9".PadRight(_tokenLength, '9'));

            var token = random.Next(startNumber, stopNumber);
            return token.ToString();
        }
    }
}
