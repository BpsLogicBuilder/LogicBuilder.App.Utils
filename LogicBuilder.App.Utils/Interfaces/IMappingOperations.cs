using LogicBuilder.Expressions.Utils.ExpansionDescriptors;
using LogicBuilder.Expressions.Utils.Expansions;
using LogicBuilder.Expressions.Utils.ExpressionBuilder;
using LogicBuilder.Expressions.Utils.ExpressionDescriptors;
using LogicBuilder.Forms.Parameters.Expansions;
using LogicBuilder.Forms.Parameters.Expressions;

namespace LogicBuilder.App.Utils.Interfaces
{
    public interface IMappingOperations
    {
        SelectExpandDefinition MapExpansion(SelectExpandDefinitionDescriptor expression);
        SelectExpandDefinition MapExpansion(SelectExpandDefinitionParameters expression);
        IExpressionPart MapToOperator(DescriptorBase expression);
        IExpressionPart MapToOperator(IExpressionParameter expression);
    }
}
