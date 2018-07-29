using System;
using System.Linq;
using Factors.Feature.Email;
using Factors.Feature.Email.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Factors.Tests
{
    [TestClass]
    public class EmailFeature
    {
        private readonly string _userAccount = Guid.NewGuid().ToString();
        private readonly string _userEmailAddress = "user@domain.tld";
        private readonly string _senderAddress = "factors@domain.tld";
        private readonly string _senderName = "Factors";
        private readonly string _smtpHost = "localhost";
        private readonly int _smtpPort = 25;
        private readonly int _tokenExpirationTime = 5;

        [TestInitialize]
        public void Initalize()
        {
            Factor.Initalize(new Models.FactorsConfiguration
            {
                StorageDatabase = new Database.InMemory.Provider(),
                EncryptionProvider = new Encryption.PlainText.Provider(),
                TokenProvider = new Token.Number.Provider()
            }).InitializeEmailFactor(new Feature.Email.Models.EmailConfiguration
            {
                FromAddress = _senderAddress,
                FromName = _senderName,
                MailProvider = new Feature.Email.Smtp.Provider(_smtpHost, _smtpPort, false),
                TokenExpirationTime = TimeSpan.FromMinutes(_tokenExpirationTime)
            });
        }

        [TestCleanup()]
        public void Dispose()
        {
            Factor.Dispose();
        }

        [TestMethod]
        public void VerifyIsInitalized()
        {
            Assert.IsTrue(!String.IsNullOrWhiteSpace(Factor.ForUser(_userAccount).UserAccount));
        }

        [TestMethod]
        public void CreateEmailCredential()
        {
            var emailCredential = Factor.ForUser(_userAccount).CreateCredential<EmailFeatureType>(_userEmailAddress);

            Assert.IsNotNull(emailCredential);
            Assert.IsTrue(emailCredential.IsSuccess);
            Assert.IsNotNull(emailCredential.TokenDetails);
        }

        [TestMethod]
        public void VerifyEmailToken()
        {
            var emailCredential = Factor.ForUser(_userAccount).CreateCredential<EmailFeatureType>(_userEmailAddress);
            var verificationResult = Factor.ForUser(_userAccount).VerifyToken<EmailFeatureType>(emailCredential.TokenDetails.VerificationToken);

            Assert.IsTrue(verificationResult.Success);
        }

        [TestMethod]
        public void VerifyEmailAccountIsValidated()
        {
            var emailCredential = Factor.ForUser(_userAccount).CreateCredential<EmailFeatureType>(_userEmailAddress);
            var verificationResult = Factor.ForUser(_userAccount).VerifyToken<EmailFeatureType>(emailCredential.TokenDetails.VerificationToken);

            var accounts = Factor.ForUser(_userAccount).ListVerifiedAccounts<EmailFeatureType>();

            Assert.IsTrue(accounts.Count() > 0);
        }

        [TestMethod]
        public void VerifyEmailAccountIsNotValidated()
        {
            var emailCredential = Factor.ForUser(_userAccount).CreateCredential<EmailFeatureType>(_userEmailAddress);
            var accounts = Factor.ForUser(_userAccount).ListUnverifiedAccounts<EmailFeatureType>();

            Assert.IsTrue(accounts.Count() > 0);
        }

        [TestMethod]
        public void TryAndPassInvalidNewAccountValidationCOde()
        {
            var emailCredential = Factor.ForUser(_userAccount).CreateCredential<EmailFeatureType>(_userEmailAddress);
            var verificationResult = Factor.ForUser(_userAccount).VerifyToken<EmailFeatureType>(Guid.NewGuid().ToString().Substring(0, 6));

            Assert.IsFalse(verificationResult.Success);
        }

        [TestMethod]
        public void GetErrorWhenCreatingDuplicateCredentials()
        {
            var emailCredential = Factor.ForUser(_userAccount).CreateCredential<EmailFeatureType>(_userEmailAddress);
            var emailCredentialTwo = Factor.ForUser(_userAccount).CreateCredential<EmailFeatureType>(_userEmailAddress);

            Assert.IsFalse(emailCredentialTwo.IsSuccess);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetExceptionWhenSetuMissingFromAddress()
        {
            Factor.Dispose();

            Factor.Initalize(new Models.FactorsConfiguration
            {
                StorageDatabase = new Database.InMemory.Provider(),
                EncryptionProvider = new Encryption.PlainText.Provider(),
                TokenProvider = new Token.Number.Provider()
            }).InitializeEmailFactor(new Feature.Email.Models.EmailConfiguration
            {
                FromName = _senderName,
                MailProvider = new Feature.Email.Smtp.Provider(_smtpHost, _smtpPort, false),
                TokenExpirationTime = TimeSpan.FromMinutes(_tokenExpirationTime)
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetExceptionWhenSetupMissingFromName()
        {
            Factor.Dispose();

            Factor.Initalize(new Models.FactorsConfiguration
            {
                StorageDatabase = new Database.InMemory.Provider(),
                EncryptionProvider = new Encryption.PlainText.Provider(),
                TokenProvider = new Token.Number.Provider()
            }).InitializeEmailFactor(new Feature.Email.Models.EmailConfiguration
            {
                FromAddress = _senderAddress,
                MailProvider = new Feature.Email.Smtp.Provider(_smtpHost, _smtpPort, false),
                TokenExpirationTime = TimeSpan.FromMinutes(_tokenExpirationTime)
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetExceptionWhenSetupMissingMailProvider()
        {
            Factor.Dispose();

            Factor.Initalize(new Models.FactorsConfiguration
            {
                StorageDatabase = new Database.InMemory.Provider(),
                EncryptionProvider = new Encryption.PlainText.Provider(),
                TokenProvider = new Token.Number.Provider()
            }).InitializeEmailFactor(new Feature.Email.Models.EmailConfiguration
            {
                FromName = _senderName,
                FromAddress = _senderAddress,
                TokenExpirationTime = TimeSpan.FromMinutes(_tokenExpirationTime)
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetExceptionWhenSetupMissingTokenExpirationTime()
        {
            Factor.Dispose();

            Factor.Initalize(new Models.FactorsConfiguration
            {
                StorageDatabase = new Database.InMemory.Provider(),
                EncryptionProvider = new Encryption.PlainText.Provider(),
                TokenProvider = new Token.Number.Provider()
            }).InitializeEmailFactor(new Feature.Email.Models.EmailConfiguration
            {
                FromName = _senderName,
                FromAddress = _senderAddress,
                MailProvider = new Feature.Email.Smtp.Provider(_smtpHost, _smtpPort, false)
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetExceptionWhenSetupTokenExpirationNegativeNumber()
        {
            Factor.Dispose();

            Factor.Initalize(new Models.FactorsConfiguration
            {
                StorageDatabase = new Database.InMemory.Provider(),
                EncryptionProvider = new Encryption.PlainText.Provider(),
                TokenProvider = new Token.Number.Provider()
            }).InitializeEmailFactor(new Feature.Email.Models.EmailConfiguration
            {
                FromName = _senderName,
                FromAddress = _senderAddress,
                MailProvider = new Feature.Email.Smtp.Provider(_smtpHost, _smtpPort, false),
                TokenExpirationTime = TimeSpan.FromMinutes(-_tokenExpirationTime)
            });
        }
    }
}
