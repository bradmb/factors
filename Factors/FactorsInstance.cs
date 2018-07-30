using Factors.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace Factors
{
    public partial class FactorsInstance : IFactorInstance
    {
        public IFactorConfiguration Configuration { get; set; }
        public string UserAccount { get; set; }

        internal Dictionary<string, IFactorFeature> Features = new Dictionary<string, IFactorFeature>();

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
