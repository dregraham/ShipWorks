using System;
using Autofac.Extras.Moq;
using ShipWorks.Filters;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Search;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Filters.Search
{
    public class SearchDefinitionProviderFactoryTest : IDisposable
    {
        readonly AutoMock mock;

        public SearchDefinitionProviderFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Theory]
        [InlineData(FilterTarget.Orders, typeof(OrderQuickSearchDefinitionProvider))]
        [InlineData(FilterTarget.Customers, typeof(CustomerQuickSearchDefinitionProvider))]
        public void CreateWithTargetAndNullDefinition_ReturnsCorrectDefinitionProvider(FilterTarget target, Type expectedType)
        {
            var testObject = mock.Create<SearchDefinitionProviderFactory>();
            var filterDefinitionProvider = testObject.Create(target, null);

            Assert.IsType(expectedType, filterDefinitionProvider);
        }

        [Theory]
        [InlineData(FilterTarget.Orders, typeof(OrderQuickSearchDefinitionProvider))]
        [InlineData(FilterTarget.Customers, typeof(CustomerQuickSearchDefinitionProvider))]
        public void CreateWithTarget_ReturnsCorrectDefinitionProvider(FilterTarget target, Type expectedType)
        {
            var testObject = mock.Create<SearchDefinitionProviderFactory>();
            var filterDefinitionProvider = testObject.Create(target);

            Assert.IsType(expectedType, filterDefinitionProvider);
        }

        [Theory]
        [InlineData(FilterTarget.Orders)]
        [InlineData(FilterTarget.Customers)]
        public void CreateWithTargetAndDefinition_ReturnsAdvancedSearchDefinitionProvider(FilterTarget target)
        {
            var testObject = mock.Create<SearchDefinitionProviderFactory>();
            var filterDefinitionProvider = testObject.Create(target, new FilterDefinition(target));

            Assert.IsType<AdvancedSearchDefinitionProvider>(filterDefinitionProvider);
        }

        [Theory]
        [InlineData(FilterTarget.Items)]
        [InlineData(FilterTarget.Shipments)]
        public void CreateWithTarget_ThrowsWhenTargetIsUnexpected(FilterTarget target)
        {
            var testObject = mock.Create<SearchDefinitionProviderFactory>();

            Assert.Throws<IndexOutOfRangeException>(() => testObject.Create(target));
        }

        public void Dispose()
        {
            mock.Dispose();
        }

    }
}