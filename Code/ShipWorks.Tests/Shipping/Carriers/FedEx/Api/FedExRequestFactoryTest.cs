using System.Linq;
using Autofac.Extras.Moq;
using Interapptive.Shared.Net;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Close.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.PackageMovement.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Api.Registration.Request.Manipulators;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api
{
    public class FedExRequestFactoryTest
    {
        private readonly AutoMock mock;
        private FedExRequestFactory testObject;
        private ShipmentEntity fedExShipment;

        public FedExRequestFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            mock.Mock<IFedExSettingsRepository>()
                .Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>()))
                .Returns(new FedExAccountEntity());

            testObject = mock.Create<FedExRequestFactory>();

            fedExShipment = new ShipmentEntity() { FedEx = new FedExShipmentEntity() };
        }

        #region CreateVersionCaptureRequest Tests

        [Fact]
        public void CreateVersionCaptureRequest_PopulatesManipulators()
        {
            CarrierRequest request = testObject.CreateVersionCaptureRequest(fedExShipment, string.Empty, new FedExAccountEntity());

            // This will obviously need to change as manipulators are added in the factory and also serve as a
            // reminder that to write the tests to ensure the manipulator type is present in the list
            Assert.Equal(3, request.Manipulators.Count());
        }

        [Fact]
        public void CreateVersionCaptureRequest_FedExRegistrationWebAuthenticationDetailManipulator()
        {
            CarrierRequest request = testObject.CreateVersionCaptureRequest(fedExShipment, string.Empty, new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRegistrationWebAuthenticationDetailManipulator)) == 1);
        }

        [Fact]
        public void CreateVersionCaptureRequest_FedExRegistrationClientDetailManipulator()
        {
            CarrierRequest request = testObject.CreateVersionCaptureRequest(fedExShipment, string.Empty, new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRegistrationClientDetailManipulator)) == 1);
        }

        [Fact]
        public void CreateVersionCaptureRequest_FedExRegistrationVersionManipulator()
        {
            CarrierRequest request = testObject.CreateVersionCaptureRequest(fedExShipment, string.Empty, new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRegistrationVersionManipulator)) == 1);
        }

        #endregion CreateVersionCaptureRequest Tests

        #region CreatePackageMovementRequest Tests

        [Fact]
        public void CreatePackageMovementRequest_PopulatesManipulators()
        {
            CarrierRequest request = testObject.CreatePackageMovementRequest(fedExShipment, new FedExAccountEntity());

            // This will obviously need to change as manipulators are added in the factory and also serve as a
            // reminder that to write the tests to ensure the manipulator type is present in the list
            Assert.Equal(3, request.Manipulators.Count());
        }

        [Fact]
        public void CreatePackageMovementRequest_WebAuthenticationDetailManipulator()
        {
            CarrierRequest request = testObject.CreatePackageMovementRequest(fedExShipment, new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExPackageMovementWebAuthenticationDetailManipulator)) == 1);
        }

        [Fact]
        public void CreatePackageMovementRequest_FedExPackageMovementClientDetailManipulator()
        {
            CarrierRequest request = testObject.CreatePackageMovementRequest(fedExShipment, new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExPackageMovementClientDetailManipulator)) == 1);
        }

        [Fact]
        public void CreatePackageMovementRequest_FedExPackageMovementVersionManipulator()
        {
            CarrierRequest request = testObject.CreatePackageMovementRequest(new ShipmentEntity(), new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExPackageMovementVersionManipulator)) == 1);
        }

        #endregion CreateVersionCaptureRequest Tests

        #region CreateGroundCloseRequest Tests

        [Fact]
        public void CreateGroundCloseRequest_PopulatesManipulators()
        {
            CarrierRequest request = testObject.CreateGroundCloseRequest(new FedExAccountEntity());

            // This will obviously need to change as manipulators are added in the factory and also serve as a
            // reminder that to write the tests to ensure the manipulator type is present in the list
            Assert.Equal(4, request.Manipulators.Count());
        }

        [Fact]
        public void CreateGroundCloseRequest_WebAuthenticationDetailManipulator()
        {
            CarrierRequest request = testObject.CreateGroundCloseRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExCloseWebAuthenticationDetailManipulator)) == 1);
        }

        [Fact]
        public void CreateGroundCloseRequest_FedExPackageMovementClientDetailManipulator()
        {
            CarrierRequest request = testObject.CreateGroundCloseRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExCloseClientDetailManipulator)) == 1);
        }

        [Fact]
        public void CreateGroundCloseRequest_FedExPackageMovementVersionManipulator()
        {
            CarrierRequest request = testObject.CreateGroundCloseRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExCloseVersionManipulator)) == 1);
        }

        [Fact]
        public void CreateGroundCloseRequest_FedExCloseDateManipulator()
        {
            CarrierRequest request = testObject.CreateGroundCloseRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExCloseDateManipulator)) == 1);
        }

        #endregion CreateCloseRequest Tests

        #region CreateSmartPostCloseRequest Tests

        [Fact]
        public void CreateSmartPostCloseRequest_PopulatesManipulators()
        {
            CarrierRequest request = testObject.CreateSmartPostCloseRequest(new FedExAccountEntity());

            // This will obviously need to change as manipulators are added in the factory and also serve as a
            // reminder that to write the tests to ensure the manipulator type is present in the list
            Assert.Equal(4, request.Manipulators.Count());
        }

        [Fact]
        public void CreateSmartPostCloseRequest_WebAuthenticationDetailManipulator()
        {
            CarrierRequest request = testObject.CreateSmartPostCloseRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExCloseWebAuthenticationDetailManipulator)) == 1);
        }

        [Fact]
        public void CreateSmartPostCloseRequest_FedExPackageMovementClientDetailManipulator()
        {
            CarrierRequest request = testObject.CreateSmartPostCloseRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExCloseClientDetailManipulator)) == 1);
        }

        [Fact]
        public void CreateSmartPostCloseRequest_FedExPackageMovementVersionManipulator()
        {
            CarrierRequest request = testObject.CreateSmartPostCloseRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExCloseVersionManipulator)) == 1);
        }

        [Fact]
        public void CreateSmartPostCloseRequest_FedExCloseDateManipulator()
        {
            CarrierRequest request = testObject.CreateSmartPostCloseRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExPickupCarrierManipulator)) == 1);
        }

        #endregion CreateSmartPostCloseRequest Tests

        #region CreateRegisterCspUserRequest Tests

        [Fact]
        public void CreateRegisterCspUserRequest_PopulatesManipulators()
        {
            CarrierRequest request = testObject.CreateRegisterCspUserRequest(new FedExAccountEntity());

            // This will obviously need to change as manipulators are added in the factory and also serve as a
            // reminder that to write the tests to ensure the manipulator type is present in the list
            Assert.Equal(4, request.Manipulators.Count());
        }

        [Fact]
        public void CreateRegisterCspUserRequest_FedExRegistrationWebAuthenticationDetailManipulator()
        {
            CarrierRequest request = testObject.CreateRegisterCspUserRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRegistrationWebAuthenticationDetailManipulator)) == 1);
        }

        [Fact]
        public void CreateRegisterCspUserRequest_FedExRegistrationClientDetailManipulator()
        {
            CarrierRequest request = testObject.CreateRegisterCspUserRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRegistrationClientDetailManipulator)) == 1);
        }

        [Fact]
        public void CreateRegisterCspUserRequest_FedExRegistrationVersionManipulator()
        {
            CarrierRequest request = testObject.CreateRegisterCspUserRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRegistrationVersionManipulator)) == 1);
        }

        [Fact]
        public void CreateRegisterCspUserRequest_FedExCspContactManipulator()
        {
            CarrierRequest request = testObject.CreateRegisterCspUserRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExCspContactManipulator)) == 1);
        }

        #endregion CreateRegisterCspUserRequest Tests

        #region CreateSubscriptionRequest Tests

        [Fact]
        public void CreateSubscriptionRequest_PopulatesManipulators()
        {
            CarrierRequest request = testObject.CreateSubscriptionRequest(new FedExAccountEntity());

            // This will obviously need to change as manipulators are added in the factory and also serve as a
            // reminder that to write the tests to ensure the manipulator type is present in the list
            Assert.Equal(4, request.Manipulators.Count());
        }

        [Fact]
        public void CreateSubscriptionRequest_FedExRegistrationWebAuthenticationDetailManipulator()
        {
            CarrierRequest request = testObject.CreateSubscriptionRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRegistrationWebAuthenticationDetailManipulator)) == 1);
        }

        [Fact]
        public void CreateSubscriptionRequest_FedExRegistrationClientDetailManipulator()
        {
            CarrierRequest request = testObject.CreateSubscriptionRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRegistrationClientDetailManipulator)) == 1);
        }

        [Fact]
        public void CreateSubscriptionRequest_FedExRegistrationVersionManipulator()
        {
            CarrierRequest request = testObject.CreateSubscriptionRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRegistrationVersionManipulator)) == 1);
        }

        [Fact]
        public void CreateSubscriptionRequest_FedExSubscriberManipulator()
        {
            CarrierRequest request = testObject.CreateSubscriptionRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExSubscriberManipulator)) == 1);
        }

        #endregion CreateSubscriptionRequest Tests

        #region CreateCertificateRequest Tests

        [Fact]
        public void CreateCertificateRequest_ReturnsCertficateReqeust()
        {
            Mock<ICertificateInspector> inspector = new Mock<ICertificateInspector>();

            ICertificateRequest request = testObject.CreateCertificateRequest(inspector.Object);

            Assert.IsAssignableFrom<CertificateRequest>(request);
        }
        #endregion
    }
}