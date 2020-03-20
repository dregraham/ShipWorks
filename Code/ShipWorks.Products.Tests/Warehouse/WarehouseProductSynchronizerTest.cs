using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Connection;
using ShipWorks.Products.Warehouse;
using ShipWorks.Products.Warehouse.DTO;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Products.Tests.Warehouse
{
    public class WarehouseProductSynchronizerTest
    {
        private readonly AutoMock mock;
        private readonly WarehouseProductSynchronizer testObject;
        private readonly Mock<IGetProductsAfterSequenceResult> result;

        public WarehouseProductSynchronizerTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            mock.Mock<ILicenseService>()
                .SetupGet(x => x.IsHub)
                .Returns(true);
            result = mock.Mock<IGetProductsAfterSequenceResult>();
            result.Setup(x => x.Apply(It.IsAny<CancellationToken>()))
            .ReturnsAsync((0L, false));
            mock.Mock<IWarehouseProductClient>()
                .Setup(x => x.GetProductsAfterSequence(AnyLong, It.IsAny<CancellationToken>()))
                .ReturnsAsync(result.Object);

            testObject = mock.Create<WarehouseProductSynchronizer>();
        }

        [Fact]
        public async Task Synchronize_DoesNotGetProductSequence_WhenIsNotHub()
        {
            mock.Mock<ILicenseService>()
                .SetupGet(x => x.IsHub)
                .Returns(false);

            await testObject.Synchronize(new CancellationToken());

            mock.Mock<IProductCatalog>()
                .Verify(x => x.FetchNewestSequence(It.IsAny<ISqlAdapter>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Synchronize_GetsProductSequence_WhenIsHub()
        {
            await testObject.Synchronize(new CancellationToken());

            mock.Mock<IProductCatalog>()
                .Verify(x => x.FetchNewestSequence(It.IsAny<ISqlAdapter>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task Synchronize_DelegatesToTheHub_WithSequence()
        {
            mock.Mock<IProductCatalog>()
                .Setup(x => x.FetchNewestSequence(It.IsAny<ISqlAdapter>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(6);
            var token = new CancellationToken();

            await testObject.Synchronize(token);

            mock.Mock<IWarehouseProductClient>()
                .Verify(x => x.GetProductsAfterSequence(6, token));
        }

        [Fact]
        public async Task Synchronize_KeepsMakingRequests_UntilResultReturnsFalse()
        {
            mock.Mock<IProductCatalog>()
                .Setup(x => x.FetchNewestSequence(It.IsAny<ISqlAdapter>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(6);
            var token = new CancellationToken();
            mock.FromFactory<IWarehouseProductClient>()
                .MockAsync(x => x.GetProductsAfterSequence(6, token))
                .Setup(x => x.Apply(token))
                .ReturnsAsync((10, true)); 
            mock.FromFactory<IWarehouseProductClient>()
                 .MockAsync(x => x.GetProductsAfterSequence(10, token))
                 .Setup(x => x.Apply(token))
                 .ReturnsAsync((15, true));
            mock.FromFactory<IWarehouseProductClient>()
                 .MockAsync(x => x.GetProductsAfterSequence(15, token))
                 .Setup(x => x.Apply(token))
                 .ReturnsAsync((15, false));

            await testObject.Synchronize(token);

            mock.Mock<IWarehouseProductClient>()
                .Verify(x => x.GetProductsAfterSequence(AnyLong, token), Times.Exactly(3));
        }
    }
}
