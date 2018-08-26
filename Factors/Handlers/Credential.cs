using Factors.Models.Interfaces;
using Factors.Models.UserAccount;
using System.Threading.Tasks;

namespace Factors
{
    public partial class FactorsApplication : IFactorsApplication
    {
        /// <summary>
        /// Creates a new credential in the database and sends out
        /// a message with a verification token which will be used to verify
        /// the credential is legitimate
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="credentialKey"></param>
        /// <returns></returns>
        public Task<FactorsCredentialCreationResult> CreateCredentialAsync<tt>(string credentialKey) where tt : IFactorsFeatureType, new()
        {
            var featureType = new tt();
            var feature = this.Features[featureType.FeatureGuid];

            return feature.CreateCredentialAsync(this, credentialKey);
        }

        /// <summary>
        /// Creates a new credential in the database and sends out
        /// an email with a verification token which will be used to verify
        /// the email address is legitimate
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="credentialKey"></param>
        /// <returns></returns>
        public FactorsCredentialCreationResult CreateCredential<tt>(string credentialKey) where tt : IFactorsFeatureType, new ()
        {
            var featureType = new tt();
            var feature = this.Features[featureType.FeatureGuid];

            return feature.CreateCredential(this, credentialKey);
        }

        /// <summary>
        /// Starts the token request process for a feature. Each feature may handle this operation
        /// differently (ex: one may send out a message, another may simply await verification)
        /// </summary>
        /// <typeparam name="tt"></typeparam>
        /// <param name="credentialKey"></param>
        /// <returns></returns>
        public FactorsTokenRequestResult BeginTokenRequest<tt>(string credentialKey) where tt : IFactorsFeatureType, new()
        {
            var featureType = new tt();
            var feature = this.Features[featureType.FeatureGuid];

            return feature.BeginTokenRequest(this, credentialKey);
        }

        /// <summary>
        /// Starts the token request process for a feature. Each feature may handle this operation
        /// differently (ex: one may send out a message, another may simply await verification)
        /// </summary>
        /// <typeparam name="tt"></typeparam>
        /// <param name="credentialKey"></param>
        /// <returns></returns>
        public Task<FactorsTokenRequestResult> BeginTokenRequestAsync<tt>(string credentialKey) where tt : IFactorsFeatureType, new()
        {
            var featureType = new tt();
            var feature = this.Features[featureType.FeatureGuid];

            return feature.BeginTokenRequestAsync(this, credentialKey);
        }
    }
}
