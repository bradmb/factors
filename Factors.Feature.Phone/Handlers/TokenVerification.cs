using Factors.Models.Interfaces;
using Factors.Models.UserAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Factors.Feature.Phone
{
    public partial class PhoneProvider : IFactorsFeatureProvider
    {
        public FactorsTokenRequestResult BeginTokenRequest(IFactorsApplication instance, string credentialKey, params KeyValuePair<string, string>[] parameters)
        {
            return this.BeginTokenRequestAsync(instance, credentialKey, false, parameters).GetAwaiter().GetResult();
        }

        public Task<FactorsTokenRequestResult> BeginTokenRequestAsync(IFactorsApplication instance, string credentialKey, params KeyValuePair<string, string>[] parameters)
        {
            return this.BeginTokenRequestAsync(instance, credentialKey, true, parameters);
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
            // Check to see if this request is for sending out a message
            // by phone call. If it is, validate phone call handling is configured
            // and abort if it's not configured
            //
            var sendAsPhoneCall = parameters?.Any(p => p.Key == "phonecall" && p.Value == "true") == true;
            if (sendAsPhoneCall && (!_configuration.EnablePhoneCallSupport || _configuration.PhoneCallInboundEndpoint == null))
            {
                return new FactorsTokenRequestResult
                {
                    IsSuccess = false,
                    Message = "Phone call support is currently not enabled, please enable first before attempting to use"
                };
            }

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
            // Generate the outbound message text
            //
            var messageText = _configuration.TextMessageTemplate
                .Replace("@APPNAME@", instance.Configuration.ApplicationName)
                .Replace("@APPCODE@", newToken);

            //
            // Send out the verification token to the user's phone number
            //
            var messageSendResult = runAsAsync
                ? await SendTokenMessageAsync(credentialKey, 
                    messageText, 
                    newToken, 
                    sendAsPhoneCall).ConfigureAwait(false)

                : SendTokenMessage(credentialKey,
                    messageText,
                    newToken,
                    sendAsPhoneCall);

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
