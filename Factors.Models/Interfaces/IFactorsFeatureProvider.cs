using Factors.Models.UserAccount;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Factors.Models.Interfaces
{
    public interface IFactorsFeatureProvider
    {
        /// <summary>
        /// Creates a new user credential in the database. Will also kick off the <c>BeginTokenRequest</c>
        /// method automatically so the user credential can be verified as legitimate
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="credentialKey"></param>
        /// <returns></returns>
        FactorsCredentialCreationResult CreateCredential(IFactorsApplication instance, string credentialKey, params KeyValuePair<string, string>[] parameters);

        /// <summary>
        /// Creates a new user credential in the database. Will also kick off the <c>BeginTokenRequest</c>
        /// method automatically so the user credential can be verified as legitimate
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="credentialKey"></param>
        /// <returns></returns>
        Task<FactorsCredentialCreationResult> CreateCredentialAsync(IFactorsApplication instance, string credentialKey, params KeyValuePair<string, string>[] parameters);

        /// <summary>
        /// Starts the token request process for a feature. Each feature may handle this operation
        /// differently (ex: one may send out a message, another may simply await verification)
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="credentialKey"></param>
        /// <returns></returns>
        FactorsTokenRequestResult BeginTokenRequest(IFactorsApplication instance, string credentialKey, params KeyValuePair<string, string>[] parameters);

        /// <summary>
        /// Starts the token request process for a feature. Each feature may handle this operation
        /// differently (ex: one may send out a message, another may simply await verification)
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="credentialKey"></param>
        /// <returns></returns>
        Task<FactorsTokenRequestResult> BeginTokenRequestAsync(IFactorsApplication instance, string credentialKey, params KeyValuePair<string, string>[] parameters);
    }
}
