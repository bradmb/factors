using Factors.Models;
using System;

namespace Factors
{
    public partial class FactorsInstance
    {
        public readonly FactorsConfiguration _configuration;
        public string UserAccount { get; internal set; }

        public FactorsInstance()
        {
            throw new UnauthorizedAccessException("Please initialize Factors by calling Factors.Initialization.Initialize");
        }

        internal FactorsInstance(FactorsConfiguration configuration)
        {
            _configuration = configuration;
            _configuration.StorageDatabase.InitializeDatabaseSchema();
        }
    }
}
