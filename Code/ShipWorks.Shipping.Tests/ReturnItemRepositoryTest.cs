using System;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests
{
    public class ReturnItemRepositoryTest : IDisposable
    {
        private readonly AutoMock mock;

        public ReturnItemRepositoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void LoadReturnData_DoesNotHitDatabase_IfShipmentAlreadyHasReturnItems()
        {
            var shipment = Create.Shipment().WithReturnItem().Build();
            var testObject = mock.Create<ReturnItemRepository>();

            testObject.LoadReturnData(shipment, true);

            mock.Mock<ISqlAdapterFactory>().Verify(f => f.Create(), Times.Never);
        }

        [Fact]
        public void LoadReturnData_DosNotCreateInitialShipmentReturnItem_WhenPassedFalseForCreateIfNone()
        {
            var shipment = Create.Shipment().WithReturnItem().Build();
            var testObject = mock.Create<ReturnItemRepository>();

            var sqlAdapter = mock.FromFactory<ISqlAdapterFactory>().Mock(x => x.Create());
            sqlAdapter.Setup(s => s.FetchQueryAsync(It.IsAny<EntityQuery<ShipmentReturnItemEntity>>()))
                .ReturnsAsync(mock.CreateMock<IEntityCollection2>().Object);

            testObject.LoadReturnData(shipment, false);

            sqlAdapter.Verify(a => a.FetchQuery(It.IsAny<DynamicQuery<ShipmentReturnItemEntity>>()), Times.Never);
        }

        [Fact]
        public void LoadReturnData_CreatesInitialShipmentReturnItem_WhenPassedTrueForCreateIfNone()
        {
            var shipment = Create.Shipment().Build();
            var testObject = mock.Create<ReturnItemRepository>();

            var sqlAdapter = mock.FromFactory<ISqlAdapterFactory>().Mock(x => x.Create());
            sqlAdapter.Setup(s => s.FetchQueryAsync(It.IsAny<EntityQuery<ShipmentReturnItemEntity>>()))
                .ReturnsAsync(mock.CreateMock<IEntityCollection2>().Object);

            testObject.LoadReturnData(shipment, true);

            sqlAdapter.Verify(a => a.FetchQuery(It.IsAny<DynamicQuery<ShipmentReturnItemEntity>>()), Times.Once);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}