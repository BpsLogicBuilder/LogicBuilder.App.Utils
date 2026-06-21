using LogicBuilder.Attributes;
using System.Collections.Generic;

namespace LogicBuilder.App.Utils.Interfaces
{
    public interface IGenericsHelpers
    {
        List<T> ToList<T>(IEnumerable<T> enumerable);

        T Single<T>(IEnumerable<T> enumerable);

        T SingleOrDefault<T>(IEnumerable<T> enumerable);

        T GetItemAtIndex<T>(IEnumerable<T> enumerable, int index);

        T? GetPropertyValue<T>(object item, string propertyName);

        TValue GetValue<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key);

        void AddItem<T>(ICollection<T> collection, T item);

        T CreateInstance<T>();

        bool IsDefault<T>(T? anyObject);

        bool Any<T>(IEnumerable<T> enumerable);
    }
}
