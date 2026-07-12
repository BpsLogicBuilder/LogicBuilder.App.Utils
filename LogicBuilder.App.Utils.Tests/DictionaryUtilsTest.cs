using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using LogicBuilder.App.Utils.Interfaces;
using LogicBuilder.App.Utils.Tests.Data;
using LogicBuilder.EntityFrameworkCore.Mapping;
using LogicBuilder.Forms.Parameters.Expressions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace LogicBuilder.App.Utils.Tests
{
    public class DictionaryUtilsTest
    {
        static DictionaryUtilsTest()
        {
            Initialize();
        }

        #region Fields
        private static MapperConfiguration MapperConfiguration;
        private static IServiceProvider serviceProvider;
        #endregion Fields

        #region Tests
        [Fact]
        public void ToDictionary_WithSimpleKeyAndValue_CreatesDictionary()
        {
            // Arrange
            var helper = serviceProvider.GetRequiredService<IDictionaryHelper>();
            var categories = GetCategories();

            var keySelector = CreateSelectorLambdaOperatorParameters<Category, int>
            (
                new MemberSelectorOperatorParameters("CategoryID", new ParameterOperatorParameters("c"))
            );

            var valueSelector = CreateSelectorLambdaOperatorParameters<Category, string>
            (
                new MemberSelectorOperatorParameters("CategoryName", new ParameterOperatorParameters("c"))
            );

            // Act
            var result = DictionaryUtils<Category, int, string>.ToDictionary(helper, categories, keySelector, valueSelector);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("CategoryOne", result[1]);
            Assert.Equal("CategoryTwo", result[2]);
        }

        [Fact]
        public void ToDictionary_WithComplexKey_CreatesDictionary()
        {
            // Arrange
            var helper = serviceProvider.GetRequiredService<IDictionaryHelper>();
            var products = GetProducts();

            var keySelector = CreateSelectorLambdaOperatorParameters<Product, int>
            (
                new MemberSelectorOperatorParameters("SupplierID", new ParameterOperatorParameters("c"))
            );

            var valueSelector = CreateSelectorLambdaOperatorParameters<Product, string>
            (
                new MemberSelectorOperatorParameters("ProductName", new ParameterOperatorParameters("c"))
            );

            // Act
            var result = DictionaryUtils<Product, int, string>.ToDictionary(helper, products, keySelector, valueSelector);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("ProductOne", result[1]);
            Assert.Equal("ProductTwo", result[2]);
        }

        [Fact]
        public void ToDictionary_WithObjectValue_CreatesDictionary()
        {
            // Arrange
            var helper = serviceProvider.GetRequiredService<IDictionaryHelper>();
            var categories = GetCategories();

            var keySelector = CreateSelectorLambdaOperatorParameters<Category, int>
            (
                new MemberSelectorOperatorParameters("CategoryID", new ParameterOperatorParameters("c"))
            );

            var valueSelector = CreateSelectorLambdaOperatorParameters<Category, Category>
            (
                new ParameterOperatorParameters("c")
            );

            // Act
            var result = DictionaryUtils<Category, int, Category>.ToDictionary(helper, categories, keySelector, valueSelector);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("CategoryOne", result[1].CategoryName);
            Assert.Equal("CategoryTwo", result[2].CategoryName);
        }

        [Fact]
        public void ToDictionary_WithNestedProperty_CreatesDictionary()
        {
            // Arrange
            var helper = serviceProvider.GetRequiredService<IDictionaryHelper>();
            var products = GetProductsWithAddresses();

            var keySelector = CreateSelectorLambdaOperatorParameters<Product, int>
            (
                new MemberSelectorOperatorParameters("ProductID", new ParameterOperatorParameters("c"))
            );

            var valueSelector = CreateSelectorLambdaOperatorParameters<Product, int>
            (
                new CountOperatorParameters(new MemberSelectorOperatorParameters("AlternateAddresses", new ParameterOperatorParameters("c")))
            );

            // Act
            var result = DictionaryUtils<Product, int, int>.ToDictionary(helper, products, keySelector, valueSelector);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(2, result[1]);
            Assert.Equal(2, result[2]);
        }

        [Fact]
        public void ToDictionary_WithEmptyEnumerable_ReturnsEmptyDictionary()
        {
            // Arrange
            var helper = serviceProvider.GetRequiredService<IDictionaryHelper>();
            var categories = Enumerable.Empty<Category>();

            var keySelector = CreateSelectorLambdaOperatorParameters<Category, int>
            (
                new MemberSelectorOperatorParameters("CategoryID", new ParameterOperatorParameters("c"))
            );

            var valueSelector = CreateSelectorLambdaOperatorParameters<Category, string>
            (
                new MemberSelectorOperatorParameters("CategoryName", new ParameterOperatorParameters("c"))
            );

            // Act
            var result = DictionaryUtils<Category, int, string>.ToDictionary(helper, categories, keySelector, valueSelector);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void ToDictionary_WithSingleElement_CreatesDictionary()
        {
            // Arrange
            var helper = serviceProvider.GetRequiredService<IDictionaryHelper>();
            var categories = new[]
            {
                new Category { CategoryID = 1, CategoryName = "CategoryOne" }
            };

            var keySelector = CreateSelectorLambdaOperatorParameters<Category, int>
            (
                new MemberSelectorOperatorParameters("CategoryID", new ParameterOperatorParameters("c"))
            );

            var valueSelector = CreateSelectorLambdaOperatorParameters<Category, string>
            (
                new MemberSelectorOperatorParameters("CategoryName", new ParameterOperatorParameters("c"))
            );

            // Act
            var result = DictionaryUtils<Category, int, string>.ToDictionary(helper, categories, keySelector, valueSelector);

            // Assert
            Assert.Single(result);
            Assert.Equal("CategoryOne", result[1]);
        }

        [Fact]
        public void ToDictionary_WithDuplicateKeys_ThrowsArgumentException()
        {
            // Arrange
            var helper = serviceProvider.GetRequiredService<IDictionaryHelper>();
            var categories = new[]
            {
                new Category { CategoryID = 1, CategoryName = "CategoryOne" },
                new Category { CategoryID = 1, CategoryName = "CategoryTwo" }
            };

            var keySelector = CreateSelectorLambdaOperatorParameters<Category, int>
            (
                new MemberSelectorOperatorParameters("CategoryID", new ParameterOperatorParameters("c"))
            );

            var valueSelector = CreateSelectorLambdaOperatorParameters<Category, string>
            (
                new MemberSelectorOperatorParameters("CategoryName", new ParameterOperatorParameters("c"))
            );

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                DictionaryUtils<Category, int, string>.ToDictionary(helper, categories, keySelector, valueSelector));
        }

        [Fact]
        public void ToDictionary_WithStringKey_CreatesDictionary()
        {
            // Arrange
            var helper = serviceProvider.GetRequiredService<IDictionaryHelper>();
            var categories = GetCategories();

            var keySelector = CreateSelectorLambdaOperatorParameters<Category, string>
            (
                new MemberSelectorOperatorParameters("CategoryName", new ParameterOperatorParameters("c"))
            );

            var valueSelector = CreateSelectorLambdaOperatorParameters<Category, int>
            (
                new MemberSelectorOperatorParameters("CategoryID", new ParameterOperatorParameters("c"))
            );

            // Act
            var result = DictionaryUtils<Category, string, int>.ToDictionary(helper, categories, keySelector, valueSelector);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(1, result["CategoryOne"]);
            Assert.Equal(2, result["CategoryTwo"]);
        }

        #endregion Tests

        #region Helpers
        [MemberNotNull(nameof(MapperConfiguration))]
        [MemberNotNull(nameof(serviceProvider))]
        private static void Initialize()
        {
            MapperConfiguration ??= ConfigurationHelper.GetMapperConfiguration(cfg =>
            {
                cfg.AddProfile<ExpressionOperatorsMappingProfile>();
                cfg.AddProfile<ExpressionParameterToDescriptorMappingProfile>();
            });

            serviceProvider = new ServiceCollection()
                .AddSingleton<AutoMapper.IConfigurationProvider>
                (
                    MapperConfiguration
                )
                .AddTransient<IMapper>(sp => new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService))
                .AddTransient<IMappingOperations, MappingOperations>()
                .AddTransient<IDictionaryHelper, DictionaryHelper>()
                .BuildServiceProvider();
        }

        private static SelectorLambdaOperatorParameters CreateSelectorLambdaOperatorParameters<TSource, TResult>(IExpressionParameter selectorBody)
        {
            return new SelectorLambdaOperatorParameters
            (
                selectorBody,
                typeof(TSource),
                "c",
                typeof(TResult)
            );
        }

        private static IEnumerable<Category> GetCategories()
            =>
            [
                new Category
                {
                    CategoryID = 1,
                    CategoryName = "CategoryOne",
                    Products = []
                },
                new Category
                {
                    CategoryID = 2,
                    CategoryName = "CategoryTwo",
                    Products = []
                }
            ];

        private static IEnumerable<Product> GetProducts()
            =>
            [
                new Product
                {
                    ProductID = 1,
                    ProductName = "ProductOne",
                    SupplierID = 1,
                    AlternateAddresses = []
                },
                new Product
                {
                    ProductID = 2,
                    ProductName = "ProductTwo",
                    SupplierID = 2,
                    AlternateAddresses = []
                }
            ];

        private static IEnumerable<Product> GetProductsWithAddresses()
            =>
            [
                new Product
                {
                    ProductID = 1,
                    ProductName = "ProductOne",
                    SupplierID = 1,
                    AlternateAddresses =
                    [
                        new Address { AddressID = 1, City = "CityOne" },
                        new Address { AddressID = 2, City = "CityTwo" }
                    ]
                },
                new Product
                {
                    ProductID = 2,
                    ProductName = "ProductTwo",
                    SupplierID = 2,
                    AlternateAddresses =
                    [
                        new Address { AddressID = 3, City = "CityThree" },
                        new Address { AddressID = 4, City = "CityFour" }
                    ]
                }
            ];
        #endregion Helpers
    }
}
