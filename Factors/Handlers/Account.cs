using Factors.Models.Interfaces;
using Factors.Models.UserAccount;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Factors
{
    public partial class FactorsApplication
    {
        public IEnumerable<FactorsCredential> ListVerifiedAccounts<tt>() where tt : IFactorsFeatureType, new()
        {
            var featureType = new tt();
            return Configuration.StorageDatabase.ListCredentialsFor(UserAccount, featureType, FactorsCredentialListQueryType.VerifiedAccounts);
        }

        public IEnumerable<FactorsCredential> ListVerifiedAccounts()
        {
            return Configuration.StorageDatabase.ListCredentialsFor(UserAccount, null, FactorsCredentialListQueryType.VerifiedAccounts);
        }

        public Task<IEnumerable<FactorsCredential>> ListVerifiedAccountAsync<tt>() where tt : IFactorsFeatureType, new()
        {
            var featureType = new tt();
            return Configuration.StorageDatabase.ListCredentialsForAsync(UserAccount, featureType, FactorsCredentialListQueryType.VerifiedAccounts);
        }

        public Task<IEnumerable<FactorsCredential>> ListVerifiedAccountAsync()
        {
            return Configuration.StorageDatabase.ListCredentialsForAsync(UserAccount, null, FactorsCredentialListQueryType.VerifiedAccounts);
        }

        public IEnumerable<FactorsCredential> ListUnverifiedAccounts<tt>() where tt : IFactorsFeatureType, new()
        {
            var featureType = new tt();
            return Configuration.StorageDatabase.ListCredentialsFor(UserAccount, featureType, FactorsCredentialListQueryType.UnverifiedAccounts);
        }

        public IEnumerable<FactorsCredential> ListUnverifiedAccounts()
        {
            return Configuration.StorageDatabase.ListCredentialsFor(UserAccount, null, FactorsCredentialListQueryType.UnverifiedAccounts);
        }

        public Task<IEnumerable<FactorsCredential>> ListUnverifiedAccountsAsync<tt>() where tt : IFactorsFeatureType, new()
        {
            var featureType = new tt();
            return Configuration.StorageDatabase.ListCredentialsForAsync(UserAccount, featureType, FactorsCredentialListQueryType.UnverifiedAccounts);
        }

        public Task<IEnumerable<FactorsCredential>> ListUnverifiedAccountsAsync()
        {
            return Configuration.StorageDatabase.ListCredentialsForAsync(UserAccount, null, FactorsCredentialListQueryType.UnverifiedAccounts);
        }

        public IEnumerable<FactorsCredential> ListAllAccounts<tt>() where tt : IFactorsFeatureType, new()
        {
            var featureType = new tt();
            return Configuration.StorageDatabase.ListCredentialsFor(UserAccount, featureType, FactorsCredentialListQueryType.AllAccount);
        }

        public IEnumerable<FactorsCredential> ListAllAccounts()
        {
            return Configuration.StorageDatabase.ListCredentialsFor(UserAccount, null, FactorsCredentialListQueryType.AllAccount);
        }

        public Task<IEnumerable<FactorsCredential>> ListAllAccountsAsync<tt>() where tt : IFactorsFeatureType, new()
        {
            var featureType = new tt();
            return Configuration.StorageDatabase.ListCredentialsForAsync(UserAccount, featureType, FactorsCredentialListQueryType.AllAccount);
        }

        public Task<IEnumerable<FactorsCredential>> ListAllAccountsAsync()
        {
            return Configuration.StorageDatabase.ListCredentialsForAsync(UserAccount, null, FactorsCredentialListQueryType.AllAccount);
        }
    }
}
