using System;

namespace Factors.Models.UserAccount
{
    public class FactorsCredentialCreationResult
    {
        public bool IsSuccess { get; set; }

        public bool VerificationMessageSent { get; set; }

        /// <summary>
        /// The unique id you will need to verify the token
        /// returned by the user
        /// </summary>
        public Guid? TokenRequestId
        {
            get
            {
                return this.TokenDetails?.TokenRequestId;
            }
        }

        public string Message { get; set; }

        public FactorsCredential CredentailDetails { get; set; }

        public FactorsCredentialGeneratedToken TokenDetails { get; set; }
    }
}
