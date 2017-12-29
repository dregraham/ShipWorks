using System;
using System.Threading.Tasks;
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
        public async Task Verify_ReturnsSuccess_WhenApiHasNotChangedAndStrategyReturnsFalse()
        {
            var testObject = mock.Create<BigCommerceConnectionVerifier>();

            var result = await testObject.Verify(new BigCommerceStoreEntity(), mock.Create<IBigCommerceAuthenticationPersistenceStrategy>());

            Assert.True(result.Success);
        }

        [Fact]
        public async Task Verify_DoesNotVerifyConnection_WhenApiHasNotChangedAndStrategyReturnsFalse()
        {
            var testObject = mock.Create<BigCommerceConnectionVerifier>();

            await testObject.Verify(new BigCommerceStoreEntity(), mock.Create<IBigCommerceAuthenticationPersistenceStrategy>());

            mock.Mock<IBigCommerceWebClientFactory>()
                .Verify(x => x.Create(It.IsAny<IBigCommerceStoreEntity>()), Times.Never);
        }

        [Fact]
        public async Task Verify_VerifiesConnection_WhenOnlyApiHasChanged()
        {
            var testObject = mock.Create<BigCommerceConnectionVerifier>();

            await testObject.Verify(new BigCommerceStoreEntity { ApiUrl = "foo" }, mock.Create<IBigCommerceAuthenticationPersistenceStrategy>());

            mock.Mock<IBigCommerceWebClientFactory>()
                .Verify(x => x.Create(It.IsAny<IBigCommerceStoreEntity>()));
        }

        [Fact]
        public async Task Verify_VerifiesConnection_WhenStrategyHasChanged()
        {
            var strategy = mock.Mock<IBigCommerceAuthenticationPersistenceStrategy>();
            strategy.Setup(x => x.ConnectionVerificationNeeded(It.IsAny<BigCommerceStoreEntity>()))
                .Returns(true);

            var testObject = mock.Create<BigCommerceConnectionVerifier>();

            await testObject.Verify(new BigCommerceStoreEntity(), strategy.Object);

            mock.Mock<IBigCommerceWebClientFactory>()
                .Verify(x => x.Create(It.IsAny<IBigCommerceStoreEntity>()));
        }

        [Fact]
        public async Task Verify_ReturnsSuccess_WhenConnectionTestSucceeds()
        {
            var testObject = mock.Create<BigCommerceConnectionVerifier>();

            var result = await testObject.Verify(new BigCommerceStoreEntity { ApiUrl = "foo" }, mock.Create<IBigCommerceAuthenticationPersistenceStrategy>());

            Assert.True(result.Success);
        }

        [Fact]
        public async Task Verify_ReturnsFailure_WhenConnectionTestFails()
        {
            var store = new BigCommerceStoreEntity { ApiUrl = "foo" };

            var webClient = mock.Mock<IBigCommerceWebClient>();
            webClient.Setup(x => x.TestConnection()).Throws<BigCommerceException>();

            mock.Mock<IBigCommerceWebClientFactory>()
                .Setup(x => x.Create(store))
                .Returns(webClient);

            var testObject = mock.Create<BigCommerceConnectionVerifier>();

            var result = await testObject.Verify(store, mock.Create<IBigCommerceAuthenticationPersistenceStrategy>());

            Assert.True(result.Failure);
        }

        [Fact]
        public async Task Verify_UpdatesFromOnline_WhenConnectionTestSucceeds()
        {
            mock.Override<IBigCommerceStatusCodeProvider>().Setup(x => x.UpdateFromOnlineStore()).Verifiable();

            var testObject = mock.Create<BigCommerceConnectionVerifier>();

            await testObject.Verify(new BigCommerceStoreEntity { ApiUrl = "foo" }, mock.Create<IBigCommerceAuthenticationPersistenceStrategy>());

            mock.VerifyAll = true;
        }

        [Fact]
        public async Task Verify_ReturnsFailure_WhenOnlineUpdateFails()
        {
            mock.Override<IBigCommerceStatusCodeProvider>()
                .Setup(x => x.UpdateFromOnlineStore())
                .Throws<BigCommerceException>();

            var testObject = mock.Create<BigCommerceConnectionVerifier>();

            var result = await testObject.Verify(new BigCommerceStoreEntity { ApiUrl = "foo" }, mock.Create<IBigCommerceAuthenticationPersistenceStrategy>());

            Assert.True(result.Failure);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
