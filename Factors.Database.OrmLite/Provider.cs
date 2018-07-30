using Factors.Models.Interfaces;
using Factors.Models.UserAccount;
using ServiceStack.OrmLite;

namespace Factors.Database.OrmLite
{
    public partial class Provider : IFactorsDatabaseProvider
    {
        private OrmLiteConnectionFactory _dbConnection;

        public Provider(string connectionString, IOrmLiteDialectProvider dialectProvider)
        {
            _dbConnection = new OrmLiteConnectionFactory(connectionString, dialectProvider);
        }

        public void InitializeDatabaseSchema()
        {
            using (var db = _dbConnection.Open())
            {
                db.CreateTableIfNotExists<FactorsCredential>();
                db.CreateTableIfNotExists<FactorsCredentialGeneratedToken>();
            }
        }

        public void Dispose()
        {
            using (var db = _dbConnection.Open())
            {
                db.DropTable<FactorsCredential>();
                db.DropTable<FactorsCredentialGeneratedToken>();
            }
        }
    }
}
