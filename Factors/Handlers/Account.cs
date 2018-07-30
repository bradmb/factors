using Factors.Models.Interfaces;
using Factors.Models.UserAccount;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Factors
{
    public partial class FactorsApplication
    {
        public IEnumerable<FactorCredential> ListVerifiedAccounts<tt>() where tt : IFactorsFeatureType, new()
        {
            var featureType = new tt();
            return Configuration.StorageDatabase.ListCredentialsFor(UserAccount, featureType, FactorCredentialVerificationType.VerifiedAccounts);
        }

        public IEnumerable<FactorCredential> ListVerifiedAccounts()
        {
            return Configuration.StorageDatabase.ListCredentialsFor(UserAccount, null, FactorCredentialVerificationType.VerifiedAccounts);
        }

        public Task<IEnumerable<FactorCredential>> ListVerifiedAccountAsync<tt>() where tt : IFactorsFeatureType, new()
        {
            var featureType = new tt();
            return Configuration.StorageDatabase.ListCredentialsForAsync(UserAccount, featureType, FactorCredentialVerificationType.VerifiedAccounts);
        }

        public Task<IEnumerable<FactorCredential>> ListVerifiedAccountAsync()
        {
            return Configuration.StorageDatabase.ListCredentialsForAsync(UserAccount, null, FactorCredentialVerificationType.VerifiedAccounts);
        }

        public IEnumerable<FactorCredential> ListUnverifiedAccounts<tt>() where tt : IFactorsFeatureType, new()
        {
            var featureType = new tt();
            return Configuration.StorageDatabase.ListCredentialsFor(UserAccount, featureType, FactorCredentialVerificationType.UnverifiedAccounts);
        }

        public IEnumerable<FactorCredential> ListUnverifiedAccounts()
        {
            return Configuration.StorageDatabase.ListCredentialsFor(UserAccount, null, FactorCredentialVerificationType.UnverifiedAccounts);
        }

        public Task<IEnumerable<FactorCredential>> ListUnverifiedAccountsAsync<tt>() where tt : IFactorsFeatureType, new()
        {
            var featureType = new tt();
            return Configuration.StorageDatabase.ListCredentialsForAsync(UserAccount, featureType, FactorCredentialVerificationType.UnverifiedAccounts);
        }

        public Task<IEnumerable<FactorCredential>> ListUnverifiedAccountsAsync()
        {
            return Configuration.StorageDatabase.ListCredentialsForAsync(UserAccount, null, FactorCredentialVerificationType.UnverifiedAccounts);
        }

        public IEnumerable<FactorCredential> ListAllAccounts<tt>() where tt : IFactorsFeatureType, new()
        {
            var featureType = new tt();
            return Configuration.StorageDatabase.ListCredentialsFor(UserAccount, featureType, FactorCredentialVerificationType.AllAccount);
        }

        public IEnumerable<FactorCredential> ListAllAccounts()
        {
            return Configuration.StorageDatabase.ListCredentialsFor(UserAccount, null, FactorCredentialVerificationType.AllAccount);
        }

        public Task<IEnumerable<FactorCredential>> ListAllAccountsAsync<tt>() where tt : IFactorsFeatureType, new()
        {
            var featureType = new tt();
            return Configuration.StorageDatabase.ListCredentialsForAsync(UserAccount, featureType, FactorCredentialVerificationType.AllAccount);
        }

        public Task<IEnumerable<FactorCredential>> ListAllAccountsAsync()
        {
            return Configuration.StorageDatabase.ListCredentialsForAsync(UserAccount, null, FactorCredentialVerificationType.AllAccount);
        }
    }
}
