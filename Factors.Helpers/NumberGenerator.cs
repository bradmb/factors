using System;
using System.Security.Cryptography;

namespace Factors.Helpers
{
    public static class NumberGenerator
    {
        private static readonly RNGCryptoServiceProvider _rng = new RNGCryptoServiceProvider();

        /// <summary>
        /// Generates a random integer for picking out our token values
        /// Base code courtesy of:
        /// http://csharphelper.com/blog/2014/08/use-a-cryptographic-random-number-generator-in-c/
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int GenerateRandomInteger(int min, int max)
        {
            var scale = uint.MaxValue;
            while (scale == uint.MaxValue)
            {
                var four_bytes = new byte[4];
                _rng.GetBytes(four_bytes);

                scale = BitConverter.ToUInt32(four_bytes, 0);
            }

            return (int)(min + (max - min) * (scale / (double)uint.MaxValue));
        }
    }
}
