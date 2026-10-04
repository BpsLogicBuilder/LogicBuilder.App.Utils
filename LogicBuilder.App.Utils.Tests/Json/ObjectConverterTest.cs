using System;
using System.Text.Json;

namespace LogicBuilder.App.Utils.Tests.Json
{
    public class ObjectConverterTest
    {
        private readonly JsonSerializerOptions _options;

        public ObjectConverterTest()
        {
            _options = new JsonSerializerOptions
            {
                Converters = { new LogicBuilder.App.Utils.Json.ObjectConverter() }
            };
        }

        #region CanConvert Tests
        [Fact]
        public void CanConvert_ReturnsTrueForObjectType()
        {
            // Arrange
            var converter = new LogicBuilder.App.Utils.Json.ObjectConverter();

            // Act
            var result = converter.CanConvert(typeof(object));

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CanConvert_ReturnsFalseForNonObjectType()
        {
            // Arrange
            var converter = new LogicBuilder.App.Utils.Json.ObjectConverter();

            // Act
            var result = converter.CanConvert(typeof(string));

            // Assert
            Assert.False(result);
        }
        #endregion

        #region Read - Primitive Types Tests
        [Fact]
        public void Read_DeserializesStringValue()
        {
            // Arrange
            var json = "\"test string\"";

            // Act
            var result = JsonSerializer.Deserialize<object>(json, _options);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<string>(result);
            Assert.Equal("test string", result);
        }

        [Fact]
        public void Read_DeserializesIntValue()
        {
            // Arrange
            var json = "42";

            // Act
            var result = JsonSerializer.Deserialize<object>(json, _options);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<int>(result);
            Assert.Equal(42, result);
        }

        [Fact]
        public void Read_DeserializesLongValue()
        {
            // Arrange
            var json = "9223372036854775807";

            // Act
            var result = JsonSerializer.Deserialize<object>(json, _options);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<long>(result);
            Assert.Equal(9223372036854775807L, result);
        }

        [Fact]
        public void Read_DeserializesDoubleValue()
        {
            // Arrange
            var json = "123.456e10";

            // Act
            var result = JsonSerializer.Deserialize<object>(json, _options);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<double>(result);
        }

        [Fact]
        public void Read_DeserializesTrueValue()
        {
            // Arrange
            var json = "true";

            // Act
            var result = JsonSerializer.Deserialize<object>(json, _options);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<bool>(result);
            Assert.True((bool)result);
        }

        [Fact]
        public void Read_DeserializesFalseValue()
        {
            // Arrange
            var json = "false";

            // Act
            var result = JsonSerializer.Deserialize<object>(json, _options);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<bool>(result);
            Assert.False((bool)result);
        }

        [Fact]
        public void Read_DeserializesNullValue()
        {
            // Arrange
            var json = "null";

            // Act
            var result = JsonSerializer.Deserialize<object>(json, _options);

            // Assert
            Assert.Null(result);
        }
        #endregion

        #region Read - Object with TypeFullName Tests
        [Fact]
        public void Read_DeserializesObjectWithTypeFullName()
        {
            // Arrange
            var json = $"{{\"typefullname\":\"{typeof(TestClassWithObjectProperties).AssemblyQualifiedName}\",\"Id\":1,\"Name\":\"Test\",\"Data\":{{\"Value\":100}}}}";

            // Act
            var result = JsonSerializer.Deserialize<object>(json, _options);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<TestClassWithObjectProperties>(result);
            var typed = (TestClassWithObjectProperties)result;
            Assert.Equal(1, typed.Id);
            Assert.Equal("Test", typed.Name);
        }

        [Fact]
        public void Read_DeserializesObjectWithTypeString()
        {
            // Arrange
            var json = $"{{\"typestring\":\"{typeof(TestClassWithObjectProperties).AssemblyQualifiedName}\",\"Id\":2,\"Name\":\"Test2\",\"Data\":null}}";

            // Act
            var result = JsonSerializer.Deserialize<object>(json, _options);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<TestClassWithObjectProperties>(result);
            var typed = (TestClassWithObjectProperties)result;
            Assert.Equal(2, typed.Id);
            Assert.Equal("Test2", typed.Name);
        }

        [Fact]
        public void Read_ThrowsInvalidOperationException_WhenTypeNotFound()
        {
            // Arrange
            var json = "{\"typefullname\":\"NonExistent.Type, NonExistent.Assembly\",\"Id\":1}";

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                JsonSerializer.Deserialize<object>(json, _options));
        }
        #endregion

