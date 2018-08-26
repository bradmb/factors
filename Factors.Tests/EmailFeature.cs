using System;
using System.Linq;
using Factors.Feature.Email;
using Factors.Feature.Email.Models;
using Factors.Models.UserAccount;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.OrmLite;

namespace Factors.Tests
{
    [TestClass]
    public class EmailFeature
    {
        private readonly string _userAccount = Guid.NewGuid().ToString();
        private readonly string _userEmailAddress = "user@domain.tld";
        private readonly string _senderAddress = "factors@domain.tld";
        private readonly string _senderName = "Factors";
#if DEBUGSMTP
        private readonly string _smtpHost = "localhost";
        private readonly int _smtpPort = 25;
#elif DEBUGPOSTMARK
        private readonly string _postmarkServerToken = "";
#endif
        private readonly int _tokenExpirationTime = 5;

        [TestInitialize]
        public void Initalize()
        {
            Factors.Initalize(new Models.FactorsConfiguration
            {
                ApplicationName = "Factors Unit Test",
                StorageDatabase = new Database.OrmLite.Provider(":memory:", SqliteDialect.Provider),
                EncryptionProvider = new Encryption.BCryptStandard.Provider(),
                TokenProvider = new Token.Number.Provider()
            }).UseEmailFactor(new EmailConfiguration
            {
                FromAddress = _senderAddress,
                FromName = _senderName,
#if DEBUGPOSTMARK
                MailProvider = new Feature.Email.Postmark.EmailPostmarkProvider(_postmarkServerToken),
#elif DEBUGSMTP
                MailProvider = new Feature.Email.Smtp.EmailSmtpProvider(_smtpHost, _smtpPort, false),
#elif !DEBUG
                MailProvider = new Feature.Email.NullRoute.EmailNullRouteProvider(),
#endif
                TokenExpirationTime = TimeSpan.FromMinutes(_tokenExpirationTime)
            });
        }

        [TestCleanup()]
        public void Dispose()
        {
            Factors.Dispose();
        }

        [TestMethod]
        public void VerifyIsInitalized()
        {
            Assert.IsTrue(!String.IsNullOrWhiteSpace(Factors.ForUser(_userAccount).UserAccount));
        }

        [TestMethod]
        public void CreateEmailCredential()
        {
            var emailCredential = Factors.ForUser(_userAccount).CreateCredential<EmailFeatureType>(_userEmailAddress);

            Assert.IsNotNull(emailCredential);
            Assert.IsTrue(emailCredential.IsSuccess);
            Assert.IsNotNull(emailCredential.TokenDetails);
        }

        [TestMethod]
        public void VerifyEmailToken()
        {
            var emailCredential = Factors.ForUser(_userAccount).CreateCredential<EmailFeatureType>(_userEmailAddress);
            var verificationResult = Factors.ForUser(_userAccount).VerifyToken<EmailFeatureType>(emailCredential.TokenRequestId.Value, emailCredential.TokenDetails.VerificationToken);

            Assert.IsTrue(verificationResult.Success);
        }

        [TestMethod]
        public void VerifyTokenWorksMultipleTimes()
        {
            var emailCredential = Factors.ForUser(_userAccount).CreateCredential<EmailFeatureType>(_userEmailAddress);
            var verificationResult = Factors.ForUser(_userAccount).VerifyToken<EmailFeatureType>(emailCredential.TokenRequestId.Value, emailCredential.TokenDetails.VerificationToken);

            Assert.IsTrue(verificationResult.Success);

            var tokenRequest = Factors.ForUser(_userAccount).BeginTokenRequest<EmailFeatureType>(_userEmailAddress);
            var tokenResult = Factors.ForUser(_userAccount).VerifyToken<EmailFeatureType>(tokenRequest.TokenRequestId.Value, tokenRequest.TokenDetails.VerificationToken);

            Assert.IsTrue(tokenResult.Success);
        }

        [TestMethod]
        public void VerifyTokenWorksWithMultiplePendingRequests()
        {
            var emailCredential = Factors.ForUser(_userAccount).CreateCredential<EmailFeatureType>(_userEmailAddress);
            var verificationResult = Factors.ForUser(_userAccount).VerifyToken<EmailFeatureType>(emailCredential.TokenRequestId.Value, emailCredential.TokenDetails.VerificationToken);

            Assert.IsTrue(verificationResult.Success);

            var totalCredentialsCreated = 0;
            var tokenRequestPos = new Random().Next(25, 75);
            FactorsTokenRequestResult tokenRequest = null;
            
            while (totalCredentialsCreated < 100 || tokenRequest == null)
            {
                totalCredentialsCreated++;

                if (totalCredentialsCreated == tokenRequestPos)
                {
                    tokenRequest = Factors.ForUser(_userAccount).BeginTokenRequest<EmailFeatureType>(_userEmailAddress);
                } else
                {
                    Factors.ForUser(_userAccount).BeginTokenRequest<EmailFeatureType>(_userEmailAddress);

                }
            }

            var tokenResult = Factors.ForUser(_userAccount).VerifyToken<EmailFeatureType>(tokenRequest.TokenRequestId.Value, tokenRequest.TokenDetails.VerificationToken);

            Assert.IsTrue(tokenResult.Success);
        }

        [TestMethod]
        public void VerifyEmailAccountIsValidated()
        {
            var emailCredential = Factors.ForUser(_userAccount).CreateCredential<EmailFeatureType>(_userEmailAddress);
            var verificationResult = Factors.ForUser(_userAccount).VerifyToken<EmailFeatureType>(emailCredential.TokenRequestId.Value, emailCredential.TokenDetails.VerificationToken);

            var accounts = Factors.ForUser(_userAccount).ListVerifiedAccounts<EmailFeatureType>();

            Assert.IsTrue(accounts.Count() > 0);
        }

