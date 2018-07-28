using Factors.Interfaces;

namespace Factors.Feature.Email.Models
{
    public class FeatureType : IFeatureTypeProvider
    {
        public string FeatureName { get { return "EmailProvider"; } }
    }
}
