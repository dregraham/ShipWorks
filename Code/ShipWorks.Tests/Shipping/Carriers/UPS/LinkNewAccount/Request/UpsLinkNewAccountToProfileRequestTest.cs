using System.Collections.Generic;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.LinkNewAccount.Request;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.WebServices.Registration;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.LinkNewAccount.Request
{
    public class UpsLinkNewAccountToProfileRequestTest
    {
        [Fact]
        public void Submit_ApplyManipulatorsCalled()
        {
            Mock<ICarrierRequestManipulator> manipulator = new Mock<ICarrierRequestManipulator>();
            Mock<IUpsServiceGateway> serviceGateway = new Mock<IUpsServiceGateway>();
            List<ICarrierRequestManipulator> manipulators = new List<ICarrierRequestManipulator> {manipulator.Object};
            UpsLinkNewAccountToProfileRequest testObject = new UpsLinkNewAccountToProfileRequest(manipulators,
                serviceGateway.Object,
                new UpsAccountEntity());

            testObject.Submit();
            manipulator.Verify(m => m.Manipulate(It.IsAny<CarrierRequest>()), Times.Once());
        }

        [Fact]
        public void Submit_ManageAccountIsCalled()
        {
            var manipulator = new Mock<ICarrierRequestManipulator>();
            Mock<IUpsServiceGateway> serviceGateway = new Mock<IUpsServiceGateway>();
            List<ICarrierRequestManipulator> manipulators = new List<ICarrierRequestManipulator> { manipulator.Object };
            UpsLinkNewAccountToProfileRequest testObject = new UpsLinkNewAccountToProfileRequest(manipulators,
                serviceGateway.Object,
                new UpsAccountEntity());

            testObject.Submit();
            serviceGateway.Verify(g=>g.LinkNewAccount(It.IsAny<ManageAccountRequest>()));
        }

        [Fact]
        public void Submit_UpsOpenAccountExceptionThrown_WhenServiceGatewayThrowsUpsWebServiceException()
        {
            var manipulator = new Mock<ICarrierRequestManipulator>();
            Mock<IUpsServiceGateway> serviceGateway = new Mock<IUpsServiceGateway>();

            serviceGateway.Setup(g => g.LinkNewAccount(It.IsAny<ManageAccountRequest>()))
                .Throws(new UpsWebServiceException("blah"));

            List<ICarrierRequestManipulator> manipulators = new List<ICarrierRequestManipulator> { manipulator.Object };
            UpsLinkNewAccountToProfileRequest testObject = new UpsLinkNewAccountToProfileRequest(manipulators,
                serviceGateway.Object,
                new UpsAccountEntity());

            Assert.Throws<UpsOpenAccountException>(() => testObject.Submit());
        }

        [Fact]
        public void Submit_UpsOpenAccountExceptionThrown_WithNotRegisteredErrorCode_WhenServiceGatewayThrowsUpsWebServiceException()
        {
            var manipulator = new Mock<ICarrierRequestManipulator>();
            Mock<IUpsServiceGateway> serviceGateway = new Mock<IUpsServiceGateway>();

            serviceGateway.Setup(g => g.LinkNewAccount(It.IsAny<ManageAccountRequest>()))
                .Throws(new UpsWebServiceException("blah"));

            List<ICarrierRequestManipulator> manipulators = new List<ICarrierRequestManipulator> { manipulator.Object };
            UpsLinkNewAccountToProfileRequest testObject = new UpsLinkNewAccountToProfileRequest(manipulators,
                serviceGateway.Object,
                new UpsAccountEntity());
            try
            {
                testObject.Submit();
            }
            catch (UpsOpenAccountException ex)
            {
                
                Assert.Equal(UpsOpenAccountErrorCode.NotRegistered, ex.ErrorCode);
            }
        }
    }
}