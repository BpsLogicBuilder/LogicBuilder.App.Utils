using LogicBuilder.RulesDirector;

namespace Contoso.Test.Flow
{
    public class Director(IFlowManager flowManager, IRulesCache rulesCache) : AppDirectorBase
    {

        #region Fields
        private readonly IFlowManager flowManager = flowManager;
        private readonly IRulesCache rulesCache = rulesCache;
        #endregion Fields

        #region Properties
        protected override IRulesCache RulesCache => this.rulesCache;
        protected override IFlowActivity FlowActivity => this.flowManager.FlowActivity;
        protected override Progress Progress => this.flowManager.Progress;
        #endregion Properties

        public override void SetCurrentBusinessBackupData() => this.flowManager.SetCurrentBusinessBackupData();
    }
}
