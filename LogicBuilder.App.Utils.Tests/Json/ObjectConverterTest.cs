using LogicBuilder.App.Utils.Json;
using LogicBuilder.Domain;
using LogicBuilder.Forms.Parameters.Expressions;
using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace LogicBuilder.App.Utils.Tests.Json
{
    public class ObjectConverterTest
    {
        private readonly JsonSerializerOptions _options;

        public ObjectConverterTest()
        {
            _options = new JsonSerializerOptions();
            _options.AddAssemblyFilteredConverter
             (
                new Utils.Json.ObjectConverter(typeof(TestClassWithObjectProperties).Assembly),
                typeof(TestClassWithObjectProperties).Assembly
             );
        }

        #region CanConvert Tests
        [Fact]
        public void CanConvert_ReturnsTrueForObjectType()
        {
            // Arrange
            var converter = new Utils.Json.ObjectConverter();

            // Act
            var result = converter.CanConvert(typeof(object));

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CanConvert_ReturnsFalseForNonObjectType()
        {
            // Arrange
            var converter = new Utils.Json.ObjectConverter();

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
            var initial = new ClassWithObjectMember { ObjectProp = "test string" };
            var json = JsonSerializer.Serialize(initial);

            // Act
            var result = JsonSerializer.Deserialize<ClassWithObjectMember>(json, _options);

            // Assert
            Assert.NotNull(result);
            var typedObjectProp = Assert.IsType<string>(result.ObjectProp);
            Assert.Equal("test string", typedObjectProp);
        }

        [Fact]
        public void Read_DeserializesIntValue()
        {
            // Arrange
            var initial = new ClassWithObjectMember { ObjectProp = 42 };
            var json = JsonSerializer.Serialize(initial);

            // Act
            var result = JsonSerializer.Deserialize<ClassWithObjectMember>(json, _options);

            // Assert
            Assert.NotNull(result);
            var typedObjectProp = Assert.IsType<int>(result.ObjectProp);
            Assert.Equal(42, typedObjectProp);
        }

        [Fact]
        public void Read_DeserializesLongValue()
        {
            // Arrange
            var initial = new ClassWithObjectMember { ObjectProp = 9223372036854775807 };
            var json = JsonSerializer.Serialize(initial);

            // Act
            var result = JsonSerializer.Deserialize<ClassWithObjectMember>(json, _options);

            // Assert
            Assert.NotNull(result);
            var typedObjectProp = Assert.IsType<long>(result.ObjectProp);
            Assert.Equal(9223372036854775807L, typedObjectProp);
        }

        [Fact]
        public void Read_DeserializesDoubleValue()
        {
            // Arrange
            var initial = new ClassWithObjectMember { ObjectProp = 123.456 };
            var json = JsonSerializer.Serialize(initial);

            // Act
            var result = JsonSerializer.Deserialize<ClassWithObjectMember>(json, _options);

            // Assert
            Assert.NotNull(result);
            var typedObjectProp = Assert.IsType<double>(result.ObjectProp);
            Assert.Equal(123.456, typedObjectProp);
        }

        [Fact]
        public void Read_DeserializesTrueValue()
        {
            // Arrange
            var initial = new ClassWithObjectMember { ObjectProp = true };
            var json = JsonSerializer.Serialize(initial);

            // Act
            var result = JsonSerializer.Deserialize<ClassWithObjectMember>(json, _options);

            // Assert
            Assert.NotNull(result);
            var typedObjectProp = Assert.IsType<bool>(result.ObjectProp);
            Assert.True(typedObjectProp);
        }

        [Fact]
        public void Read_DeserializesFalseValue()
        {
            // Arrange
            var initial = new ClassWithObjectMember { ObjectProp = false };
            var json = JsonSerializer.Serialize(initial);

            // Act
            var result = JsonSerializer.Deserialize<ClassWithObjectMember>(json, _options);

            // Assert
            Assert.NotNull(result);
            var typedObjectProp = Assert.IsType<bool>(result.ObjectProp);
            Assert.False(typedObjectProp);
        }

        [Fact]
        public void Read_DeserializesNullValue()
        {
            // Arrange
            var initial = new ClassWithObjectMember { ObjectProp = null };
            var json = JsonSerializer.Serialize(initial);

            // Act
            var result = JsonSerializer.Deserialize<ClassWithObjectMember>(json, _options);

            // Assert
            Assert.NotNull(result);
            var typed = Assert.IsType<ClassWithObjectMember>(result);
            Assert.Null(typed.ObjectProp);
        }
        #endregion

        #region Read - Object with TypeFullName Tests
        [Fact]
        public void Read_DeserializesObjectWithTypeFullName()
        {
            // Arrange
            var initial = new ClassWithObjectMember
            {
                ObjectProp = new
                {
                    TypeFullName = typeof(TestClassWithObjectProperties).AssemblyQualifiedName,
                    Id = 1,
                    Name = "Test",
                    Data = new { Value = 100 }
                }
            };
            var json = JsonSerializer.Serialize(initial);

            // Act
            var result = JsonSerializer.Deserialize<ClassWithObjectMember>(json, _options);

            // Assert
            Assert.NotNull(result);
            var typed = Assert.IsType<TestClassWithObjectProperties>(result.ObjectProp);
            Assert.Equal(1, typed.Id);
            Assert.Equal("Test", typed.Name);
        }

        [Fact]
        public void Read_DeserializesObjectWithTypeString()
        {
            // Arrange
            var initial = new ClassWithObjectMember 
            { 
                ObjectProp = new 
                { 
                    TypeString = typeof(TestClassWithObjectProperties).AssemblyQualifiedName, 
                    Id = 2,
                    Name = "Test2"
                } 
            };
            var json = JsonSerializer.Serialize(initial);

            // Act
            var result = JsonSerializer.Deserialize<ClassWithObjectMember>(json, _options);

            // Assert
            Assert.NotNull(result);
            var typed = Assert.IsType<TestClassWithObjectProperties>(result.ObjectProp);
            Assert.Equal(2, typed.Id);
            Assert.Equal("Test2", typed.Name);
        }

        [Fact]
        public void Read_ThrowsJsonException_WhenTypeNotFound()
        {
            // Arrange
            var initial = new ClassWithObjectMember { ObjectProp = new { TypeFullName = "NonExistent.Type, NonExistent.Assembly", Id = 1 } };
            var json = JsonSerializer.Serialize(initial);

            // Act & Assert
            var exception = Assert.Throws<JsonException>(() =>
                JsonSerializer.Deserialize<ClassWithObjectMember>(json, _options));
            Assert.Equal($"Type \"NonExistent.Type, NonExistent.Assembly\" is not an allowed type for {typeof(object).FullName}.", exception.Message);
        }
        #endregion

        #region Read - Anonymous Type Tests
        [Fact]
        public void Read_DeserializesAnonymousObjectWithMixedTypes()
        {
            // Arrange
            var initial = new ClassWithObjectMember
            {
                ObjectProp = new
                {
                    StringProp = "value",
                    IntProp = 42,
                    BoolProp = true
                }
            };
            var json = JsonSerializer.Serialize(initial);

            // Act
            var result = JsonSerializer.Deserialize<ClassWithObjectMember>(json, _options);

            // Assert
            Assert.NotNull(result);
            var resultType = result.ObjectProp!.GetType();

            var stringProp = resultType.GetProperty("StringProp");
            Assert.NotNull(stringProp);
            Assert.Equal("value", stringProp.GetValue(result.ObjectProp));

            var intProp = resultType.GetProperty("IntProp");
            Assert.NotNull(intProp);
            Assert.Equal(42, intProp.GetValue(result.ObjectProp)!);

            var boolProp = resultType.GetProperty("BoolProp");
            Assert.NotNull(boolProp);
            Assert.True((bool)boolProp.GetValue(result.ObjectProp)!);
        }

        [Fact]
        public void Read_DeserializesAnonymousObjectWithNumericTypes()
        {
            // Arrange
            var initial = new ClassWithObjectMember
            {
                ObjectProp = new
                {
                    ByteVal = 255,
                    ShortVal = 32767,
                    IntVal = 2147483647,
                    LongVal = 9223372036854775807
                }
            };
            var json = JsonSerializer.Serialize(initial);

            // Act
            var result = JsonSerializer.Deserialize<ClassWithObjectMember>(json, _options);

            // Assert
            Assert.NotNull(result);
            var resultType = result.ObjectProp!.GetType();

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
            var initial = new ClassWithObjectMember
            {
                ObjectProp = new
                {
                    Name = "Test",
                    NullValue = (object?)null
                }
            };
            var json = JsonSerializer.Serialize(initial);

            // Act
            var result = JsonSerializer.Deserialize<ClassWithObjectMember>(json, _options);

            // Assert
            Assert.NotNull(result);
            var resultType = result.ObjectProp!.GetType();

            var nameProp = resultType.GetProperty("Name");
            Assert.NotNull(nameProp);
            Assert.Equal("Test", nameProp.GetValue(result.ObjectProp));

            var nullProp = resultType.GetProperty("NullValue");
            Assert.NotNull(nullProp);
            Assert.Null(nullProp.GetValue(result.ObjectProp));
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
            var initial = new ClassWithObjectMember
            {
                ObjectProp = new
                {
                    Id = 1,
                    Name = "Parent",
                    Child = new { Value = "ChildValue" }
                }
            };
            var json = JsonSerializer.Serialize(initial);

            // Act
            var result = JsonSerializer.Deserialize<ClassWithObjectMember>(json, _options);

            // Assert
            Assert.NotNull(result);
            var resultType = result.ObjectProp!.GetType();

            var idProp = resultType.GetProperty("Id");
            Assert.NotNull(idProp);
            Assert.Equal(1, idProp.GetValue(result.ObjectProp));

            var nameProp = resultType.GetProperty("Name");
            Assert.NotNull(nameProp);
            Assert.Equal("Parent", nameProp.GetValue(result.ObjectProp));

            var childProp = resultType.GetProperty("Child");
            Assert.NotNull(childProp);
            Assert.NotNull(childProp.GetValue(result.ObjectProp));
        }

        [Fact]
        public void Read_DeserializesNestedAnonymousObjects()
        {
            // Arrange
            var initial = new ClassWithObjectMember { ObjectProp = new { Level1 = new { Level2 = new { Value = "deep" } } } };
            var json = JsonSerializer.Serialize(initial);

            // Act
            var result = JsonSerializer.Deserialize<ClassWithObjectMember>(json, _options);

            // Assert
            Assert.NotNull(result);
            var resultType = result.ObjectProp!.GetType();
            Assert.Contains("AnonymousType", resultType.Name);
            var level1Prop = resultType.GetProperty("Level1");
            Assert.NotNull(level1Prop);
            var level1Value = level1Prop.GetValue(result.ObjectProp);
            Assert.NotNull(level1Value);
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

        #region KnowTypeTests
        [Fact]
        public void ObjectConverterDoesNotInstantiate_TypeOutsideAllowlist()
        {
            // Arrange
            var initial = new ClassWithObjectMember
            {
                ObjectProp = new { TypeString = typeof(ParameterOperatorParameters).AssemblyQualifiedName }
            };
            var json = JsonSerializer.Serialize(initial);

            // Act & Assert
            var exception = Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<ClassWithObjectMember>(json, _options));
            Assert.Contains($"is not an allowed type for {typeof(object).FullName}.", exception.Message);
        }

        [Fact]
        public void ObjectConverterRejects_TypeFromUnregisteredAssembly()
        {
            // Arrange
            var initial = new ClassWithObjectMember
            {
                ObjectProp = new Expressions.Utils.ExpressionDescriptors.ParameterDescriptor("p")
            };
            var json = JsonSerializer.Serialize(initial);

            // Act & Assert
            Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<ClassWithObjectMember>(json, _options));
            Assert.Throws<JsonException>(() => JsonSerializer.Serialize<ClassWithObjectMember>(initial, _options));
        }

        [Fact]
        public void ObjectConverterAccepts_DescriptorSubtypeFromRegisteredAssembly()
        {
            // Arrange
            var initial = new ClassWithObjectMember
            {
                ObjectProp = new ExternalModel { Name = "A" }
            };
            var json = JsonSerializer.Serialize(initial);

            // Act
            var result = JsonSerializer.Deserialize<ClassWithObjectMember>(json, _options)!;

            // Assert
            Assert.Equal("A", Assert.IsType<ExternalModel>(result.ObjectProp).Name);
        }

        [Fact]
        public void ObjectConverterAccepts_TypesFromRegisteredAssembly_UsingTypesListConstructor()
        {
            // Arrange
            JsonSerializerOptions options = new();
            options.Converters.Add(new TestObjectconverter(typeof(ExternalModel).Assembly.GetTypes().Where(t => typeof(BaseModel).IsAssignableFrom(t)).ToArray()));
            string json = JsonSerializer.Serialize<BaseModel>(new ExternalModel { Name = "A" }, options);

            // Act
            BaseModel result = JsonSerializer.Deserialize<BaseModel>(json, options)!;

            // Assert
            Assert.Equal("A", Assert.IsType<ExternalModel>(result).Name);
        }

        [Fact]
        public void ObjectConverterThrowsJsonException_WhenJsonTpePropertyNameIsNotAString()
        {
            // Arrange
            JsonSerializerOptions options = new();
            options.Converters.Add(new TestObjectconverter(typeof(ExternalModelWithInvalidPropertyType)));
            string json = JsonSerializer.Serialize(new ExternalModelWithInvalidPropertyType { Name = "A" }, options);

            // Act Assert
            Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<object>(json, options)!);
        }

        [Fact]
        public void ObjectConverterAccepts_TypeStringWithDifferentAssemblyVersion()
        {
            // Arrange
            string typeString = $"{typeof(ExternalModel).FullName}, {typeof(ExternalModel).Assembly.GetName().Name}, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null";
            var initial = new ClassWithObjectMember
            {
                ObjectProp = new { TypeString = typeString }
            };
            var json = JsonSerializer.Serialize(initial);

            // Act & Assert
            Assert.IsType<ExternalModel>(JsonSerializer.Deserialize<ClassWithObjectMember>(json, _options)!.ObjectProp);
        }

        [Fact]
        public void CreateConverterThrows_WhenTypesListContainsInvalidTypes()
        {
            // Act Assert
            Assert.Throws<ArgumentException>(() =>
            {
                new TestObjectconverter(typeof(ExternalModel).Assembly.GetTypes().ToArray());
            });
        }

        [Fact]
        public void CreateConverterThrows_WhenTypesListIsNull()
        {
            // Act Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new TestObjectconverter((Type[])null!);
            });
        }

        [Fact]
        public void CreateConverterThrows_WhenAssemblyListIsNull()
        {
            // Act Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new TestObjectconverter((Assembly[])null!);
            });
        }

        [Fact]
        public void ObjectConverterAccepts_ObjectSubtypeFromRegisteredAssembly()
        {
            // Arrange
            string json = JsonSerializer.Serialize(new ClassWithObjectMember { StringProp = "A", ObjectProp = new ExternalModel { Name = "A" } }, _options);

            // Act
            object result = JsonSerializer.Deserialize<ClassWithObjectMember>(json, _options)!;

            // Assert
            var classWithObjectMember = Assert.IsType<ClassWithObjectMember>(result);
            Assert.Equal("A", classWithObjectMember.StringProp);
            var extenalModel = Assert.IsType<ExternalModel>(classWithObjectMember.ObjectProp);
            Assert.Equal("A", extenalModel.Name);
        }

        [Fact]
        public void ObjectConverterWorks_WithGenericChildObjectromRegisteredAssembly()
        {
            var model = new ClassWithObjectMember
            {
                ObjectProp = new ScreenSettings<string>("dialog-settings")
            };

            var json = JsonSerializer.Serialize(model);
            var result = JsonSerializer.Deserialize<ClassWithObjectMember>(json, _options);

            Assert.NotNull(result);
            Assert.IsType<ScreenSettings<string>>(result.ObjectProp);
        }
        #endregion

        #region Test Helper Classes
        public class ExternalModel : BaseModel
        {
            public int ID { get; set; }
            public string? Name { get; set; }
        }

        public class ExternalModelWithInvalidPropertyType
        {
            public int ID { get; set; }
            public string? Name { get; set; }
            public int TypeString { get; set; }
        }

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

        public class ClassWithObjectMember
        {
            public string? StringProp { get; set; }
            public object? ObjectProp { get; set; }
        }

        internal class TestObjectconverter : Utils.Json.ObjectConverter
        {
            public TestObjectconverter()
            {
            }

            public TestObjectconverter(params Assembly[] additionalAssemblies)
                : base(additionalAssemblies)
            {
            }

            public TestObjectconverter(params Type[] types)
                : base(types)
            {
            }
        }

        public class ScreenSettings<TDialogSetting>(TDialogSetting settings)
        {
            public TDialogSetting Settings { get; set; } = settings;
            public string TypeString => this.GetType().AssemblyQualifiedName!;
        }
        #endregion
    }
}
