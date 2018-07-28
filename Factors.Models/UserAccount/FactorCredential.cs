using System;

namespace Factors.Models.UserAccount
{
    public class FactorCredential
    {
        public long Id { get; set; }

        public string UserAccountId { get; set; }

        public DateTime CreatedDateUtc { get; set; }

        public DateTime ModifiedDateUtc { get; set; }

        public string CredentialType { get; set; }

        public string CredentialKey { get; set; }

        public string CredentialSecondaryKey { get; set; }

        public bool CredentialIsValidated { get; set; }
    }
}
