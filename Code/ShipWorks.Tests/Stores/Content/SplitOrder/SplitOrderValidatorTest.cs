using System;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Orders.Split;
using ShipWorks.Tests.Shared;
using ShipWorks.Users.Security;
using Xunit;

namespace ShipWorks.Tests.Stores.Content.SplitOrder
{
    public class SplitOrderValidatorTest : IDisposable
    {
        private readonly AutoMock mock;

        public SplitOrderValidatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            mock.Mock<ISecurityContext>()
                .Setup(x => x.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>()))
                .Returns(true);

            mock.Mock<IOrderSplitGateway>()
                .Setup(x => x.CanSplit(It.IsAny<long>()))
                .Returns(true);
        }

        [Fact]
        public void Validate_ReturnSuccess_WhenOrderCountIsOne()
        {
            var testObject = mock.Create<OrderSplitValidator>();
            var result = testObject.Validate(new long[] { 1006 });
            Assert.True(result.Success);
        }

        [Fact]
        public void Validate_ReturnFailure_WhenOrderCountNone()
        {
            var testObject = mock.Create<OrderSplitValidator>();
            var result = testObject.Validate(new long[] { });
            Assert.False(result.Success);
        }

        [Fact]
        public void Validate_ReturnFailure_WhenOrderCountIsGreaterThanOne()
        {
            var testObject = mock.Create<OrderSplitValidator>();
            var result = testObject.Validate(new long[] { 1006, 206 });
            Assert.False(result.Success);
        }

        [Fact]
        public void Validate_ReturnFailure_WhenUserDoesNotHavePermission()
        {
            mock.Mock<ISecurityContext>()
                .Setup(x => x.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>()))
                .Returns(false);

            var testObject = mock.Create<OrderSplitValidator>();
            var result = testObject.Validate(new long[] { 1006 });

            Assert.True(result.Failure);
        }

        [Fact]
        public void Validate_ReturnFalse_WhenOrderCanNotBeSplit()
        {
            mock.Mock<IOrderSplitGateway>()
                .Setup(x => x.CanSplit(It.IsAny<long>()))
                .Returns(false);

            var testObject = mock.Create<OrderSplitValidator>();
            var result = testObject.Validate(new long[] { 1006 });

            Assert.False(result.Success);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
