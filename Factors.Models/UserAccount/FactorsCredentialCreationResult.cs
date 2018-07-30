namespace Factors.Models.UserAccount
{
    public class FactorsCredentialCreationResult
    {
        public bool IsSuccess { get; set; }

        public bool VerificationMessageSent { get; set; }

        public string Message { get; set; }

        public FactorsCredential CredentailDetails { get; set; }

        public FactorsGeneratedToken TokenDetails { get; set; }
    }
}
