using Factors.Models.Interfaces;
using Factors.Models.UserAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Factors.Feature.Email
{
    public partial class EmailProvider : IFactorsFeatureProvider
    {
        public FactorsAuthenticationCreationResult SendCredentialValidation(IFactorsApplication instance, string credentialKey)
        {
            return this.SendCredentialValidationAsync(instance, credentialKey, false).GetAwaiter().GetResult();
        }

        public Task<FactorsAuthenticationCreationResult> SendCredentialValidationAsync(IFactorsApplication instance, string credentialKey)
        {
            return this.SendCredentialValidationAsync(instance, credentialKey, true);
        }

        private async Task<FactorsAuthenticationCreationResult> SendCredentialValidationAsync(IFactorsApplication instance, string credentialKey, bool runAsAsync)
        {
            //
            // Sets up our return model on the event of a successful
            // credential creation
            //
            var credentialResult = new FactorsAuthenticationCreationResult
            {
                IsSuccess = true,
                VerificationMessageSent = true,
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
                return new FactorsAuthenticationCreationResult
                {
                    IsSuccess = false,
                    VerificationMessageSent = false,
                    Message = $"There was an issue when sending out the verification token: Unable to identify credentials"
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
                return new FactorsAuthenticationCreationResult
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
