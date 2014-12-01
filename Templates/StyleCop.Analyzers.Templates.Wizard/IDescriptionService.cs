namespace StyleCop.Analyzers.Templates.Wizard
{
    using System.Threading.Tasks;

    public interface IDescriptionService
    {
        Task<PageInfo> GetPageInfo(decimal saId);
        Task<bool> RuleExists(decimal saId);
    }
}