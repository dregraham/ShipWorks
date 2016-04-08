using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.WebServices.Registration;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount.Api;
using ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount;
using System;
using Autofac;
using ShipWorks.Shipping.Carriers.UPS.InvoiceRegistration;
using Autofac.Extras.Moq;

namespace ShipWorks.Tests.Shipping.Carriers.UPS
{
    public class UpsClerkTest : IDisposable
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
        private AutoMock mock;

        public UpsClerkTest()
        {
            mock = AutoMock.GetLoose();

            openAccountCarrierResponse = mock.Mock<ICarrierResponse>();
            openAccountCarrierResponse.Setup(r => r.NativeResponse).Returns(nativeOpenAccountResponse);
            openAccountCarrierResponse.Setup(r => r.Process());

            openAccountRequest = mock.Mock<CarrierRequest>();
            openAccountRequest.Setup(c => c.Submit()).Returns(openAccountCarrierResponse.Object);

            openAccountRequestFactory = mock.Mock<IUpsOpenAccountRequestFactory>();
            openAccountRequestFactory.Setup(f => f.CreateOpenAccountRequest(It.IsAny<OpenAccountRequest>())).Returns(openAccountRequest.Object);

            upsAccount = new UpsAccountEntity();

            authorizationData = new UpsOltInvoiceAuthorizationData
            {
                ControlID = "1234ABCD",
                InvoiceAmount = 45.23M,
                InvoiceDate = DateTime.Today,
                InvoiceNumber = "000111233"
            };

            invoiceRegistrationCarrierResponse = mock.Mock<ICarrierResponse>();
            invoiceRegistrationCarrierResponse.Setup(r => r.Process());

            invoiceRegistrationCarrierRequest = mock.Mock<CarrierRequest>();
            invoiceRegistrationCarrierRequest.Setup(r => r.Submit()).Returns(invoiceRegistrationCarrierResponse.Object);

            invoiceRegistrationRequestFactory = mock.Mock<IUpsInvoiceRegistrationRequestFactory>();
            invoiceRegistrationRequestFactory.Setup(f => f.CreateInvoiceRegistrationRequest(It.IsAny<UpsAccountEntity>(), It.IsAny<UpsOltInvoiceAuthorizationData>()))
                                             .Returns(invoiceRegistrationCarrierRequest.Object);

            testObject = mock.Create<UpsClerk>(new TypedParameter(typeof(UpsAccountEntity), upsAccount));
        }

        #region "Open Account Tests"
        [Fact]
        public void OpenAccount_ThrowsNoError_ResponseContainsNoAddressCandidates()
        {
            testObject.OpenAccount(new OpenAccountRequest());
        }
        #endregion

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

        [Fact]
        public void RegisterAccount_ReturnsSuccess_WhenRegistrationIsSuccessful()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var nativeResponse = new RegisterResponse();
                nativeResponse.ShipperAccountStatus = new RegCodeDescriptionType[] { new RegCodeDescriptionType() { Code = "foo", Description = "bar" } };

                var response = mock.Mock<ICarrierResponse>();
                response.SetupGet(r => r.NativeResponse).Returns(nativeResponse);

                var requestFactory = mock.Mock<IUpsInvoiceRegistrationRequestFactory>();

                var request = mock.Mock<CarrierRequest>();
                request.Setup(r => r.Submit()).Returns(response.Object);

                requestFactory.Setup(r => r.CreateInvoiceRegistrationRequest(It.IsAny<UpsAccountEntity>(), It.IsAny<UpsOltInvoiceAuthorizationData>()))
                            .Returns(request.Object);

                UpsClerk clerk = mock.Create<UpsClerk>(new TypedParameter(typeof(UpsAccountEntity), upsAccount));

                Assert.Equal(UpsRegistrationStatus.Success, clerk.RegisterAccount(new UpsAccountEntity()));
            }
        }

        [Fact]
        public void RegisterAccount_ReturnsInvoiceAuthenticationRequired_WhenRegistrationInvoiceAuthenticationRequired()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var nativeResponse = new RegisterResponse();
                nativeResponse.ShipperAccountStatus = new[] { new RegCodeDescriptionType() { Code = "045", Description = "bar" } };

                var response = mock.Mock<ICarrierResponse>();
                response.SetupGet(r => r.NativeResponse).Returns(nativeResponse);

                var requestFactory = mock.Mock<IUpsInvoiceRegistrationRequestFactory>();

                var request = mock.Mock<CarrierRequest>();
                request.Setup(r => r.Submit()).Returns(response.Object);

                requestFactory.Setup(r => r.CreateInvoiceRegistrationRequest(It.IsAny<UpsAccountEntity>(), It.IsAny<UpsOltInvoiceAuthorizationData>()))
                            .Returns(request.Object);

                UpsClerk clerk = mock.Create<UpsClerk>(new TypedParameter(typeof(UpsAccountEntity), upsAccount));

                Assert.Equal(UpsRegistrationStatus.InvoiceAuthenticationRequired, clerk.RegisterAccount(new UpsAccountEntity()));
            }
        }

        [Fact]
        public void RegisterAccount_ReturnsFailed_WhenRegistrationFailsDueToUsernameUniqueness()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var nativeResponse = new RegisterResponse();
                nativeResponse.ShipperAccountStatus = new RegCodeDescriptionType[] { new RegCodeDescriptionType() { Code = "045", Description = "bar" } };

                var response = mock.Mock<ICarrierResponse>();
                response.SetupGet(r => r.NativeResponse).Returns(nativeResponse);

                var requestFactory = mock.Mock<IUpsInvoiceRegistrationRequestFactory>();

                var request = mock.Mock<CarrierRequest>();
                request.Setup(r => r.Submit()).Throws(new UpsWebServiceException("9570100"));

                requestFactory.Setup(r => r.CreateInvoiceRegistrationRequest(It.IsAny<UpsAccountEntity>(), It.IsAny<UpsOltInvoiceAuthorizationData>()))
                            .Returns(request.Object);

                UpsClerk clerk = mock.Create<UpsClerk>(new TypedParameter(typeof(UpsAccountEntity), upsAccount));

                Assert.Equal(UpsRegistrationStatus.Failed, clerk.RegisterAccount(new UpsAccountEntity()));
            }
        }

        [Fact]
        public void RegisterAccount_ThrowsUpsWebServiceException_WhenExceptionCodeIsNotDueToUniqueness()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var nativeResponse = new RegisterResponse();
                nativeResponse.ShipperAccountStatus = new RegCodeDescriptionType[] { new RegCodeDescriptionType() { Code = "045", Description = "bar" } };

                var response = mock.Mock<ICarrierResponse>();
                response.SetupGet(r => r.NativeResponse).Returns(nativeResponse);

                var requestFactory = mock.Mock<IUpsInvoiceRegistrationRequestFactory>();

                var request = mock.Mock<CarrierRequest>();
                request.Setup(r => r.Submit()).Throws(new UpsWebServiceException("888"));

                requestFactory.Setup(r => r.CreateInvoiceRegistrationRequest(It.IsAny<UpsAccountEntity>(), It.IsAny<UpsOltInvoiceAuthorizationData>()))
                            .Returns(request.Object);
                UpsClerk clerk = mock.Create<UpsClerk>(new TypedParameter(typeof(UpsAccountEntity), upsAccount));


                Assert.Throws<UpsWebServiceException>(() => clerk.RegisterAccount(new UpsAccountEntity()));
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            mock.Dispose();
        }
    }
}