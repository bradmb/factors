namespace Factors.Models.UserAccount
{
    /// <summary>
    /// Result of when you are sending a verification message to
    /// a pre-authorized two-factor verification credential
    /// </summary>
    public class FactorsAuthenticationCreationResult
    {
        public bool IsSuccess { get; set; }

        public bool VerificationMessageSent { get; set; }

        public string Message { get; set; }

        public FactorsCredential CredentailDetails { get; set; }

        public FactorsCredentialGeneratedToken TokenDetails { get; set; }
    }
}
