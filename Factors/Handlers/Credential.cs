using Factors.Models.Interfaces;
using Factors.Models.UserAccount;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Factors
{
    public partial class FactorsApplication : IFactorsApplication
    {
        /// <summary>
        /// Creates a new email credential in the database and sends out
        /// an email with a verification token which will be used to verify
        /// the email address is legitimate
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="credentialKey"></param>
        /// <returns></returns>
        public Task<FactorCredentialCreationResult> CreateCredentialAsync<tt>(string credentialKey) where tt : IFactorsFeatureType, new()
        {
            var featureType = new tt();
            var feature = this.Features[featureType.FeatureGuid];

            return feature.CreateCredentialAsync(this, credentialKey);
        }

        /// <summary>
        /// Creates a new email credential in the database and sends out
        /// an email with a verification token which will be used to verify
        /// the email address is legitimate
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="credentialKey"></param>
        /// <returns></returns>
        public FactorCredentialCreationResult CreateCredential<tt>(string credentialKey) where tt : IFactorsFeatureType, new ()
        {
            var featureType = new tt();
            var feature = this.Features[featureType.FeatureGuid];

            return feature.CreateCredential(this, credentialKey);
        }
    }
}
