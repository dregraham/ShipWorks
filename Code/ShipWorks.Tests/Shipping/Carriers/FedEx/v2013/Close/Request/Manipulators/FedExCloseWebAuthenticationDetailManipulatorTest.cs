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
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Close.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Close;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.v2013.Close.Request.Manipulators
{
    [TestClass]
    public class FedExCloseWebAuthenticationDetailManipulatorTest
    {
        private FedExCloseWebAuthenticationDetailManipulator testObject;

        private Mock<ICarrierSettingsRepository> settingsRepository;
        private ShippingSettingsEntity shippingSettings;

        private Mock<CarrierRequest> groundCarrierRequest;
        private GroundCloseRequest nativeGroundRequest;

        private Mock<CarrierRequest> smartPostCarrierRequest;
        private SmartPostCloseRequest nativeSmartPostRequest;

        [TestInitialize]
        public void Initialize()
        {
            shippingSettings = new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" };

            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(shippingSettings);
         
            nativeGroundRequest = new GroundCloseRequest { WebAuthenticationDetail = new WebAuthenticationDetail() };
            groundCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeGroundRequest);

            nativeSmartPostRequest = new SmartPostCloseRequest { WebAuthenticationDetail = new WebAuthenticationDetail() };
            smartPostCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeSmartPostRequest);

            testObject = new FedExCloseWebAuthenticationDetailManipulator(settingsRepository.Object);
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
            groundCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), null);

            testObject.Manipulate(groundCarrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotProcessShipmentRequest_AndNotSmartPostCloseReport_Test()
        {
            // Setup the native request to be an unexpected type
            groundCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new SmartPostCloseReply());

            testObject.Manipulate(groundCarrierRequest.Object);
        }

        [TestMethod]
        public void Manipulate_SetsWebAuthenticationDetail_WhenWebAuthenticationDetailsIsNull_ForGroundClose_Test()
        {
            // Only setup is  to set the detail to null value
            nativeGroundRequest.WebAuthenticationDetail = null;

            testObject.Manipulate(groundCarrierRequest.Object);

            WebAuthenticationDetail detail = ((GroundCloseRequest)groundCarrierRequest.Object.NativeRequest).WebAuthenticationDetail;
            Assert.IsNotNull(detail);
        }

        [TestMethod]
        public void Manipulate_SetsWebAuthenticationDetail_WhenWebAuthenticationDetailsIsNotNull_ForGroundClose_Test()
        {
            // No additional setup since everything is in the Initialize method
            testObject.Manipulate(groundCarrierRequest.Object);

            WebAuthenticationDetail detail = ((GroundCloseRequest)groundCarrierRequest.Object.NativeRequest).WebAuthenticationDetail;
            Assert.IsNotNull(detail);
        }



        [TestMethod]
        public void Manipulate_SetsWebAuthenticationDetail_WhenWebAuthenticationDetailsIsNull_ForSmartPostClose_Test()
        {
            // Only setup is  to set the detail to null value
            nativeSmartPostRequest.WebAuthenticationDetail = null;

            testObject.Manipulate(smartPostCarrierRequest.Object);

            WebAuthenticationDetail detail = ((SmartPostCloseRequest)smartPostCarrierRequest.Object.NativeRequest).WebAuthenticationDetail;
            Assert.IsNotNull(detail);
        }

        [TestMethod]
        public void Manipulate_SetsWebAuthenticationDetail_WhenWebAuthenticationDetailsIsNotNull_ForSmartPostClose_Test()
        {
            // No additional setup since everything is in the Initialize method
            testObject.Manipulate(smartPostCarrierRequest.Object);

            WebAuthenticationDetail detail = ((SmartPostCloseRequest)smartPostCarrierRequest.Object.NativeRequest).WebAuthenticationDetail;
            Assert.IsNotNull(detail);
        }





    }
}
