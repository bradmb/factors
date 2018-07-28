using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Factors.Tests
{
    [TestClass]
    public class Initalization
    {
        [TestMethod]
        public void InitalizeFactors()
        {
            var userAccount = new Guid().ToString();

            Factor.Initalize(new Models.FactorsConfiguration
            {
                StorageDatabase = new Database.InMemory.Provider(),
                EncryptionProvider = new Encryption.PlainText.Provider(),
                TokenProvider = new Token.Number.Provider()
            });

            Assert.IsNotNull(Factor.ForUser(userAccount));
        }
    }
}