using AutoMapper;
using LogicBuilder.App.Utils.Interfaces;
using LogicBuilder.EntityFrameworkCore.Mapping;
using LogicBuilder.Expressions.Utils.ExpansionDescriptors;
using LogicBuilder.Expressions.Utils.ExpressionBuilder;
using LogicBuilder.Expressions.Utils.ExpressionDescriptors;
using LogicBuilder.Expressions.Utils.Strutures;
using LogicBuilder.Forms.Parameters.Expansions;
using LogicBuilder.Forms.Parameters.Expressions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LogicBuilder.App.Utils.Tests
{
    public class MappingOperationsTest
    {
        static MappingOperationsTest()
        {
            InitializeMapperConfiguration();
        }

        public MappingOperationsTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        private static MapperConfiguration MapperConfiguration;
        #endregion Fields

        #region Tests

        [Fact]
        public void MapExpansion_From_SelectExpandDefinitionDescriptor_Returns_SelectExpandDefinition()
        {
            // Arrange
            var mappingOperations = serviceProvider.GetRequiredService<IMappingOperations>();
            var descriptor = new SelectExpandDefinitionDescriptor
            (
                ["Products"],
                [
                    new("CategoryID"),
                    new("CategoryName")
                ]
            );

            // Act
            var result = mappingOperations.MapExpansion(descriptor);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.ExpandedItems);
            Assert.NotNull(result.Selects);
            Assert.Single(result.Selects);
            Assert.Equal(2, result.ExpandedItems.Count);
            Assert.Equal("Products", result.Selects[0]);
            Assert.Equal("CategoryID", result.ExpandedItems[0].MemberName);
            Assert.Equal("CategoryName", result.ExpandedItems[1].MemberName);
        }

        [Fact]
        public void MapExpansion_From_SelectExpandDefinitionParameters_Returns_SelectExpandDefinition()
        {
            // Arrange
            var mappingOperations = serviceProvider.GetRequiredService<IMappingOperations>();
            var parameters = new SelectExpandDefinitionParameters
            (
                ["Products"],
                [
                    new("CategoryID"),
                    new("CategoryName")
                ]
            );

            // Act
            var result = mappingOperations.MapExpansion(parameters);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.ExpandedItems);
            Assert.NotNull(result.Selects);
            Assert.Single(result.Selects);
            Assert.Equal(2, result.ExpandedItems.Count);
            Assert.Equal("Products", result.Selects[0]);
            Assert.Equal("CategoryID", result.ExpandedItems[0].MemberName);
            Assert.Equal("CategoryName", result.ExpandedItems[1].MemberName);
        }

        [Fact]
        public void MapToOperator_From_DescriptorBase_Returns_IExpressionPart()
        {
            // Arrange
            var mappingOperations = serviceProvider.GetRequiredService<IMappingOperations>();
            var descriptor = new WhereDescriptor
            (
                new ParameterDescriptor("q"),
                new EqualsBinaryDescriptor
                (
                    new MemberSelectorDescriptor("CategoryID", new ParameterDescriptor("a")),
                    new ConstantDescriptor(1)
                ),
                "a"
            );

            // Act
            var result = mappingOperations.MapToOperator(descriptor);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<IExpressionPart>(result, exactMatch: false);
        }

        [Fact]
        public void MapToOperator_From_IExpressionParameter_Returns_IExpressionPart()
        {
            // Arrange
            var mappingOperations = serviceProvider.GetRequiredService<IMappingOperations>();
            var parameter = new WhereOperatorParameters
            (
                new ParameterOperatorParameters("q"),
                new EqualsBinaryOperatorParameters
                (
                    new MemberSelectorOperatorParameters("CategoryID", new ParameterOperatorParameters("a")),
                    new ConstantOperatorParameters(1)
                ),
                "a"
            );

            // Act
            var result = mappingOperations.MapToOperator(parameter);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<IExpressionPart>(result, exactMatch: false);
        }

        [Fact]
        public void MapToOperator_From_ComplexDescriptor_Returns_IExpressionPart()
        {
            // Arrange
            var mappingOperations = serviceProvider.GetRequiredService<IMappingOperations>();
            var descriptor = new SelectDescriptor
            (
                new OrderByDescriptor
                (
                    new ParameterDescriptor("q"),
                    new MemberSelectorDescriptor("CategoryID", new ParameterDescriptor("a")),
                    ListSortDirection.Descending,
                    "a"
                ),
                new MemberInitDescriptor
                (
                    new Dictionary<string, DescriptorBase>
                    {
                        ["CategoryID"] = new MemberSelectorDescriptor("CategoryID", new ParameterDescriptor("a")),
                        ["CategoryName"] = new MemberSelectorDescriptor("CategoryName", new ParameterDescriptor("a"))
                    }
                ),
                "a"
            );

            // Act
            var result = mappingOperations.MapToOperator(descriptor);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<IExpressionPart>(result, exactMatch: false);
        }

        [Fact]
        public void MapToOperator_From_ComplexParameter_Returns_IExpressionPart()
        {
            // Arrange
            var mappingOperations = serviceProvider.GetRequiredService<IMappingOperations>();
            var parameter = new SelectOperatorParameters
            (
                new OrderByOperatorParameters
                (
                    new ParameterOperatorParameters("q"),
                    new MemberSelectorOperatorParameters("CategoryID", new ParameterOperatorParameters("a")),
                    ListSortDirection.Descending,
                    "a"
                ),
                new MemberInitOperatorParameters
                (
                    [
                        new("CategoryID", new MemberSelectorOperatorParameters("CategoryID", new ParameterOperatorParameters("a"))),
                        new("CategoryName", new MemberSelectorOperatorParameters("CategoryName", new ParameterOperatorParameters("a")))
                    ]
                ),
                "a"
            );

            // Act
            var result = mappingOperations.MapToOperator(parameter);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<IExpressionPart>(result, exactMatch: false);
        }

        [Fact]
        public void MapToOperator_From_AggregateDescriptor_Returns_IExpressionPart()
        {
            // Arrange
            var mappingOperations = serviceProvider.GetRequiredService<IMappingOperations>();
            var descriptor = new AverageDescriptor
            (
                new ParameterDescriptor("q"),
                new MemberSelectorDescriptor("CategoryID", new ParameterDescriptor("a")),
                "a"
            );

            // Act
            var result = mappingOperations.MapToOperator(descriptor);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<IExpressionPart>(result, exactMatch: false);
        }

        [Fact]
        public void MapToOperator_From_AggregateParameter_Returns_IExpressionPart()
        {
            // Arrange
            var mappingOperations = serviceProvider.GetRequiredService<IMappingOperations>();
            var parameter = new CountOperatorParameters
            (
                new ParameterOperatorParameters("q")
            );

            // Act
            var result = mappingOperations.MapToOperator(parameter);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<IExpressionPart>(result, exactMatch: false);
        }

        [Fact]
        public void MapExpansion_From_EmptyDescriptor_Returns_EmptySelectExpandDefinition()
        {
            // Arrange
            var mappingOperations = serviceProvider.GetRequiredService<IMappingOperations>();
            var descriptor = new SelectExpandDefinitionDescriptor
            (
                [],
                []
            );

            // Act
            var result = mappingOperations.MapExpansion(descriptor);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.ExpandedItems);
            Assert.NotNull(result.Selects);
            Assert.Empty(result.ExpandedItems);
            Assert.Empty(result.Selects);
        }

        [Fact]
        public void MapExpansion_From_EmptyParameters_Returns_EmptySelectExpandDefinition()
        {
            // Arrange
            var mappingOperations = serviceProvider.GetRequiredService<IMappingOperations>();
            var parameters = new SelectExpandDefinitionParameters
            (
                [],
                []
            );

            // Act
            var result = mappingOperations.MapExpansion(parameters);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.ExpandedItems);
            Assert.NotNull(result.Selects);
            Assert.Empty(result.ExpandedItems);
            Assert.Empty(result.Selects);
        }

        [Fact]
        public void MapExpansion_From_Expansion_With_Sort_And_Filter_Succeeds()
        {
            // Arrange
            var mappingOperations = serviceProvider.GetRequiredService<IMappingOperations>();
            var parameters = new SelectExpandDefinitionParameters
            (
                [],
                [
                    new SelectExpandItemParameters
                    (
                        "enrollments",
                        new SelectExpandItemFilterParameters
                        (
                            new FilterLambdaOperatorParameters
                            (
                                new GreaterThanBinaryOperatorParameters
                                (
                                    new MemberSelectorOperatorParameters("enrollmentID", new ParameterOperatorParameters("a")),
                                    new ConstantOperatorParameters(0)
                                ),
                                typeof(Data.Enrollment),
                                "a"
                            )
                        ),
                        new SelectExpandItemQueryFunctionParameters
                        (
                            new SortCollectionParameters
                            (
                                [
                                    new SortDescriptionParameters("Grade", Expressions.Utils.Strutures.ListSortDirection.Ascending)
                                ],
                                null,
                                null
                            )
                        ),
                        null,
                        null
                    )
                ]
            );

            // Act
            var result = mappingOperations.MapExpansion(parameters);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.ExpandedItems);
            Assert.NotNull(result.Selects);
            Assert.NotEmpty(result.ExpandedItems);
            Assert.Empty(result.Selects);
        }

        [Fact]
        public void MapToOperator_From_GroupByDescriptor_Returns_IExpressionPart()
        {
            // Arrange
            var mappingOperations = serviceProvider.GetRequiredService<IMappingOperations>();
            var descriptor = new GroupByDescriptor
            (
                new ParameterDescriptor("q"),
                new MemberSelectorDescriptor("SupplierID", new ParameterDescriptor("a")),
                "a"
            );

            // Act
            var result = mappingOperations.MapToOperator(descriptor);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<IExpressionPart>(result, exactMatch: false);
        }

        [Fact]
        public void MapToOperator_From_GroupByParameter_Returns_IExpressionPart()
        {
            // Arrange
            var mappingOperations = serviceProvider.GetRequiredService<IMappingOperations>();
            var parameter = new GroupByOperatorParameters
            (
                new ParameterOperatorParameters("q"),
                new MemberSelectorOperatorParameters("SupplierID", new ParameterOperatorParameters("a")),
                "a"
            );

            // Act
            var result = mappingOperations.MapToOperator(parameter);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<IExpressionPart>(result, exactMatch: false);
        }

        #endregion Tests

        #region Helpers

        [MemberNotNull(nameof(MapperConfiguration))]
        private static void InitializeMapperConfiguration()
        {
            MapperConfiguration ??= new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ExpressionOperatorsMappingProfile>();
                cfg.AddProfile<ExpressionParameterToDescriptorMappingProfile>();
                cfg.AddProfile<ExpansionParameterToDescriptorMappingProfile>();
                cfg.AddProfile<ExpansionDescriptorToOperatorMappingProfile>();
            }, Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory.Instance);
        }

        [MemberNotNull(nameof(serviceProvider))]
        private void Initialize()
        {
            serviceProvider = new ServiceCollection()
                .AddSingleton<AutoMapper.IConfigurationProvider>(MapperConfiguration)
                .AddTransient<IMapper>(sp => new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService))
                .AddAppUtilsServices()
                .BuildServiceProvider();
        }

        #endregion Helpers
    }
}
