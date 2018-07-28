using Factors.Interfaces;
using Factors.Models.UserAccount;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Factors
{
    public partial class FactorsInstance
    {
        public IEnumerable<FactorCredential> ListVerifiedAccountsFor(IFeatureTypeProvider feature)
        {
            return _configuration.StorageDatabase.ListCredentialsFor(UserAccount, feature.FeatureName, FactorCredentialVerificationType.VerifiedAccounts);
        }

        public Task<IEnumerable<FactorCredential>> ListVerifiedAccountsForAsync(IFeatureTypeProvider feature)
        {
            return _configuration.StorageDatabase.ListCredentialsForAsync(UserAccount, feature.FeatureName, FactorCredentialVerificationType.VerifiedAccounts);
        }

        public IEnumerable<FactorCredential> ListUnverifiedAccountsFor(IFeatureTypeProvider feature)
        {
            return _configuration.StorageDatabase.ListCredentialsFor(UserAccount, feature.FeatureName, FactorCredentialVerificationType.UnverifiedAccounts);
        }

        public Task<IEnumerable<FactorCredential>> ListUnverifiedAccountsForAsync(IFeatureTypeProvider feature)
        {
            return _configuration.StorageDatabase.ListCredentialsForAsync(UserAccount, feature.FeatureName, FactorCredentialVerificationType.UnverifiedAccounts);
        }

        public IEnumerable<FactorCredential> ListAllAccountsFor(IFeatureTypeProvider feature)
        {
            return _configuration.StorageDatabase.ListCredentialsFor(UserAccount, feature.FeatureName, FactorCredentialVerificationType.AllAccount);
        }

        public Task<IEnumerable<FactorCredential>> ListAllAccountsForAsync(IFeatureTypeProvider feature)
        {
            return _configuration.StorageDatabase.ListCredentialsForAsync(UserAccount, feature.FeatureName, FactorCredentialVerificationType.AllAccount);
        }
    }
}
