using LogicBuilder.App.Utils.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LogicBuilder.App.Utils
{
    public class GenericsHelpers(ILogger<GenericsHelpers> logger) : IGenericsHelpers
    {
        private readonly ILogger<GenericsHelpers> _logger = logger;

        public void AddItem<T>(ICollection<T> collection, T item) => collection.Add(item);

        public bool Any<T>(IEnumerable<T> enumerable) => enumerable.Any();

        public T CreateInstance<T>() => Activator.CreateInstance<T>();

        public T GetItemAtIndex<T>(IEnumerable<T> enumerable, int index) => enumerable.ElementAt(index);

        public T? GetPropertyValue<T>(object item, string propertyName)
        {
            try
            {
                return GetPropertyValue<T>
                (
                    item.GetType().GetProperty
                    (
                        propertyName,
                        BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance
                    )
                    .GetValue(item)
                );
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "{ExceptionType} : {ExceptionMessage}", ex.GetType().Name, ex.Message);
                throw new InvalidOperationException($"Failed to get property '{propertyName}' from type '{item?.GetType().Name}'.", ex);
            }
        }

        public bool IsDefault<T>(T? anyObject)
        {
            if (anyObject is null && default(T) is null)
                return true;

            return anyObject?.Equals(default(T)) == true;
        }

        public T Single<T>(IEnumerable<T> enumerable) => enumerable.Single();

        public T SingleOrDefault<T>(IEnumerable<T> enumerable) => enumerable.SingleOrDefault();

        public List<T> ToList<T>(IEnumerable<T> enumerable) => [.. enumerable];

        private static T? GetPropertyValue<T>(object valueObject)
        {
            if (valueObject == null)
                return default;

            Type valueObjectType = valueObject.GetType();
            if (typeof(T) == valueObjectType)
                return (T)valueObject;

            return (T)Convert.ChangeType(valueObject, typeof(T));
        }
    }
}
