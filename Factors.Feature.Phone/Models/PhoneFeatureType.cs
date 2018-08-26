using Factors.Models.Interfaces;

namespace Factors.Feature.Phone.Models
{
    public class PhoneFeatureType : IFactorsFeatureType
    {
        public string FeatureName { get { return "FactorPhone"; } }
        public string FeatureGuid { get { return "FF-PH-2018-8-26"; } }
    }
}
