using LogicBuilder.Attributes;
using System.Collections.Generic;

namespace LogicBuilder.App.Utils.Interfaces
{
    public interface IGenericsHelpers
    {
        [AlsoKnownAs("ToList")]
        [FunctionGroup(FunctionGroup.Standard)]
        List<T> ToList<T>(IEnumerable<T> enumerable);

        [AlsoKnownAs("Single")]
        [FunctionGroup(FunctionGroup.Standard)]
        T Single<T>(IEnumerable<T> enumerable);

        [AlsoKnownAs("SingleOrDefault")]
        [FunctionGroup(FunctionGroup.Standard)]
        T SingleOrDefault<T>(IEnumerable<T> enumerable);

        [AlsoKnownAs("GetItemAtIndex")]
        [FunctionGroup(FunctionGroup.Standard)]
        T GetItemAtIndex<T>(IEnumerable<T> enumerable, int index);

        [AlsoKnownAs("GetPropertyValue")]
        [FunctionGroup(FunctionGroup.Standard)]
        T? GetPropertyValue<T>(object item, string propertyName);

        [AlsoKnownAs("AddItem")]
        [FunctionGroup(FunctionGroup.Standard)]
        void AddItem<T>(ICollection<T> collection, T item);

        [AlsoKnownAs("CreateInstance")]
        T CreateInstance<T>();

        [AlsoKnownAs("IsDefault")]
        bool IsDefault<T>(T? anyObject);

        [AlsoKnownAs("Any")]
        [FunctionGroup(FunctionGroup.Standard)]
        bool Any<T>(IEnumerable<T> enumerable);
    }
}
