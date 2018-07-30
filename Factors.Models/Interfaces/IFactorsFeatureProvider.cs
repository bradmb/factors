using Factors.Models.UserAccount;
using System.Threading.Tasks;

namespace Factors.Models.Interfaces
{
    public interface IFactorsFeatureProvider
    {
        Task<FactorCredentialCreationResult> CreateCredentialAsync(IFactorsApplication instance, string credentialKey);

        FactorCredentialCreationResult CreateCredential(IFactorsApplication instance, string credentialKey);
    }
}
