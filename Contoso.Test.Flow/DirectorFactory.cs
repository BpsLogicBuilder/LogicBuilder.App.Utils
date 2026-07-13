using LogicBuilder.RulesDirector;

namespace Contoso.Test.Flow
{
    public class DirectorFactory(IRulesCache rulesCache)
    {

        #region Variables
        private readonly IRulesCache _rulesCache = rulesCache;
        #endregion Variables

        public DirectorBase Create(IFlowManager flowManager)
            => new Director(flowManager, _rulesCache);
    }
}
