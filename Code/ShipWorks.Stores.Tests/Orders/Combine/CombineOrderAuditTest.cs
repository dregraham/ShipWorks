using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Orders.Combine;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.Stores.Platforms.Ebay;
using ShipWorks.Tests.Shared;
using ShipWorks.Users.Audit;
using Xunit;

namespace ShipWorks.Stores.Tests.Orders.Combine
{
    public class CombineOrderAuditTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly Mock<IConfigurationEntity> configuration;
        private CombineOrderAudit testObject;

        public CombineOrderAuditTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            configuration = mock.Mock<IConfigurationEntity>();
            configuration.Setup(c => c.AuditEnabled).Returns(true);
            mock.Mock<IConfigurationData>().Setup(c => c.FetchReadOnly()).Returns(configuration);

            testObject = mock.Create<CombineOrderAudit>();
        }

        [Fact]
        public async Task Audit_WithEbayOrders_UseProperValues()
        {
            var orders = new[]
            {
                new EbayOrderEntity { SellingManagerRecord = 1234 },
                new EbayOrderEntity { EbayOrderID = 6 },
                new EbayOrderEntity(),
            };

            mock.Mock<IStoreTypeManager>()
                .Setup(x => x.GetType(It.IsAny<long>()))
                .Returns(mock.Create<EbayStoreType>(TypedParameter.From<StoreEntity>(null)));

            await testObject.Audit(1006, orders);

            mock.Mock<IAuditUtility>()
                .Verify(x => x.AuditAsync(1006,
                    AuditActionType.CombineOrder,
                    It.Is<AuditReason>(a => a.ReasonDetail == "Combined from orders : 1234, 6, 0"),
                    It.IsAny<ISqlAdapter>()));
        }

        [Fact]
        public async Task Audit_WithAmazonOrders_UseProperValues()
        {
            var orders = new[]
            {
                new AmazonOrderEntity { AmazonOrderID = "1234" },
                new AmazonOrderEntity { AmazonOrderID = "5678" }
            };

            mock.Mock<IStoreTypeManager>()
                .Setup(x => x.GetType(It.IsAny<long>()))
                .Returns(mock.Create<AmazonStoreType>(TypedParameter.From<StoreEntity>(null)));

            await testObject.Audit(1006, orders);

            mock.Mock<IAuditUtility>()
                .Verify(x => x.AuditAsync(1006,
                    AuditActionType.CombineOrder,
                    It.Is<AuditReason>(a => a.ReasonDetail == "Combined from orders : 1234, 5678"),
                    It.IsAny<ISqlAdapter>()));
        }

        [Fact]
        public async Task Audit_DoesNotAudit_WhenAuditingIsDisabled()
        {
            var orders = new[]
            {
                new AmazonOrderEntity { AmazonOrderID = "1234" },
                new AmazonOrderEntity { AmazonOrderID = "5678" }
            };

            mock.Mock<IStoreTypeManager>()
                .Setup(x => x.GetType(It.IsAny<long>()))
                .Returns(mock.Create<AmazonStoreType>(TypedParameter.From<StoreEntity>(null)));

            configuration.Setup(c => c.AuditEnabled).Returns(false);
            mock.Mock<IConfigurationData>().Setup(c => c.FetchReadOnly()).Returns(configuration);

            testObject = mock.Create<CombineOrderAudit>();

            await testObject.Audit(1006, orders);

            mock.Mock<IAuditUtility>()
                .Verify(x => x.AuditAsync(1006,
                    AuditActionType.CombineOrder,
                    It.Is<AuditReason>(a => a.ReasonDetail == "Combined from orders : 1234, 5678"),
                    It.IsAny<ISqlAdapter>()), Times.Never);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
