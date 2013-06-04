using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Rate.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Rate;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.v2013.Rate.Request.Manipulators
{
    [TestClass]
    public class FedExRateWebAuthenticationManipulatorTest
    {
        private FedExRateWebAuthenticationManipulator testObject;

        private Mock<ICarrierSettingsRepository> settingsRepository;
        private ShippingSettingsEntity shippingSettings;
        private FedExSettings settings;

        private Mock<CarrierRequest> carrierRequest;
        private RateRequest nativeRequest;

        [TestInitialize]
        public void Initialize()
        {
            shippingSettings = new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" };

            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(shippingSettings);

            settings = new FedExSettings(settingsRepository.Object);

            nativeRequest = new RateRequest { WebAuthenticationDetail = new WebAuthenticationDetail() };
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeRequest);

            testObject = new FedExRateWebAuthenticationManipulator(settings);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull_Test()
        {
            testObject.Manipulate(null);
        }

        [TestMethod]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull_Test()
        {
            // Setup the native request to be null
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), null);

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotRateRequest_Test()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new RateReply());

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        public void Manipulate_SetsWebAuthenticationDetail_WhenWebAuthenticationDetailsIsNull_Test()
        {
            // Only setup is  to set the detail to null value
            nativeRequest.WebAuthenticationDetail = null;

            testObject.Manipulate(carrierRequest.Object);

            WebAuthenticationDetail detail = ((RateRequest)carrierRequest.Object.NativeRequest).WebAuthenticationDetail;
            Assert.IsNotNull(detail);
        }

        [TestMethod]
        public void Manipulate_SetsWebAuthenticationDetail_WhenWebAuthenticationDetailsIsNotNull_Test()
        {
            // No additional setup since everything is in the Initialize method
            testObject.Manipulate(carrierRequest.Object);

            WebAuthenticationDetail detail = ((RateRequest)carrierRequest.Object.NativeRequest).WebAuthenticationDetail;
            Assert.IsNotNull(detail);
        }
    }
}
