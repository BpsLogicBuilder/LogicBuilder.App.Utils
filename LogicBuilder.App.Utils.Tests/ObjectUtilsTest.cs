using System.Collections.Generic;

namespace LogicBuilder.App.Utils.Tests
{
    public class ObjectUtilsTest
    {
        private readonly ObjectHelper _objectHelper;

        public ObjectUtilsTest()
        {
            _objectHelper = new ObjectHelper();
        }

        #region Null Property Tests
        [Fact]
        public void Null_ReturnsNull()
        {
            // Act
            var result = ObjectUtils.Null;

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
            var result = ObjectUtils.IsNull(_objectHelper, nullObject);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsNull_WithNonNullObject_ReturnsFalse()
        {
            // Arrange
            var nonNullObject = new object();

            // Act
            var result = ObjectUtils.IsNull(_objectHelper, nonNullObject);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsNull_WithString_ReturnsFalse()
        {
            // Arrange
            string nonNullString = "test";

            // Act
            var result = ObjectUtils.IsNull(_objectHelper, nonNullString);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsNull_WithEmptyString_ReturnsFalse()
        {
            // Arrange
            string emptyString = string.Empty;

            // Act
            var result = ObjectUtils.IsNull(_objectHelper, emptyString);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsNull_WithInteger_ReturnsFalse()
        {
            // Arrange
            int number = 42;

            // Act
            var result = ObjectUtils.IsNull(_objectHelper, number);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsNull_WithZero_ReturnsFalse()
        {
            // Arrange
            int zero = 0;

            // Act
            var result = ObjectUtils.IsNull(_objectHelper, zero);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsNull_WithList_ReturnsFalse()
        {
            // Arrange
            var list = new List<int>();

            // Act
            var result = ObjectUtils.IsNull(_objectHelper, list);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsNull_WithNullableIntWithValue_ReturnsFalse()
        {
            // Arrange
            int? nullableInt = 5;

            // Act
            var result = ObjectUtils.IsNull(_objectHelper, nullableInt);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsNull_WithNullableIntWithoutValue_ReturnsTrue()
        {
            // Arrange
            int? nullableInt = null;

            // Act
            var result = ObjectUtils.IsNull(_objectHelper, nullableInt);

            // Assert
            Assert.True(result);
        }
        #endregion
    }
}
