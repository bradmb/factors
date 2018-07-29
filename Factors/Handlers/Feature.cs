using Factors.Models.Interfaces;
using Factors.Models.Exception;
using System.Collections.Generic;

namespace Factors
{
    public partial class FactorsInstance : IFactorInstance
    {
        private Dictionary<string, IFactorFeature> Features = new Dictionary<string, IFactorFeature>();

        public void RegisterFeature<tt>(IFactorFeature feature) where tt : IFeatureType, new ()
        {
            var featureType = new tt();
            if (Features.ContainsKey(featureType.FeatureGuid))
            {
                var featureName = featureType.GetType().Name;
                throw new FactorFeatureAlreadyRegisteredException($"Feature {featureName} is already registered with Factors.");
            }

            Features.Add(featureType.FeatureGuid, feature);
        }
    }
}
