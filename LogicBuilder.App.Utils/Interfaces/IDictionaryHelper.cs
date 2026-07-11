using LogicBuilder.Forms.Parameters.Expressions;
using System.Collections.Generic;

namespace LogicBuilder.App.Utils.Interfaces
{
    public interface IDictionaryHelper
    {
        IDictionary<TKey, TValue> ToDictionary<TSource, TKey, TValue>(IEnumerable<TSource> enumerable, SelectorLambdaOperatorParameters keySelector, SelectorLambdaOperatorParameters valueSelector);
    }
}
