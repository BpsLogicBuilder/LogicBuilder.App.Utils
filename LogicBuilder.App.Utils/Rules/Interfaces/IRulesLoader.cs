using LogicBuilder.RulesDirector;
using System.Threading.Tasks;

namespace LogicBuilder.App.Utils.Rules.Interfaces
{
    public interface IRulesLoader
    {
        Task LoadRulesOnStartUp(RulesModule module, IRulesCache cache, RulesLoaderRequest rulesLoaderRequest);
        Task LoadRules(RulesModule module, IRulesCache cache, RulesLoaderRequest rulesLoaderRequest);
    }
}
