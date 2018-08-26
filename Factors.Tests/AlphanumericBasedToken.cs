using Factors.Feature.Email;
using Factors.Feature.Email.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.OrmLite;
using System;
using System.Text.RegularExpressions;

namespace Factors.Tests
{
    [TestClass]
    public class AlphanumericBasedToken
    {
        private readonly string _userAccount = Guid.NewGuid().ToString();
        private readonly string _userEmailAddress = "user@domain.tld";
        private readonly string _senderAddress = "factors@domain.tld";
        private readonly string _senderName = "Factors";
        private readonly int _tokenExpirationTime = 5;

        [TestInitialize]
        public void Initalize()
        {
            Factors.Initalize(new Models.FactorsConfiguration
            {
                ApplicationName = "Factors Unit Test",
                StorageDatabase = new Database.OrmLite.Provider(":memory:", SqliteDialect.Provider),
                EncryptionProvider = new Encryption.PlainText.Provider(),
                TokenProvider = new Token.Alphanumeric.Provider(),
            }).UseEmailFactor(new EmailConfiguration
            {
                FromAddress = _senderAddress,
                FromName = _senderName,
                MailProvider = new Feature.Email.NullRoute.EmailNullRouteProvider(),
                TokenExpirationTime = TimeSpan.FromMinutes(_tokenExpirationTime)
            });
        }

        [TestMethod]
        public void VerifyTokenHasLettersAndNumbers()
        {
            var emailCredential = Factors.ForUser(_userAccount).CreateCredential<EmailFeatureType>(_userEmailAddress);

            Assert.IsNotNull(emailCredential);
            Assert.IsTrue(emailCredential.IsSuccess);
            Assert.IsNotNull(emailCredential.TokenDetails);

            Assert.IsTrue(Regex.IsMatch(emailCredential.TokenDetails.VerificationToken, @"^[a-zA-Z0-9]+$"));
        }

        [TestCleanup()]
        public void Dispose()
        {
            Factors.Dispose();
        }

        [TestMethod]
        public void VerifyInitalization()
        {
            Assert.IsNotNull(Factors.ForUser(_userAccount));
        }
    }
}