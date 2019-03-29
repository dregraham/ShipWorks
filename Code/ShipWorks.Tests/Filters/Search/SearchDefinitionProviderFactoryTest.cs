using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore.Settings;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Filters;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions.QuickSearch;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Filters.Search;
using ShipWorks.Stores;
using ShipWorks.Tests.Shared;
using ShipWorks.Users;
using Xunit;

namespace ShipWorks.Tests.Filters.Search
{
    public class SearchDefinitionProviderFactoryTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly Mock<IUserSession> userSession;
        private IUserSettingsEntity userSettings;

        public SearchDefinitionProviderFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            userSettings = new UserSettingsEntity(3)
            {
                SingleScanSettings = (int) SingleScanSettings.Disabled
            };

            userSession = mock.Mock<IUserSession>();
            userSession.Setup(u => u.Settings).Returns(userSettings);
        }

        [Theory]
        [InlineData(FilterTarget.Orders, typeof(OrderQuickSearchDefinitionProvider), false, SingleScanSettings.Disabled)]
        [InlineData(FilterTarget.Customers, typeof(CustomerQuickSearchDefinitionProvider), false, SingleScanSettings.Disabled)]
        [InlineData(FilterTarget.Orders, typeof(OrderQuickSearchDefinitionProvider), true, SingleScanSettings.Disabled)]
        [InlineData(FilterTarget.Customers, typeof(CustomerQuickSearchDefinitionProvider), true, SingleScanSettings.Disabled)]
        [InlineData(FilterTarget.Orders, typeof(OrderQuickSearchDefinitionProvider), false, SingleScanSettings.Scan)]
        [InlineData(FilterTarget.Customers, typeof(CustomerQuickSearchDefinitionProvider), false, SingleScanSettings.Scan)]
        [InlineData(FilterTarget.Orders, typeof(SingleScanSearchDefinitionProvider), true, SingleScanSettings.Scan)]
        [InlineData(FilterTarget.Customers, typeof(CustomerQuickSearchDefinitionProvider), true, SingleScanSettings.Scan)]
        [InlineData(FilterTarget.Orders, typeof(OrderQuickSearchDefinitionProvider), false, SingleScanSettings.AutoPrint)]
        [InlineData(FilterTarget.Customers, typeof(CustomerQuickSearchDefinitionProvider), false, SingleScanSettings.AutoPrint)]
        [InlineData(FilterTarget.Orders, typeof(SingleScanSearchDefinitionProvider), true, SingleScanSettings.AutoPrint)]
        [InlineData(FilterTarget.Customers, typeof(CustomerQuickSearchDefinitionProvider), true, SingleScanSettings.AutoPrint)]
        public void CreateWithTargetAndNullDefinition_ReturnsCorrectDefinitionProvider(FilterTarget target, Type expectedType, bool isScan, SingleScanSettings singleScanSetting)
        {
            userSettings = new UserSettingsEntity(3)
            {
                SingleScanSettings = (int)singleScanSetting
            };
            userSession.Setup(u => u.Settings).Returns(userSettings);

            var testObject = mock.Create<SearchDefinitionProviderFactory>();
            var filterDefinitionProvider = testObject.Create(target, null, isScan);

            Assert.IsType(expectedType, filterDefinitionProvider);
        }

        [Theory]
        [InlineData(FilterTarget.Orders, typeof(OrderQuickSearchDefinitionProvider), false, SingleScanSettings.Disabled)]
        [InlineData(FilterTarget.Orders, typeof(OrderQuickSearchDefinitionProvider), false, SingleScanSettings.Scan)]
        [InlineData(FilterTarget.Orders, typeof(OrderQuickSearchDefinitionProvider), false, SingleScanSettings.AutoPrint)]
        [InlineData(FilterTarget.Orders, typeof(OrderQuickSearchDefinitionProvider), true, SingleScanSettings.Disabled)]
        [InlineData(FilterTarget.Orders, typeof(SingleScanSearchDefinitionProvider), true, SingleScanSettings.Scan)]
        [InlineData(FilterTarget.Orders, typeof(SingleScanSearchDefinitionProvider), true, SingleScanSettings.AutoPrint)]
        [InlineData(FilterTarget.Customers, typeof(CustomerQuickSearchDefinitionProvider), false, SingleScanSettings.Disabled)]
        [InlineData(FilterTarget.Customers, typeof(CustomerQuickSearchDefinitionProvider), false, SingleScanSettings.Scan)]
        [InlineData(FilterTarget.Customers, typeof(CustomerQuickSearchDefinitionProvider), false, SingleScanSettings.AutoPrint)]
        [InlineData(FilterTarget.Customers, typeof(CustomerQuickSearchDefinitionProvider), true, SingleScanSettings.Disabled)]
        [InlineData(FilterTarget.Customers, typeof(CustomerQuickSearchDefinitionProvider), true, SingleScanSettings.Scan)]
        [InlineData(FilterTarget.Customers, typeof(CustomerQuickSearchDefinitionProvider), true, SingleScanSettings.AutoPrint)]
        public void CreateWithTarget_ReturnsCorrectDefinitionProvider(FilterTarget target, Type expectedType, bool isScan, SingleScanSettings singleScanSetting)
        {
            userSettings = new UserSettingsEntity(3)
            {
                SingleScanSettings = (int)singleScanSetting
            };
            userSession.Setup(u => u.Settings).Returns(userSettings);

            var testObject = mock.Create<SearchDefinitionProviderFactory>();
            var filterDefinitionProvider = testObject.Create(target, isScan);

            Assert.IsType(expectedType, filterDefinitionProvider);
        }

        [Theory]
        [InlineData(FilterTarget.Orders, false, SingleScanSettings.Disabled)]
        [InlineData(FilterTarget.Customers, false, SingleScanSettings.Disabled)]
        [InlineData(FilterTarget.Orders, true, SingleScanSettings.Disabled)]
        [InlineData(FilterTarget.Customers, true, SingleScanSettings.Disabled)]
        [InlineData(FilterTarget.Orders, false, SingleScanSettings.Scan)]
        [InlineData(FilterTarget.Customers, false, SingleScanSettings.Scan)]
        [InlineData(FilterTarget.Orders, true, SingleScanSettings.Scan)]
        [InlineData(FilterTarget.Customers, true, SingleScanSettings.Scan)]
        [InlineData(FilterTarget.Orders, false, SingleScanSettings.AutoPrint)]
        [InlineData(FilterTarget.Customers, false, SingleScanSettings.AutoPrint)]
        [InlineData(FilterTarget.Orders, true, SingleScanSettings.AutoPrint)]
        [InlineData(FilterTarget.Customers, true, SingleScanSettings.AutoPrint)]
        public void CreateWithTargetAndDefinition_ReturnsAdvancedSearchDefinitionProvider(FilterTarget target, bool isScan, SingleScanSettings singleScanSetting)
        {
            userSettings = new UserSettingsEntity(3)
            {
                SingleScanSettings = (int)singleScanSetting
            };
            userSession.Setup(u => u.Settings).Returns(userSettings);

            var testObject = mock.Create<SearchDefinitionProviderFactory>();
            var filterDefinitionProvider = testObject.Create(target, new FilterDefinition(target), isScan);

            Assert.IsType<AdvancedSearchDefinitionProvider>(filterDefinitionProvider);
        }

        [Theory]
        [InlineData(FilterTarget.Items, false, SingleScanSettings.Disabled)]
        [InlineData(FilterTarget.Shipments, false, SingleScanSettings.Disabled)]
        [InlineData(FilterTarget.Items, true, SingleScanSettings.Disabled)]
        [InlineData(FilterTarget.Shipments, true, SingleScanSettings.Disabled)]
        [InlineData(FilterTarget.Items, false, SingleScanSettings.Scan)]
        [InlineData(FilterTarget.Shipments, false, SingleScanSettings.Scan)]
        [InlineData(FilterTarget.Items, true, SingleScanSettings.Scan)]
        [InlineData(FilterTarget.Shipments, true, SingleScanSettings.Scan)]
        [InlineData(FilterTarget.Items, false, SingleScanSettings.AutoPrint)]
        [InlineData(FilterTarget.Shipments, false, SingleScanSettings.AutoPrint)]
        [InlineData(FilterTarget.Items, true, SingleScanSettings.AutoPrint)]
        [InlineData(FilterTarget.Shipments, true, SingleScanSettings.AutoPrint)]
        public void CreateWithTarget_ThrowsWhenTargetIsUnexpected(FilterTarget target, bool isScan, SingleScanSettings singleScanSetting)
        {
            userSettings = new UserSettingsEntity(3)
            {
                SingleScanSettings = (int)singleScanSetting
            };
            userSession.Setup(u => u.Settings).Returns(userSettings);

            var testObject = mock.Create<SearchDefinitionProviderFactory>();

            Assert.Throws<IndexOutOfRangeException>(() => testObject.Create(target, isScan));
        }

        [Fact]
        public void OrderQuickSearch_FiltersStoreSqlToStoresThatAreEnabled()
        {
            userSettings = new UserSettingsEntity(3)
            {
                SingleScanSettings = 0
            };
            userSession.Setup(u => u.Settings).Returns(userSettings);

            Mock<IQuickSearchStoreSql> storeSearchSql = mock.Mock<IQuickSearchStoreSql>();
            storeSearchSql.Setup(s => s.StoreType).Returns(StoreTypeCode.Amazon);
            storeSearchSql.Setup(s => s.GenerateSql(It.IsAny<SqlGenerationContext>(), It.IsAny<string>()))
                .Returns(new string[]
                {
                    "select OrderId from [SomeStore] where SomeField LIKE ",
                    "select OrderId from [SomeOtherStore] where SomeOtherField LIKE "
                });

            var amazonStoreType = mock.Mock<StoreType>();
            amazonStoreType.Setup(s => s.TypeCode).Returns(StoreTypeCode.Amazon);
            mock.Mock<IStoreManager>().Setup(m => m.GetUniqueStoreTypes()).Returns(new[] { amazonStoreType.Object });

            var testObject = mock.Create<SearchDefinitionProviderFactory>();
            var filterDefinitionProvider = testObject.Create(FilterTarget.Orders, false);
            var filterDefinition = filterDefinitionProvider.GetDefinition("hi");

            IEnumerable<QuickSearchCondition> quickSearchConditions = filterDefinition.RootContainer.FirstGroup.Conditions.Cast<QuickSearchCondition>();
            Assert.Equal(1, quickSearchConditions.Count());

            QuickSearchCondition quickSearchCondition = quickSearchConditions.Single();

            SqlGenerationContext context = new SqlGenerationContext(FilterTarget.Orders);
            string sql = quickSearchCondition.GenerateSql(context).ToLowerInvariant();

            Assert.True(sql.Contains("SELECT OrderId FROM [Order] WHERE OrderNumberComplete LIKE".ToLowerInvariant()));
            Assert.True(sql.Contains("SELECT OrderId FROM [OrderSearch] WHERE OrderNumberComplete LIKE".ToLowerInvariant()));
            Assert.True(sql.Contains("select OrderId from [SomeStore] where SomeField LIKE".ToLowerInvariant()));
            Assert.True(sql.Contains("select OrderId from [SomeOtherStore] where SomeOtherField LIKE".ToLowerInvariant()));

            Assert.Equal(8, Regex.Matches(sql, $"'hi%'".ToLowerInvariant()).Count);
        }

        [Fact]
        public void OrderQuickSearch_FiltersOutStoreSqlStoresThatAreDisabled()
        {
            userSettings = new UserSettingsEntity(3)
            {
                SingleScanSettings = 0
            };
            userSession.Setup(u => u.Settings).Returns(userSettings);

            Mock<IQuickSearchStoreSql> storeSearchSql = new Mock<IQuickSearchStoreSql>();
            storeSearchSql.Setup(s => s.StoreType).Returns(StoreTypeCode.Amazon);
            storeSearchSql.Setup(s => s.GenerateSql(It.IsAny<SqlGenerationContext>(), It.IsAny<string>()))
                .Returns(new string[]
                {
                    "select OrderId from [SomeStore] where SomeField LIKE ",
                    "select OrderId from [SomeOtherStore] where SomeOtherField LIKE "
                });

            mock.Mock<IStoreManager>().Setup(m => m.GetUniqueStoreTypes()).Returns(new[] { mock.Mock<StoreType>().Object });

            var testObject = mock.Create<SearchDefinitionProviderFactory>();
            var filterDefinitionProvider = testObject.Create(FilterTarget.Orders, false);
            var filterDefinition = filterDefinitionProvider.GetDefinition("hi");

            IEnumerable<QuickSearchCondition> quickSearchConditions = filterDefinition.RootContainer.FirstGroup.Conditions.Cast<QuickSearchCondition>();
            Assert.Equal(1, quickSearchConditions.Count());

            QuickSearchCondition quickSearchCondition = quickSearchConditions.Single();

            SqlGenerationContext context = new SqlGenerationContext(FilterTarget.Orders);
            string sql = quickSearchCondition.GenerateSql(context).ToLowerInvariant();

            Assert.True(sql.Contains("SELECT OrderId FROM [Order] WHERE OrderNumberComplete LIKE".ToLowerInvariant()));
            Assert.True(sql.Contains("SELECT OrderId FROM [OrderSearch] WHERE OrderNumberComplete LIKE".ToLowerInvariant()));
            Assert.False(sql.Contains("select OrderId from [SomeStore] where SomeField LIKE".ToLowerInvariant()));
            Assert.False(sql.Contains("select OrderId from [SomeOtherStore] where SomeOtherField LIKE".ToLowerInvariant()));

            Assert.Equal(8, Regex.Matches(sql, $"'hi%'".ToLowerInvariant()).Count);
        }

        public void Dispose()
        {
            mock.Dispose();
        }

    }
}