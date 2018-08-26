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
        public Task<FactorsCredentialCreationVerificationResult> VerifyTokenAsync(string userAccountId, IFactorsFeatureType featureType, Guid tokenRequestId, string tokenValue)
        {
            return VerifyTokenAsync(userAccountId, featureType, tokenRequestId, tokenValue, true);
        }

        public FactorsCredentialCreationVerificationResult VerifyToken(string userAccountId, IFactorsFeatureType featureType, Guid tokenRequestId, string tokenValue)
        {
            return VerifyTokenAsync(userAccountId, featureType, tokenRequestId, tokenValue, false).GetAwaiter().GetResult();
        }

        private async Task<FactorsCredentialCreationVerificationResult> VerifyTokenAsync(string userAccountId, IFactorsFeatureType featureType, Guid tokenRequestId, string tokenValue, bool runAsAsync)
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
                        && cred.TokenRequestId == tokenRequestId
                        && cred.FeatureTypeGuid == featureType.FeatureGuid
                        && cred.ExpirationDateUtc >= currentDateUtc
                    );

                var tokenResult = runAsAsync
                    ? await db.SingleAsync(query).ConfigureAwait(false)
                    : db.Single(query);

                //
                // If it isn't found (because expired, no hash matches,
                // or just doesn't exist), return to user
                //
                if (tokenResult == null || !_encryption.VerifyHash(tokenValue, tokenResult.VerificationToken))
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
                    await db.DeleteByIdAsync<FactorsCredentialGeneratedToken>(tokenResult.Id).ConfigureAwait(false);
                }
                else
                {
                    db.DeleteById<FactorsCredentialGeneratedToken>(tokenResult.Id);
                }

                //
                // Now we're going to update the credential in the database
                // that is currently marked as "unverified" and change that flag
                // to "verified"
                //
                var userCredentialQuery = db.From<FactorsCredential>()
                    .Where(cred => cred.UserAccountId == userAccountId
                    && cred.CredentialKey == tokenResult.CredentialKey
                    && cred.CredentialIsValidated == false);

                var userCredential = runAsAsync
                    ? await db.SingleAsync(userCredentialQuery).ConfigureAwait(false)
                    : db.Single(userCredentialQuery);

                if (userCredential != null)
                {
                    userCredential.CredentialIsValidated = true;
                    if (runAsAsync)
                    {
                        await db.UpdateAsync(userCredential);
                    }
                    else
                    {
                        db.Update(userCredential);
                    }
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
                //
                // Generates a new token request id and ensures that
                // no other duplcates exist in the database for this user.
                // If there are duplicates, then regenerate the id and try again.
                //
                while (model.TokenRequestId == default(Guid))
                {
                    var guidCheck = Guid.NewGuid();

                    var guidCheckQuery = db.From<FactorsCredentialGeneratedToken>()
                        .Where(f => f.UserAccountId == model.UserAccountId
                        && f.FeatureTypeGuid == model.FeatureTypeGuid
                        && f.TokenRequestId == guidCheck);

                    var guidExists = runAsAsync
                        ? await db.ExistsAsync(guidCheckQuery).ConfigureAwait(false)
                        : db.Exists(guidCheckQuery);

                    if (!guidExists)
                    {
                        model.TokenRequestId = guidCheck;
                    }
                }

                var tokenId = runAsAsync
                    ? await db.InsertAsync(model, selectIdentity: true).ConfigureAwait(false)
                    : db.Insert(model, selectIdentity: true);

                model.VerificationToken = unhashedToken;
                model.Id = tokenId;

                return model;
            }
        }
        #endregion STORE TOKEN
    }
}
