using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Enums;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators
{
    public class FedExRatePickupManipulatorTest
    {
        private FedExRatePickupManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private RateRequest nativeRequest;
        private ShipmentEntity shipmentEntity;

        public FedExRatePickupManipulatorTest()
        {
            shipmentEntity = new ShipmentEntity();
            shipmentEntity.FedEx = new FedExShipmentEntity();
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.Box;
            shipmentEntity.ShipDate = DateTime.Now.AddDays(1);

            nativeRequest = new RateRequest();
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject = new FedExRatePickupManipulator();
        }

        [Fact]
        public void Manipulate_FedExPickupManipulator_ReturnsRequestedDropoffRequestCourrier_Test()
        {
            shipmentEntity.FedEx.DropoffType = (int)FedExDropoffType.RequestCourier;

            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a the same values back
            Assert.Equal(DropoffType.REQUEST_COURIER, nativeRequest.RequestedShipment.DropoffType);
        }

        [Fact]
        public void Manipulate_FedExPickupManipulator_ReturnsRequestedDropoffStation_Test()
        {
            shipmentEntity.FedEx.DropoffType = (int)FedExDropoffType.Station;

            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a the same values back
            Assert.Equal(DropoffType.STATION, nativeRequest.RequestedShipment.DropoffType);
        }
        
        [Fact]
        public void Manipulate_DropoffTypeSpecifiedIsTrue_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a the same values back
            Assert.True(nativeRequest.RequestedShipment.DropoffTypeSpecified);
        }
    }
}
