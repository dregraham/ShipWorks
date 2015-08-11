using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    public class FedExShippingWebAuthenticationDetailManipulatorTest
    {
        private FedExShippingWebAuthenticationDetailManipulator testObject;

        private Mock<ICarrierSettingsRepository> settingsRepository;
        private ShippingSettingsEntity shippingSettings;
        private FedExSettings settings;

        private Mock<CarrierRequest> carrierRequest;
        private ProcessShipmentRequest nativeRequest;

        [TestInitialize]
        public void Initialize()
        {
            shippingSettings = new ShippingSettingsEntity {FedExPassword = "password", FedExUsername = "username"};

            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(shippingSettings);

            settings = new FedExSettings(settingsRepository.Object);

            nativeRequest = new ProcessShipmentRequest {WebAuthenticationDetail = new WebAuthenticationDetail()};
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeRequest);

            testObject = new FedExShippingWebAuthenticationDetailManipulator(settings);
        }

        [Fact]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull_Test()
        {
            testObject.Manipulate(null);
        }

        [Fact]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull_Test()
        {
            // Setup the native request to be null
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), null);

            testObject.Manipulate(carrierRequest.Object);
        }

        [Fact]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotProcessShipmentRequest_Test()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new object());

            testObject.Manipulate(carrierRequest.Object);
        }

        [Fact]
        public void Manipulate_SetsWebAuthenticationDetail_WhenWebAuthenticationDetailsIsNull_Test()
        {
            // Only setup is  to set the detail to null value
            nativeRequest.WebAuthenticationDetail = null;

            testObject.Manipulate(carrierRequest.Object);

            WebAuthenticationDetail detail = ((ProcessShipmentRequest) carrierRequest.Object.NativeRequest).WebAuthenticationDetail;
            Assert.IsNotNull(detail);
        }

        [Fact]
        public void Manipulate_SetsWebAuthenticationDetail_WhenWebAuthenticationDetailsIsNotNull_Test()
        {
            // No additional setup since everything is in the Initialize method
            testObject.Manipulate(carrierRequest.Object);

            WebAuthenticationDetail detail = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).WebAuthenticationDetail;
            Assert.IsNotNull(detail);
        }
    }
}