        [TestMethod]
        public void VerifyEmailAccountIsNotValidated()
        {
            var emailCredential = Factors.ForUser(_userAccount).CreateCredential<EmailFeatureType>(_userEmailAddress);
            var accounts = Factors.ForUser(_userAccount).ListUnverifiedAccounts<EmailFeatureType>();

            Assert.IsTrue(accounts.Count() > 0);
        }

        [TestMethod]
        public void TryAndPassInvalidNewAccountValidationCode()
        {
            var emailCredential = Factors.ForUser(_userAccount).CreateCredential<EmailFeatureType>(_userEmailAddress);
            var verificationResult = Factors.ForUser(_userAccount).VerifyToken<EmailFeatureType>(emailCredential.TokenRequestId.Value, Guid.NewGuid().ToString().Substring(0, 6));

            Assert.IsFalse(verificationResult.Success);
        }

        [TestMethod]
        public void GetErrorWhenCreatingDuplicateCredentials()
        {
            var emailCredential = Factors.ForUser(_userAccount).CreateCredential<EmailFeatureType>(_userEmailAddress);
            var emailCredentialTwo = Factors.ForUser(_userAccount).CreateCredential<EmailFeatureType>(_userEmailAddress);

            Assert.IsFalse(emailCredentialTwo.IsSuccess);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetExceptionWhenSetuMissingFromAddress()
        {
            Factors.Dispose();

            Factors.Initalize(new Models.FactorsConfiguration
            {
                StorageDatabase = new Database.OrmLite.Provider(":memory:", SqliteDialect.Provider),
                EncryptionProvider = new Encryption.PlainText.Provider(),
                TokenProvider = new Token.Number.Provider()
            }).UseEmailFactor(new EmailConfiguration
            {
                FromName = _senderName,
#if DEBUGSMTP
                MailProvider = new Feature.Email.Smtp.EmailSmtpProvider(_smtpHost, _smtpPort, false),
#elif !DEBUG
                MailProvider = new Feature.Email.NullRoute.EmailNullRouteProvider(),
#endif
                TokenExpirationTime = TimeSpan.FromMinutes(_tokenExpirationTime)
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetExceptionWhenSetupMissingFromName()
        {
            Factors.Dispose();

            Factors.Initalize(new Models.FactorsConfiguration
            {
                StorageDatabase = new Database.OrmLite.Provider(":memory:", SqliteDialect.Provider),
                EncryptionProvider = new Encryption.PlainText.Provider(),
                TokenProvider = new Token.Number.Provider()
            }).UseEmailFactor(new EmailConfiguration
            {
                FromAddress = _senderAddress,
#if DEBUGSMTP
                MailProvider = new Feature.Email.Smtp.EmailSmtpProvider(_smtpHost, _smtpPort, false),
#elif !DEBUG
                MailProvider = new Feature.Email.NullRoute.EmailNullRouteProvider(),
#endif
                TokenExpirationTime = TimeSpan.FromMinutes(_tokenExpirationTime)
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetExceptionWhenSetupMissingMailProvider()
        {
            Factors.Dispose();

            Factors.Initalize(new Models.FactorsConfiguration
            {
                StorageDatabase = new Database.OrmLite.Provider(":memory:", SqliteDialect.Provider),
                EncryptionProvider = new Encryption.PlainText.Provider(),
                TokenProvider = new Token.Number.Provider()
            }).UseEmailFactor(new EmailConfiguration
            {
                FromAddress = _senderAddress,
                FromName = _senderName,
                TokenExpirationTime = TimeSpan.FromMinutes(_tokenExpirationTime)
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetExceptionWhenSetupMissingTokenExpirationTime()
        {
            Factors.Dispose();

            Factors.Initalize(new Models.FactorsConfiguration
            {
                StorageDatabase = new Database.OrmLite.Provider(":memory:", SqliteDialect.Provider),
                EncryptionProvider = new Encryption.PlainText.Provider(),
                TokenProvider = new Token.Number.Provider()
            }).UseEmailFactor(new EmailConfiguration
            {
                FromAddress = _senderAddress,
#if DEBUGSMTP
                MailProvider = new Feature.Email.Smtp.EmailSmtpProvider(_smtpHost, _smtpPort, false)
#elif !DEBUG
                MailProvider = new Feature.Email.NullRoute.EmailNullRouteProvider()
#endif
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetExceptionWhenSetupTokenExpirationNegativeNumber()
        {
            Factors.Dispose();

            Factors.Initalize(new Models.FactorsConfiguration
            {
                StorageDatabase = new Database.OrmLite.Provider(":memory:", SqliteDialect.Provider),
                EncryptionProvider = new Encryption.PlainText.Provider(),
                TokenProvider = new Token.Number.Provider()
            }).UseEmailFactor(new EmailConfiguration
            {
                FromAddress = _senderAddress,
                FromName = _senderName,
#if DEBUGSMTP
                MailProvider = new Feature.Email.Smtp.EmailSmtpProvider(_smtpHost, _smtpPort, false),
#elif !DEBUG
                MailProvider = new Feature.Email.NullRoute.EmailNullRouteProvider(),
#endif
                TokenExpirationTime = TimeSpan.FromMinutes(-_tokenExpirationTime)
            });
        }
    }
}
