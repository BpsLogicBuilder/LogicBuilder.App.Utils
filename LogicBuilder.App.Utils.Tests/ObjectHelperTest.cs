using System.Collections.Generic;

namespace LogicBuilder.App.Utils.Tests
{
    public class ObjectHelperTest
    {
        private readonly ObjectHelper _objectHelper;

        public ObjectHelperTest()
        {
            _objectHelper = new ObjectHelper();
        }

        #region Null Property Tests
        [Fact]
        public void Null_ReturnsNull()
        {
            // Act
            var result = _objectHelper.Null;

            // Assert
            Assert.Null(result);
        }
        #endregion

        #region IsNull Tests
        [Fact]
        public void IsNull_WithNullObject_ReturnsTrue()
        {
            // Arrange
            object? nullObject = null;

            // Act
            var result = _objectHelper.IsNull(nullObject);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsNull_WithNonNullObject_ReturnsFalse()
        {
            // Arrange
            var nonNullObject = new object();

            // Act
            var result = _objectHelper.IsNull(nonNullObject);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsNull_WithString_ReturnsFalse()
        {
            // Arrange
            string nonNullString = "test";

            // Act
            var result = _objectHelper.IsNull(nonNullString);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsNull_WithEmptyString_ReturnsFalse()
        {
            // Arrange
            string emptyString = string.Empty;

            // Act
            var result = _objectHelper.IsNull(emptyString);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsNull_WithInteger_ReturnsFalse()
        {
            // Arrange
            int number = 42;

            // Act
            var result = _objectHelper.IsNull(number);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsNull_WithZero_ReturnsFalse()
        {
            // Arrange
            int zero = 0;

            // Act
            var result = _objectHelper.IsNull(zero);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsNull_WithList_ReturnsFalse()
        {
            // Arrange
            var list = new List<int>();

            // Act
            var result = _objectHelper.IsNull(list);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsNull_WithNullableIntWithValue_ReturnsFalse()
        {
            // Arrange
            int? nullableInt = 5;

            // Act
            var result = _objectHelper.IsNull(nullableInt);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsNull_WithNullableIntWithoutValue_ReturnsTrue()
        {
            // Arrange
            int? nullableInt = null;

            // Act
            var result = _objectHelper.IsNull(nullableInt);

            // Assert
            Assert.True(result);
        }
        #endregion
    }
}
