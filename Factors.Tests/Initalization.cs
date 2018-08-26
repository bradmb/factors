using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.OrmLite;
using System;

namespace Factors.Tests
{
    [TestClass]
    public class Initalization
    {
        private readonly string _userAccount = Guid.NewGuid().ToString();

        [TestInitialize]
        public void Initalize()
        {
            Factors.Initalize(new Models.FactorsConfiguration
            {
                ApplicationName = "Factors Unit Test",
                StorageDatabase = new Database.OrmLite.Provider(":memory:", SqliteDialect.Provider),
                EncryptionProvider = new Encryption.PlainText.Provider(),
                TokenProvider = new Token.Number.Provider()
            });
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