using Factors.Models.Interfaces;
using Factors.Models.UserAccount;
using ServiceStack.OrmLite;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Factors.Database.OrmLite
{
    public partial class Provider : IFactorsDatabaseProvider
    {
        #region VERIFY TOKEN
        public Task<FactorsCredentialCreationVerificationResult> VerifyTokenAsync(string userAccountId, IFactorsFeatureType featureType, string tokenValue)
        {
            return VerifyTokenAsync(userAccountId, featureType, tokenValue, true);
        }

        public FactorsCredentialCreationVerificationResult VerifyToken(string userAccountId, IFactorsFeatureType featureType, string tokenValue)
        {
            return VerifyTokenAsync(userAccountId, featureType, tokenValue, false).GetAwaiter().GetResult();
        }

        private async Task<FactorsCredentialCreationVerificationResult> VerifyTokenAsync(string userAccountId, IFactorsFeatureType featureType, string tokenValue, bool runAsAsync)
        {
            using (var db = (runAsAsync ? await _dbConnection.OpenAsync().ConfigureAwait(false) : _dbConnection.Open()))
            {
                //
                // Locates the verification token in the database,
                // assuming it hasn't expired and the passed token
                // value is correct
                //
                var currentDateUtc = DateTime.UtcNow;
                var query = db.From<FactorsCredentialGeneratedToken>()
                    .Where(cred => 
                        cred.UserAccountId == userAccountId
                        && cred.FeatureTypeGuid == featureType.FeatureGuid
                        && cred.ExpirationDateUtc >= currentDateUtc
                    );

                var queryResult = runAsAsync
                    ? await db.SelectAsync(query).ConfigureAwait(false)
                    : db.Select(query);

                //
                // If it isn't found (because expired, no hash matches,
                // or just doesn't exist), return to user
                //
                var matchingResults = queryResult.Where(qr => _encryption.VerifyHash(tokenValue, qr.VerificationToken));
                if (!matchingResults.Any())
                {
                    return new FactorsCredentialCreationVerificationResult
                    {
                        Success = false,
                        Message = "Unable to verify token"
                    };
                }

                //
                // If we made it this far, everything matches. We'll now
                // delete the verification record since it's not needed anymore
                //
                var matchingIds = matchingResults.Select(mr => mr.Id);
                if (runAsAsync)
                {
                    await db.DeleteByIdsAsync<FactorsCredentialGeneratedToken>(matchingIds).ConfigureAwait(false);
                }
                else
                {
                    db.DeleteByIds<FactorsCredentialGeneratedToken>(matchingIds);
                }

                //
                // Now we're going to update the credential in the database
                // that is currently marked as "unverified" and change that flag
                // to "verified"
                //
                var matchingKeys = matchingResults.Select(mr => mr.CredentialKey);
                var userCredentialQuery = db.From<FactorsCredential>()
                    .Where(cred => cred.UserAccountId == userAccountId
                    && matchingKeys.Contains(cred.CredentialKey)
                    && cred.CredentialIsValidated == false);

                var userCredentials = runAsAsync
                    ? await db.SelectAsync(userCredentialQuery).ConfigureAwait(false)
                    : db.Select(userCredentialQuery);

                userCredentials.ForEach(uc => uc.CredentialIsValidated = true);
                if (runAsAsync)
                {
                    await db.UpdateAllAsync(userCredentials);
                }
                else
                {
                    db.UpdateAll(userCredentials);
                }

                //
                // And we're done!
                //
                return new FactorsCredentialCreationVerificationResult
                {
                    Success = true,
                    Message = "Token verified"
                };
            }
        }
        #endregion GET TOKEN

        #region STORE TOKEN
        public Task<FactorsCredentialGeneratedToken> StoreTokenAsync(FactorsCredentialGeneratedToken model)
        {
            return StoreTokenAsync(model, true);
        }

        public FactorsCredentialGeneratedToken StoreToken(FactorsCredentialGeneratedToken model)
        {
            return StoreTokenAsync(model, false).GetAwaiter().GetResult();
        }

        private async Task<FactorsCredentialGeneratedToken> StoreTokenAsync(FactorsCredentialGeneratedToken model, bool runAsAsync)
        {
            model.Id = 0;
            model.CreatedDateUtc = DateTime.UtcNow;

            //
            // Hash the verification token in the database, but
            // put an unhashed copy of it aside so we can return
            // it to the user's code
            //
            var unhashedToken = model.VerificationToken;
            model.VerificationToken = _encryption.HashData(model.VerificationToken);

            using (var db = (runAsAsync ? await _dbConnection.OpenAsync().ConfigureAwait(false) : _dbConnection.Open()))
            {
                var tokenId = runAsAsync
                    ? await db.InsertAsync(model).ConfigureAwait(false)
                    : db.Insert(model);

                model.VerificationToken = unhashedToken;
                model.Id = tokenId;

                return model;
            }
        }
        #endregion STORE TOKEN
    }
}
