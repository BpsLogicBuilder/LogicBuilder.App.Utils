using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogicBuilder.App.Utils.Tests
{
    public class GenericsHelpersTest
    {
        #region AddItem Tests
        [Fact]
        public void AddItem_AddsItemToCollection()
        {
            // Arrange
            var helper = new GenericsHelpers<int>();
            var collection = new List<int> { 1, 2, 3 };

            // Act
            helper.AddItem(collection, 4);

            // Assert
            Assert.Equal(4, collection.Count);
            Assert.Contains(4, collection);
        }

        [Fact]
        public void AddItem_AddsStringItemToCollection()
        {
            // Arrange
            var helper = new GenericsHelpers<string>();
            var collection = new List<string> { "apple", "banana" };

            // Act
            helper.AddItem(collection, "cherry");

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
            var helper = new GenericsHelpers<int>();
            var enumerable = new List<int> { 1, 2, 3 };

            // Act
            var result = helper.Any(enumerable);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Any_ReturnsFalseForEmptyEnumerable()
        {
            // Arrange
            var helper = new GenericsHelpers<int>();
            var enumerable = new List<int>();

            // Act
            var result = helper.Any(enumerable);

            // Assert
            Assert.False(result);
        }
        #endregion

        #region CreateInstance Tests
        [Fact]
        public void CreateInstance_CreatesNewInstance()
        {
            // Arrange
            var helper = new GenericsHelpers<TestClass>();

            // Act
            var result = helper.CreateInstance();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<TestClass>(result);
        }

        [Fact]
        public void CreateInstance_CreatesValueTypeInstance()
        {
            // Arrange
            var helper = new GenericsHelpers<int>();

            // Act
            var result = helper.CreateInstance();

            // Assert
            Assert.Equal(0, result);
        }
        #endregion

        #region GetItemAtIndex Tests
        [Fact]
        public void GetItemAtIndex_ReturnsCorrectItem()
        {
            // Arrange
            var helper = new GenericsHelpers<string>();
            var enumerable = new List<string> { "first", "second", "third" };

            // Act
            var result = helper.GetItemAtIndex(enumerable, 1);

            // Assert
            Assert.Equal("second", result);
        }

        [Fact]
        public void GetItemAtIndex_ThrowsForInvalidIndex()
        {
            // Arrange
            var helper = new GenericsHelpers<int>();
            var enumerable = new List<int> { 1, 2, 3 };

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => helper.GetItemAtIndex(enumerable, 5));
        }

        [Fact]
        public void GetItemAtIndex_ReturnsFirstItemAtIndexZero()
        {
            // Arrange
            var helper = new GenericsHelpers<int>();
            var enumerable = new List<int> { 10, 20, 30 };

            // Act
            var result = helper.GetItemAtIndex(enumerable, 0);

            // Assert
            Assert.Equal(10, result);
        }
        #endregion

        #region IsDefault Tests
        [Fact]
        public void IsDefault_ReturnsTrueForDefaultValueType()
        {
            // Arrange
            var helper = new GenericsHelpers<int>();

            // Act
            var result = helper.IsDefault(0);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsDefault_ReturnsFalseForNonDefaultValueType()
        {
            // Arrange
            var helper = new GenericsHelpers<int>();

            // Act
            var result = helper.IsDefault(5);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsDefault_ReturnsFalseForNullReferenceType()
        {
            // Arrange
            var helper = new GenericsHelpers<string>();

            // Act
            var result = helper.IsDefault(null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsDefault_ReturnsFalseForNonDefaultReferenceType()
        {
            // Arrange
            var helper = new GenericsHelpers<string>();

            // Act
            var result = helper.IsDefault("test");

            // Assert
            Assert.False(result);
        }
        #endregion

        #region Single Tests
        [Fact]
        public void Single_ReturnsOnlyItem()
        {
            // Arrange
            var helper = new GenericsHelpers<int>();
            var enumerable = new List<int> { 42 };

            // Act
            var result = helper.Single(enumerable);

            // Assert
            Assert.Equal(42, result);
        }

        [Fact]
        public void Single_ThrowsForEmptyEnumerable()
        {
            // Arrange
            var helper = new GenericsHelpers<int>();
            var enumerable = new List<int>();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => helper.Single(enumerable));
        }

        [Fact]
        public void Single_ThrowsForMultipleItems()
        {
            // Arrange
            var helper = new GenericsHelpers<int>();
            var enumerable = new List<int> { 1, 2, 3 };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => helper.Single(enumerable));
        }
        #endregion

        #region SingleOrDefault Tests
        [Fact]
        public void SingleOrDefault_ReturnsOnlyItem()
        {
            // Arrange
            var helper = new GenericsHelpers<string>();
            var enumerable = new List<string> { "only" };

            // Act
            var result = helper.SingleOrDefault(enumerable);

            // Assert
            Assert.Equal("only", result);
        }

        [Fact]
        public void SingleOrDefault_ReturnsDefaultForEmptyEnumerable()
        {
            // Arrange
            var helper = new GenericsHelpers<int>();
            var enumerable = new List<int>();

            // Act
            var result = helper.SingleOrDefault(enumerable);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void SingleOrDefault_ReturnsNullForEmptyReferenceTypeEnumerable()
        {
            // Arrange
            var helper = new GenericsHelpers<string>();
            var enumerable = new List<string>();

            // Act
            var result = helper.SingleOrDefault(enumerable);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void SingleOrDefault_ThrowsForMultipleItems()
        {
            // Arrange
            var helper = new GenericsHelpers<int>();
            var enumerable = new List<int> { 1, 2 };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => helper.SingleOrDefault(enumerable));
        }
        #endregion

        #region ToList Tests
        [Fact]
        public void ToList_ConvertsEnumerableToList()
        {
            // Arrange
            var helper = new GenericsHelpers<int>();
            var enumerable = new[] { 1, 2, 3, 4, 5 };

            // Act
            var result = helper.ToList(enumerable);

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
            var helper = new GenericsHelpers<string>();
            var enumerable = Enumerable.Empty<string>();

            // Act
            var result = helper.ToList(enumerable);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void ToList_PreservesOrder()
        {
            // Arrange
            var helper = new GenericsHelpers<string>();
            var enumerable = new[] { "z", "a", "m", "b" };

            // Act
            var result = helper.ToList(enumerable);

            // Assert
#pragma warning disable CA1861//used for testing
            Assert.Equal(new[] { "z", "a", "m", "b" }, result);
#pragma warning disable CA1861
        }
        #endregion

        #region Helper Classes
        private class TestClass
        {
#pragma warning disable S1144//used for testing
            public int Value { get; set; }
#pragma warning restore S1144
        }
        #endregion
    }
}
