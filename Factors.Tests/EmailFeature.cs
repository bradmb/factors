using System;
using System.Linq;
using Factors.Feature.Email;
using Factors.Feature.Email.Models;
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
                StorageDatabase = new Database.OrmLite.Provider(":memory:", SqliteDialect.Provider),
                EncryptionProvider = new Encryption.PlainText.Provider(),
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
            var verificationResult = Factors.ForUser(_userAccount).VerifyCredentialRegistration<EmailFeatureType>(emailCredential.TokenDetails.VerificationToken);

            Assert.IsTrue(verificationResult.Success);
        }

        [TestMethod]
        public void VerifyEmailAccountIsValidated()
        {
            var emailCredential = Factors.ForUser(_userAccount).CreateCredential<EmailFeatureType>(_userEmailAddress);
            var verificationResult = Factors.ForUser(_userAccount).VerifyCredentialRegistration<EmailFeatureType>(emailCredential.TokenDetails.VerificationToken);

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
        public void TryAndPassInvalidNewAccountValidationCOde()
        {
            var emailCredential = Factors.ForUser(_userAccount).CreateCredential<EmailFeatureType>(_userEmailAddress);
            var verificationResult = Factors.ForUser(_userAccount).VerifyCredentialRegistration<EmailFeatureType>(Guid.NewGuid().ToString().Substring(0, 6));

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
