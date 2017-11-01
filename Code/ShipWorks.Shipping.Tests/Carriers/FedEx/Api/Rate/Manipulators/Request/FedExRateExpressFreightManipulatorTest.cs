using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Enums;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;


namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    public class FedExRateExpressFreightManipulatorTest
    {
        private FedExRateExpressFreightManipulator testObject;

        private RateRequest nativeRequest;
        private ShipmentEntity shipmentEntity;
        private Mock<IFedExSettingsRepository> settingsRepository;

        public FedExRateExpressFreightManipulatorTest()
        {
            SetupShipment();
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedEx1DayFreight;

            nativeRequest = new RateRequest();

            // Return a FedEx account that has been migrated
            settingsRepository = new Mock<IFedExSettingsRepository>();
            settingsRepository.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>())).Returns(new FedExAccountEntity() { CountryCode = "US" });
            settingsRepository.Setup(r => r.IsInterapptiveUser).Returns(false);

            testObject = new FedExRateExpressFreightManipulator(settingsRepository.Object);
        }

        private void SetupShipment()
        {
            shipmentEntity = new ShipmentEntity
            {
                FedEx = new FedExShipmentEntity { WeightUnitType = (int) WeightUnitOfMeasure.Pounds }
            };

            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity()
            {
                DimsLength = 2,
                DimsWidth = 4,
                DimsHeight = 8,
                // total weight should be 48
                DimsWeight = 16,
                DimsAddWeight = true,
                Weight = 32,
                DeclaredValue = 64
            });

            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity()
            {
                DimsLength = 3,
                DimsWidth = 6,
                DimsHeight = 12,
                // total weight should be 72
                DimsWeight = 24,
                DimsAddWeight = true,
                Weight = 48,
                DeclaredValue = 96
            });

            nativeRequest = new RateRequest
            {
                RequestedShipment = new RequestedShipment
                {
                    RequestedPackageLineItems = new RequestedPackageLineItem[0]
                }
            };
        }

        [Fact]
        public void Manipulate_FedExFreightManipulator_ReturnsRequestedShipment()
        {
            testObject.Manipulate(shipmentEntity, nativeRequest);

            Assert.IsAssignableFrom<RequestedShipment>(nativeRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_FedExFreightManipulator_ReturnsNoExpressFreight_WhenServiceIsNotFreight()
        {
            shipmentEntity.FedEx.Service = (int) FedExServiceType.FedExGround;
            testObject.Manipulate(shipmentEntity, nativeRequest);

            Assert.Null(nativeRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_FedExFreightManipulator_ReturnsRequestedUSValues()
        {
            shipmentEntity.ShipCountryCode = "US";
            shipmentEntity.OriginCountryCode = "US";

            FedExShipmentEntity fedEx = shipmentEntity.FedEx;
            fedEx.Service = (int)FedExServiceType.FedEx1DayFreight;
            fedEx.FreightBookingNumber = "fbn123";
            fedEx.FreightLoadAndCount = 23;
            fedEx.FreightInsideDelivery = true;
            fedEx.FreightInsidePickup = true;

            testObject.Manipulate(shipmentEntity, nativeRequest);

            Assert.Equal("fbn123", nativeRequest.RequestedShipment.ExpressFreightDetail.BookingConfirmationNumber);
            Assert.Equal("23", nativeRequest.RequestedShipment.ExpressFreightDetail.ShippersLoadAndCount);

            List<ShipmentSpecialServiceType> specialServiceTypes = nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.ToList();

            Assert.Equal(2, specialServiceTypes.Count);
            Assert.Equal(1, specialServiceTypes.Count(sst => sst == ShipmentSpecialServiceType.INSIDE_DELIVERY));
            Assert.Equal(1, specialServiceTypes.Count(sst => sst == ShipmentSpecialServiceType.INSIDE_PICKUP));
        }

        [Fact]
        public void Manipulate_FedExFreightManipulator_ReturnsRequestedCAValues()
        {
            shipmentEntity.ShipCountryCode = "CA";

            FedExShipmentEntity fedEx = shipmentEntity.FedEx;
            fedEx.Service = (int)FedExServiceType.FedEx1DayFreight;
            fedEx.FreightBookingNumber = "fbn123";
            fedEx.FreightLoadAndCount = 23;
            fedEx.FreightInsideDelivery = true;
            fedEx.FreightInsidePickup = true;

            testObject.Manipulate(shipmentEntity, nativeRequest);

            Assert.Equal("fbn123", nativeRequest.RequestedShipment.ExpressFreightDetail.BookingConfirmationNumber);
            Assert.Equal("23", nativeRequest.RequestedShipment.ExpressFreightDetail.ShippersLoadAndCount);

            List<ShipmentSpecialServiceType> specialServiceTypes = nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.ToList();

            Assert.Equal(0, specialServiceTypes.Count);
        }

        [Fact]
        public void Manipulate_FedExFreightManipulator_ContainsShippingDocumentType()
        {
            FedExShipmentEntity fedEx = shipmentEntity.FedEx;
            fedEx.Service = (int)FedExServiceType.FedEx1DayFreight;
            testObject.Manipulate(shipmentEntity, nativeRequest);

            List<RequestedShippingDocumentType> shippingDocumentTypes =
                nativeRequest.RequestedShipment.ShippingDocumentSpecification.ShippingDocumentTypes.ToList();

            Assert.Equal(1, shippingDocumentTypes.Count(sdt => sdt == RequestedShippingDocumentType.LABEL));
        }

    }
}
