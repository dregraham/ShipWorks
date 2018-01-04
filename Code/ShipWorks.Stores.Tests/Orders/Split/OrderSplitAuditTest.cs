using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Orders.Split;
using ShipWorks.Stores.Platforms.Ebay;
using ShipWorks.Tests.Shared;
using ShipWorks.Users.Audit;
using Xunit;

namespace ShipWorks.Stores.Tests.Orders.Split
{
    public class OrderSplitAuditTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly OrderSplitAudit testObject;

        public OrderSplitAuditTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<OrderSplitAudit>();
        }

        [Fact]
        public async Task Audit_WithEbayOrders_UseProperValues()
        {
            EbayOrderEntity originalOrder = new EbayOrderEntity { OrderID = 1006, OrderNumber = 1234, EbayOrderID = 60, SellingManagerRecord = 1234};
            EbayOrderEntity splitOrder = new EbayOrderEntity { OrderID = 2006, OrderNumber = 5678, EbayOrderID = 6};

            mock.Mock<IStoreTypeManager>()
                .Setup(x => x.GetType(It.IsAny<long>()))
                .Returns(mock.Create<EbayStoreType>(TypedParameter.From<StoreEntity>(null)));

            await testObject.Audit(originalOrder, splitOrder);

            mock.Mock<IAuditUtility>()
                .Verify(x => x.AuditAsync(originalOrder.OrderID,
                    AuditActionType.SplitOrder,
                    It.Is<AuditReason>(a => a.ReasonDetail == $"Split to order : {splitOrder.EbayOrderID}"),
                    It.IsAny<ISqlAdapter>()));

            mock.Mock<IAuditUtility>()
                .Verify(x => x.AuditAsync(splitOrder.OrderID,
                    AuditActionType.SplitOrder,
                    It.Is<AuditReason>(a => a.ReasonDetail == $"Split from order : {originalOrder.OrderNumber}"),
                    It.IsAny<ISqlAdapter>()));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
