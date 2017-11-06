using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    public class FedExFreightManipulatorTest
    {
        private FedExFreightManipulator testObject;
        private readonly AutoMock mock;
        private ShipmentEntity shipment;

        public FedExFreightManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            shipment = BuildFedExShipmentEntity.SetupRequestShipmentEntity();
            shipment.FedEx.Service = (int) FedExServiceType.FedEx1DayFreight;

            testObject = mock.Create<FedExFreightManipulator>();
        }

        [Theory]
        [InlineData(FedExServiceType.FedEx1DayFreight, true)]
        [InlineData(FedExServiceType.FedEx2DayFreight, true)]
        [InlineData(FedExServiceType.FedEx3DayFreight, true)]
        [InlineData(FedExServiceType.FirstFreight, true)]
        [InlineData(FedExServiceType.InternationalEconomyFreight, true)]
        [InlineData(FedExServiceType.InternationalPriorityFreight, true)]
        [InlineData(FedExServiceType.FedExNextDayFreight, false)]
        [InlineData(FedExServiceType.FedExFreightEconomy, false)]
        [InlineData(FedExServiceType.FedExFreightPriority, false)]
        [InlineData(FedExServiceType.PriorityOvernight, false)]
        [InlineData(FedExServiceType.FedExGround, false)]
        public void ShouldApply_ReturnsAppropriateValue_ForGivenInput(FedExServiceType serviceType, bool expected)
        {
            var testShipment = Create.Shipment().AsFedEx(f => f.Set(x => x.Service, (int) serviceType)).Build();
            var result = testObject.ShouldApply(testShipment, 0);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Manipulate_FedExFreightManipulator_ReturnsRequestedUSValues()
        {
            shipment.ShipCountryCode = "US";
            shipment.OriginCountryCode = "US";

            FedExShipmentEntity fedEx = shipment.FedEx;
            fedEx.Service = (int) FedExServiceType.FedEx1DayFreight;
            fedEx.FreightBookingNumber = "fbn123";
            fedEx.FreightLoadAndCount = 23;
            fedEx.FreightInsideDelivery = true;
            fedEx.FreightInsidePickup = true;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Equal("fbn123", result.Value.RequestedShipment.ExpressFreightDetail.BookingConfirmationNumber);
            Assert.Equal("23", result.Value.RequestedShipment.ExpressFreightDetail.ShippersLoadAndCount);

            List<ShipmentSpecialServiceType> specialServiceTypes = result.Value.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.ToList();

            Assert.Equal(2, specialServiceTypes.Count);
            Assert.Contains(ShipmentSpecialServiceType.INSIDE_DELIVERY, specialServiceTypes);
            Assert.Contains(ShipmentSpecialServiceType.INSIDE_PICKUP, specialServiceTypes);
        }

        [Fact]
        public void Manipulate_FedExFreightManipulator_ReturnsRequestedCAValues()
        {
            shipment.ShipCountryCode = "CA";

            FedExShipmentEntity fedEx = shipment.FedEx;
            fedEx.Service = (int) FedExServiceType.FedEx1DayFreight;
            fedEx.FreightBookingNumber = "fbn123";
            fedEx.FreightLoadAndCount = 23;
            fedEx.FreightInsideDelivery = true;
            fedEx.FreightInsidePickup = true;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Equal("fbn123", result.Value.RequestedShipment.ExpressFreightDetail.BookingConfirmationNumber);
            Assert.Equal("23", result.Value.RequestedShipment.ExpressFreightDetail.ShippersLoadAndCount);

            List<ShipmentSpecialServiceType> specialServiceTypes = result.Value.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.ToList();

            Assert.Equal(0, specialServiceTypes.Count);
        }

        [Fact]
        public void Manipulate_FedExFreightManipulator_ContainsShippingDocumentType()
        {
            FedExShipmentEntity fedEx = shipment.FedEx;
            fedEx.Service = (int) FedExServiceType.FedEx1DayFreight;
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            List<RequestedShippingDocumentType> shippingDocumentTypes =
                result.Value.RequestedShipment.ShippingDocumentSpecification.ShippingDocumentTypes.ToList();

            Assert.Equal(1, shippingDocumentTypes.Count(sdt => sdt == RequestedShippingDocumentType.LABEL));
        }

    }
}
