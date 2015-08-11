using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;
using ShipWorks.Shipping.Carriers.UPS.InvoiceRegistration.Api.Request.Manipulators;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.WebServices.Registration;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.InvoiceRegistration.Api.Request.Manipulators
{
    public class UpsInvoiceRegistrationNewProfileCredentialsManipulatorTest
    {
        UpsInvoiceRegistrationNewProfileCredentialsManipulator testObject;

        private CarrierRequest request;

        private RegisterRequest registerRequest;

        public UpsInvoiceRegistrationNewProfileCredentialsManipulatorTest()
        {
            registerRequest=new RegisterRequest();

            Mock<CarrierRequest> mockRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(),null,registerRequest);

            request = mockRequest.Object;

            testObject = new UpsInvoiceRegistrationNewProfileCredentialsManipulator();           
        }

        [Fact]
        public void Manipulate_UsernameIsPopulated_Test()
        {
            testObject.Manipulate(request);

            Assert.False(string.IsNullOrWhiteSpace(registerRequest.Username));
        }

        [Fact]
        public void Manipulate_PasswordIsPopulated_Test()
        {
            testObject.Manipulate(request);

            Assert.False(string.IsNullOrWhiteSpace(registerRequest.Password));
        }

        [Fact]
        public void Manipulate_SuggestUserNameIndicatorCorrect_Test()
        {
            testObject.Manipulate(request);

            Assert.Equal("N", registerRequest.SuggestUsernameIndicator);
        }

        [Fact]
        public void Manipulate_NotificationCodeCorrect_Test()
        {
            testObject.Manipulate(request);

            Assert.Equal("00", registerRequest.NotificationCode);
        }

        [Fact]
        public void Manipulate_UsernameIsDifferentAfterSecondCall_Test()
        {
            testObject.Manipulate(request);

            string firstUsername = registerRequest.Username;

            testObject.Manipulate(request);

            Assert.NotEqual(firstUsername, registerRequest.Username);
        }
    }
}
