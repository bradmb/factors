using Factors.Interfaces;
using Factors.Models.UserAccount;
using System.Threading.Tasks;

namespace Factors
{
    public partial class FactorsInstance
    {
        /// <summary>
        /// Verifies that the token passed for the specified user is correct,
        /// and also updates the associated credential in the database as verified
        /// (assuming the token is correct)
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="tokenValue"></param>
        /// <returns></returns>
        public FactorVerificationResult VerifyToken(IFeatureTypeProvider feature, string tokenValue)
        {
            return _configuration.StorageDatabase.VerifyToken(UserAccount, feature.FeatureName, tokenValue);
        }

        /// <summary>
        /// Verifies that the token passed for the specified user is correct,
        /// and also updates the associated credential in the database as verified
        /// (assuming the token is correct)
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="tokenValue"></param>
        /// <returns></returns>
        public Task<FactorVerificationResult> VerifyTokenAsync(IFeatureTypeProvider feature, string tokenValue)
        {
            return _configuration.StorageDatabase.VerifyTokenAsync(UserAccount, feature.FeatureName, tokenValue);
        }
    }
}
