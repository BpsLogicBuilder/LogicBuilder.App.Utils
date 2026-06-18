using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Globalization;
using Xunit;

namespace LogicBuilder.App.Utils.Tests
{
    public class TypeHelperTest
    {
        private readonly Mock<ILogger<TypeHelper>> _mockLogger;
        private readonly TypeHelper _typeHelper;

        public TypeHelperTest()
        {
            _mockLogger = new Mock<ILogger<TypeHelper>>();
            _typeHelper = new TypeHelper(_mockLogger.Object);
        }

        #region GetPropertyValue Tests

        [Fact]
        public void GetPropertyValue_ReturnsCorrectValue_ForValidProperty()
        {
            // Arrange
            var testObject = new TestClass { Name = "Test", Age = 25 };

            // Act
            var result = _typeHelper.GetPropertyValue(testObject, "Name");

            // Assert
            Assert.Equal("Test", result);
        }

        [Fact]
        public void GetPropertyValue_IsCaseInsensitive()
        {
            // Arrange
            var testObject = new TestClass { Name = "Test", Age = 25 };

            // Act
            var result = _typeHelper.GetPropertyValue(testObject, "name");

            // Assert
            Assert.Equal("Test", result);
        }

        [Fact]
        public void GetPropertyValue_ThrowsInvalidOperationException_ForInvalidProperty()
        {
            // Arrange
            var testObject = new TestClass { Name = "Test", Age = 25 };

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() =>
                _typeHelper.GetPropertyValue(testObject, "NonExistentProperty"));

            Assert.Contains("Failed to get property 'NonExistentProperty'", exception.Message);
        }

