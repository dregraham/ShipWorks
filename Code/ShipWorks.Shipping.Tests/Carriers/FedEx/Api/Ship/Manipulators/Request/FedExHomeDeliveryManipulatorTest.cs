using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    public class FedExHomeDeliveryManipulatorTest
    {
        private readonly FedExHomeDeliveryManipulator testObject;
        private readonly ShipmentEntity shipment;

        public FedExHomeDeliveryManipulatorTest()
        {
            shipment = new ShipmentEntity
            {
                FedEx = new FedExShipmentEntity
                {
                    Service = (int) FedExServiceType.GroundHomeDelivery,
                    HomeDeliveryType = (int) FedExHomeDeliveryType.Appointment,
                    HomeDeliveryInstructions = "Some instructions",
                    HomeDeliveryDate = DateTime.Parse("1/1/2013")
                }
            };

            testObject = new FedExHomeDeliveryManipulator();
        }

        [Theory]
        [InlineData(FedExServiceType.GroundHomeDelivery, FedExHomeDeliveryType.Appointment, true)]
        [InlineData(FedExServiceType.GroundHomeDelivery, FedExHomeDeliveryType.DateCertain, true)]
        [InlineData(FedExServiceType.GroundHomeDelivery, FedExHomeDeliveryType.Evening, true)]
        [InlineData(FedExServiceType.GroundHomeDelivery, FedExHomeDeliveryType.None, false)]
        [InlineData(FedExServiceType.PriorityOvernight, FedExHomeDeliveryType.DateCertain, false)]
        [InlineData(FedExServiceType.FedExFreightPriority, FedExHomeDeliveryType.DateCertain, false)]
        [InlineData(FedExServiceType.FedExGround, FedExHomeDeliveryType.DateCertain, false)]
        public void ShoudlApply_ReturnsAppropriateValue_ForGivenInputs(FedExServiceType service, FedExHomeDeliveryType homeDelivery, bool expected)
        {
            var testShipment = Create.Shipment().AsFedEx(f => f
                    .Set(x => x.Service, (int) service)
                    .Set(x => x.HomeDeliveryType, (int) homeDelivery))
                .Build();
            var result = testObject.ShouldApply(testShipment);
            Assert.Equal(expected, result);
        }
        
        [Fact]
        public void Manipulate_ServiceTypeArraySizeIsOne()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Equal(1, result.Value.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Length);
        }

        [Fact]
        public void Manipulate_AddsHomeDeliveryServiceType_WhenServiceTypeArrayIsNull()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Contains(ShipmentSpecialServiceType.HOME_DELIVERY_PREMIUM, result.Value.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);
        }

        [Fact]
        public void Manipulate_AddsHomeDeliveryServiceType_WhenSpecialServicesRequestIsNull()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Contains(ShipmentSpecialServiceType.HOME_DELIVERY_PREMIUM, result.Value.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);
        }

        [Fact]
        public void Manipulate_HomeDeliveryDetailsIsNotNull()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.NotNull(result.Value.RequestedShipment.SpecialServicesRequested.HomeDeliveryPremiumDetail.HomeDeliveryPremiumType);
        }

        [Fact]
        public void Manipulate_PremiumTypeIsDateCertain()
        {
            shipment.FedEx.HomeDeliveryType = (int) FedExHomeDeliveryType.DateCertain;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Equal(HomeDeliveryPremiumType.DATE_CERTAIN, result.Value.RequestedShipment.SpecialServicesRequested.HomeDeliveryPremiumDetail.HomeDeliveryPremiumType);
        }

        [Fact]
        public void Manipulate_PremiumTypeIsEvening()
        {
            shipment.FedEx.HomeDeliveryType = (int) FedExHomeDeliveryType.Evening;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Equal(HomeDeliveryPremiumType.EVENING, result.Value.RequestedShipment.SpecialServicesRequested.HomeDeliveryPremiumDetail.HomeDeliveryPremiumType);
        }

        [Fact]
        public void Manipulate_PremiumTypeIsAppointment()
        {
            shipment.FedEx.HomeDeliveryType = (int) FedExHomeDeliveryType.Appointment;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Equal(HomeDeliveryPremiumType.APPOINTMENT, result.Value.RequestedShipment.SpecialServicesRequested.HomeDeliveryPremiumDetail.HomeDeliveryPremiumType);
        }

        [Fact]
        public void Manipulate_HomeDeliveryPhoneNumber()
        {
            shipment.FedEx.HomeDeliveryType = (int) FedExHomeDeliveryType.Appointment;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Equal(shipment.FedEx.HomeDeliveryPhone, result.Value.RequestedShipment.SpecialServicesRequested.HomeDeliveryPremiumDetail.PhoneNumber);
        }

        [Fact]
        public void Manipulate_HomeDeliveryDateIsAssigned_WhenDeliveryTypeIsDateCertain()
        {
            shipment.FedEx.HomeDeliveryType = (int) FedExHomeDeliveryType.DateCertain;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Equal(shipment.FedEx.HomeDeliveryDate, result.Value.RequestedShipment.SpecialServicesRequested.HomeDeliveryPremiumDetail.Date);
        }

        [Fact]
        public void Manipulate_HomeDeliveryDateSpecifiedIsTrue_WhenDeliveryTypeIsDateCertain()
        {
            shipment.FedEx.HomeDeliveryType = (int) FedExHomeDeliveryType.DateCertain;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.True(result.Value.RequestedShipment.SpecialServicesRequested.HomeDeliveryPremiumDetail.DateSpecified);
        }

    }
}