        #region Read - Anonymous Type Tests
        [Fact]
        public void Read_DeserializesAnonymousObjectWithMixedTypes()
        {
            // Arrange
            var json = "{\"StringProp\":\"value\",\"IntProp\":42,\"BoolProp\":true}";

            // Act
            var result = JsonSerializer.Deserialize<object>(json, _options);

            // Assert
            Assert.NotNull(result);
            var resultType = result.GetType();

            var stringProp = resultType.GetProperty("StringProp");
            Assert.NotNull(stringProp);
            Assert.Equal("value", stringProp.GetValue(result));

            var intProp = resultType.GetProperty("IntProp");
            Assert.NotNull(intProp);
            Assert.Equal(42, intProp.GetValue(result)!);

            var boolProp = resultType.GetProperty("BoolProp");
            Assert.NotNull(boolProp);
            Assert.True((bool)boolProp.GetValue(result)!);
        }

        [Fact]
        public void Read_DeserializesAnonymousObjectWithNumericTypes()
        {
            // Arrange
            var json = "{\"ByteVal\":255,\"ShortVal\":32767,\"IntVal\":2147483647,\"LongVal\":9223372036854775807}";

            // Act
            var result = JsonSerializer.Deserialize<object>(json, _options);

            // Assert
            Assert.NotNull(result);
            var resultType = result.GetType();

            var byteVal = resultType.GetProperty("ByteVal");
            Assert.NotNull(byteVal);

            var intVal = resultType.GetProperty("IntVal");
            Assert.NotNull(intVal);

            var longVal = resultType.GetProperty("LongVal");
            Assert.NotNull(longVal);
        }

        [Fact]
        public void Read_DeserializesAnonymousObjectWithNullProperty()
        {
            // Arrange
            var json = "{\"Name\":\"Test\",\"NullValue\":null}";

            // Act
            var result = JsonSerializer.Deserialize<object>(json, _options);

            // Assert
            Assert.NotNull(result);
            var resultType = result.GetType();

            var nameProp = resultType.GetProperty("Name");
            Assert.NotNull(nameProp);
            Assert.Equal("Test", nameProp.GetValue(result));

            var nullProp = resultType.GetProperty("NullValue");
            Assert.NotNull(nullProp);
            Assert.Null(nullProp.GetValue(result));
        }

        [Fact]
        public void Read_DeserializesEmptyObject()
        {
            // Arrange
            var json = "{}";

            // Act
            var result = JsonSerializer.Deserialize<object>(json, _options);

            // Assert
            Assert.NotNull(result);
        }
        #endregion

        #region Read - Complex Objects Tests
        [Fact]
        public void Read_DeserializesObjectWithObjectProperties()
        {
            // Arrange
            var json = "{\"Id\":1,\"Name\":\"Parent\",\"Child\":{\"Value\":\"ChildValue\"}}";

            // Act
            var result = JsonSerializer.Deserialize<object>(json, _options);

            // Assert
            Assert.NotNull(result);
            var resultType = result.GetType();

            var idProp = resultType.GetProperty("Id");
            Assert.NotNull(idProp);
            Assert.Equal(1, idProp.GetValue(result));

            var nameProp = resultType.GetProperty("Name");
            Assert.NotNull(nameProp);
            Assert.Equal("Parent", nameProp.GetValue(result));

            var childProp = resultType.GetProperty("Child");
            Assert.NotNull(childProp);
            Assert.NotNull(childProp.GetValue(result));
        }

