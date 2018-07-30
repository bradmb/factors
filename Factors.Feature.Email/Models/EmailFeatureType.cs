using Factors.Models.Interfaces;

namespace Factors.Feature.Email.Models
{
    public class EmailFeatureType : IFactorsFeatureType
    {
        public string FeatureName { get { return "FactorEmail"; } }
        public string FeatureGuid { get { return "FF-EM-2018-7-29"; } }
    }
}