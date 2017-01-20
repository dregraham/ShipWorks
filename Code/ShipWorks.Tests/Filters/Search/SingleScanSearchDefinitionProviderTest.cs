using System;
using System.Linq;
using Autofac.Extras.Moq;
using ShipWorks.Filters;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.Conditions.Orders;
using ShipWorks.Filters.Search;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Filters.Search
{
    public class SingleScanSearchDefinitionProviderTest : IDisposable
    {
        private readonly SingleScanSearchDefinitionProvider testObject;
        readonly AutoMock mock;
        private string numericOrderNumber = "12345";
        private string stringOrderNumber = "X-12345-YYZ";

        public SingleScanSearchDefinitionProviderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<SingleScanSearchDefinitionProvider>();
        }

        [Fact]
        public void GetDefinition_ReturnsDefinitionWithOrdersFilterTarget()
        {
            var definition = testObject.GetDefinition(numericOrderNumber);

            Assert.Equal(FilterTarget.Orders, definition.FilterTarget);
        }

        [Fact]
        public void GetDefinition_ReturnsDefinitionWithSingleCondition()
        {
            var definition = testObject.GetDefinition(numericOrderNumber);

            Assert.Equal(1, definition.RootContainer.FirstGroup.Conditions.Count);
        }

        [Fact]
        public void GetDefinition_ReturnsDefinitionWithOrderNumberCondition()
        {
            var definition = testObject.GetDefinition(numericOrderNumber);

            Assert.IsAssignableFrom(typeof(OrderNumberCondition), definition.RootContainer.FirstGroup.Conditions.FirstOrDefault());
        }

        [Fact]
        public void GetDefinition_ReturnsDefinitionWithNumericOrderNumberCondition_WhenQuickSearchIsNumber()
        {
            var definition = testObject.GetDefinition(numericOrderNumber);

            OrderNumberCondition condition = (OrderNumberCondition) definition.RootContainer.FirstGroup.Conditions.FirstOrDefault();

            Assert.True(condition.IsNumeric);
        }

        [Fact]
        public void GetDefinition_ReturnsDefinitionWithStringOrderNumberCondition_WhenQuickSearchIsNotNumber()
        {
            var definition = testObject.GetDefinition(stringOrderNumber);

            OrderNumberCondition condition = (OrderNumberCondition)definition.RootContainer.FirstGroup.Conditions.FirstOrDefault();

            Assert.False(condition.IsNumeric);
        }

        [Fact]
        public void GetDefinition_ReturnsDefinitionWithNumericOperatorEqual_WhenQuickSearchIsNumber()
        {
            var definition = testObject.GetDefinition(numericOrderNumber);

            OrderNumberCondition condition = (OrderNumberCondition)definition.RootContainer.FirstGroup.Conditions.FirstOrDefault();

            Assert.Equal(NumericOperator.Equal , condition.Operator);
        }

        [Fact]
        public void GetDefinition_ReturnsDefinitionWithStringOperatorEquals_WhenQuickSearchIsNotNumber()
        {
            var definition = testObject.GetDefinition(stringOrderNumber);

            OrderNumberCondition condition = (OrderNumberCondition)definition.RootContainer.FirstGroup.Conditions.FirstOrDefault();

            Assert.Equal(StringOperator.Equals, condition.StringOperator);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}