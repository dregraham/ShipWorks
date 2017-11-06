using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Ship.Manipulators.Request.International
{
    public class FedExTrafficInArmsManipulatorTest
    {
        private FedExTrafficInArmsManipulator testObject;
        private ProcessShipmentRequest processShipmentRequest;
        private ShipmentEntity shipment;

        public FedExTrafficInArmsManipulatorTest()
        {
            shipment = new ShipmentEntity
            {
                FedEx = new FedExShipmentEntity()
            };

            processShipmentRequest = new ProcessShipmentRequest()
            {
                RequestedShipment = new RequestedShipment()
                {
                    SpecialServicesRequested = new ShipmentSpecialServicesRequested() { SpecialServiceTypes = new ShipmentSpecialServiceType[0] }
                }
            };

            testObject = new FedExTrafficInArmsManipulator();
        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(null, new ProcessShipmentRequest(), 0));
        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenProcessShipmentRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(new ShipmentEntity(), null, 0));
        }

        [Theory]
        [InlineData(null, null, false)]
        [InlineData(true, null, true)]
        [InlineData(true, "asdf", true)]
        [InlineData(null, null, false)]
        [InlineData(false, null, false)]
        [InlineData(false, "asdf", true)]
        public void ShouldApply_ReturnsCorrectValue(bool? serviceEnabled, string licenseNumber, bool expectedResult)
        {
            shipment.FedEx.InternationalTrafficInArmsService = serviceEnabled;
            shipment.FedEx.TrafficInArmsLicenseNumber = licenseNumber;

            Assert.Equal(expectedResult, testObject.ShouldApply(shipment, 0));
        }

        [Fact]
        public void Manipulate_AccountsForNullRequestedShipment()
        {
            processShipmentRequest.RequestedShipment = null;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.NotNull(processShipmentRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_AccountsForNullSpecialServicesRequested_WhenLicencseNumberIsProvided()
        {
            shipment.FedEx.TrafficInArmsLicenseNumber = "123456";
            shipment.FedEx.InternationalTrafficInArmsService = true;

            processShipmentRequest.RequestedShipment.SpecialServicesRequested = null;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.NotNull(processShipmentRequest.RequestedShipment.SpecialServicesRequested);
        }

        [Fact]
        public void Manipulate_AccountsForNullSpecialServiceTypes_WhenLicencseNumberIsProvided()
        {
            shipment.FedEx.TrafficInArmsLicenseNumber = "123456";
            shipment.FedEx.InternationalTrafficInArmsService = true;

            processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = null;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.NotNull(processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);
        }

        [Fact]
        public void Manipulate_AccountsForEmptySpecialServiceTypes_WhenLicencseNumberIsProvided()
        {
            shipment.FedEx.TrafficInArmsLicenseNumber = "123456";
            shipment.FedEx.InternationalTrafficInArmsService = true;

            processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = new ShipmentSpecialServiceType[0];

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(1, processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Length);
        }

        [Fact]
        public void Manipulate_OptionIsNotAddedToSpecialServiceTypes_WhenLicencseNumberIsEmptyString()
        {
            shipment.FedEx.TrafficInArmsLicenseNumber = string.Empty;

            processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = new ShipmentSpecialServiceType[0];

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(0, processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Length);
        }

        [Fact]
        public void Manipulate_OptionIsNotAddedToSpecialServiceTypes_WhenLicencseNumberIsNull()
        {
            shipment.FedEx.TrafficInArmsLicenseNumber = null;

            processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = new ShipmentSpecialServiceType[0];

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(0, processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Length);
        }

        [Fact]
        public void Manipulate_ArmsDetailIsNotNull_WhenLicencseNumberIsProvided()
        {
            shipment.FedEx.TrafficInArmsLicenseNumber = "123456";

            processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = new ShipmentSpecialServiceType[0];

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.NotNull(processShipmentRequest.RequestedShipment.SpecialServicesRequested.InternationalTrafficInArmsRegulationsDetail);
        }

        [Fact]
        public void Manipulate_LicenseNumberIsAssigned_WhenLicencseNumberIsProvided()
        {
            shipment.FedEx.TrafficInArmsLicenseNumber = "123456";

            processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = new ShipmentSpecialServiceType[0];

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal("123456", processShipmentRequest.RequestedShipment.SpecialServicesRequested.InternationalTrafficInArmsRegulationsDetail.LicenseOrExemptionNumber);
        }
    }
}
