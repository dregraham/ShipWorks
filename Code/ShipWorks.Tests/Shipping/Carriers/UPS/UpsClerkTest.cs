using System.Web.Services.Protocols;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.WebServices.Registration;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount.Api;
using ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount;
using System;
using ShipWorks.Tests.Shipping.Carriers.UPS;
using System.Xml;
using System.Xml.Linq;
using ShipWorks.Shipping.Carriers.UPS.InvoiceRegistration;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.OpenAccount
{
    public class UpsClerkTest
    {
        private UpsClerk testObject;

        private readonly OpenAccountResponse nativeOpenAccountResponse = new OpenAccountResponse();

        private Mock<CarrierRequest> openAccountRequest;
        private Mock<ICarrierResponse> openAccountCarrierResponse;
        private Mock<IUpsOpenAccountRequestFactory> openAccountRequestFactory;
        private Mock<IUpsInvoiceRegistrationRequestFactory> invoiceRegistrationRequestFactory;


        private UpsOltInvoiceAuthorizationData authorizationData;

        private UpsAccountEntity upsAccount;

        private Mock<CarrierRequest> invoiceRegistrationCarrierRequest;
        private Mock<ICarrierResponse> invoiceRegistrationCarrierResponse;

        UpsWebServiceException upsWebService9570100Exception = new UpsWebServiceException("9570100");
        UpsWebServiceException upsWebService100Exception = new UpsWebServiceException("100");

        public UpsClerkTest()
        {
            openAccountCarrierResponse = new Mock<ICarrierResponse>();
            openAccountCarrierResponse.Setup(r => r.NativeResponse).Returns(nativeOpenAccountResponse);
            openAccountCarrierResponse.Setup(r => r.Process());

            openAccountRequest = new Mock<CarrierRequest>();
            openAccountRequest.Setup(c => c.Submit()).Returns(openAccountCarrierResponse.Object);

            openAccountRequestFactory = new Mock<IUpsOpenAccountRequestFactory>();
            openAccountRequestFactory.Setup(f => f.CreateOpenAccountRequest(It.IsAny<OpenAccountRequest>())).Returns(openAccountRequest.Object);

            upsAccount = new UpsAccountEntity();

            authorizationData = new UpsOltInvoiceAuthorizationData
            {
                ControlID = "1234ABCD",
                InvoiceAmount = 45.23M,
                InvoiceDate = DateTime.Today,
                InvoiceNumber = "000111233"
            };

            invoiceRegistrationCarrierResponse = new Mock<ICarrierResponse>();
            invoiceRegistrationCarrierResponse.Setup(r => r.Process());

            invoiceRegistrationCarrierRequest = new Mock<CarrierRequest>();
            invoiceRegistrationCarrierRequest.Setup(r => r.Submit()).Returns(invoiceRegistrationCarrierResponse.Object);

            invoiceRegistrationRequestFactory = new Mock<IUpsInvoiceRegistrationRequestFactory>();
            invoiceRegistrationRequestFactory.Setup(f => f.CreateInvoiceRegistrationRequest(It.IsAny<UpsAccountEntity>(), It.IsAny<UpsOltInvoiceAuthorizationData>()))
                                             .Returns(invoiceRegistrationCarrierRequest.Object);

            testObject = new UpsClerk(openAccountRequestFactory.Object, invoiceRegistrationRequestFactory.Object);
        }

        #region "Open Account Tests"
        [Fact]
        public void OpenAccount_ThrowsNoError_ResponseContainsNoAddressCandidates()
        {
            testObject.OpenAccount(new OpenAccountRequest());
        }
        #endregion

        [Fact]
        public void RegisterAccount_ThrowsUpsException_WhenAuthorizatioInvoiceIsNull()
        {
            Assert.Throws<UpsException>(() => testObject.RegisterAccount(new UpsAccountEntity(), null));
        }

        [Fact]
        public void RegisterAccount_ThrowsUpsException_WhenInvoiceNumberNotProvided()
        {
            authorizationData.InvoiceNumber = string.Empty;
            Assert.Throws<UpsException>(() => testObject.RegisterAccount(new UpsAccountEntity(), authorizationData));
        }

        [Fact]
        public void RegisterAccount_DelegatesToInvoiceRegistrationRequest()
        {
            UpsAccountEntity account = new UpsAccountEntity();
            testObject.RegisterAccount(account, authorizationData);

            invoiceRegistrationCarrierRequest.Verify(r => r.Submit(), Times.Once());
        }

        [Fact]
        public void RegisterAccount_DelegatesToInvoiceRegistrationResponse()
        {
            UpsAccountEntity account = new UpsAccountEntity();
            testObject.RegisterAccount(account, authorizationData);

            invoiceRegistrationCarrierResponse.Verify(r => r.Process(), Times.Once());
        }

        [Fact]
        public void RegisterAccount_ThrowsUpsWebServiceException_WhenRegistrationFails_AndErrorCodeIsNot9570100()
        {
            // Setup to simulate a soap exception being thrown
            invoiceRegistrationCarrierRequest.Setup(c => c.Submit())
                                             .Throws(upsWebService100Exception);

            UpsAccountEntity account = new UpsAccountEntity();

            Assert.Throws<UpsWebServiceException>(() => testObject.RegisterAccount(account, authorizationData));
        }

        [Fact]
        public void RegisterAccount_AttemptsThreeTimes_WhenErrorCodeIs9570100()
        {
            // Setup to simulate a soap exception being thrown
            invoiceRegistrationCarrierRequest.Setup(c => c.Submit())
                                             .Throws(upsWebService9570100Exception);

            UpsAccountEntity account = new UpsAccountEntity();

            try
            {
                testObject.RegisterAccount(account, authorizationData);
            }
            catch (UpsException)
            {
                // Eat the exception, so we can check that three attempts to the gateway were made
                invoiceRegistrationCarrierRequest.Verify(c => c.Submit(), Times.Exactly(3));
            }
        }

        [Fact]
        public void RegisterAccount_ThrowsUpsException_WhenErrorCodeIs9570100()
        {
            invoiceRegistrationCarrierRequest.Setup(c => c.Submit())
                                  .Throws(upsWebService9570100Exception);

            UpsAccountEntity account = new UpsAccountEntity();

            Assert.Throws<UpsException>(() => testObject.RegisterAccount(account, authorizationData));
        }

        [Fact]
        public void RegisterAccount_ThrowsUpsWebServiceExcetion_WhenErrorCodeIsNot9570100()
        {
            // Setup to simulate a soap exception being thrown
            invoiceRegistrationCarrierRequest.Setup(c => c.Submit())
                                             .Throws(upsWebService100Exception);

            UpsAccountEntity account = new UpsAccountEntity();

            Assert.Throws<UpsWebServiceException>(() => testObject.RegisterAccount(account, authorizationData));
        }

        [Fact]
        public void RegisterAccount_DoesNotMakeAdditionalAttempts_WhenErrorCodeIsNot9570100()
        {

            // Setup to simulate a soap exception being thrown
            invoiceRegistrationCarrierRequest.Setup(c => c.Submit())
                                             .Throws(upsWebService100Exception);

            UpsAccountEntity account = new UpsAccountEntity();

            try
            {
                testObject.RegisterAccount(account, authorizationData);
            }
            catch (UpsException)
            {
                // Eat the exception, so we can check that only one attempt to the gateway was made
                invoiceRegistrationCarrierRequest.Verify(c => c.Submit(), Times.Once());
            }
        }
    }
}