namespace Factors.Models.UserAccount
{
    public class FactorCredentialCreationResult
    {
        public bool IsSuccess { get; set; }

        public bool VerificationMessageSent { get; set; }

        public string Message { get; set; }

        public FactorCredential CredentailDetails { get; set; }

        public FactorGeneratedToken TokenDetails { get; set; }
    }
}
