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

        [TestInitialize]
        public void Initialize()
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
        public void Manipulate_AccountUserIDSet_Test()
        {
            testObject.Manipulate(upsInvoiceRegistrationResponse);

            Assert.AreEqual(registerRequest.Username, upsAccount.UserID);
        }

        [Fact]
        public void Manipulate_AccountPasswordSet_Test()
        {
            testObject.Manipulate(upsInvoiceRegistrationResponse);

            Assert.AreEqual(registerRequest.Password, upsAccount.Password);
        }

        [Fact]
        public void Manipulate_AccountInvoiceAuthSetToTrue()
        {
            testObject.Manipulate(upsInvoiceRegistrationResponse);

            Assert.IsTrue(upsAccount.InvoiceAuth);
        }
    }
}
