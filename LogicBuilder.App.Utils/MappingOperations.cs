using AutoMapper;
using LogicBuilder.App.Utils.Interfaces;
using LogicBuilder.Expressions.Utils.ExpansionDescriptors;
using LogicBuilder.Expressions.Utils.Expansions;
using LogicBuilder.Expressions.Utils.ExpressionBuilder;
using LogicBuilder.Expressions.Utils.ExpressionDescriptors;
using LogicBuilder.Forms.Parameters.Expansions;
using LogicBuilder.Forms.Parameters.Expressions;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LogicBuilder.App.Utils
{
    public class MappingOperations(IMapper mapper) : IMappingOperations
    {
        private readonly IMapper _mapper = mapper;
        const string PARAMETERS_KEY = "parameters";

        public SelectExpandDefinition MapExpansion(SelectExpandDefinitionDescriptor expression)
            => _mapper.Map<SelectExpandDefinition>
            (
                expression,
                opts => opts.Items[PARAMETERS_KEY] = new Dictionary<string, ParameterExpression>()
            );

        public SelectExpandDefinition MapExpansion(SelectExpandDefinitionParameters expression)
            => MapExpansion
            (
                _mapper.Map<SelectExpandDefinitionDescriptor>(expression)
            );

        public IExpressionPart MapToOperator(DescriptorBase expression)
            => _mapper.Map<IExpressionPart>
            (
                expression,
                opts => opts.Items[PARAMETERS_KEY] = new Dictionary<string, ParameterExpression>()
            );

        public IExpressionPart MapToOperator(IExpressionParameter expression)
            => MapToOperator
            (
                _mapper.Map<DescriptorBase>(expression)
            );
    }
}
