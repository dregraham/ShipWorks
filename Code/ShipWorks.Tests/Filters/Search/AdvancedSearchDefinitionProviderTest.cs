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
            Mock<IFilterDefinitionProvider> quickFilterDefinitionProvider = mock.Mock<IFilterDefinitionProvider>();
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

            AdvancedSearchDefinitionProvider testObject = mock.Create<AdvancedSearchDefinitionProvider>(new TypedParameter(typeof(FilterDefinition), advancedDefinitions));
            testObject.GetDefinition("something");

            quickFilterDefinitionProvider.Verify(q => q.GetDefinition("something"));
        }


        [Fact]
        public void GetDefinition_DoesNotGetQuickSearchFilterDefinitionFromQuickSearchDefinitionProvider_WhenQuickSearchStringIsEmpty()
        {
            Mock<IFilterDefinitionProvider> quickFilterDefinitionProvider = mock.Mock<IFilterDefinitionProvider>();
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

            AdvancedSearchDefinitionProvider testObject = mock.Create<AdvancedSearchDefinitionProvider>(new TypedParameter(typeof(FilterDefinition), advancedDefinitions));
            testObject.GetDefinition(string.Empty);

            quickFilterDefinitionProvider.Verify(q => q.GetDefinition(string.Empty), Times.Never);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}