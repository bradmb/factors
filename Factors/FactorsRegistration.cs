using Factors.Models.Exception;
using Factors.Models.Interfaces;

namespace Factors
{
    public class FactorsRegistration {
        /// <summary>
        /// Registers a new feature with the Factors instance
        /// </summary>
        /// <typeparam name="tt"></typeparam>
        /// <param name="feature"></param>
        public void RegisterFeature<tt>(IFactorsFeatureProvider feature) where tt : IFactorsFeatureType, new()
        {
            var featureType = new tt();
            if (Factors.Instance.Features.ContainsKey(featureType.FeatureGuid))
            {
                var featureName = featureType.GetType().Name;
                throw new FactorFeatureAlreadyRegisteredException($"Feature {featureName} is already registered with Factors.");
            }

            Factors.Instance.Features.Add(featureType.FeatureGuid, feature);
        }
    }
}
