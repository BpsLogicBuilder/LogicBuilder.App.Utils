using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LogicBuilder.App.Utils.Tests
{
    public class GenericsHelpersWrapperTest
    {
        private readonly Mock<ILogger<GenericsHelpers>> _mockLogger;

        public GenericsHelpersWrapperTest()
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
            GenericsHelpersWrapper<int>.AddItem(helper, collection, 4);

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
            GenericsHelpersWrapper<string>.AddItem(helper, collection, "cherry");

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
            var result = GenericsHelpersWrapper<int>.Any(helper, enumerable);

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
            var result = GenericsHelpersWrapper<int>.Any(helper, enumerable);

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
            var result = GenericsHelpersWrapper<TestClass>.CreateInstance(helper);

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
            var result = GenericsHelpersWrapper<int>.CreateInstance(helper);

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
            var result = GenericsHelpersWrapper<string>.GetItemAtIndex(helper, enumerable, 1);

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
            Assert.Throws<ArgumentOutOfRangeException>(() => GenericsHelpersWrapper<int>.GetItemAtIndex(helper, enumerable, 5));
        }

        [Fact]
        public void GetItemAtIndex_ReturnsFirstItemAtIndexZero()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var enumerable = new List<int> { 10, 20, 30 };

            // Act
            var result = GenericsHelpersWrapper<int>.GetItemAtIndex(helper, enumerable, 0);

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
            var result = GenericsHelpersWrapper<int>.IsDefault(helper, 0);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsDefault_ReturnsFalseForNonDefaultValueType()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);

            // Act
            var result = GenericsHelpersWrapper<int>.IsDefault(helper, 5);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsDefault_ReturnsFalseForNullReferenceType()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);

            // Act
            var result = GenericsHelpersWrapper<string>.IsDefault(helper, null);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsDefault_ReturnsFalseForNonDefaultReferenceType()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);

            // Act
            var result = GenericsHelpersWrapper<string>.IsDefault(helper, "test");

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
            var result = GenericsHelpersWrapper<int>.Single(helper, enumerable);

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
            Assert.Throws<InvalidOperationException>(() => GenericsHelpersWrapper<int>.Single(helper, enumerable));
        }

        [Fact]
        public void Single_ThrowsForMultipleItems()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var enumerable = new List<int> { 1, 2, 3 };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => GenericsHelpersWrapper<int>.Single(helper, enumerable));
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
            var result = GenericsHelpersWrapper<string>.SingleOrDefault(helper, enumerable);

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
            var result = GenericsHelpersWrapper<int>.SingleOrDefault(helper, enumerable);

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
            var result = GenericsHelpersWrapper<string>.SingleOrDefault(helper, enumerable);

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
            Assert.Throws<InvalidOperationException>(() => GenericsHelpersWrapper<int>.SingleOrDefault(helper, enumerable));
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
            var result = GenericsHelpersWrapper<int>.ToList(helper, enumerable);

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
            var result = GenericsHelpersWrapper<string>.ToList(helper, enumerable);

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
            var result = GenericsHelpersWrapper<string>.ToList(helper, enumerable);

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
            var result = GenericsHelpersWrapper<string>.GetPropertyValue(helper, testObj, "StringProperty");

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
            var result = GenericsHelpersWrapper<string>.GetPropertyValue(helper, testObj, "stringproperty");

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
            var result = GenericsHelpersWrapper<int>.GetPropertyValue(helper, testObj, "IntProperty");

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
            var result = GenericsHelpersWrapper<bool>.GetPropertyValue(helper, testObj, "BoolProperty");

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
            var result = GenericsHelpersWrapper<string>.GetPropertyValue(helper, testObj, "NullableProperty");

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
            var result = GenericsHelpersWrapper<string>.GetPropertyValue(helper, testObj, "IntProperty");

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
            var result = GenericsHelpersWrapper<int>.GetPropertyValue(helper, testObj, "StringProperty");

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
                GenericsHelpersWrapper<string>.GetPropertyValue(helper, testObj, "NonExistentProperty"));
        }

        [Fact]
        public void GetPropertyValue_ThrowsForNullObject()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                GenericsHelpersWrapper<string>.GetPropertyValue(helper, null!, "AnyProperty"));
        }

        [Fact]
        public void GetPropertyValue_ThrowsForInvalidTypeConversion()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var testObj = new PropertyTestClass { StringProperty = "NotANumber" };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                GenericsHelpersWrapper<int>.GetPropertyValue(helper, testObj, "StringProperty"));
        }

        [Fact]
        public void GetPropertyValue_ReturnsDoubleProperty()
        {
            // Arrange
            var helper = new GenericsHelpers(_mockLogger.Object);
            var testObj = new PropertyTestClass { DoubleProperty = 3.14 };

            // Act
            var result = GenericsHelpersWrapper<double>.GetPropertyValue(helper, testObj, "DoubleProperty");

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
            var result = GenericsHelpersWrapper<DateTime>.GetPropertyValue(helper, testObj, "DateTimeProperty");

            // Assert
            Assert.Equal(expectedDate, result);
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
