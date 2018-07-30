using Factors.Models.Interfaces;
using Factors.Models.UserAccount;
using ServiceStack.OrmLite;
using System;
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
                var query = db.From<FactorsGeneratedToken>()
                    .Where(cred => 
                        cred.UserAccountId == userAccountId
                        && cred.FeatureTypeGuid == featureType.FeatureGuid
                        && cred.ExpirationDateUtc >= currentDateUtc
                        && cred.VerificationToken == tokenValue
                    );

                var queryResult = runAsAsync
                    ? await db.SingleAsync(query)
                    : db.Single(query);

                //
                // If it found (because expired or a bad token),
                // return to user
                //
                if (queryResult == null)
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
                if (runAsAsync)
                {
                    await db.DeleteByIdAsync<FactorsGeneratedToken>(queryResult.Id).ConfigureAwait(false);
                } else
                {
                    db.DeleteById<FactorsGeneratedToken>(queryResult.Id);
                }

                //
                // Now we're going to update the credential in the database
                // that is currently marked as "unverified" and change that flag
                // to "verified"
                //
                var userCredentialQuery = db.From<FactorsCredential>()
                    .Where(cred => cred.UserAccountId == userAccountId
                    && cred.CredentialKey == queryResult.CredentialKey
                    && cred.CredentialIsValidated == false);

                var userCredential = runAsAsync
                    ? await db.SingleAsync(userCredentialQuery).ConfigureAwait(false)
                    : db.Single(userCredentialQuery);

                userCredential.CredentialIsValidated = true;

                if (runAsAsync)
                {
                    await db.UpdateAsync(userCredential);
                } else
                {
                    db.Update(userCredential);
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
        public Task<FactorsGeneratedToken> StoreTokenAsync(FactorsGeneratedToken model)
        {
            return StoreTokenAsync(model, true);
        }

        public FactorsGeneratedToken StoreToken(FactorsGeneratedToken model)
        {
            return StoreTokenAsync(model, false).GetAwaiter().GetResult();
        }

        private async Task<FactorsGeneratedToken> StoreTokenAsync(FactorsGeneratedToken model, bool runAsAsync)
        {
            model.Id = 0;
            model.CreatedDateUtc = DateTime.UtcNow;

            using (var db = (runAsAsync ? await _dbConnection.OpenAsync().ConfigureAwait(false) : _dbConnection.Open()))
            {
                var tokenId = runAsAsync
                    ? await db.InsertAsync(model).ConfigureAwait(false)
                    : db.Insert(model);

                model.Id = tokenId;
                return model;
            }
        }
        #endregion STORE TOKEN
    }
}
