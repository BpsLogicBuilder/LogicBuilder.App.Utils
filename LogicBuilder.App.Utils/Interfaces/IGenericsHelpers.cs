using LogicBuilder.Attributes;
using System.Collections.Generic;

namespace LogicBuilder.App.Utils.Interfaces
{
    public interface IGenericsHelpers<T>
    {
        [AlsoKnownAs("ToList")]
        [FunctionGroup(FunctionGroup.Standard)]
        List<T> ToList(IEnumerable<T> enumerable);

        [AlsoKnownAs("Single")]
        [FunctionGroup(FunctionGroup.Standard)]
        T Single(IEnumerable<T> enumerable);

        [AlsoKnownAs("SingleOrDefault")]
        [FunctionGroup(FunctionGroup.Standard)]
        T SingleOrDefault(IEnumerable<T> enumerable);

        [AlsoKnownAs("GetItemAtIndex")]
        [FunctionGroup(FunctionGroup.Standard)]
        T GetItemAtIndex(IEnumerable<T> enumerable, int index);

        [AlsoKnownAs("AddItem")]
        [FunctionGroup(FunctionGroup.Standard)]
        void AddItem(ICollection<T> collection, T item);

        [AlsoKnownAs("CreateInstance")]
        T CreateInstance();

        [AlsoKnownAs("IsDefault")]
        bool IsDefault(T? anyObject);

        [AlsoKnownAs("Any")]
        [FunctionGroup(FunctionGroup.Standard)]
        bool Any(IEnumerable<T> enumerable);
    }
}