        [Fact]
        public void Read_DeserializesNestedAnonymousObjects()
        {
            // Arrange
            var json = "{\"Level1\":{\"Level2\":{\"Value\":\"deep\"}}}";

            // Act
            var result = JsonSerializer.Deserialize<object>(json, _options);

            // Assert
            Assert.NotNull(result);
            var resultType = result.GetType();

            var level1Prop = resultType.GetProperty("Level1");
            Assert.NotNull(level1Prop);
            Assert.NotNull(level1Prop.GetValue(result));
        }
        #endregion

        #region Write Tests
        [Fact]
        public void Write_SerializesObjectCorrectly()
        {
            // Arrange
            var testObject = new TestClassWithObjectProperties
            {
                Id = 1,
                Name = "Test",
                Data = "test data"
            };

            // Act
            var json = JsonSerializer.Serialize<object>(testObject, _options);
            var result = JsonSerializer.Deserialize<TestClassWithObjectProperties>(json);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Test", result.Name);
        }

        [Fact]
        public void Write_SerializesPrimitiveTypes()
        {
            // Arrange
            object intValue = 42;
            object stringValue = "test";
            object boolValue = true;

            // Act
            var intJson = JsonSerializer.Serialize(intValue, _options);
            var stringJson = JsonSerializer.Serialize(stringValue, _options);
            var boolJson = JsonSerializer.Serialize(boolValue, _options);

            // Assert
            Assert.Equal("42", intJson);
            Assert.Equal("\"test\"", stringJson);
            Assert.Equal("true", boolJson);
        }

        [Fact]
        public void Write_SerializesObjectWithObjectProperties()
        {
            // Arrange
            var testObject = new TestClassWithObjectProperties
            {
                Id = 1,
                Name = "Test",
                Data = new { NestedValue = 100 }
            };

            // Act
            var json = JsonSerializer.Serialize<object>(testObject, _options);

            // Assert
            Assert.NotNull(json);
            Assert.Contains("\"Id\":1", json);
            Assert.Contains("\"Name\":\"Test\"", json);
        }

        [Fact]
        public void Write_SerializesComplexObjectWithMultipleObjectProperties()
        {
            // Arrange
            var testObject = new ComplexTestClass
            {
                StringProp = "test",
                IntProp = 42,
                ObjectProp1 = new { Value = "obj1" },
                ObjectProp2 = new { Value = 100 },
                NullObjectProp = null
            };

            // Act
            var json = JsonSerializer.Serialize<object>(testObject, _options);

            // Assert
            Assert.NotNull(json);
            Assert.Contains("\"StringProp\":\"test\"", json);
            Assert.Contains("\"IntProp\":42", json);
        }
        #endregion

        #region Round-trip Tests
        [Fact]
        public void RoundTrip_PreservesDataForSimpleObject()
        {
            // Arrange
            var original = new TestClassWithObjectProperties
            {
                Id = 1,
                Name = "Test",
                Data = "test data"
            };

            // Act
            var json = JsonSerializer.Serialize<object>(original, _options);
            var result = JsonSerializer.Deserialize<TestClassWithObjectProperties>(json, _options);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(original.Id, result.Id);
            Assert.Equal(original.Name, result.Name);
        }

        [Fact]
        public void RoundTrip_PreservesDataForComplexObject()
        {
            // Arrange
            var original = new ComplexTestClass
            {
                StringProp = "test",
                IntProp = 42,
                ObjectProp1 = new { Value = "obj1" },
                ObjectProp2 = 100
            };

            // Act
            var json = JsonSerializer.Serialize<object>(original, _options);
            var result = JsonSerializer.Deserialize<ComplexTestClass>(json, _options);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(original.StringProp, result.StringProp);
            Assert.Equal(original.IntProp, result.IntProp);
        }
        #endregion

        #region Test Helper Classes
        public class TestClassWithObjectProperties
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public object? Data { get; set; }
        }

        public class ComplexTestClass
        {
            public string? StringProp { get; set; }
            public int IntProp { get; set; }
            public object? ObjectProp1 { get; set; }
            public object? ObjectProp2 { get; set; }
            public object? NullObjectProp { get; set; }
        }
        #endregion
    }
}
