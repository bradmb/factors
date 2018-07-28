using Factors.Interfaces;
using Factors.Models.UserAccount;
using ServiceStack.OrmLite;

namespace Factors.Database.InMemory
{
    public partial class Provider : IFactorsDatabase
    {
        public void InitializeDatabaseSchema()
        {
            using (var db = _dbConnection.Open())
            {
                db.CreateTableIfNotExists<FactorCredential>();
                db.CreateTableIfNotExists<FactorGeneratedToken>();
            }
        }
    }
}
