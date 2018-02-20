using System;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using ShipWorks.Filters;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Filters.Search;
using Xunit;

namespace ShipWorks.Tests.Filters.Search
{
    public class SingleScanSearchConditionTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly SqlGenerationContext context;

        public SingleScanSearchConditionTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            context = new SqlGenerationContext(FilterTarget.Orders);
        }

        [Fact]
        public void GenerateSql_SearchesOrderNumberComplete_WhenSearchTextIsNonNumeric()
        {
            var testObject = new SingleScanSearchCondition("52x");
            var sql = testObject.GenerateSql(context);

            Assert.Contains("SELECT OrderId FROM [Order] WHERE OrderNumberComplete LIKE N'52x%'", sql);
            Assert.Contains("SELECT OrderId FROM [OrderSearch] WHERE OrderNumberComplete LIKE N'52x%'", sql);
        }

        [Fact]
        public void GenerateSql_DoesNotSearchOrderNumber_WhenSearchTextIsNonNumeric()
        {
            var testObject = new SingleScanSearchCondition("52x");
            var sql = testObject.GenerateSql(context);

            Assert.DoesNotContain("SELECT OrderId FROM [Order] WHERE OrderNumber ", sql);
            Assert.DoesNotContain("SELECT OrderId FROM [OrderSearch] WHERE OrderNumber ", sql);
        }

        [Fact]
        public void GenerateSql_SearchesOrderNumberComplete_WhenSearchTextIsNumeric()
        {
            var testObject = new SingleScanSearchCondition("52");
            var sql = testObject.GenerateSql(context);

            Assert.Contains("SELECT OrderId FROM [Order] WHERE OrderNumberComplete LIKE N'52%'", sql);
            Assert.Contains("SELECT OrderId FROM [OrderSearch] WHERE OrderNumberComplete LIKE N'52%'", sql);
        }

        [Fact]
        public void GenerateSql_SearchesOrderNumber_WhenSearchTextIsNonNumeric()
        {
            var testObject = new SingleScanSearchCondition("52");
            var sql = testObject.GenerateSql(context);

            Assert.Contains("SELECT OrderId FROM [Order] WHERE OrderNumber = 52", sql);
            Assert.Contains("SELECT OrderId FROM [OrderSearch] WHERE OrderNumber = 52", sql);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}