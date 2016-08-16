using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.InvoiceRegistration.Api.Response;
using ShipWorks.Shipping.Carriers.UPS.InvoiceRegistration.Api.Response.Manipulators;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.WebServices.Registration;


namespace ShipWorks.Tests.Shipping.Carriers.UPS.InvoiceRegistration.Api.Response.Manipulators
{
    public class UpsSaveCredentialsManipulatorTest
    {
        private UpsSaveCredentialsManipulator testObject;

        private UpsInvoiceRegistrationResponse upsInvoiceRegistrationResponse;
        private Mock<CarrierRequest> carrierRequest;
        private RegisterRequest registerRequest;

        private UpsAccountEntity upsAccount;

        public UpsSaveCredentialsManipulatorTest()
        {
            upsAccount = new UpsAccountEntity();

            registerRequest = new RegisterRequest()
            {
                Username = "theUser",
                Password = "thePassword"
            };
            
            carrierRequest=new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), null, registerRequest);
            carrierRequest.Setup(r => r.CarrierAccountEntity).Returns(upsAccount);

            upsInvoiceRegistrationResponse = new UpsInvoiceRegistrationResponse(new RegisterResponse(), carrierRequest.Object, null);

            testObject = new UpsSaveCredentialsManipulator();
        }

        [Fact]
        public void Manipulate_AccountUserIDSet()
        {
            testObject.Manipulate(upsInvoiceRegistrationResponse);

            Assert.Equal(registerRequest.Username, upsAccount.UserID);
        }

        [Fact]
        public void Manipulate_AccountPasswordSet()
        {
            testObject.Manipulate(upsInvoiceRegistrationResponse);

            Assert.Equal(registerRequest.Password, upsAccount.Password);
        }

        [Fact]
        public void Manipulate_AccountInvoiceAuthSetToTrue()
        {
            testObject.Manipulate(upsInvoiceRegistrationResponse);

            Assert.True(upsAccount.InvoiceAuth);
        }
    }
}
