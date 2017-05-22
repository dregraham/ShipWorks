using System;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.BigCommerce;
using ShipWorks.Stores.Platforms.BigCommerce.AccountSettings;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.BigCommerce.AccountSettings
{
    public class BigCommerceConnectionVerifierTest : IDisposable
    {
        readonly AutoMock mock;

        public BigCommerceConnectionVerifierTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Verify_ReturnsSuccess_WhenApiHasNotChangedAndStrategyReturnsFalse()
        {
            var testObject = mock.Create<BigCommerceConnectionVerifier>();

            var result = testObject.Verify(new BigCommerceStoreEntity(), mock.Create<IBigCommerceAuthenticationPersistenceStrategy>());

            Assert.True(result.Success);
        }

        [Fact]
        public void Verify_DoesNotVerifyConnection_WhenApiHasNotChangedAndStrategyReturnsFalse()
        {
            var testObject = mock.Create<BigCommerceConnectionVerifier>();

            testObject.Verify(new BigCommerceStoreEntity(), mock.Create<IBigCommerceAuthenticationPersistenceStrategy>());

            mock.Mock<IBigCommerceWebClientFactory>()
                .Verify(x => x.Create(It.IsAny<IBigCommerceStoreEntity>()), Times.Never);
        }

        [Fact]
        public void Verify_VerifiesConnection_WhenOnlyApiHasChanged()
        {
            var testObject = mock.Create<BigCommerceConnectionVerifier>();

            testObject.Verify(new BigCommerceStoreEntity { ApiUrl = "foo" }, mock.Create<IBigCommerceAuthenticationPersistenceStrategy>());

            mock.Mock<IBigCommerceWebClientFactory>()
                .Verify(x => x.Create(It.IsAny<IBigCommerceStoreEntity>()));
        }

        [Fact]
        public void Verify_VerifiesConnection_WhenStrategyHasChanged()
        {
            var strategy = mock.Mock<IBigCommerceAuthenticationPersistenceStrategy>();
            strategy.Setup(x => x.ConnectionVerificationNeeded(It.IsAny<BigCommerceStoreEntity>()))
                .Returns(true);

            var testObject = mock.Create<BigCommerceConnectionVerifier>();

            testObject.Verify(new BigCommerceStoreEntity(), strategy.Object);

            mock.Mock<IBigCommerceWebClientFactory>()
                .Verify(x => x.Create(It.IsAny<IBigCommerceStoreEntity>()));
        }

        [Fact]
        public void Verify_ReturnsSuccess_WhenConnectionTestSucceeds()
        {
            var testObject = mock.Create<BigCommerceConnectionVerifier>();

            var result = testObject.Verify(new BigCommerceStoreEntity { ApiUrl = "foo" }, mock.Create<IBigCommerceAuthenticationPersistenceStrategy>());

            Assert.True(result.Success);
        }

        [Fact]
        public void Verify_ReturnsFailure_WhenConnectionTestFails()
        {
            var store = new BigCommerceStoreEntity { ApiUrl = "foo" };

            var webClient = mock.Mock<IBigCommerceWebClient>();
            webClient.Setup(x => x.TestConnection()).Throws<BigCommerceException>();

            mock.Mock<IBigCommerceWebClientFactory>()
                .Setup(x => x.Create(store))
                .Returns(webClient);

            var testObject = mock.Create<BigCommerceConnectionVerifier>();

            var result = testObject.Verify(store, mock.Create<IBigCommerceAuthenticationPersistenceStrategy>());

            Assert.True(result.Failure);
        }

        [Fact]
        public void Verify_UpdatesFromOnline_WhenConnectionTestSucceeds()
        {
            mock.Override<IBigCommerceStatusCodeProvider>().Setup(x => x.UpdateFromOnlineStore()).Verifiable();

            var testObject = mock.Create<BigCommerceConnectionVerifier>();

            testObject.Verify(new BigCommerceStoreEntity { ApiUrl = "foo" }, mock.Create<IBigCommerceAuthenticationPersistenceStrategy>());

            mock.VerifyAll = true;
        }

        [Fact]
        public void Verify_ReturnsFailure_WhenOnlineUpdateFails()
        {
            mock.Override<IBigCommerceStatusCodeProvider>()
                .Setup(x => x.UpdateFromOnlineStore())
                .Throws<BigCommerceException>();

            var testObject = mock.Create<BigCommerceConnectionVerifier>();

            var result = testObject.Verify(new BigCommerceStoreEntity { ApiUrl = "foo" }, mock.Create<IBigCommerceAuthenticationPersistenceStrategy>());

            Assert.True(result.Failure);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
