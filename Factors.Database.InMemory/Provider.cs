using Factors.Models.Interfaces;
using Factors.Models.UserAccount;
using ServiceStack.OrmLite;

namespace Factors.Database.InMemory
{
    public partial class Provider : IFactorsDatabase
    {
        private OrmLiteConnectionFactory _dbConnection;

        public Provider()
        {
            _dbConnection = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider);
        }

        public void InitializeDatabaseSchema()
        {
            using (var db = _dbConnection.Open())
            {
                db.CreateTableIfNotExists<FactorCredential>();
                db.CreateTableIfNotExists<FactorGeneratedToken>();
            }
        }

        public void Dispose()
        {
            using (var db = _dbConnection.Open())
            {
                db.DropTable<FactorCredential>();
                db.DropTable<FactorGeneratedToken>();
            }
        }
    }
}
