using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.Stores.Platforms.Ebay;
using ShipWorks.Tests.Shared;
using ShipWorks.Users.Audit;
using Xunit;

namespace ShipWorks.Stores.Tests.Content
{
    public class CombineOrderAuditTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly CombineOrderAudit testObject;

        public CombineOrderAuditTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
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

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
