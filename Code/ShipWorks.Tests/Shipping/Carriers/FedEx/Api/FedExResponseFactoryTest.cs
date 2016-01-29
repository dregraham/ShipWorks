using System.Linq;
using Moq;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Close.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.Close.Response.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.Registration.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Close;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api
{
    public class FedExResponseFactoryTest
    {
        private FedExResponseFactory testObject;

        private ProcessShipmentReply nativeShipResponse;
        private Mock<CarrierRequest> carrierRequest;


        public FedExResponseFactoryTest()
        {
            carrierRequest = new Mock<CarrierRequest>(null, new ShipmentEntity());

            // Create a ship response with the correct native type
            nativeShipResponse = new ProcessShipmentReply();

            var dataResourceManager = new Mock<IDataResourceManager>();

            testObject = new FedExResponseFactory(new FedExLabelRepository(dataResourceManager.Object));
        }

        [Fact]
        public void CreateShipResponse_ReturnsFedExShipResponse()
        {
            ICarrierResponse carrierResponse = testObject.CreateShipResponse(nativeShipResponse, carrierRequest.Object, new ShipmentEntity());
            Assert.IsAssignableFrom<FedExShipResponse>(carrierResponse);
        }

        [Fact]
        public void CreateShipResponse_ThrowsCarrierException_WhenNativeResponseIsNotProcessShipmentReply()
        {
            // Try sending a string as the native response
            const string nativeResponseText = "the native response";

            Assert.Throws<CarrierException>(() => testObject.CreateShipResponse(nativeResponseText, carrierRequest.Object, new ShipmentEntity()));
        }

        [Fact]
        public void CreateShipResponse_PopulatesManipulators()
        {
            // This will obviously need to change as manipulators are added in the factory and also serve as a
            // reminder that to write the tests to ensure the manipulator type is present in the list
            FedExShipResponse carrierResponse = testObject.CreateShipResponse(nativeShipResponse, carrierRequest.Object, new ShipmentEntity()) as FedExShipResponse;
            Assert.Equal(3, carrierResponse.ShipmentManipulators.Count());
        }

        [Fact]
        public void CreateShipResponse_AddsFedExShipmentTrackingManipulator()
        {
            FedExShipResponse carrierResponse = testObject.CreateShipResponse(nativeShipResponse, carrierRequest.Object, new ShipmentEntity()) as FedExShipResponse;
            Assert.Equal(1, carrierResponse.ShipmentManipulators.Count(m => m.GetType() == typeof(FedExShipmentTrackingManipulator)));
        }

        [Fact]
        public void CreateShipResponse_AddsFedExShipmentCodManipulator()
        {
            FedExShipResponse carrierResponse = testObject.CreateShipResponse(nativeShipResponse, carrierRequest.Object, new ShipmentEntity()) as FedExShipResponse;
            Assert.Equal(1, carrierResponse.ShipmentManipulators.Count(m => m.GetType() == typeof(FedExShipmentCodManipulator)));
        }

        [Fact]
        public void CreateShipResponse_AddsFedExShipmentCostManipulator()
        {
            FedExShipResponse carrierResponse = testObject.CreateShipResponse(nativeShipResponse, carrierRequest.Object, new ShipmentEntity()) as FedExShipResponse;
            Assert.Equal(1, carrierResponse.ShipmentManipulators.Count(m => m.GetType() == typeof(FedExShipmentCostManipulator)));
        }

        [Fact]
        public void CreateShipResponse_AddsFedExLabelRepository()
        {
            FedExShipResponse carrierResponse = testObject.CreateShipResponse(nativeShipResponse, carrierRequest.Object, new ShipmentEntity()) as FedExShipResponse;
            Assert.IsAssignableFrom<FedExLabelRepository>(carrierResponse.LabelRepository);
        }


        #region CreateGroundResponse Tests

        [Fact]
        public void CreateGroundCloseResponse_ThrowsCarrierException_WhenResponseIsNull()
        {
            GroundCloseReply closeReply = null;

            Assert.Throws<CarrierException>(() => testObject.CreateGroundCloseResponse(closeReply, carrierRequest.Object));
        }

        [Fact]
        public void CreateGroundCloseResponse_ThrowsCarrierException_WhenNativeResponseIsNotGroundCloseReply()
        {
            SmartPostCloseReply smartPostCloseReply = new SmartPostCloseReply();

            Assert.Throws<CarrierException>(() => testObject.CreateGroundCloseResponse(smartPostCloseReply, carrierRequest.Object));
        }

        [Fact]
        public void CreateGroundCloseResponse_IsNotNull()
        {
            ICarrierResponse response = testObject.CreateGroundCloseResponse(new GroundCloseReply(), carrierRequest.Object);

            Assert.NotNull(response);
        }

        [Fact]
        public void CreateGroundCloseResponse_ReturnsGroundCarrierResponseType()
        {
            ICarrierResponse response = testObject.CreateGroundCloseResponse(new GroundCloseReply(), carrierRequest.Object);

            Assert.IsAssignableFrom<FedExGroundCloseResponse>(response);
        }

        [Fact]
        public void CreateGroundCloseResponse_AddsManipulators()
        {
            FedExGroundCloseResponse response = testObject.CreateGroundCloseResponse(new GroundCloseReply(), carrierRequest.Object) as FedExGroundCloseResponse;

            Assert.Equal(1, response.Manipulators.Count());
        }

        [Fact]
        public void CreateGroundCloseResponse_AddsFedExCloseReportManipulator()
        {
            FedExGroundCloseResponse response = testObject.CreateGroundCloseResponse(new GroundCloseReply(), carrierRequest.Object) as FedExGroundCloseResponse;

            Assert.Equal(1, response.Manipulators.Count(m => m.GetType() == typeof(FedExGroundCloseReportManipulator)));
        }

        #endregion CreateGroundResponse Tests

        #region CreateSmartPostResponse Tests

        [Fact]
        public void CreateSmartPostCloseResponse_ThrowsCarrierException_WhenResponseIsNull()
        {
            SmartPostCloseReply closeReply = null;

            Assert.Throws<CarrierException>(() => testObject.CreateSmartPostCloseResponse(closeReply, carrierRequest.Object));
        }

        [Fact]
        public void CreateSmartPostCloseResponse_ThrowsCarrierException_WhenNativeResponseIsNotSmartPostCloseReply()
        {
            GroundCloseReply smartPostCloseReply = new GroundCloseReply();

            Assert.Throws<CarrierException>(() => testObject.CreateSmartPostCloseResponse(smartPostCloseReply, carrierRequest.Object));
        }

        [Fact]
        public void CreateSmartPostCloseResponse_IsNotNull()
        {
            ICarrierResponse response = testObject.CreateSmartPostCloseResponse(new SmartPostCloseReply(), carrierRequest.Object);

            Assert.NotNull(response);
        }

        [Fact]
        public void CreateSmartPostCloseResponse_ReturnsGroundCarrierResponseType()
        {
            ICarrierResponse response = testObject.CreateSmartPostCloseResponse(new SmartPostCloseReply(), carrierRequest.Object);

            Assert.IsAssignableFrom<FedExSmartPostCloseResponse>(response);
        }

        [Fact]
        public void CreateSmartPostCloseResponse_AddsManipulators()
        {
            FedExSmartPostCloseResponse response = testObject.CreateSmartPostCloseResponse(new SmartPostCloseReply(), carrierRequest.Object) as FedExSmartPostCloseResponse;

            Assert.Equal(1, response.Manipulators.Count());
        }

        [Fact]
        public void CreateSmartPostCloseResponse_AddsFedExCloseReportManipulator()
        {
            FedExSmartPostCloseResponse response = testObject.CreateSmartPostCloseResponse(new SmartPostCloseReply(), carrierRequest.Object) as FedExSmartPostCloseResponse;

            Assert.Equal(1, response.Manipulators.Count(m => m.GetType() == typeof(FedExSmartPostCloseEntityManipulator)));
        }

        #endregion CreateSmartPostResponse Tests

        #region CreateRegisterCspUserResponse Tests

        [Fact]
        public void CreateRegisterCspUserResponse_ThrowsCarrierException_WhenInvalidNativeResponseIsNull()
        {
            Assert.Throws<CarrierException>(() => testObject.CreateRegisterUserResponse(null, carrierRequest.Object));
        }

        [Fact]
        public void CreateRegisterCspUserResponse_ThrowsCarrierException_WhenInvalidNativeResponseProvided()
        {
            RegisterWebUserRequest invalidType = new RegisterWebUserRequest();
            Assert.Throws<CarrierException>(() => testObject.CreateRegisterUserResponse(invalidType, carrierRequest.Object));
        }

        [Fact]
        public void CreateRegisterCspUserResponse_ReturnsFedExRegisterCspUserResponse()
        {
            RegisterWebUserReply validType = new RegisterWebUserReply();

            ICarrierResponse response = testObject.CreateRegisterUserResponse(validType, carrierRequest.Object);

            Assert.IsAssignableFrom<FedExRegisterCspUserResponse>(response);
        }

        #endregion CreateRegisterCspUserResponse Tests

        #region CreateSubscriptionResponse Tests

        [Fact]
        public void CreateSubscriptionResponse_ThrowsCarrierException_WhenInvalidNativeResponseIsNull()
        {
            Assert.Throws<CarrierException>(() => testObject.CreateSubscriptionResponse(null, carrierRequest.Object));
        }

        [Fact]
        public void CreateSubscriptionResponse_ThrowsCarrierException_WhenInvalidNativeResponseProvided()
        {
            RegisterWebUserReply invalidType = new RegisterWebUserReply();
            Assert.Throws<CarrierException>(() => testObject.CreateSubscriptionResponse(invalidType, carrierRequest.Object));
        }

        [Fact]
        public void CreateSubscriptionResponse_ReturnsFedExSubscriptionResponse()
        {
            SubscriptionReply validType = new SubscriptionReply();

            ICarrierResponse response = testObject.CreateSubscriptionResponse(validType, carrierRequest.Object);

            Assert.IsAssignableFrom<FedExSubscriptionResponse>(response);
        }

        #endregion CreateSubscriptionResponse Tests

        [Fact]
        public void CreateRateResponse_ThrowsCarrierException_WhenNativeResponseIsNotRateReply()
        {
            Assert.Throws<CarrierException>(() => testObject.CreateRateResponse(new GroundCloseReply(), carrierRequest.Object));
        }

        [Fact]
        public void CreateRateResponse_ReturnsFedExRateResponse()
        {
            ICarrierResponse response = testObject.CreateRateResponse(new RateReply(), carrierRequest.Object);

            Assert.IsAssignableFrom<FedExRateResponse>(response);
        }
    }
}
