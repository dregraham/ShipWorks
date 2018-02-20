using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Filters;
using ShipWorks.Filters.Content.Conditions.QuickSearch;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Filters.Search;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Filters.Search
{
    public class OrderQuickSearchDefinitionProviderTest : IDisposable
    {
        readonly AutoMock mock;

        private const string testNumericQuery = "42";
        private const string testOneWordQuery = "WordUp";
        private const string testTwoWordQuery = "First Last";
        OrderQuickSearchDefinitionProvider testObject;

        public OrderQuickSearchDefinitionProviderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            
            testObject = mock.Create<OrderQuickSearchDefinitionProvider>();
        }

        [Fact]
        public void GetDefinition_ReturnsOrderDefinition()
        {
            var definition = testObject.GetDefinition(testNumericQuery);

            Assert.Equal(FilterTarget.Orders, definition.FilterTarget);
        }

        [Fact]
        public void GetDefinition_DefinitionIncludesOrderNumberCompleteCondition()
        {
            var definition = testObject.GetDefinition(testNumericQuery);

            IEnumerable<QuickSearchCondition> quickSearchConditions = definition.RootContainer.FirstGroup.Conditions.Cast<QuickSearchCondition>();
            Assert.Equal(1, quickSearchConditions.Count());

            QuickSearchCondition quickSearchCondition = quickSearchConditions.Single();

            SqlGenerationContext context = new SqlGenerationContext(FilterTarget.Orders);
            string sql = quickSearchCondition.GenerateSql(context).ToLowerInvariant();

            Assert.True(sql.Contains("SELECT OrderId FROM [Order] WHERE OrderNumberComplete LIKE".ToLowerInvariant()));
            Assert.True(sql.Contains("SELECT OrderId FROM [OrderSearch] WHERE OrderNumberComplete LIKE".ToLowerInvariant()));
            
            Assert.Equal(2, Regex.Matches(sql, $"'{testNumericQuery}%'".ToLowerInvariant()).Count);
        }

        [Fact]
        public void GetDefinition_DefinitionIncludesOrderNumberCondition_WhenTextIsNumeric()
        {
            var definition = testObject.GetDefinition(testNumericQuery);

            IEnumerable<QuickSearchCondition> quickSearchConditions = definition.RootContainer.FirstGroup.Conditions.Cast<QuickSearchCondition>();
            Assert.Equal(1, quickSearchConditions.Count());

            QuickSearchCondition quickSearchCondition = quickSearchConditions.Single();

            SqlGenerationContext context = new SqlGenerationContext(FilterTarget.Orders);
            string sql = quickSearchCondition.GenerateSql(context).ToLowerInvariant();

            Assert.True(sql.Contains("SELECT OrderId FROM [Order] WHERE OrderNumber =".ToLowerInvariant()));
            Assert.True(sql.Contains("SELECT OrderId FROM [OrderSearch] WHERE OrderNumber =".ToLowerInvariant()));
            Assert.Equal(2, Regex.Matches(sql, $"= {testNumericQuery}".ToLowerInvariant()).Count);
            Assert.Equal(2, Regex.Matches(sql, $"'{testNumericQuery}%'".ToLowerInvariant()).Count);
        }

        [Fact]
        public void GetDefinition_DefinitionDoesNotIncludeNameConditions()
        {
            var definition = testObject.GetDefinition(testNumericQuery);

            IEnumerable<QuickSearchCondition> quickSearchConditions = definition.RootContainer.FirstGroup.Conditions.Cast<QuickSearchCondition>();
            Assert.Equal(1, quickSearchConditions.Count());

            QuickSearchCondition quickSearchCondition = quickSearchConditions.Single();

            SqlGenerationContext context = new SqlGenerationContext(FilterTarget.Orders);
            string sql = quickSearchCondition.GenerateSql(context).ToLowerInvariant();

            Assert.False(sql.Contains("FirstName".ToLowerInvariant()));
            Assert.False(sql.Contains("LastName".ToLowerInvariant()));
            Assert.False(sql.Contains("Email".ToLowerInvariant()));

            Assert.True(sql.ToLowerInvariant().Contains($"like n'{testNumericQuery}%'"));
            Assert.True(sql.ToLowerInvariant().Contains($"= {testNumericQuery}"));
        }

        [Fact]
        public void GetDefinition_DefinitionIncludesNameConditionsWhenPassedFirstAndLastName()
        {
            var definition = testObject.GetDefinition(testTwoWordQuery);

            IEnumerable<QuickSearchCondition> quickSearchConditions = definition.RootContainer.FirstGroup.Conditions.Cast<QuickSearchCondition>();
            Assert.Equal(1, quickSearchConditions.Count());

            QuickSearchCondition quickSearchCondition = quickSearchConditions.Single();

            SqlGenerationContext context = new SqlGenerationContext(FilterTarget.Orders);
            string sql = quickSearchCondition.GenerateSql(context).ToLowerInvariant();

            Assert.True(sql.Contains("FirstName".ToLowerInvariant()));
            Assert.True(sql.Contains("LastName".ToLowerInvariant()));
            Assert.False(sql.Contains("Email".ToLowerInvariant()));
            Assert.True(sql.Contains($"like n'{testTwoWordQuery}%'".ToLowerInvariant()));
            Assert.True(sql.Contains("'First%".ToLowerInvariant()));
            Assert.True(sql.Contains("'Last%".ToLowerInvariant()));
        }

        [Fact]
        public void GetDefinition_DefinitionIncludesNameAndEmailConditionWhenSingleWord()
        {
            var definition = testObject.GetDefinition(testOneWordQuery);

            IEnumerable<QuickSearchCondition> quickSearchConditions = definition.RootContainer.FirstGroup.Conditions.Cast<QuickSearchCondition>();
            Assert.Equal(1, quickSearchConditions.Count());

            QuickSearchCondition quickSearchCondition = quickSearchConditions.Single();

            SqlGenerationContext context = new SqlGenerationContext(FilterTarget.Orders);
            string sql = quickSearchCondition.GenerateSql(context).ToLowerInvariant();

            Assert.True(sql.Contains("FirstName".ToLowerInvariant()));
            Assert.True(sql.Contains("LastName".ToLowerInvariant()));
            Assert.True(sql.Contains("Email".ToLowerInvariant()));
            Assert.Equal(8, Regex.Matches(sql, $"'{testOneWordQuery}%'".ToLowerInvariant()).Count);
        }

        [Fact]
        public void GetDefinition_IncludesStoreSpecificCriteria_WhenStoreHasCriteria()
        {
            Mock<IQuickSearchStoreSql> storeSearchSql = mock.Mock<IQuickSearchStoreSql>();
            storeSearchSql.Setup(s => s.GenerateSql(It.IsAny<SqlGenerationContext>(), It.IsAny<string>()))
                .Returns(new string[]
                {
                    "select OrderId from [SomeStore] where SomeField LIKE ",
                    "select OrderId from [SomeOtherStore] where SomeOtherField LIKE "
                });

            IEnumerable<IQuickSearchStoreSql> storeSqls = new IQuickSearchStoreSql[] { storeSearchSql.Object };
            mock.Provide(storeSqls);

            testObject = mock.Create<OrderQuickSearchDefinitionProvider>();
            var definition = testObject.GetDefinition(testNumericQuery);

            IEnumerable<QuickSearchCondition> quickSearchConditions = definition.RootContainer.FirstGroup.Conditions.Cast<QuickSearchCondition>();
            Assert.Equal(1, quickSearchConditions.Count());

            QuickSearchCondition quickSearchCondition = quickSearchConditions.Single();

            SqlGenerationContext context = new SqlGenerationContext(FilterTarget.Orders);
            string sql = quickSearchCondition.GenerateSql(context).ToLowerInvariant();

            Assert.True(sql.Contains("SELECT OrderId FROM [Order] WHERE OrderNumberComplete LIKE".ToLowerInvariant()));
            Assert.True(sql.Contains("SELECT OrderId FROM [OrderSearch] WHERE OrderNumberComplete LIKE".ToLowerInvariant()));
            Assert.True(sql.Contains("select OrderId from [SomeStore] where SomeField LIKE".ToLowerInvariant()));
            Assert.True(sql.Contains("select OrderId from [SomeOtherStore] where SomeOtherField LIKE".ToLowerInvariant()));
            
            Assert.Equal(2, Regex.Matches(sql, $"= {testNumericQuery}".ToLowerInvariant()).Count);
            Assert.Equal(2, Regex.Matches(sql, $"'{testNumericQuery}%'".ToLowerInvariant()).Count);
        }

        public void Dispose()
        {
            mock.Dispose();
        }

    }
}