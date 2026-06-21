using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LogicBuilder.App.Utils.Tests
{
    public class GenericsHelpersOfTTest
    {
        private readonly Mock<ILogger<GenericsHelpers>> _mockLogger;

        public GenericsHelpersOfTTest()
        {
            _mockLogger = new Mock<ILogger<GenericsHelpers>>();
        }

        #region AddItem Tests
        [Fact]
        public void AddItem_AddsItemToCollection()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var collection = new List<int> { 1, 2, 3 };

            // Act
            GenericsHelpers<int>.AddItem(helper, collection, 4);

            // Assert
            Assert.Equal(4, collection.Count);
            Assert.Contains(4, collection);
        }

        [Fact]
        public void AddItem_AddsStringItemToCollection()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var collection = new List<string> { "apple", "banana" };

            // Act
            GenericsHelpers<string>.AddItem(helper, collection, "cherry");

            // Assert
            Assert.Equal(3, collection.Count);
            Assert.Contains("cherry", collection);
        }
        #endregion

        #region Any Tests
        [Fact]
        public void Any_ReturnsTrueForNonEmptyEnumerable()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var enumerable = new List<int> { 1, 2, 3 };

            // Act
            var result = GenericsHelpers<int>.Any(helper, enumerable);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Any_ReturnsFalseForEmptyEnumerable()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var enumerable = new List<int>();

            // Act
            var result = GenericsHelpers<int>.Any(helper, enumerable);

            // Assert
            Assert.False(result);
        }
        #endregion

        #region CreateInstance Tests
        [Fact]
        public void CreateInstance_CreatesNewInstance()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);

            // Act
            var result = GenericsHelpers<TestClass>.CreateInstance(helper);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<TestClass>(result);
        }

        [Fact]
        public void CreateInstance_CreatesValueTypeInstance()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);

            // Act
            var result = GenericsHelpers<int>.CreateInstance(helper);

            // Assert
            Assert.Equal(0, result);
        }
        #endregion

        #region GetItemAtIndex Tests
        [Fact]
        public void GetItemAtIndex_ReturnsCorrectItem()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var enumerable = new List<string> { "first", "second", "third" };

            // Act
            var result = GenericsHelpers<string>.GetItemAtIndex(helper, enumerable, 1);

            // Assert
            Assert.Equal("second", result);
        }

        [Fact]
        public void GetItemAtIndex_ThrowsForInvalidIndex()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var enumerable = new List<int> { 1, 2, 3 };

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => GenericsHelpers<int>.GetItemAtIndex(helper, enumerable, 5));
        }

        [Fact]
        public void GetItemAtIndex_ReturnsFirstItemAtIndexZero()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var enumerable = new List<int> { 10, 20, 30 };

            // Act
            var result = GenericsHelpers<int>.GetItemAtIndex(helper, enumerable, 0);

            // Assert
            Assert.Equal(10, result);
        }
        #endregion

        #region IsDefault Tests
        [Fact]
        public void IsDefault_ReturnsTrueForDefaultValueType()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);

            // Act
            var result = GenericsHelpers<int>.IsDefault(helper, 0);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsDefault_ReturnsFalseForNonDefaultValueType()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);

            // Act
            var result = GenericsHelpers<int>.IsDefault(helper, 5);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsDefault_ReturnsFalseForNullReferenceType()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);

            // Act
            var result = GenericsHelpers<string>.IsDefault(helper, null);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsDefault_ReturnsFalseForNonDefaultReferenceType()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);

            // Act
            var result = GenericsHelpers<string>.IsDefault(helper, "test");

            // Assert
            Assert.False(result);
        }
        #endregion

        #region Single Tests
        [Fact]
        public void Single_ReturnsOnlyItem()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var enumerable = new List<int> { 42 };

            // Act
            var result = GenericsHelpers<int>.Single(helper, enumerable);

            // Assert
            Assert.Equal(42, result);
        }

        [Fact]
        public void Single_ThrowsForEmptyEnumerable()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var enumerable = new List<int>();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => GenericsHelpers<int>.Single(helper, enumerable));
        }

        [Fact]
        public void Single_ThrowsForMultipleItems()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var enumerable = new List<int> { 1, 2, 3 };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => GenericsHelpers<int>.Single(helper, enumerable));
        }
        #endregion

        #region SingleOrDefault Tests
        [Fact]
        public void SingleOrDefault_ReturnsOnlyItem()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var enumerable = new List<string> { "only" };

            // Act
            var result = GenericsHelpers<string>.SingleOrDefault(helper, enumerable);

            // Assert
            Assert.Equal("only", result);
        }

        [Fact]
        public void SingleOrDefault_ReturnsDefaultForEmptyEnumerable()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var enumerable = new List<int>();

            // Act
            var result = GenericsHelpers<int>.SingleOrDefault(helper, enumerable);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void SingleOrDefault_ReturnsNullForEmptyReferenceTypeEnumerable()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var enumerable = new List<string>();

            // Act
            var result = GenericsHelpers<string>.SingleOrDefault(helper, enumerable);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void SingleOrDefault_ThrowsForMultipleItems()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var enumerable = new List<int> { 1, 2 };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => GenericsHelpers<int>.SingleOrDefault(helper, enumerable));
        }
        #endregion

        #region ToList Tests
        [Fact]
        public void ToList_ConvertsEnumerableToList()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var enumerable = new[] { 1, 2, 3, 4, 5 };

            // Act
            var result = GenericsHelpers<int>.ToList(helper, enumerable);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<int>>(result);
            Assert.Equal(5, result.Count);
            Assert.Equal(enumerable, result);
        }

        [Fact]
        public void ToList_ConvertsEmptyEnumerableToEmptyList()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var enumerable = Enumerable.Empty<string>();

            // Act
            var result = GenericsHelpers<string>.ToList(helper, enumerable);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void ToList_PreservesOrder()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var enumerable = new[] { "z", "a", "m", "b" };

            // Act
            var result = GenericsHelpers<string>.ToList(helper, enumerable);

            // Assert
