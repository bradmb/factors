using ServiceStack.DataAnnotations;
using System;

namespace Factors.Models.UserAccount
{
    [Alias("Factors_VerificationToken")]
    public class FactorsCredentialGeneratedToken
    {
        [PrimaryKey]
        public long Id { get; set; }

        [Index]
        public string UserAccountId { get; set; }

        public DateTime CreatedDateUtc { get; set; }

        public DateTime ExpirationDateUtc { get; set; }

        [Index]
        public string FeatureTypeGuid { get; set; }

        public string CredentialKey { get; set; }

        public string VerificationToken { get; set; }
    }
}
