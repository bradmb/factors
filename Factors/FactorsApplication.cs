using Factors.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace Factors
{
    public partial class FactorsApplication : IFactorsApplication
    {
        public IFactorsConfiguration Configuration { get; set; }
        public string UserAccount { get; set; }

        internal Dictionary<string, IFactorsFeatureProvider> Features = new Dictionary<string, IFactorsFeatureProvider>();

        public FactorsApplication()
        {
            throw new UnauthorizedAccessException("Please initialize Factors by calling Factors.Initialization.Initialize");
        }

        internal FactorsApplication(IFactorsConfiguration configuration)
        {
            Configuration = configuration;
            Configuration.StorageDatabase.InitializeDatabaseSchema();
        }
    }
}
