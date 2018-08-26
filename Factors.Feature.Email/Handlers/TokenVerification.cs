using Factors.Models.Interfaces;
using Factors.Models.UserAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Factors.Feature.Email
{
    public partial class EmailProvider : IFactorsFeatureProvider
    {
        public FactorsTokenRequestResult BeginTokenRequest(IFactorsApplication instance, string credentialKey, params KeyValuePair<string, string>[] parameters)
        {
            return this.BeginTokenRequestAsync(instance, credentialKey, false).GetAwaiter().GetResult();
        }

        public Task<FactorsTokenRequestResult> BeginTokenRequestAsync(IFactorsApplication instance, string credentialKey, params KeyValuePair<string, string>[] parameters)
        {
            return this.BeginTokenRequestAsync(instance, credentialKey, true);
        }

        private async Task<FactorsTokenRequestResult> BeginTokenRequestAsync(IFactorsApplication instance, string credentialKey, bool runAsAsync, params KeyValuePair<string, string>[] parameters)
        {
            //
            // Sets up our return model on the event of a successful
            // credential creation
            //
            var credentialResult = new FactorsTokenRequestResult
            {
                IsSuccess = true,
                Message = "Validation token sent to credential",
            };

            //
            // Make sure we have a matching credential in the database, even
            // if it's an unverified one
            //
            var credentialList = runAsAsync
                ? await instance.Configuration.StorageDatabase.ListCredentialsForAsync(instance.UserAccount, _featureType, FactorsCredentialListQueryType.AllAccount).ConfigureAwait(false)
                : instance.Configuration.StorageDatabase.ListCredentialsFor(instance.UserAccount, _featureType, FactorsCredentialListQueryType.AllAccount);

            if (credentialList?.Any(cd => String.Equals(cd.CredentialKey, credentialKey, StringComparison.InvariantCultureIgnoreCase)) != true)
            {
                return new FactorsTokenRequestResult
                {
                    IsSuccess = false,
                    Message = $"There was an issue when sending out the token: Unable to identify user account is setup for email tokens"
                };
            }

            //
            // Generates a new token to be used to verify the credential
            //
            var newToken = instance.Configuration.TokenProvider.GenerateToken();

            //
            // Stores the new token in the database
            //
            var tokenDetails = new FactorsCredentialGeneratedToken
            {
                UserAccountId = instance.UserAccount,
                VerificationToken = newToken,
                FeatureTypeGuid = _featureType.FeatureGuid,
                ExpirationDateUtc = DateTime.UtcNow.Add(_configuration.TokenExpirationTime),
                CredentialKey = credentialKey
            };

            credentialResult.TokenDetails = runAsAsync
                ? await instance.Configuration.StorageDatabase.StoreTokenAsync(tokenDetails).ConfigureAwait(false)
                : instance.Configuration.StorageDatabase.StoreToken(tokenDetails);

            //
            // Send out the verification token to the user's email address
            //
            var messageSendResult = runAsAsync
                ? await SendTokenMessageAsync(credentialKey, newToken).ConfigureAwait(false)
                : SendTokenMessage(credentialKey, newToken);

            if (!messageSendResult.IsSuccess)
            {
                return new FactorsTokenRequestResult
                {
                    IsSuccess = false,
                    Message = messageSendResult.Message
                };
            }

            //
            // And finally return our result
            //
            return credentialResult;
        }
    }
}
