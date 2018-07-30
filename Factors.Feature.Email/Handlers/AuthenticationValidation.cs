using Factors.Models.Interfaces;
using Factors.Models.UserAccount;
using System;
using System.Collections.Generic;
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
            // Creates the new "pending verification" credential
            //
            var credentailDetails = new FactorsCredential
            {
                UserAccountId = instance.UserAccount,
                FeatureTypeGuid = _featureType.FeatureGuid,
                CredentialKey = credentialKey
            };

            try
            {
                credentialResult.CredentailDetails = runAsAsync
                    ? await instance.Configuration.StorageDatabase.CreateCredentialAsync(credentailDetails).ConfigureAwait(false)
                    : instance.Configuration.StorageDatabase.CreateCredential(credentailDetails);
            }
            catch (Exception ex)
            {
                return new FactorsAuthenticationCreationResult
                {
                    IsSuccess = false,
                    VerificationMessageSent = false,
                    Message = $"There was an issue when creating the credential: {ex.Message}"
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
