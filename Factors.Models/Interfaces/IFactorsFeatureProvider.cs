using Factors.Models.UserAccount;
using System.Threading.Tasks;

namespace Factors.Models.Interfaces
{
    public interface IFactorsFeatureProvider
    {
        FactorsCredentialCreationResult CreateCredential(IFactorsApplication instance, string credentialKey);

        Task<FactorsCredentialCreationResult> CreateCredentialAsync(IFactorsApplication instance, string credentialKey);

        FactorsAuthenticationCreationResult SendCredentialValidation(IFactorsApplication instance, string credentialKey);

        Task<FactorsAuthenticationCreationResult> SendCredentialValidationAsync(IFactorsApplication instance, string credentialKey);
    }
}
