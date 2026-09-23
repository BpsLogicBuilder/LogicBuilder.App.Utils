using Microsoft.Extensions.Configuration;
using Moq;

namespace LogicBuilder.App.Utils.Tests
{
    public class EnvironmentHelpersTest
    {
        private readonly Mock<IConfiguration> configuration;
        private readonly EnvironmentHelpers itemToTest;

        public EnvironmentHelpersTest()
        {
            configuration = new Mock<IConfiguration>();
            itemToTest = new(configuration.Object);
        }

        [Fact]
        public void GetEnvironmentVariable_ReturnsExistingValue()
        {
            //arrange
            configuration.Setup(item => item["key"]).Returns("myValue");

            //act
            var result = itemToTest.GetEnvironmentVariable("key");

            //assert
            Assert.Equal("myValue", result);
        }

        [Fact]
        public void GetEnvironmentVariable_ReturnsDefaultValue_WhenTheKeyIsNotPresent()
        {
            //arrange
            configuration.Setup(item => item["key"]).Returns((string?)null);

            //act
            var result = itemToTest.GetEnvironmentVariable("key", "defaultValueText");

            //assert
            Assert.Equal("defaultValueText", result);
        }

        [Fact]
        public void GetEnvironmentVariable_ReturnsNull_WhenTheKeyIsNotPresent_And_DefaultValueIsNull()
        {
            //arrange
            configuration.Setup(item => item["key"]).Returns((string?)null);

            //act
            var result = itemToTest.GetEnvironmentVariable("key");

            //assert
            Assert.Null(result);
        }
    }
}