#pragma warning disable CA1861
            Assert.Equal(new[] { "z", "a", "m", "b" }, result);
#pragma warning restore CA1861
        }
        #endregion

        #region GetPropertyValue Tests
        [Fact]
        public void GetPropertyValue_ReturnsPropertyValue()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var testObj = new PropertyTestClass { StringProperty = "TestValue" };

            // Act
            var result = GenericsHelpers<string>.GetPropertyValue(helper, testObj, "StringProperty");

            // Assert
            Assert.Equal("TestValue", result);
        }

        [Fact]
        public void GetPropertyValue_IsCaseInsensitive()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var testObj = new PropertyTestClass { StringProperty = "TestValue" };

            // Act
            var result = GenericsHelpers<string>.GetPropertyValue(helper, testObj, "stringproperty");

            // Assert
            Assert.Equal("TestValue", result);
        }

        [Fact]
        public void GetPropertyValue_ReturnsIntProperty()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var testObj = new PropertyTestClass { IntProperty = 42 };

            // Act
            var result = GenericsHelpers<int>.GetPropertyValue(helper, testObj, "IntProperty");

            // Assert
            Assert.Equal(42, result);
        }

        [Fact]
        public void GetPropertyValue_ReturnsBoolProperty()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var testObj = new PropertyTestClass { BoolProperty = true };

            // Act
            var result = GenericsHelpers<bool>.GetPropertyValue(helper, testObj, "BoolProperty");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetPropertyValue_ReturnsNullForNullableProperty()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var testObj = new PropertyTestClass { NullableProperty = null };

            // Act
            var result = GenericsHelpers<string>.GetPropertyValue(helper, testObj, "NullableProperty");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetPropertyValue_ConvertsIntToString()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var testObj = new PropertyTestClass { IntProperty = 123 };

            // Act
            var result = GenericsHelpers<string>.GetPropertyValue(helper, testObj, "IntProperty");

            // Assert
            Assert.Equal("123", result);
        }

        [Fact]
        public void GetPropertyValue_ConvertsStringToInt()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var testObj = new PropertyTestClass { StringProperty = "456" };

            // Act
            var result = GenericsHelpers<int>.GetPropertyValue(helper, testObj, "StringProperty");

            // Assert
            Assert.Equal(456, result);
        }

        [Fact]
        public void GetPropertyValue_ThrowsForInvalidPropertyName()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var testObj = new PropertyTestClass();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                GenericsHelpers<string>.GetPropertyValue(helper, testObj, "NonExistentProperty"));
        }

        [Fact]
        public void GetPropertyValue_ThrowsForNullObject()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                GenericsHelpers<string>.GetPropertyValue(helper, null!, "AnyProperty"));
        }

        [Fact]
        public void GetPropertyValue_ThrowsForInvalidTypeConversion()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var testObj = new PropertyTestClass { StringProperty = "NotANumber" };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                GenericsHelpers<int>.GetPropertyValue(helper, testObj, "StringProperty"));
        }

        [Fact]
        public void GetPropertyValue_ReturnsDoubleProperty()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var testObj = new PropertyTestClass { DoubleProperty = 3.14 };

            // Act
            var result = GenericsHelpers<double>.GetPropertyValue(helper, testObj, "DoubleProperty");

            // Assert
            Assert.Equal(3.14, result);
        }

        [Fact]
        public void GetPropertyValue_ReturnsDateTimeProperty()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var expectedDate = new DateTime(2026, 6, 18, 0, 0, 0, DateTimeKind.Unspecified);
            var testObj = new PropertyTestClass { DateTimeProperty = expectedDate };

            // Act
            var result = GenericsHelpers<DateTime>.GetPropertyValue(helper, testObj, "DateTimeProperty");

            // Assert
            Assert.Equal(expectedDate, result);
        }
        #endregion

        #region GetValue Tests
        [Fact]
        public void GetValue_ReturnsValueWhenKeyExists()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var dictionary = new Dictionary<string, int>
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 }
            };

            // Act
            var result = GenericsHelpers<string, int>.GetValue(helper, dictionary, "two");

            // Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public void GetValue_ReturnsDefaultValueWhenKeyDoesNotExist()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var dictionary = new Dictionary<string, int>
            {
                { "one", 1 },
                { "two", 2 }
            };

            // Act
            var result = GenericsHelpers<string, int>.GetValue(helper, dictionary, "three");

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void GetValue_ReturnsNullWhenKeyDoesNotExistForReferenceType()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var dictionary = new Dictionary<int, string>
            {
                { 1, "one" },
                { 2, "two" }
            };

            // Act
            var result = GenericsHelpers<int, string>.GetValue(helper, dictionary, 3);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetValue_ReturnsStringValueWhenKeyExists()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var dictionary = new Dictionary<int, string>
            {
                { 1, "apple" },
                { 2, "banana" },
                { 3, "cherry" }
            };

            // Act
            var result = GenericsHelpers<int, string>.GetValue(helper, dictionary, 2);

            // Assert
            Assert.Equal("banana", result);
        }

        [Fact]
        public void GetValue_WorksWithComplexObjectValues()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var obj1 = new TestClass { Value = 10 };
            var obj2 = new TestClass { Value = 20 };
            var dictionary = new Dictionary<string, TestClass>
            {
                { "first", obj1 },
                { "second", obj2 }
            };

            // Act
            var result = GenericsHelpers<string, TestClass>.GetValue(helper, dictionary, "first");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.Value);
            Assert.Same(obj1, result);
        }

        [Fact]
        public void GetValue_ReturnsNullForComplexObjectWhenKeyDoesNotExist()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var dictionary = new Dictionary<string, TestClass>
            {
                { "first", new TestClass { Value = 10 } }
            };

            // Act
            var result = GenericsHelpers<string, TestClass>.GetValue(helper, dictionary, "second");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetValue_WorksWithBoolValues()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var dictionary = new Dictionary<string, bool>
            {
                { "isActive", true },
                { "isDeleted", false }
            };

            // Act
            var result = GenericsHelpers<string, bool>.GetValue(helper, dictionary, "isActive");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetValue_ReturnsFalseForBoolWhenKeyDoesNotExist()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var dictionary = new Dictionary<string, bool>
            {
                { "isActive", true }
            };

            // Act
            var result = GenericsHelpers<string, bool>.GetValue(helper, dictionary, "isDeleted");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetValue_WorksWithNullableValueTypes()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var dictionary = new Dictionary<string, int?>
            {
                { "one", 1 },
                { "two", 2 },
                { "three", null }
            };

            // Act
            var result = GenericsHelpers<string, int?>.GetValue(helper, dictionary, "three");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetValue_ReturnsNullForNullableWhenKeyDoesNotExist()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var dictionary = new Dictionary<string, int?>
            {
                { "one", 1 }
            };

            // Act
            var result = GenericsHelpers<string, int?>.GetValue(helper, dictionary, "two");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetValue_WorksWithDoubleValues()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var dictionary = new Dictionary<string, double>
            {
                { "pi", 3.14159 },
                { "e", 2.71828 }
            };

            // Act
            var result = GenericsHelpers<string, double>.GetValue(helper, dictionary, "pi");

            // Assert
            Assert.Equal(3.14159, result);
        }

        [Fact]
        public void GetValue_WorksWithEmptyDictionary()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var dictionary = new Dictionary<string, int>();

            // Act
            var result = GenericsHelpers<string, int>.GetValue(helper, dictionary, "any");

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void GetValue_CanReturnNullValueFromDictionary()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var dictionary = new Dictionary<string, string>
            {
                { "key1", "value1" },
                { "key2", null! }
            };

            // Act
            var result = GenericsHelpers<string, string>.GetValue(helper, dictionary, "key2");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetValue_WorksWithIntegerKeys()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var dictionary = new Dictionary<int, string>
            {
                { 100, "hundred" },
                { 200, "two hundred" }
            };

            // Act
            var result = GenericsHelpers<int, string>.GetValue(helper, dictionary, 100);

            // Assert
            Assert.Equal("hundred", result);
        }
        #endregion

        #region Helper Classes
        private class TestClass
        {
#pragma warning disable S1144//used for testing
            public int Value { get; set; }
#pragma warning restore S1144
        }

        private class PropertyTestClass
        {
            public string StringProperty { get; set; } = "";
            public int IntProperty { get; set; }
            public bool BoolProperty { get; set; }
            public string? NullableProperty { get; set; }
            public double DoubleProperty { get; set; }
            public DateTime DateTimeProperty { get; set; }
        }
        #endregion
    }
}
