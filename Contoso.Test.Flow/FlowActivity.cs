using Contoso.Domain.Entities;
using LogicBuilder.App.Utils.Rules.Interfaces;
using LogicBuilder.RulesDirector;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Contoso.Test.Flow
{
    public class FlowActivity(IFlowManager flowManager) : IFlowActivity
    {

        #region Fields
        private readonly IFlowManager flowManager = flowManager;
        #endregion Fields

        #region Properties
        public DirectorBase Director => this.flowManager.Director;
        public static Assembly[] MustReference => 
            [//each assembly needed by the Logic Builder muts be referenced
                typeof(StudentModel).Assembly, 
                typeof(IRulesLoader).Assembly
            ];
        #endregion Properties

        #region Methods
        public string FormatString(string format, Collection<object> list)
            => FormatString(format, list.ToArray());

        public string FormatString(string format, object[] list) 
            => string.Format(CultureInfo.CurrentCulture, format, list);

        public void FlowComplete() => this.flowManager.FlowComplete();

        public void Terminate() => this.flowManager.Terminate();
        #endregion Methods
    }
}
