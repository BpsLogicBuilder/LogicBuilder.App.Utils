using LogicBuilder.RulesDirector;

namespace Contoso.Test.Flow
{
    public static class FlowActivityFactory
    {
        #region Variables
        #endregion Variables

        public static IFlowActivity Create(IFlowManager flowManager)
            => new FlowActivity(flowManager);
    }
}
