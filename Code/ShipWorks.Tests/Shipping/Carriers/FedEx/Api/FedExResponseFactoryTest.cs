using System.Linq;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Close.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.Close.Response.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.Registration.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Close;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api
{
    public class FedExResponseFactoryTest
    {
        private FedExResponseFactory testObject;

        private ProcessShipmentReply nativeShipResponse;
        private Mock<CarrierRequest> carrierRequest;


        [TestInitialize]
        public void Initialize()
        {
            carrierRequest = new Mock<CarrierRequest>(null, new ShipmentEntity());

            // Create a ship response with the correct native type
            nativeShipResponse = new ProcessShipmentReply();

            testObject = new FedExResponseFactory();
        }

        [Fact]
        public void CreateShipResponse_ReturnsFedExShipResponse_Test()
        {
            ICarrierResponse carrierResponse = testObject.CreateShipResponse(nativeShipResponse, carrierRequest.Object, new ShipmentEntity());
            Assert.IsInstanceOfType(carrierResponse, typeof(FedExShipResponse), "Initialization failed as CreateShipResponse did not return a type of FedExShipResponse.");
        }

        [Fact]
        [ExpectedException(typeof(CarrierException))]
        public void CreateShipResponse_ThrowsCarrierException_WhenNativeResponseIsNotProcessShipmentReply_Test()
        {
            // Try sending a string as the native response
            const string nativeResponseText = "the native response";

            testObject.CreateShipResponse(nativeResponseText, carrierRequest.Object, new ShipmentEntity());
        }

        [Fact]
        public void CreateShipResponse_PopulatesManipulators_Test()
        {
            // This will obviously need to change as manipulators are added in the factory and also serve as a
            // reminder that to write the tests to ensure the manipulator type is present in the list
            FedExShipResponse carrierResponse = testObject.CreateShipResponse(nativeShipResponse, carrierRequest.Object, new ShipmentEntity()) as FedExShipResponse;
            Assert.AreEqual(3, carrierResponse.ShipmentManipulators.Count());
        }

        [Fact]
        public void CreateShipResponse_AddsFedExShipmentTrackingManipulator_Test()
        {
            FedExShipResponse carrierResponse = testObject.CreateShipResponse(nativeShipResponse, carrierRequest.Object, new ShipmentEntity()) as FedExShipResponse;
            Assert.AreEqual(1, carrierResponse.ShipmentManipulators.Count(m => m.GetType() == typeof(FedExShipmentTrackingManipulator)));
        }

        [Fact]
        public void CreateShipResponse_AddsFedExShipmentCodManipulator_Test()
        {
            FedExShipResponse carrierResponse = testObject.CreateShipResponse(nativeShipResponse, carrierRequest.Object, new ShipmentEntity()) as FedExShipResponse;
            Assert.AreEqual(1, carrierResponse.ShipmentManipulators.Count(m => m.GetType() == typeof(FedExShipmentCodManipulator)));
        }

        [Fact]
        public void CreateShipResponse_AddsFedExShipmentCostManipulator_Test()
        {
            FedExShipResponse carrierResponse = testObject.CreateShipResponse(nativeShipResponse, carrierRequest.Object, new ShipmentEntity()) as FedExShipResponse;
            Assert.AreEqual(1, carrierResponse.ShipmentManipulators.Count(m => m.GetType() == typeof(FedExShipmentCostManipulator)));
        }

        [Fact]
        public void CreateShipResponse_AddsFedExLabelRepository_Test()
        {
            FedExShipResponse carrierResponse = testObject.CreateShipResponse(nativeShipResponse, carrierRequest.Object, new ShipmentEntity()) as FedExShipResponse;
            Assert.IsInstanceOfType(carrierResponse.LabelRepository, typeof(FedExLabelRepository));
        }


        #region CreateGroundResponse Tests

        [Fact]
        [ExpectedException(typeof(CarrierException))]
        public void CreateGroundCloseResponse_ThrowsCarrierException_WhenResponseIsNull_Test()
        {
            GroundCloseReply closeReply = null;

            testObject.CreateGroundCloseResponse(closeReply, carrierRequest.Object);
        }

        [Fact]
        [ExpectedException(typeof(CarrierException))]
        public void CreateGroundCloseResponse_ThrowsCarrierException_WhenNativeResponseIsNotGroundCloseReply_Test()
        {
            SmartPostCloseReply smartPostCloseReply = new SmartPostCloseReply();

            testObject.CreateGroundCloseResponse(smartPostCloseReply, carrierRequest.Object);
        }

        [Fact]
        public void CreateGroundCloseResponse_IsNotNull_Test()
        {
            ICarrierResponse response = testObject.CreateGroundCloseResponse(new GroundCloseReply(), carrierRequest.Object);

            Assert.IsNotNull(response);
        }

        [Fact]
        public void CreateGroundCloseResponse_ReturnsGroundCarrierResponseType_Test()
        {
            ICarrierResponse response = testObject.CreateGroundCloseResponse(new GroundCloseReply(), carrierRequest.Object);

            Assert.IsInstanceOfType(response, typeof (FedExGroundCloseResponse));
        }

        [Fact]
        public void CreateGroundCloseResponse_AddsManipulators_Test()
        {
            FedExGroundCloseResponse response = testObject.CreateGroundCloseResponse(new GroundCloseReply(), carrierRequest.Object) as FedExGroundCloseResponse;

            Assert.AreEqual(1, response.Manipulators.Count());
        }

        [Fact]
        public void CreateGroundCloseResponse_AddsFedExCloseReportManipulator_Test()
        {
            FedExGroundCloseResponse response = testObject.CreateGroundCloseResponse(new GroundCloseReply(), carrierRequest.Object) as FedExGroundCloseResponse;

            Assert.AreEqual(1, response.Manipulators.Count(m => m.GetType() == typeof(FedExGroundCloseReportManipulator)));
        }

        #endregion CreateGroundResponse Tests

        #region CreateSmartPostResponse Tests

        [Fact]
        [ExpectedException(typeof(CarrierException))]
        public void CreateSmartPostCloseResponse_ThrowsCarrierException_WhenResponseIsNull_Test()
        {
            SmartPostCloseReply closeReply = null;

            testObject.CreateSmartPostCloseResponse(closeReply, carrierRequest.Object);
        }

        [Fact]
        [ExpectedException(typeof(CarrierException))]
        public void CreateSmartPostCloseResponse_ThrowsCarrierException_WhenNativeResponseIsNotSmartPostCloseReply_Test()
        {
            GroundCloseReply smartPostCloseReply = new GroundCloseReply();

            testObject.CreateSmartPostCloseResponse(smartPostCloseReply, carrierRequest.Object);
        }

        [Fact]
        public void CreateSmartPostCloseResponse_IsNotNull_Test()
        {
            ICarrierResponse response = testObject.CreateSmartPostCloseResponse(new SmartPostCloseReply(), carrierRequest.Object);

            Assert.IsNotNull(response);
        }

        [Fact]
        public void CreateSmartPostCloseResponse_ReturnsGroundCarrierResponseType_Test()
        {
            ICarrierResponse response = testObject.CreateSmartPostCloseResponse(new SmartPostCloseReply(), carrierRequest.Object);

            Assert.IsInstanceOfType(response, typeof(FedExSmartPostCloseResponse));
        }

        [Fact]
        public void CreateSmartPostCloseResponse_AddsManipulators_Test()
        {
            FedExSmartPostCloseResponse response = testObject.CreateSmartPostCloseResponse(new SmartPostCloseReply(), carrierRequest.Object) as FedExSmartPostCloseResponse;

            Assert.AreEqual(1, response.Manipulators.Count());
        }

        [Fact]
        public void CreateSmartPostCloseResponse_AddsFedExCloseReportManipulator_Test()
        {
            FedExSmartPostCloseResponse response = testObject.CreateSmartPostCloseResponse(new SmartPostCloseReply(), carrierRequest.Object) as FedExSmartPostCloseResponse;

            Assert.AreEqual(1, response.Manipulators.Count(m => m.GetType() == typeof(FedExSmartPostCloseEntityManipulator)));
        }

        #endregion CreateSmartPostResponse Tests

        #region CreateRegisterCspUserResponse Tests

        [Fact]
        [ExpectedException(typeof(CarrierException))]
        public void CreateRegisterCspUserResponse_ThrowsCarrierException_WhenInvalidNativeResponseIsNull_Test()
        {
            testObject.CreateRegisterUserResponse(null, carrierRequest.Object);
        }

        [Fact]
        [ExpectedException(typeof(CarrierException))]
        public void CreateRegisterCspUserResponse_ThrowsCarrierException_WhenInvalidNativeResponseProvided_Test()
        {
            RegisterWebCspUserRequest invalidType = new RegisterWebCspUserRequest();
            testObject.CreateRegisterUserResponse(invalidType, carrierRequest.Object);
        }

        [Fact]
        public void CreateRegisterCspUserResponse_ReturnsFedExRegisterCspUserResponse_Test()
        {
            RegisterWebCspUserReply validType = new RegisterWebCspUserReply();

            ICarrierResponse response = testObject.CreateRegisterUserResponse(validType, carrierRequest.Object);

            Assert.IsInstanceOfType(response, typeof (FedExRegisterCspUserResponse));
        }

        #endregion CreateRegisterCspUserResponse Tests

        #region CreateSubscriptionResponse Tests

        [Fact]
        [ExpectedException(typeof(CarrierException))]
        public void CreateSubscriptionResponse_ThrowsCarrierException_WhenInvalidNativeResponseIsNull_Test()
        {
            testObject.CreateSubscriptionResponse(null, carrierRequest.Object);
        }

        [Fact]
        [ExpectedException(typeof(CarrierException))]
        public void CreateSubscriptionResponse_ThrowsCarrierException_WhenInvalidNativeResponseProvided_Test()
        {
            RegisterWebCspUserReply invalidType = new RegisterWebCspUserReply();
            testObject.CreateSubscriptionResponse(invalidType, carrierRequest.Object);
        }

        [Fact]
        public void CreateSubscriptionResponse_ReturnsFedExSubscriptionResponse_Test()
        {
            SubscriptionReply validType = new SubscriptionReply();

            ICarrierResponse response = testObject.CreateSubscriptionResponse(validType, carrierRequest.Object);

            Assert.IsInstanceOfType(response, typeof(FedExSubscriptionResponse));
        }

        #endregion CreateSubscriptionResponse Tests

        [Fact]
        [ExpectedException(typeof(CarrierException))]
        public void CreateRateResponse_ThrowsCarrierException_WhenNativeResponseIsNotRateReply_Test()
        {
            testObject.CreateRateResponse(new GroundCloseReply(), carrierRequest.Object);
        }

        [Fact]
        public void CreateRateResponse_ReturnsFedExRateResponse_Test()
        {
            ICarrierResponse response = testObject.CreateRateResponse(new RateReply(), carrierRequest.Object);

            Assert.IsInstanceOfType(response, typeof(FedExRateResponse));
        }
    }
}
