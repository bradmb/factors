using Factors.Models.Interfaces;
using Factors.Models.UserAccount;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Factors.Database.SqlServer
{
    public class Provider : IFactorsDatabase
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initalizes the database provider. Ensure sure your connection
        /// string provides the database to be used.
        /// </summary>
        /// <param name="connectionString"></param>
        public Provider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public FactorCredential CreateCredential(FactorCredential model)
        {
            throw new NotImplementedException();
        }

        public Task<FactorCredential> CreateCredentialAsync(FactorCredential model)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void InitializeDatabaseSchema()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FactorCredential> ListCredentialsFor(string userAccountId, IFeatureType featureType, FactorCredentialVerificationType accountsToInclude)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FactorCredential>> ListCredentialsForAsync(string userAccountId, IFeatureType featureType, FactorCredentialVerificationType accountsToInclude)
        {
            throw new NotImplementedException();
        }

        public FactorGeneratedToken StoreToken(FactorGeneratedToken model)
        {
            throw new NotImplementedException();
        }

        public Task<FactorGeneratedToken> StoreTokenAsync(FactorGeneratedToken model)
        {
            throw new NotImplementedException();
        }

        public FactorVerificationResult VerifyToken(string userAccountId, IFeatureType featureType, string tokenValue)
        {
            throw new NotImplementedException();
        }

        public Task<FactorVerificationResult> VerifyTokenAsync(string userAccountId, IFeatureType featureType, string tokenValue)
        {
            throw new NotImplementedException();
        }
    }
}
