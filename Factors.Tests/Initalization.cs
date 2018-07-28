﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            Factor.Initalize(new Models.FactorsConfiguration
            {
                StorageDatabase = new Database.InMemory.Provider(),
                EncryptionProvider = new Encryption.PlainText.Provider(),
                TokenProvider = new Token.Number.Provider()
            });
        }

        [TestCleanup()]
        public void Dispose()
        {
            Factor.Dispose();
        }

        [TestMethod]
        public void VerifyInitalization()
        {
            Assert.IsNotNull(Factor.ForUser(_userAccount));
        }
    }
}