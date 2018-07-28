using Factors.Models.UserAccount;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Factors.Feature.Email
{
    public static partial class EmailProvider
    {
        /// <summary>
        /// Creates a new email credential in the database and sends out
        /// an email with a verification token which will be used to verify
        /// the email address is legitimate
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public static Task<FactorCredentialCreationResult> CreateEmailCredentialAsync(this FactorsInstance instance, string emailAddress)
        {
            return CreateEmailCredentialAsync(instance, emailAddress, true);
        }

        /// <summary>
        /// Creates a new email credential in the database and sends out
        /// an email with a verification token which will be used to verify
        /// the email address is legitimate
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public static FactorCredentialCreationResult CreateEmailCredential(this FactorsInstance instance, string emailAddress)
        {
            return CreateEmailCredentialAsync(instance, emailAddress, false).GetAwaiter().GetResult();
        }

        private static async Task<FactorCredentialCreationResult> CreateEmailCredentialAsync(this FactorsInstance instance, string emailAddress, bool runAsAsync)
        {
            //
            // Attempts to parse the email address. Will throw an exception if
            // it fails to.
            //
            new MailAddress(emailAddress);

            //
            // Creates the new "pending verification" credential
            //
            var credentailDetails = new FactorCredential
            {
                UserAccountId = instance.UserAccount,
                CredentialType = FeatureType.FeatureName,
                CredentialKey = emailAddress
            };

            var newCredential = runAsAsync 
                ? await instance._configuration.StorageDatabase.CreateCredentialAsync(credentailDetails).ConfigureAwait(false)
                : instance._configuration.StorageDatabase.CreateCredential(credentailDetails);

            //
            // Generates a new token to be used to verify the credential
            //
            var newToken = instance._configuration.TokenProvider.GenerateToken();

            //
            // Stores the new token in the database
            //
            var tokenDetails = new FactorGeneratedToken
            {
                UserAccountId = instance.UserAccount,
                VerificationToken = newToken,
                CredentialType = FeatureType.FeatureName,
                ExpirationDateUtc = DateTime.UtcNow.Add(_configuration.TokenExpirationTime),
                CredentialKey = emailAddress
            };

            tokenDetails = runAsAsync
                ? await instance._configuration.StorageDatabase.StoreTokenAsync(tokenDetails).ConfigureAwait(false)
                : instance._configuration.StorageDatabase.StoreToken(tokenDetails);

            //
            // TODO: Logic for sending out verification token
            //

            return new FactorCredentialCreationResult
            {
                IsSuccess = true,
                VerificationMessageSent = true,
                CredentailDetails = newCredential,
                TokenDetails = tokenDetails,
                Message = "Account registered as pending verification, validation token sent"
            };
        }
    }
}
