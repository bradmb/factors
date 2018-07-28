using System;

namespace Factors.Models.UserAccount
{
    public class FactorGeneratedToken
    {
        public long Id { get; set; }

        public string UserAccountId { get; set; }

        public DateTime CreatedDateUtc { get; set; }

        public DateTime ExpirationDateUtc { get; set; }

        public string CredentialType { get; set; }

        public string CredentialKey { get; set; }

        public string VerificationToken { get; set; }
    }
}
