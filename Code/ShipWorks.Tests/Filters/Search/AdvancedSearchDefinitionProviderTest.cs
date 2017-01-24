using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Filters;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.Conditions.Orders;
using ShipWorks.Filters.Search;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Filters.Search
{
    public class AdvancedSearchDefinitionProviderTest : IDisposable
    {
        private readonly AutoMock mock;

        public AdvancedSearchDefinitionProviderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void GetDefinition_ReturnsNull_WhenAdvancedFilterDefinitionIsEmpty()
        {
            AdvancedSearchDefinitionProvider testObject = new AdvancedSearchDefinitionProvider(new FilterDefinition(FilterTarget.Orders), null);

            Assert.Null(testObject.GetDefinition(""));
        }

        [Fact]
        public void GetDefinition_GetsQuickSearchFilterDefinitionFromQuickSearchDefinitionProvider_WhenQuickSearchStringIsNotEmpty()
        {
            Mock<ISearchDefinitionProvider> quickFilterDefinitionProvider = mock.Mock<ISearchDefinitionProvider>();
            AdvancedSearchDefinitionProvider testObject = GetDefaultAdvancedSearchProvider(quickFilterDefinitionProvider);
            testObject.GetDefinition("something");

            quickFilterDefinitionProvider.Verify(q => q.GetDefinition("something"));
        }


        [Fact]
        public void GetDefinition_DoesNotGetQuickSearchFilterDefinitionFromQuickSearchDefinitionProvider_WhenQuickSearchStringIsEmpty()
        {
            Mock<ISearchDefinitionProvider> quickFilterDefinitionProvider = mock.Mock<ISearchDefinitionProvider>();
            AdvancedSearchDefinitionProvider testObject = GetDefaultAdvancedSearchProvider(quickFilterDefinitionProvider);
            testObject.GetDefinition(string.Empty);

            quickFilterDefinitionProvider.Verify(q => q.GetDefinition(string.Empty), Times.Never);
        }

        [Fact]
        public void GetDefinition_HasSearch_AsItsFilterDefinitionSource()
        {
            Mock<ISearchDefinitionProvider> quickFilterDefinitionProvider = mock.Mock<ISearchDefinitionProvider>();
            AdvancedSearchDefinitionProvider testObject = GetDefaultAdvancedSearchProvider(quickFilterDefinitionProvider);

            Assert.Equal(FilterDefinitionSourceType.Search, testObject.GetDefinition("something").FilterDefinitionSource);
        }

        /// <summary>
        /// Get a default advanced search definition provider to use with testing.
        /// </summary>
        private AdvancedSearchDefinitionProvider GetDefaultAdvancedSearchProvider(Mock<ISearchDefinitionProvider> quickFilterDefinitionProvider)
        {
            quickFilterDefinitionProvider.Setup(q => q.GetDefinition(It.IsAny<string>()))
                .Returns(new FilterDefinition(FilterTarget.Orders));

            FilterDefinition advancedDefinitions = new FilterDefinition(FilterTarget.Orders)
            {
                RootContainer = new ConditionGroupContainer(new ConditionGroup())
            };

            advancedDefinitions.RootContainer.FirstGroup.Conditions.Add(new OrderNumberCondition
            {
                IsNumeric = false,
                StringOperator = StringOperator.BeginsWith,
                StringValue = "something"
            });

            return mock.Create<AdvancedSearchDefinitionProvider>(new TypedParameter(typeof(FilterDefinition), advancedDefinitions));
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}