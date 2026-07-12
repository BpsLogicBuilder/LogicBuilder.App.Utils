using LogicBuilder.App.Utils.Interfaces;
using LogicBuilder.Forms.Parameters.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LogicBuilder.App.Utils
{
    public class DictionaryHelper(IMappingOperations mappingOperations) : IDictionaryHelper
    {
        private readonly IMappingOperations _mappingOperations = mappingOperations;

        public IDictionary<TKey, TValue> ToDictionary<TSource, TKey, TValue>(IEnumerable<TSource> enumerable, SelectorLambdaOperatorParameters keySelector, SelectorLambdaOperatorParameters valueSelector)
            => enumerable.ToDictionary
            (
                ((Expression<Func<TSource, TKey>>)_mappingOperations.MapToOperator(keySelector).Build()).Compile(),
                ((Expression<Func<TSource, TValue>>)_mappingOperations.MapToOperator(valueSelector).Build()).Compile()
            );
    }
}
