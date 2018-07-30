using Factors.Models.Interfaces;
using Factors.Models.UserAccount;
using ServiceStack.OrmLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Factors.Database.OrmLite
{
    public partial class Provider : IFactorsDatabaseProvider
    {
        #region CREDENTIAL CREATION
        public FactorsCredential CreateCredential(FactorsCredential model)
        {
            return this.CreateCredentialAsync(model, false).GetAwaiter().GetResult();
        }

        public Task<FactorsCredential> CreateCredentialAsync(FactorsCredential model)
        {
            return this.CreateCredentialAsync(model, true);
        }

        private async Task<FactorsCredential> CreateCredentialAsync(FactorsCredential model, bool runAsAsync)
        {
            //
            // Fill out the basic model details
            //
            model.Id = 0;
            model.CreatedDateUtc = DateTime.UtcNow;
            model.ModifiedDateUtc = DateTime.UtcNow;
            model.CredentialIsValidated = false;

            using (var db = (runAsAsync ? await _dbConnection.OpenAsync().ConfigureAwait(false) : _dbConnection.Open()))
            {
                //
                // Check to see if the credential already exists in the database
                //
                var hasExistingCredential = runAsAsync
                    ? await db.ExistsAsync<FactorsCredential>(fc =>
                            fc.UserAccountId == model.UserAccountId
                            && fc.FeatureTypeGuid == model.FeatureTypeGuid
                            && fc.CredentialKey == model.CredentialKey
                        )
                        .ConfigureAwait(false)
                    : db.Exists<FactorsCredential>(fc =>
                            fc.UserAccountId == model.UserAccountId
                            && fc.FeatureTypeGuid == model.FeatureTypeGuid
                            && fc.CredentialKey == model.CredentialKey
                        );

                //
                // If it is an existing credential exit out
                //
                if (hasExistingCredential)
                {
                    throw new Exception("Credential already exists in database for specified user");
                }

                //
                // If not, let's add it
                //
                var credentialId = runAsAsync
                    ? await db.InsertAsync(model).ConfigureAwait(false)
                    : db.Insert(model);

                model.Id = credentialId;
                return model;
            }
        }
        #endregion CREDENTIAL CREATION

        #region LIST CREDENTIAL
        public IEnumerable<FactorsCredential> ListCredentialsFor(string userAccountId, IFactorsFeatureType featureType, FactorsCredentialListQueryType accountsToInclude)
        {
            return this.ListCredentialsForAsync(userAccountId, featureType, accountsToInclude, false).GetAwaiter().GetResult();
        }

        public Task<IEnumerable<FactorsCredential>> ListCredentialsForAsync(string userAccountId, IFactorsFeatureType featureType, FactorsCredentialListQueryType accountsToInclude)
        {
            return this.ListCredentialsForAsync(userAccountId, featureType, accountsToInclude, true);
        }

        private async Task<IEnumerable<FactorsCredential>> ListCredentialsForAsync(string userAccountId, IFactorsFeatureType featureType, FactorsCredentialListQueryType accountsToInclude, bool runAsAsync)
        {
            using (var db = (runAsAsync ? await _dbConnection.OpenAsync().ConfigureAwait(false) : _dbConnection.Open()))
            {
                var query = db.From<FactorsCredential>()
                    .Where(cred => cred.UserAccountId == userAccountId);

                if (featureType != null)
                {
                    query = query.Where(cred => cred.FeatureTypeGuid == featureType.FeatureGuid);
                }

                switch (accountsToInclude)
                {
                    case FactorsCredentialListQueryType.UnverifiedAccounts:
                        query = query.Where(cred => cred.CredentialIsValidated == false);
                        break;
                    case FactorsCredentialListQueryType.VerifiedAccounts:
                        query = query.Where(cred => cred.CredentialIsValidated == true);
                        break;
                }

                var queryResult = runAsAsync
                    ? await db.SelectAsync(query)
                    : db.Select(query);

                return queryResult;
            }
        }
        #endregion LIST CREDENTIAL
    }
}
