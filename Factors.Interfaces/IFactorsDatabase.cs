using Factors.Models.UserAccount;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Factors.Interfaces
{
    public interface IFactorsDatabase
    {
        /// <summary>
        /// Creates the initial database schema if the
        /// database does not currently exist
        /// </summary>
        void InitializeDatabaseSchema();

        /// <summary>
        /// Verifies that the token passed for the specified user is correct,
        /// and also updates the associated credential in the database as verified
        /// (assuming the token is correct)
        /// </summary>
        /// <param name="userAccountId"></param>
        /// <param name="credentialType"></param>
        /// <param name="tokenValue"></param>
        /// <returns></returns>
        FactorVerificationResult VerifyToken(string userAccountId, string credentialType, string tokenValue);

        /// <summary>
        /// Verifies that the token passed for the specified user is correct,
        /// and also updates the associated credential in the database as verified
        /// (assuming the token is correct)
        /// </summary>
        /// <param name="userAccountId"></param>
        /// <param name="credentialType"></param>
        /// <param name="tokenValue"></param>
        /// <returns></returns>
        Task<FactorVerificationResult> VerifyTokenAsync(string userAccountId, string credentialType, string tokenValue);

        /// <summary>
        /// Stores a two-factor token in the database. Tokens are used
        /// for verifying new and existing accounts
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        FactorGeneratedToken StoreToken(FactorGeneratedToken model);

        /// <summary>
        /// Stores a two-factor token in the database. Tokens are used
        /// for verifying new and existing accounts
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<FactorGeneratedToken> StoreTokenAsync(FactorGeneratedToken model);

        /// <summary>
        /// Creates a new credential for a user in the database. Will be created as a
        /// "pending verification" credential.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        FactorCredential CreateCredential(FactorCredential model);

        /// <summary>
        /// Creates a new credential for a user in the database. Will be created as a
        /// "pending verification" credential.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<FactorCredential> CreateCredentialAsync(FactorCredential model);

        /// <summary>
        /// Lists all factor credentials for the specified User Account ID
        /// </summary>
        /// <param name="userAccountId"></param>
        /// <param name="excludePendingVerification">If <c>false</c>, will include any credential pending approval</param>
        /// <returns></returns>
        IEnumerable<FactorCredential> ListCredentialsFor(string userAccountId, string credentialType, FactorCredentialVerificationType accountsToInclude);

        /// <summary>
        /// Lists all factor credentials for the specified User Account ID
        /// </summary>
        /// <param name="userAccountId"></param>
        /// <param name="excludePendingVerification">If <c>false</c>, will include any credential pending approval</param>
        /// <returns></returns>
        Task<IEnumerable<FactorCredential>> ListCredentialsForAsync(string userAccountId, string credentialType, FactorCredentialVerificationType accountsToInclude);
    }
}
