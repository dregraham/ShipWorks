using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content;
using ShipWorks.Tests.Shared;
using ShipWorks.Users.Security;
using Xunit;

namespace ShipWorks.Tests.Stores.Content.Combine
{
    public class OrderCombineValidatorTest : IDisposable
    {
        private AutoMock mock;
        private OrderCombineValidator testObject;

        public OrderCombineValidatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<OrderCombineValidator>();

            mock.Mock<ISecurityContext>()
                .Setup(x => x.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>()))
                .Returns(true);

            mock.Mock<ICombineOrdersGateway>()
                .Setup(x => x.CanCombine(It.IsAny<IStoreEntity>(), It.IsAny<IEnumerable<long>>()))
                .Returns(true);
        }

        [Fact]
        public void Validate_ReturnSuccess_WhenOrderCountIsGreaterThanOne()
        {
            var result = testObject.Validate(new long[] { 1, 2 });
            Assert.True(result.Success);
        }

        [Fact]
        public void Validate_ReturnSuccess_WhenOrderCountNone()
        {
            var result = testObject.Validate(new long[] { });
            Assert.True(result.Failure);
        }

        [Fact]
        public void Validate_ReturnFailure_WhenOrderCountIsLessThanTwo()
        {
            var result = testObject.Validate(new long[] { 1 });
            Assert.False(result.Success);
        }

        [Fact]
        public void Validate_ReturnFailure_WhenUserDoesNotHavePermission()
        {
            mock.Mock<ISecurityContext>()
                .Setup(x => x.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>()))
                .Returns(false);

            var testObject = mock.Create<OrderCombineValidator>();
            var result = testObject.Validate(new long[] { 1, 2 });

            Assert.True(result.Failure);
        }

        [Fact]
        public void Validate_ReturnFalse_WhenOrderCanNotBeCombined()
        {
            mock.Mock<ICombineOrdersGateway>()
                .Setup(x => x.CanCombine(It.IsAny<IStoreEntity>(), It.IsAny<IEnumerable<long>>()))
                .Returns(false);

            var testObject = mock.Create<OrderCombineValidator>();
            var result = testObject.Validate(new long[] { 1, 2 });

            Assert.False(result.Success);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
