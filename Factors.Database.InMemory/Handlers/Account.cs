using Factors.Interfaces;
using Factors.Models.UserAccount;
using ServiceStack.OrmLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Factors.Database.InMemory
{
    public partial class Provider : IFactorsDatabase
    {
        #region CREDENTIAL CREATION
        public FactorCredential CreateCredential(FactorCredential model)
        {
            return this.CreateCredentialAsync(model, false).GetAwaiter().GetResult();
        }

        public Task<FactorCredential> CreateCredentialAsync(FactorCredential model)
        {
            return this.CreateCredentialAsync(model, true);
        }

        private async Task<FactorCredential> CreateCredentialAsync(FactorCredential model, bool runAsAsync)
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
                    ? await db.ExistsAsync<FactorCredential>(fc =>
                            fc.UserAccountId == model.UserAccountId
                            && fc.CredentialType == model.CredentialType
                            && fc.CredentialKey == model.CredentialKey
                            && fc.CredentialSecondaryKey == model.CredentialSecondaryKey
                        )
                        .ConfigureAwait(false)
                    : db.Exists<FactorCredential>(fc =>
                            fc.UserAccountId == model.UserAccountId
                            && fc.CredentialType == model.CredentialType
                            && fc.CredentialKey == model.CredentialKey
                            && fc.CredentialSecondaryKey == model.CredentialSecondaryKey
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
        public IEnumerable<FactorCredential> ListCredentialsFor(string userAccountId, string credentialType, FactorCredentialVerificationType accountsToInclude)
        {
            return this.ListCredentialsForAsync(userAccountId, credentialType, accountsToInclude, false).GetAwaiter().GetResult();
        }

        public Task<IEnumerable<FactorCredential>> ListCredentialsForAsync(string userAccountId, string credentialType, FactorCredentialVerificationType accountsToInclude)
        {
            return this.ListCredentialsForAsync(userAccountId, credentialType, accountsToInclude, true);
        }

        private async Task<IEnumerable<FactorCredential>> ListCredentialsForAsync(string userAccountId, string credentialType, FactorCredentialVerificationType accountsToInclude, bool runAsAsync)
        {
            using (var db = (runAsAsync ? await _dbConnection.OpenAsync().ConfigureAwait(false) : _dbConnection.Open()))
            {
                var query = db.From<FactorCredential>()
                    .Where(cred => cred.UserAccountId == userAccountId);

                if (!string.IsNullOrWhiteSpace(credentialType))
                {
                    query = query.Where(cred => cred.CredentialType == credentialType);
                }

                switch (accountsToInclude)
                {
                    case FactorCredentialVerificationType.UnverifiedAccounts:
                        query = query.Where(cred => cred.CredentialIsValidated == false);
                        break;
                    case FactorCredentialVerificationType.VerifiedAccounts:
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
