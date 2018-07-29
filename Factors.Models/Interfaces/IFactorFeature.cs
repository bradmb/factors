using Factors.Models.UserAccount;
using System.Threading.Tasks;

namespace Factors.Models.Interfaces
{
    public interface IFactorFeature
    {
        Task<FactorCredentialCreationResult> CreateCredentialAsync(IFactorInstance instance, string credentialKey);

        FactorCredentialCreationResult CreateCredential(IFactorInstance instance, string credentialKey);
    }
}
