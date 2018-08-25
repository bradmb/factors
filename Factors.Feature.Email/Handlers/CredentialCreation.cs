using Factors.Models.Interfaces;
using Factors.Models.UserAccount;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Factors.Feature.Email
{
    public partial class EmailProvider : IFactorsFeatureProvider
    {
        /// <summary>
        /// Creates a new email credential in the database and sends out
        /// an email with a verification token which will be used to verify
        /// the email address is legitimate
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="credentialKey"></param>
        /// <returns></returns>
        public Task<FactorsCredentialCreationResult> CreateCredentialAsync(IFactorsApplication instance, string credentialKey)
        {
            return CreateEmailCredentialAsync(instance, credentialKey, true);
        }

        /// <summary>
        /// Creates a new email credential in the database and sends out
        /// an email with a verification token which will be used to verify
        /// the email address is legitimate
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="credentialKey"></param>
        /// <returns></returns>
        public FactorsCredentialCreationResult CreateCredential(IFactorsApplication instance, string credentialKey)
        {
            return CreateEmailCredentialAsync(instance, credentialKey, false).GetAwaiter().GetResult();
        }

        private async Task<FactorsCredentialCreationResult> CreateEmailCredentialAsync(IFactorsApplication instance, string credentialKey, bool runAsAsync)
        {
            //
            // Sets up our return model on the event of a successful
            // credential creation
            //
            var credentialResult = new FactorsCredentialCreationResult
            {
                IsSuccess = true,
                VerificationMessageSent = true,
                Message = "Account registered as pending verification, validation token sent",
            };

            //
            // Attempts to parse the email address. Will throw an exception if
            // it fails to.
            //
            new MailAddress(credentialKey);

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
                return new FactorsCredentialCreationResult
                {
                    IsSuccess = false,
                    VerificationMessageSent = false,
                    Message = $"There was an issue when creating the credential: {ex.Message}"
                };
            }

            var sendTokenResult = runAsAsync
                ? await this.SendCredentialValidationAsync(instance, credentialKey).ConfigureAwait(false)
                : this.SendCredentialValidation(instance, credentialKey);

            if (!sendTokenResult.IsSuccess)
            {
                return new FactorsCredentialCreationResult
                {
                    IsSuccess = false,
                    Message = sendTokenResult.Message
                };
            }

            //
            // And finally return our result
            //
            credentialResult.TokenDetails = sendTokenResult.TokenDetails;
            return credentialResult;
        }
    }
}