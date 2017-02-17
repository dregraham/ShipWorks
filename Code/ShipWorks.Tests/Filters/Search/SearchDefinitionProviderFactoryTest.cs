using System;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore.Options;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Filters;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Search;
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

        public void Dispose()
        {
            mock.Dispose();
        }

    }
}