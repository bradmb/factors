using System;
using System.Linq;
using Factors.Feature.Phone;
using Factors.Feature.Phone.Models;
using Factors.Feature.Phone.Twilio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.OrmLite;
using Factors.Feature.Phone.NullRoute;

namespace Factors.Tests
{
    [TestClass]
    public class PhoneFeature
    {
        private readonly string _userAccount = Guid.NewGuid().ToString();
        private readonly string _userPhoneNumber = "USERPHONENUMBER";
        private readonly string _accountSid = "ACCOUNTSID";
        private readonly string _authToken = "AUTHTOKEN";
        private readonly string _phoneNumber = "PHONENUMBER";
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
            }).UsePhoneFactor(new PhoneConfiguration
            {
#if DEBUGTWILIO
                MessagingProvider = new PhoneTwilioProvider(_accountSid, _authToken, _phoneNumber),
#else
                MessagingProvider = new PhoneNullRouteProvider(),
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
        public void CreatePhoneCredential()
        {
            var phoneCredential = Factors.ForUser(_userAccount).CreateCredential<PhoneFeatureType>(_userPhoneNumber);

            Assert.IsNotNull(phoneCredential);
            Assert.IsTrue(phoneCredential.IsSuccess);
            Assert.IsNotNull(phoneCredential.TokenDetails);
        }

        [TestMethod]
        public void VerifyPhoneToken()
        {
            var phoneCredential = Factors.ForUser(_userAccount).CreateCredential<PhoneFeatureType>(_userPhoneNumber);
            var verificationResult = Factors.ForUser(_userAccount).VerifyToken<PhoneFeatureType>(phoneCredential.TokenRequestId.Value, phoneCredential.TokenDetails.VerificationToken);

            Assert.IsTrue(verificationResult.Success);
        }

        [TestMethod]
        public void VerifyTokenWorksMultipleTimes()
        {
            var phoneCredential = Factors.ForUser(_userAccount).CreateCredential<PhoneFeatureType>(_userPhoneNumber);
            var verificationResult = Factors.ForUser(_userAccount).VerifyToken<PhoneFeatureType>(phoneCredential.TokenRequestId.Value, phoneCredential.TokenDetails.VerificationToken);

            Assert.IsTrue(verificationResult.Success);

            var tokenRequest = Factors.ForUser(_userAccount).BeginTokenRequest<PhoneFeatureType>(_userPhoneNumber);
            var tokenResult = Factors.ForUser(_userAccount).VerifyToken<PhoneFeatureType>(tokenRequest.TokenRequestId.Value, tokenRequest.TokenDetails.VerificationToken);

            Assert.IsTrue(tokenResult.Success);
        }

        [TestMethod]
        public void VerifyPhoneAccountIsValidated()
        {
            var phoneCredential = Factors.ForUser(_userAccount).CreateCredential<PhoneFeatureType>(_userPhoneNumber);
            var verificationResult = Factors.ForUser(_userAccount).VerifyToken<PhoneFeatureType>(phoneCredential.TokenRequestId.Value, phoneCredential.TokenDetails.VerificationToken);

            var accounts = Factors.ForUser(_userAccount).ListVerifiedAccounts<PhoneFeatureType>();

            Assert.IsTrue(accounts.Count() > 0);
        }

        [TestMethod]
        public void VerifyPhoneAccountIsNotValidated()
        {
            var phoneCredential = Factors.ForUser(_userAccount).CreateCredential<PhoneFeatureType>(_userPhoneNumber);
            var accounts = Factors.ForUser(_userAccount).ListUnverifiedAccounts<PhoneFeatureType>();

            Assert.IsTrue(accounts.Count() > 0);
        }

        [TestMethod]
        public void TryAndPassInvalidNewAccountValidationCode()
        {
            var phoneCredential = Factors.ForUser(_userAccount).CreateCredential<PhoneFeatureType>(_userPhoneNumber);
            var verificationResult = Factors.ForUser(_userAccount).VerifyToken<PhoneFeatureType>(phoneCredential.TokenRequestId.Value, Guid.NewGuid().ToString().Substring(0, 6));

            Assert.IsFalse(verificationResult.Success);
        }

        [TestMethod]
        public void GetErrorWhenCreatingDuplicateCredentials()
        {
            var phoneCredential = Factors.ForUser(_userAccount).CreateCredential<PhoneFeatureType>(_userPhoneNumber);
            var phoneCredentialTwo = Factors.ForUser(_userAccount).CreateCredential<PhoneFeatureType>(_userPhoneNumber);

            Assert.IsFalse(phoneCredentialTwo.IsSuccess);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetExceptionWhenSetupMissingAuthToken()
        {
            Factors.Dispose();

            Factors.Initalize(new Models.FactorsConfiguration
            {
                StorageDatabase = new Database.OrmLite.Provider(":memory:", SqliteDialect.Provider),
                EncryptionProvider = new Encryption.PlainText.Provider(),
                TokenProvider = new Token.Number.Provider()
            }).UsePhoneFactor(new PhoneConfiguration
            {
                MessagingProvider = new PhoneTwilioProvider(_accountSid, String.Empty, _phoneNumber),
                TokenExpirationTime = TimeSpan.FromMinutes(_tokenExpirationTime)
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetExceptionWhenSetupMissingAccountSid()
        {
            Factors.Dispose();

            Factors.Initalize(new Models.FactorsConfiguration
            {
                StorageDatabase = new Database.OrmLite.Provider(":memory:", SqliteDialect.Provider),
                EncryptionProvider = new Encryption.PlainText.Provider(),
                TokenProvider = new Token.Number.Provider()
            }).UsePhoneFactor(new PhoneConfiguration
            {
                MessagingProvider = new PhoneTwilioProvider(String.Empty, _authToken, _phoneNumber),
                TokenExpirationTime = TimeSpan.FromMinutes(_tokenExpirationTime)
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetExceptionWhenSetupMissingPhoneNumber()
        {
            Factors.Dispose();

            Factors.Initalize(new Models.FactorsConfiguration
            {
                StorageDatabase = new Database.OrmLite.Provider(":memory:", SqliteDialect.Provider),
                EncryptionProvider = new Encryption.PlainText.Provider(),
                TokenProvider = new Token.Number.Provider()
            }).UsePhoneFactor(new PhoneConfiguration
            {
                MessagingProvider = new PhoneTwilioProvider(_accountSid, _authToken, String.Empty),
                TokenExpirationTime = TimeSpan.FromMinutes(_tokenExpirationTime)
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetExceptionWhenSetupMissingTextMessageTemplate()
        {
            Factors.Dispose();

            Factors.Initalize(new Models.FactorsConfiguration
            {
                StorageDatabase = new Database.OrmLite.Provider(":memory:", SqliteDialect.Provider),
                EncryptionProvider = new Encryption.PlainText.Provider(),
                TokenProvider = new Token.Number.Provider()
            }).UsePhoneFactor(new PhoneConfiguration
            {
                TextMessageTemplate = String.Empty,
                MessagingProvider = new PhoneTwilioProvider(_accountSid, _authToken, _phoneNumber),
                TokenExpirationTime = TimeSpan.FromMinutes(_tokenExpirationTime)
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetExceptionWhenSetupMissingMessagingProvider()
        {
            Factors.Dispose();

            Factors.Initalize(new Models.FactorsConfiguration
            {
                StorageDatabase = new Database.OrmLite.Provider(":memory:", SqliteDialect.Provider),
                EncryptionProvider = new Encryption.PlainText.Provider(),
                TokenProvider = new Token.Number.Provider()
            }).UsePhoneFactor(new PhoneConfiguration
            {
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
            }).UsePhoneFactor(new PhoneConfiguration
            {
                MessagingProvider = new PhoneTwilioProvider(_accountSid, _authToken, _phoneNumber)
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
            }).UsePhoneFactor(new PhoneConfiguration
            {
                MessagingProvider = new PhoneTwilioProvider(_accountSid, _authToken, _phoneNumber),
                TokenExpirationTime = TimeSpan.FromMinutes(-_tokenExpirationTime)
            });
        }
    }
}
