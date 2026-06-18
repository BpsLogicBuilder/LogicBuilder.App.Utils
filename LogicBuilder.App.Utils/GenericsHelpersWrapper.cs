using LogicBuilder.App.Utils.Interfaces;
using LogicBuilder.Attributes;
using System.Collections.Generic;

namespace LogicBuilder.App.Utils
{
    public static class GenericsHelpersWrapper<T>
    {
        [AlsoKnownAs("ToList")]
        [FunctionGroup(FunctionGroup.Standard)]
        public static List<T> ToList(IGenericsHelpers genericsHelpers, IEnumerable<T> enumerable) => genericsHelpers.ToList(enumerable);

        [AlsoKnownAs("Single")]
        [FunctionGroup(FunctionGroup.Standard)]
        public static T Single(IGenericsHelpers genericsHelpers, IEnumerable<T> enumerable) => genericsHelpers.Single(enumerable);

        [AlsoKnownAs("SingleOrDefault")]
        [FunctionGroup(FunctionGroup.Standard)]
        public static T SingleOrDefault(IGenericsHelpers genericsHelpers, IEnumerable<T> enumerable) => genericsHelpers.SingleOrDefault(enumerable);

        [AlsoKnownAs("GetItemAtIndex")]
        [FunctionGroup(FunctionGroup.Standard)]
        public static T GetItemAtIndex(IGenericsHelpers genericsHelpers, IEnumerable<T> enumerable, int index) => genericsHelpers.GetItemAtIndex(enumerable, index);

        [AlsoKnownAs("GetPropertyValue")]
        [FunctionGroup(FunctionGroup.Standard)]
        public static T? GetPropertyValue(IGenericsHelpers genericsHelpers, object item, string propertyName) => genericsHelpers.GetPropertyValue<T>(item, propertyName);

        [AlsoKnownAs("AddItem")]
        [FunctionGroup(FunctionGroup.Standard)]
        public static void AddItem(IGenericsHelpers genericsHelpers, ICollection<T> collection, T item) => genericsHelpers.AddItem(collection, item);

        [AlsoKnownAs("CreateInstance")]
        public static T CreateInstance(IGenericsHelpers genericsHelpers) => genericsHelpers.CreateInstance<T>();

        [AlsoKnownAs("IsDefault")]
        public static bool IsDefault(IGenericsHelpers genericsHelpers, T? anyObject) => genericsHelpers.IsDefault(anyObject);

        [AlsoKnownAs("Any")]
        [FunctionGroup(FunctionGroup.Standard)]
        public static bool Any(IGenericsHelpers genericsHelpers, IEnumerable<T> enumerable) => genericsHelpers.Any(enumerable);
    }
}
