using System;
using System.Linq;
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
    public class SingleScanSearchDefinitionProviderTest : IDisposable
    {
        private readonly SingleScanSearchDefinitionProvider testObject;
        readonly AutoMock mock;
        private readonly Mock<ISingleScanOrderPrefix> orderPrefix;
        private string numericOrderNumber = "12345";
        private string stringOrderNumber = "X-12345-YYZ";
        private string singleScanOrderNumber = "SWO1006";

        public SingleScanSearchDefinitionProviderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            orderPrefix = mock.Mock<ISingleScanOrderPrefix>();
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

        [Fact]
        public void GetDefinition_ReturnsOrderIDCondition_WhenQuickSearchIsSingleScan()
        {
            orderPrefix.Setup(o => o.Contains(It.Is<string>(s => s == singleScanOrderNumber))).Returns(true);
            orderPrefix.Setup(o => o.GetOrderID(It.Is<string>(s => s == singleScanOrderNumber))).Returns(1006);

            FilterDefinition definition = testObject.GetDefinition(singleScanOrderNumber);
            Condition condition = definition.RootContainer.FirstGroup.Conditions.FirstOrDefault();

            Assert.True(condition is OrderIDCondition);
        }

        [Fact]
        public void GetDefinition_DelegatesToSingleScanOrderPrefix_ToCheckForPrefix()
        {
            orderPrefix.Setup(o => o.Contains(It.Is<string>(s => s == singleScanOrderNumber))).Returns(true);
            orderPrefix.Setup(o => o.GetOrderID(It.Is<string>(s => s == singleScanOrderNumber))).Returns(1006);

            testObject.GetDefinition(singleScanOrderNumber);

            orderPrefix.Verify(o => o.Contains(It.Is<string>(s => s == singleScanOrderNumber)));
        }

        [Fact]
        public void GetDefinition_DelegatesToSingleScanOrderPrefix_ForOrderId()
        {
            orderPrefix.Setup(o => o.Contains(It.Is<string>(s => s == singleScanOrderNumber))).Returns(true);
            orderPrefix.Setup(o => o.GetOrderID(It.Is<string>(s => s == singleScanOrderNumber))).Returns(1006);

            testObject.GetDefinition(singleScanOrderNumber);

            orderPrefix.Verify(o => o.GetOrderID(It.Is<string>(s => s == singleScanOrderNumber)));
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}