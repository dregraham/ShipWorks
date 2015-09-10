﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.Business;
using Moq;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using Xunit;
using ShipWorks.Shipping;
using ShipWorks.Users.Security;

namespace ShipWorks.Tests.Shipping
{
    public class ShipmentOrderLoaderTest
    {
        private ShipmentLoader testObject;
        private OrderEntity orderEntity;
        private ShipmentEntity shipmentEntity;

        private Mock<IShippingPanelConfigurator> shippingPanelConfigurator;
        private Mock<IShippingManager> shippingManager;
        private Mock<IFilterHelper> filterHelper;

        public ShipmentOrderLoaderTest()
        {
            orderEntity = new OrderEntity(1006);
            shipmentEntity = new ShipmentEntity(1031);
            shipmentEntity.Order = orderEntity;

            shippingPanelConfigurator = new Mock<IShippingPanelConfigurator>();
            shippingPanelConfigurator.Setup(s => s.AutoCreateShipments).Returns(true);
            shippingPanelConfigurator.Setup(s => s.UserHasPermission(It.IsAny<PermissionType>(), It.IsAny<long>())).Returns(true);
            shippingPanelConfigurator.Setup(s => s.GetAddressValidation(It.IsAny<ShipmentEntity>())).Returns(true);

            shippingManager = new Mock<IShippingManager>();
            shippingManager.Setup(s => s.GetShipments(It.IsAny<long>(), It.IsAny<bool>())).Returns(new List<ShipmentEntity>() { shipmentEntity });

            filterHelper = new Mock<IFilterHelper>();
            filterHelper.Setup(s => s.EnsureFiltersUpToDate(It.IsAny<TimeSpan>())).Returns(true);

            testObject = new ShipmentLoader(shippingPanelConfigurator.Object, shippingManager.Object, filterHelper.Object);
        }

        [Fact]
        public async void ShipmentAndSuccess_WhenOrderHasOneShipment_ReturnsThatShipment_Test()
        {
            ShippingPanelLoadedShipment shipmentPanelLoadedShipment = await testObject.LoadAsync(orderEntity);

            Assert.Equal(shipmentEntity.ShipmentID, shipmentPanelLoadedShipment.Shipment.ShipmentID);
            Assert.Equal(ShippingPanelLoadedShipmentResult.Success, shipmentPanelLoadedShipment.Result);
        }
        
    }
}
