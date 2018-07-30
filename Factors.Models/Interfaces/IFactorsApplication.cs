namespace Factors.Models.Interfaces
{
    public interface IFactorsApplication
    {
        string UserAccount { get; set; }

        IFactorsConfiguration Configuration { get; set; }
    }
}
