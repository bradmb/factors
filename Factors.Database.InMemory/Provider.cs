using Factors.Interfaces;
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
    }
}
