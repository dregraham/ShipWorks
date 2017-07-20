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

        public OrderCombineValidatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            mock.Mock<ISecurityContext>()
                .Setup(x => x.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>()))
                .Returns(true);

            mock.Mock<ICombineOrderGateway>()
                .Setup(x => x.CanCombine(It.IsAny<IStoreEntity>(), It.IsAny<IEnumerable<long>>()))
                .Returns(true);
        }

        [Fact]
        public void Validate_ReturnSuccess_WhenOrderCountIsGreaterThanOne()
        {
            var testObject = mock.Create<CombineOrderValidator>();
            var result = testObject.Validate(new long[] { 1006, 2006 });
            Assert.True(result.Success);
        }

        [Fact]
        public void Validate_ReturnSuccess_WhenOrderCountNone()
        {
            var testObject = mock.Create<CombineOrderValidator>();
            var result = testObject.Validate(new long[] { });
            Assert.True(result.Failure);
        }

        [Fact]
        public void Validate_ReturnFailure_WhenOrderCountIsLessThanTwo()
        {
            var testObject = mock.Create<CombineOrderValidator>();
            var result = testObject.Validate(new long[] { 1006 });
            Assert.False(result.Success);
        }

        [Fact]
        public void Validate_ReturnFailure_WhenUserDoesNotHavePermission()
        {
            mock.Mock<ISecurityContext>()
                .Setup(x => x.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>()))
                .Returns(false);

            var testObject = mock.Create<CombineOrderValidator>();
            var result = testObject.Validate(new long[] { 1006, 2006 });

            Assert.True(result.Failure);
        }

        [Fact]
        public void Validate_ReturnFalse_WhenOrderCanNotBeCombined()
        {
            mock.Mock<ICombineOrderGateway>()
                .Setup(x => x.CanCombine(It.IsAny<IStoreEntity>(), It.IsAny<IEnumerable<long>>()))
                .Returns(false);

            var testObject = mock.Create<CombineOrderValidator>();
            var result = testObject.Validate(new long[] { 1006, 2006 });

            Assert.False(result.Success);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
