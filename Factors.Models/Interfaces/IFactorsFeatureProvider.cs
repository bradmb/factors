using Factors.Models.UserAccount;
using System.Threading.Tasks;

namespace Factors.Models.Interfaces
{
    public interface IFactorsFeatureProvider
    {
        Task<FactorsCredentialCreationResult> CreateCredentialAsync(IFactorsApplication instance, string credentialKey);

        FactorsCredentialCreationResult CreateCredential(IFactorsApplication instance, string credentialKey);


    }
}
