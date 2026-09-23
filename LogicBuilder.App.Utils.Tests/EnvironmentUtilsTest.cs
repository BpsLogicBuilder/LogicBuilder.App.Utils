using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogicBuilder.App.Utils.Tests
{
    public class EnvironmentUtilsTest
    {
        private readonly Mock<IConfiguration> configuration;
        private readonly EnvironmentHelpers environmentHelpsers;

        public EnvironmentUtilsTest()
        {
            configuration = new Mock<IConfiguration>();
            environmentHelpsers = new(configuration.Object);
        }

        [Fact]
        public void GetEnvironmentVariable_ReturnsExistingValue()
        {
            //arrange
            configuration.Setup(item => item["key"]).Returns("myValue");

            //act
            var result = EnvironmentUtils.GetEnvironmentVariable(environmentHelpsers, "key");

            //assert
            Assert.Equal("myValue", result);
        }

        [Fact]
        public void GetEnvironmentVariable_ReturnsDefaultValue_WhenTheKeyIsNotPresent()
        {
            //arrange
            configuration.Setup(item => item["key"]).Returns((string?)null);

            //act
            var result = EnvironmentUtils.GetEnvironmentVariable(environmentHelpsers, "key", "defaultValueText");

            //assert
            Assert.Equal("defaultValueText", result);
        }

        [Fact]
        public void GetEnvironmentVariable_ReturnsNull_WhenTheKeyIsNotPresent_And_DefaultValueIsNull()
        {
            //arrange
            configuration.Setup(item => item["key"]).Returns((string?)null);

            //act
            var result = EnvironmentUtils.GetEnvironmentVariable(environmentHelpsers, "key");

            //assert
            Assert.Null(result);
        }
    }
}
