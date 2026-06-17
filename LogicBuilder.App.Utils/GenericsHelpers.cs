using LogicBuilder.App.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LogicBuilder.App.Utils
{
    public class GenericsHelpers<T> : IGenericsHelpers<T>
    {
        public void AddItem(ICollection<T> collection, T item) => collection.Add(item);

        public bool Any(IEnumerable<T> enumerable) => enumerable.Any();

        public T CreateInstance() => Activator.CreateInstance<T>();

        public T GetItemAtIndex(IEnumerable<T> enumerable, int index) => enumerable.ElementAt(index);

        public bool IsDefault(T? anyObject) => anyObject?.Equals(default(T)) == true;

        public T Single(IEnumerable<T> enumerable) => enumerable.Single();

        public T SingleOrDefault(IEnumerable<T> enumerable) => enumerable.SingleOrDefault();

        public List<T> ToList(IEnumerable<T> enumerable) => [.. enumerable];
    }
}
