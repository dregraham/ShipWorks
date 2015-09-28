using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    public class FedExFreightManipulatorTest
    {
        private FedExFreightManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private ProcessShipmentRequest nativeRequest;
        private ShipmentEntity shipmentEntity;

        public FedExFreightManipulatorTest()
        {
            shipmentEntity = BuildFedExShipmentEntity.SetupRequestShipmentEntity();
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedEx1DayFreight;

            nativeRequest = new ProcessShipmentRequest();
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject = new FedExFreightManipulator();
        }

        [Fact]
        public void Manipulate_FedExFreightManipulator_ReturnsRequestedShipment_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.IsAssignableFrom<RequestedShipment>(nativeRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_FedExFreightManipulator_ReturnsNoExpressFreight_WhenServiceIsNotFreight_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
            testObject.Manipulate(carrierRequest.Object);

            Assert.Null(nativeRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_FedExFreightManipulator_ReturnsRequestedUSValues_Test()
        {
            shipmentEntity.ShipCountryCode = "US";
            shipmentEntity.OriginCountryCode = "US";

            FedExShipmentEntity fedEx = shipmentEntity.FedEx;
            fedEx.Service = (int)FedExServiceType.FedEx1DayFreight;
            fedEx.FreightBookingNumber = "fbn123";
            fedEx.FreightLoadAndCount = 23;
            fedEx.FreightInsideDelivery = true;
            fedEx.FreightInsidePickup = true;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal("fbn123", nativeRequest.RequestedShipment.ExpressFreightDetail.BookingConfirmationNumber);
            Assert.Null(nativeRequest.RequestedShipment.ExpressFreightDetail.ShippersLoadAndCount);

            List<ShipmentSpecialServiceType> specialServiceTypes = nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.ToList();

            Assert.Equal(2, specialServiceTypes.Count);
            Assert.Equal(1, specialServiceTypes.Count(sst => sst == ShipmentSpecialServiceType.INSIDE_DELIVERY));
            Assert.Equal(1, specialServiceTypes.Count(sst => sst == ShipmentSpecialServiceType.INSIDE_PICKUP));
        }

        [Fact]
        public void Manipulate_FedExFreightManipulator_ReturnsRequestedCAValues_Test()
        {
            shipmentEntity.ShipCountryCode = "CA";

            FedExShipmentEntity fedEx = shipmentEntity.FedEx;
            fedEx.Service = (int)FedExServiceType.FedEx1DayFreight;
            fedEx.FreightBookingNumber = "fbn123";
            fedEx.FreightLoadAndCount = 23;
            fedEx.FreightInsideDelivery = true;
            fedEx.FreightInsidePickup = true;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal("fbn123", nativeRequest.RequestedShipment.ExpressFreightDetail.BookingConfirmationNumber);
            Assert.Equal("23", nativeRequest.RequestedShipment.ExpressFreightDetail.ShippersLoadAndCount);

            List<ShipmentSpecialServiceType> specialServiceTypes = nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.ToList();

            Assert.Equal(0, specialServiceTypes.Count);
        }

        [Fact]
        public void Manipulate_FedExFreightManipulator_ContainsShippingDocumentType_Test()
        {
            FedExShipmentEntity fedEx = shipmentEntity.FedEx;
            fedEx.Service = (int)FedExServiceType.FedEx1DayFreight;
            testObject.Manipulate(carrierRequest.Object);

            List<RequestedShippingDocumentType> shippingDocumentTypes =
                nativeRequest.RequestedShipment.ShippingDocumentSpecification.ShippingDocumentTypes.ToList();

            Assert.Equal(1, shippingDocumentTypes.Count(sdt => sdt == RequestedShippingDocumentType.LABEL));
        }

    }
}
