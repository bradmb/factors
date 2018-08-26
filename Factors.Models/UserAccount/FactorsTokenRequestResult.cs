using System;

namespace Factors.Models.UserAccount
{
    /// <summary>
    /// Result of when you are starting a new token
    /// request for a user credential
    /// </summary>
    public class FactorsTokenRequestResult
    {
        /// <summary>
        /// If true, your token request has been a success
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// The unique id you will need to verify the token
        /// returned by the user
        /// </summary>
        public Guid? TokenRequestId {
            get
            {
                return this.TokenDetails?.TokenRequestId;
            }
        }

        /// <summary>
        /// Any message returned by the system
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Full details about the user's credentials
        /// </summary>
        public FactorsCredential CredentialDetails { get; set; }

        /// <summary>
        /// Full details about the token generated
        /// </summary>
        public FactorsCredentialGeneratedToken TokenDetails { get; set; }
    }
}
