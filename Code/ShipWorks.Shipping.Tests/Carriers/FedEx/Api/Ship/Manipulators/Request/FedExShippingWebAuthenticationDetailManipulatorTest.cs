using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    public class FedExShippingWebAuthenticationDetailManipulatorTest
    {
        private FedExShippingWebAuthenticationDetailManipulator testObject;

        private Mock<ICarrierSettingsRepository> settingsRepository;
        private ShippingSettingsEntity shippingSettings;
        private FedExSettings settings;
        private readonly ProcessShipmentRequest processShipmentRequest;
        private readonly ShipmentEntity shipment;

        public FedExShippingWebAuthenticationDetailManipulatorTest()
        {
            shippingSettings = new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" };

            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(shippingSettings);

            settings = new FedExSettings(settingsRepository.Object);

            shipment = new ShipmentEntity();

            processShipmentRequest = new ProcessShipmentRequest { WebAuthenticationDetail = new WebAuthenticationDetail() };

            testObject = new FedExShippingWebAuthenticationDetailManipulator(settings);
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

        [Fact]
        public void Manipulate_SetsWebAuthenticationDetail_WhenWebAuthenticationDetailsIsNull()
        {
            // Only setup is  to set the detail to null value
            processShipmentRequest.WebAuthenticationDetail = null;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            WebAuthenticationDetail detail = processShipmentRequest.WebAuthenticationDetail;
            Assert.NotNull(detail);
        }

        [Fact]
        public void Manipulate_SetsWebAuthenticationDetail_WhenWebAuthenticationDetailsIsNotNull()
        {
            // No additional setup since everything is in the Initialize method
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            WebAuthenticationDetail detail = processShipmentRequest.WebAuthenticationDetail;
            Assert.NotNull(detail);
        }
    }
}