        [Fact]
        public void GetPropertyValue_ThrowsInvalidOperationException_ForNullObject()
        {
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                _typeHelper.GetPropertyValue(null!, "Name"));
        }

        #endregion

        #region GetType Tests

        [Fact]
        public void GetType_ReturnsCorrectType_ForValidAssemblyQualifiedName()
        {
            // Arrange
            var typeName = typeof(string).AssemblyQualifiedName;

            // Act
            var result = _typeHelper.GetType(typeName!);

            // Assert
            Assert.Equal(typeof(string), result);
        }

        [Fact]
        public void GetType_ReturnsNull_ForInvalidTypeName()
        {
            // Arrange
            var invalidTypeName = "Invalid.Type.Name, Invalid.Assembly";

            // Act
            var result = _typeHelper.GetType(invalidTypeName);

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region ToTypeString Tests

        [Fact]
        public void ToTypeString_ReturnsAssemblyQualifiedName()
        {
            // Arrange
            var type = typeof(int);

            // Act
            var result = _typeHelper.ToTypeString(type);

            // Assert
            Assert.Equal(type.AssemblyQualifiedName, result);
        }

        #endregion

        #region TryParse Tests

        [Fact]
        public void TryParse_ParsesStringSuccessfully()
        {
            // Act
            var success = _typeHelper.TryParse("Hello", typeof(string), out var result);

            // Assert
            Assert.True(success);
            Assert.Equal("Hello", result);
        }

        [Theory]
        [InlineData("42", typeof(int), 42)]
        [InlineData("3.14", typeof(double), 3.14)]
        [InlineData("true", typeof(bool), true)]
        [InlineData("100", typeof(byte), (byte)100)]
        [InlineData("1000", typeof(short), (short)1000)]
        [InlineData("999999999", typeof(long), 999999999L)]
        [InlineData("2.5", typeof(float), 2.5f)]
        [InlineData("127", typeof(sbyte), (sbyte)127)]
        [InlineData("65535", typeof(ushort), (ushort)65535)]
        [InlineData("4294967295", typeof(uint), 4294967295u)]
        public void TryParse_ParsesNumericTypesSuccessfully(string input, Type type, object expected)
        {
            // Act
            var success = _typeHelper.TryParse(input, type, out var result);

            // Assert
            Assert.True(success);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TryParse_ParsesDateTimeSuccessfully()
        {
            // Arrange
            var dateString = "2023-01-15";
            var expectedDate = DateTime.Parse(dateString, CultureInfo.CurrentCulture);

            // Act
            var success = _typeHelper.TryParse(dateString, typeof(DateTime), out var result);

            // Assert
            Assert.True(success);
            Assert.Equal(expectedDate, result);
        }

        [Fact]
        public void TryParse_ParsesGuidSuccessfully()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var guidString = guid.ToString();

            // Act
            var success = _typeHelper.TryParse(guidString, typeof(Guid), out var result);

            // Assert
            Assert.True(success);
            Assert.Equal(guid, result);
        }

        [Fact]
        public void TryParse_ParsesTimeSpanSuccessfully()
        {
            // Arrange
            var timeSpan = "01:30:00";

            // Act
            var success = _typeHelper.TryParse(timeSpan, typeof(TimeSpan), out var result);

            // Assert
            Assert.True(success);
            Assert.Equal(TimeSpan.Parse(timeSpan, CultureInfo.CurrentCulture), result);
        }

        [Fact]
        public void TryParse_ParsesDateTimeOffsetSuccessfully()
        {
            // Arrange
            var dateOffsetString = "2023-01-15T10:30:00+00:00";
            var expectedDateOffset = DateTimeOffset.Parse(dateOffsetString, CultureInfo.CurrentCulture);

            // Act
            var success = _typeHelper.TryParse(dateOffsetString, typeof(DateTimeOffset), out var result);

            // Assert
            Assert.True(success);
            Assert.Equal(expectedDateOffset, result);
        }

        [Fact]
        public void TryParse_ParsesEnumByNameSuccessfully()
        {
            // Act
            var success = _typeHelper.TryParse("Value2", typeof(TestType), out var result);

            // Assert
            Assert.True(success);
            Assert.Equal(TestType.Value2, result);
        }

        [Fact]
        public void TryParse_ParsesEnumByValueSuccessfully()
        {
            // Act
            var success = _typeHelper.TryParse("1", typeof(TestType), out var result);

            // Assert
            Assert.True(success);
            Assert.Equal(TestType.Value2, result);
        }

        [Fact]
        public void TryParse_ReturnsFalse_ForInvalidEnumValue()
        {
            // Act
            var success = _typeHelper.TryParse("InvalidValue", typeof(TestType), out var result);

            // Assert
            Assert.False(success);
            Assert.Null(result);
        }

        [Fact]
        public void TryParse_ParsesNullableIntSuccessfully()
        {
            // Act
            var success = _typeHelper.TryParse("42", typeof(int?), out var result);

            // Assert
            Assert.True(success);
            Assert.Equal(42, result);
        }

        [Fact]
        public void TryParse_ReturnsFalse_ForInvalidIntValue()
        {
            // Act
            var success = _typeHelper.TryParse("not a number", typeof(int), out var result);

            // Assert
            Assert.False(success);
            Assert.Null(result);
        }

        [Fact]
        public void TryParse_ReturnsFalse_ForInvalidDateTimeValue()
        {
            // Act
            var success = _typeHelper.TryParse("not a date", typeof(DateTime), out var result);

            // Assert
            Assert.False(success);
            Assert.Null(result);
        }

        [Fact]
        public void TryParse_ThrowsArgumentException_ForNullType()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                _typeHelper.TryParse("test", null!, out var _));

            Assert.Contains("Argument cannot be null", exception.Message);
            Assert.Equal("type", exception.ParamName);
        }

        [Fact]
        public void TryParse_ThrowsArgumentException_ForNonLiteralType()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                _typeHelper.TryParse("test", typeof(TestClass), out var _));

            Assert.Contains("Not a valid literal type", exception.Message);
            Assert.Equal("type", exception.ParamName);
        }

        [Fact]
        public void TryParse_ParsesCharSuccessfully()
        {
            // Act
            var success = _typeHelper.TryParse("A", typeof(char), out var result);

            // Assert
            Assert.True(success);
            Assert.Equal('A', result);
        }

        #endregion

        #region Test Helper Classes

        private class TestClass
        {
            public string Name { get; set; } = "";
            public int Age { get; set; }
        }

        private enum TestType
        {
            Value1 = 0,
            Value2 = 1,
            Value3 = 2
        }

        #endregion
    }
}
