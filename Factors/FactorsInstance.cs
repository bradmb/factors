using Factors.Models.Interfaces;
using System;

namespace Factors
{
    public partial class FactorsInstance : IFactorInstance
    {
        public IFactorConfiguration Configuration { get; set; }
        public string UserAccount { get; set; }

        public FactorsInstance()
        {
            throw new UnauthorizedAccessException("Please initialize Factors by calling Factors.Initialization.Initialize");
        }

        internal FactorsInstance(IFactorConfiguration configuration)
        {
            Configuration = configuration;
            Configuration.StorageDatabase.InitializeDatabaseSchema();
        }
    }
}
