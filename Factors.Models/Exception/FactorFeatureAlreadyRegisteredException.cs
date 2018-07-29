namespace Factors.Models.Exception
{
    public class FactorFeatureAlreadyRegisteredException : System.Exception
    {
        public FactorFeatureAlreadyRegisteredException() {}

        public FactorFeatureAlreadyRegisteredException(string message) : base(message) {}

        public FactorFeatureAlreadyRegisteredException(string message, System.Exception inner) : base(message, inner) {}
    }
}
