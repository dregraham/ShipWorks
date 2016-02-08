using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators.International;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators.International
{
    public class FedExTrafficInArmsManipulatorTest
    {
        private FedExTrafficInArmsManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private ProcessShipmentRequest nativeRequest;
        private ShipmentEntity shipmentEntity;

        public FedExTrafficInArmsManipulatorTest()
        {
            shipmentEntity = new ShipmentEntity
            {
                FedEx = new FedExShipmentEntity()
            };

            nativeRequest = new ProcessShipmentRequest()
            {
                RequestedShipment = new RequestedShipment()
                {
                    SpecialServicesRequested = new ShipmentSpecialServicesRequested() { SpecialServiceTypes = new ShipmentSpecialServiceType[0] }
                }
            };

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            testObject = new FedExTrafficInArmsManipulator();
        }


        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(null));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull()
        {
            // Setup the native request to be null
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, null);

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotProcessShipmentRequest()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new object());

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_AccountsForNullRequestedShipment()
        {
            // setup the test by setting the requested shipment to null
            nativeRequest.RequestedShipment = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.NotNull(nativeRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_AccountsForNullSpecialServicesRequested_WhenLicencseNumberIsProvided()
        {
            shipmentEntity.FedEx.TrafficInArmsLicenseNumber = "123456";

            nativeRequest.RequestedShipment.SpecialServicesRequested = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.NotNull(nativeRequest.RequestedShipment.SpecialServicesRequested);
        }

        [Fact]
        public void Manipulate_AccountsForNullSpecialServiceTypes_WhenLicencseNumberIsProvided()
        {
            shipmentEntity.FedEx.TrafficInArmsLicenseNumber = "123456";

            nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.NotNull(nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);
        }

        [Fact]
        public void Manipulate_AccountsForEmptySpecialServiceTypes_WhenLicencseNumberIsProvided()
        {
            shipmentEntity.FedEx.TrafficInArmsLicenseNumber = "123456";

            nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = new ShipmentSpecialServiceType[0];
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(1, nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Length);
        }

        [Fact]
        public void Manipulate_OptionIsNotAddedToSpecialServiceTypes_WhenLicencseNumberIsEmptyString()
        {
            shipmentEntity.FedEx.TrafficInArmsLicenseNumber = string.Empty;

            nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = new ShipmentSpecialServiceType[0];
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(0, nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Length);
        }

        [Fact]
        public void Manipulate_OptionIsNotAddedToSpecialServiceTypes_WhenLicencseNumberIsNull()
        {
            shipmentEntity.FedEx.TrafficInArmsLicenseNumber = null;

            nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = new ShipmentSpecialServiceType[0];
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(0, nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Length);
        }

        [Fact]
        public void Manipulate_ArmsDetailIsNotNull_WhenLicencseNumberIsProvided()
        {
            shipmentEntity.FedEx.TrafficInArmsLicenseNumber = "123456";

            nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = new ShipmentSpecialServiceType[0];
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.NotNull(nativeRequest.RequestedShipment.SpecialServicesRequested.InternationalTrafficInArmsRegulationsDetail);
        }

        [Fact]
        public void Manipulate_LicenseNumberIsAssigned_WhenLicencseNumberIsProvided()
        {
            shipmentEntity.FedEx.TrafficInArmsLicenseNumber = "123456";

            nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = new ShipmentSpecialServiceType[0];
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal("123456", nativeRequest.RequestedShipment.SpecialServicesRequested.InternationalTrafficInArmsRegulationsDetail.LicenseOrExemptionNumber);
        }
    }
}
