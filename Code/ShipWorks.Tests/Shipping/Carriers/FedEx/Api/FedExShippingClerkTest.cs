using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services.Protocols;
using System.Xml;
using Autofac.Extras.Moq;
using Interapptive.Shared.Net;
using log4net;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Close.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.Close.Response.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Api.PackageMovement.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Close;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;
using Notification = ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate.Notification;
using ServiceType = ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate.ServiceType;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api
{
    public class FedExShippingClerkTest
    {
        private FedExShippingClerk testObject;

        private Mock<ICarrierSettingsRepository> settingsRepository;
        private Mock<ICertificateRequest> certificateRequest;

        private readonly AutoMock mock;

        private Mock<CarrierRequest> packageMovementRequest;
        private Mock<CarrierRequest> versionCaptureRequest;

        private Mock<CarrierRequest> shippingRequest;
        private Mock<ICarrierResponse> shipResponse;

        private Mock<CarrierRequest> groundCloseRequest;
        private Mock<ICarrierResponse> groundCloseResponse;

        private Mock<CarrierRequest> smartPostCloseRequest;
        private Mock<ICarrierResponse> smartPostCloseResponse;

        private Mock<CarrierRequest> registrationRequest;
        private Mock<ICarrierResponse> registrationResponse;

        private Mock<CarrierRequest> subscriptionRequest;
        private Mock<ICarrierResponse> subscriptionResponse;

        private Mock<IFedExRateRequest> rateRequest;
        private Mock<IFedExRateResponse> rateResponse;
        private RateReply nativeRateReply;

        private PostalCodeInquiryReply reply;

        private ShipmentEntity shipmentEntity;

        public FedExShippingClerkTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            settingsRepository = mock.Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetAccounts()).Returns
                (
                    new List<FedExAccountEntity>()
                    {
                        new FedExAccountEntity() { MeterNumber = "123" },
                        new FedExAccountEntity() { MeterNumber = "456" },
                        new FedExAccountEntity() { MeterNumber = "789" }
                    }
                );

            // Return a FedEx account that has been migrated
            settingsRepository.Setup(r => r.GetAccount(AnyShipment)).Returns(new FedExAccountEntity() { MeterNumber = "123" });

            certificateRequest = new Mock<ICertificateRequest>();
            certificateRequest.Setup(r => r.Submit()).Returns(CertificateSecurityLevel.Trusted);

            reply = new PostalCodeInquiryReply()
            {
                ExpressDescription = new PostalCodeServiceAreaDescription() { LocationId = "ABC" },
                HighestSeverity = ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement.NotificationSeverityType.SUCCESS
            };

            packageMovementRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), null);
            packageMovementRequest.Setup(r => r.Submit()).Returns(new FedExPackageMovementResponse(reply, packageMovementRequest.Object));

            versionCaptureRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), null);
            versionCaptureRequest.Setup(r => r.Submit());

            shipResponse = new Mock<ICarrierResponse>();
            shipResponse.Setup(r => r.Process());

            shippingRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), null);
            shippingRequest.Setup(r => r.Submit()).Returns(shipResponse.Object);

            groundCloseResponse = new Mock<ICarrierResponse>();
            groundCloseResponse.Setup(r => r.Process());

            groundCloseRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), null);
            groundCloseRequest.Setup(r => r.Submit()).Returns(groundCloseResponse.Object);

            smartPostCloseResponse = new Mock<ICarrierResponse>();
            smartPostCloseResponse.Setup(r => r.Process());

            smartPostCloseRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), null);
            smartPostCloseRequest.Setup(r => r.Submit()).Returns(smartPostCloseResponse.Object);

            // Registration request/response
            registrationResponse = new Mock<ICarrierResponse>();
            registrationResponse.Setup(r => r.Process());

            registrationRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), null);
            registrationRequest.Setup(r => r.Submit()).Returns(registrationResponse.Object);

            // Subscription request/response
            subscriptionResponse = new Mock<ICarrierResponse>();
            subscriptionResponse.Setup(r => r.Process());

            subscriptionRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), null);
            subscriptionRequest.Setup(r => r.Submit()).Returns(subscriptionResponse.Object);

            // Rate request/response
            nativeRateReply = new RateReply
            {
                RateReplyDetails = new RateReplyDetail[]
                {
                    new RateReplyDetail()
                    {
                        ActualRateType = ReturnedRateType.PAYOR_ACCOUNT_PACKAGE,
                        ActualRateTypeSpecified = true,

                        RatedShipmentDetails = new RatedShipmentDetail[]
                        {
                            new RatedShipmentDetail
                            {
                                ShipmentRateDetail = new ShipmentRateDetail
                                {
                                    RateType = ReturnedRateType.PAYOR_ACCOUNT_PACKAGE,
                                    RateTypeSpecified = true,
                                    TotalNetCharge = new Money { Amount = 40.12M },
                                    RatedWeightMethod = RatedWeightMethod.ACTUAL,
                                    RatedWeightMethodSpecified = true,
                                    TotalBillingWeight = new Weight { Value = 0.1M },
                                    TotalDimWeight = new Weight { Value = 0.1M }

                                }
                            }
                        },
                        ServiceType = ServiceType.PRIORITY_OVERNIGHT
                    },
                    new RateReplyDetail()
                    {
                        ActualRateType = ReturnedRateType.PAYOR_ACCOUNT_PACKAGE,
                        ActualRateTypeSpecified = true,

                        RatedShipmentDetails = new RatedShipmentDetail[]
                        {
                            new RatedShipmentDetail
                            {
                                ShipmentRateDetail = new ShipmentRateDetail
                                {
                                    RateType = ReturnedRateType.PAYOR_ACCOUNT_PACKAGE,
                                    RateTypeSpecified = true,
                                    TotalNetCharge = new Money { Amount = 40.12M },
                                    RatedWeightMethod = RatedWeightMethod.ACTUAL,
                                    RatedWeightMethodSpecified = true,
                                    TotalBillingWeight = new Weight { Value = 0.1M },
                                    TotalDimWeight = new Weight { Value = 0.1M }
                                }
                            }
                        },
                        ServiceType = ServiceType.PRIORITY_OVERNIGHT
                    }
                }
            };

            var requestFactory = mock.Mock<IFedExRequestFactory>();
            requestFactory.Setup(f => f.CreatePackageMovementRequest(AnyShipment, It.IsAny<FedExAccountEntity>())).Returns(packageMovementRequest.Object);
            requestFactory.Setup(f => f.CreateShipRequest(AnyShipment)).Returns(shippingRequest.Object);
            requestFactory.Setup(f => f.CreateVersionCaptureRequest(AnyShipment, It.IsAny<string>(), It.IsAny<FedExAccountEntity>())).Returns(versionCaptureRequest.Object);
            requestFactory.Setup(f => f.CreateGroundCloseRequest(It.IsAny<FedExAccountEntity>())).Returns(groundCloseRequest.Object);
            requestFactory.Setup(f => f.CreateSmartPostCloseRequest(It.IsAny<FedExAccountEntity>())).Returns(smartPostCloseRequest.Object);
            requestFactory.Setup(f => f.CreateRegisterCspUserRequest(It.IsAny<FedExAccountEntity>())).Returns(registrationRequest.Object);
            requestFactory.Setup(f => f.CreateSubscriptionRequest(It.IsAny<FedExAccountEntity>())).Returns(subscriptionRequest.Object);
            requestFactory.Setup(f => f.CreateCertificateRequest(It.IsAny<ICertificateInspector>())).Returns(certificateRequest.Object);

            rateResponse = mock.CreateMock<IFedExRateResponse>(x => x.Setup(r => r.Process()).Returns(nativeRateReply));
            rateRequest = mock.CreateMock<IFedExRateRequest>(x =>
            {
                x.Setup(r => r.Submit(AnyIShipment)).Returns(rateResponse);
                x.Setup(r => r.Submit(AnyIShipment, It.IsAny<FedExRateRequestOptions>())).Returns(rateResponse);
            });

            requestFactory.Setup(x => x.CreateRateRequest())
                .Returns(rateRequest);

            shipmentEntity = BuildFedExShipmentEntity.SetupBaseShipmentEntity();
            shipmentEntity.FedEx.SmartPostHubID = "5571";
            shipmentEntity.ShipmentType = (int) ShipmentTypeCode.FedEx;

            testObject = mock.Create<FedExShippingClerk>();
        }

        [Fact]
        public void PerformVersionCapture_WritesToLog()
        {
            testObject.ForceVersionCapture = true;

            testObject.PerformVersionCapture(new ShipmentEntity());

            // The string should indicate the capture was forced since our test object was configured as such
            mock.Mock<ILog>().Verify(l => l.Info("Performing FedEx version capture (forced)"), Times.Once());
        }

        [Fact]
        public void PerformVersionCapture_DelegatesToFactory_ToCreatePackageMovementRequest_ForEachFedExAccountTest()
        {
            testObject.ForceVersionCapture = true;

            testObject.PerformVersionCapture(new ShipmentEntity());

            // Our repository is setup to return 3 accounts
            mock.Mock<IFedExRequestFactory>()
                .Verify(f => f.CreatePackageMovementRequest(AnyShipment, It.IsAny<FedExAccountEntity>()), Times.Exactly(3));
        }

        [Fact]
        public void PerformVersionCapture_DelegatesToPackageMovementRequest_ForEachFedExAccount()
        {
            testObject.ForceVersionCapture = true;

            testObject.PerformVersionCapture(new ShipmentEntity());

            // Our repository is setup to return 3 accounts
            packageMovementRequest.Verify(r => r.Submit(), Times.Exactly(3));
        }

        [Fact]
        public void PerformVersionCapture_ThrowsFedExException_WhenMovementResponseIsNull()
        {
            testObject.ForceVersionCapture = true;

            // Setup the request to return a null value
            packageMovementRequest.Setup(r => r.Submit()).Returns((ICarrierResponse) null);

            Assert.Throws<FedExException>(() => testObject.PerformVersionCapture(new ShipmentEntity()));
        }

        [Fact]
        public void PerformVersionCapture_ThrowsFedExException_WhenUnexpectedResponseTypeIsReturned()
        {
            testObject.ForceVersionCapture = true;

            // Setup the request to return a null value
            packageMovementRequest.Setup(r => r.Submit()).Returns(new FedExShipResponse(null, null, null, null, null));

            Assert.Throws<FedExException>(() => testObject.PerformVersionCapture(new ShipmentEntity()));
        }

        [Fact]
        public void PerformVersionCapture_DelegatesToFactory_ToCreateVersionCaptureRequest_ForEachFedExAccount()
        {
            testObject.ForceVersionCapture = true;

            testObject.PerformVersionCapture(new ShipmentEntity());

            // Our repository is setup to return 3 accounts
            mock.Mock<IFedExRequestFactory>()
                .Verify(f => f.CreateVersionCaptureRequest(AnyShipment, It.IsAny<string>(), It.IsAny<FedExAccountEntity>()), Times.Exactly(3));
        }

        [Fact]
        public void PerformVersionCapture_DelegatesToVersionCaptureRequest_ForEachFedExAccount()
        {
            testObject.ForceVersionCapture = true;

            testObject.PerformVersionCapture(new ShipmentEntity());

            // Our repository is setup to return 3 accounts
            versionCaptureRequest.Verify(r => r.Submit(), Times.Exactly(3));
        }

        [Fact]
        public void PerformVersionCapture_SetsVersionCaptureFlagToTrue_WithActiveFedExAccounts()
        {
            testObject.PerformVersionCapture(new ShipmentEntity());

            Assert.True(testObject.HasDoneVersionCapture);
        }


        [Fact]
        public void PerformVersionCapture_SetsVersionCaptureFlagToTrue_WithoutActiveFedExAccounts()
        {
            // Setup the repository to return accounts that are not active
            settingsRepository.Setup(r => r.GetAccounts())
                .Returns
                (
                    new List<FedExAccountEntity>
                    {
                        new FedExAccountEntity { MeterNumber = string.Empty },
                        new FedExAccountEntity { MeterNumber = string.Empty }
                    }
                );

            testObject.PerformVersionCapture(new ShipmentEntity());
            Assert.True(testObject.HasDoneVersionCapture);
        }

        [Fact]
        public void Ship_ThrowsFedExException_WhenFedExAccountIsNull()
        {
            // Create the shipment and setup the repository to return a null account for this test
            settingsRepository.Setup(r => r.GetAccount(AnyShipment)).Returns<FedExAccountEntity>(null);

            Assert.Throws<FedExException>(() => testObject.Ship(shipmentEntity));
        }

        [Fact]
        public void Ship_WritesToErrorLog_WhenFedExAccountIsNull()
        {
            try
            {
                // Create the shipment and setup the repository to return a null account for this test
                shipmentEntity.ShipmentID = 1001;
                settingsRepository.Setup(r => r.GetAccount(AnyShipment)).Returns<FedExAccountEntity>(null);

                testObject.Ship(shipmentEntity);
            }
            // catch the exception that is thrown so we can verify the correct message gets logged
            catch (FedExException)
            { }
            finally
            {
                // Hard code the shipment IDn the expected error message since it was setup in the shipment above
                const string expectedErrorMessage = "Shipment ID 1001 does not have a FedEx account selected. Select a valid FedEx account that is available in ShipWorks.";
                mock.Mock<ILog>().Verify(l => l.Error(expectedErrorMessage), Times.Once());
            }
        }

        [Fact]
        public void Ship_DelegatesToRequestFactory_WhenCreatingShipRequest()
        {
            testObject.Ship(shipmentEntity);

            mock.Mock<IFedExRequestFactory>()
                .Verify(f => f.CreateShipRequest(shipmentEntity), Times.Exactly(2));
        }

        [Fact]
        public void Ship_SubmitsShippingRequest()
        {
            testObject.Ship(shipmentEntity);

            shippingRequest.Verify(r => r.Submit(), Times.Exactly(2));
        }

        [Fact]
        public void Ship_PerformsVersionCapture()
        {
            testObject.Ship(shipmentEntity);

            // Kind of a weak test due to the fact that the version capture is static, but we're
            // really just interested in the fact that the version capture has been performed
            Assert.True(testObject.HasDoneVersionCapture);
        }

        [Fact]
        public void Ship_CatchesSoapException_AndThrowsFedExSoapException()
        {
            // Setup the ship request to throw a soap exception
            shippingRequest.Setup(r => r.Submit()).Throws(new SoapException("Catch me!", XmlQualifiedName.Empty));

            Assert.Throws<FedExSoapCarrierException>(() => testObject.Ship(shipmentEntity));
        }

        [Fact]
        public void Ship_CatchesCarrierException_AndThrowsFedExException()
        {
            // Setup the ship request to throw an exception unrelated to a web request
            shippingRequest.Setup(r => r.Submit()).Throws(new CarrierException());

            Assert.Throws<FedExException>(() => testObject.Ship(shipmentEntity));
        }

        [Fact]
        public void Ship_CatchesWebException_AndThrowsFedExException()
        {
            // Setup the ship request to throw a "web-request-type" of exception
            shippingRequest.Setup(r => r.Submit()).Throws(new TimeoutException("This is slow"));

            Assert.Throws<FedExException>(() => testObject.Ship(shipmentEntity));
        }

        [Fact]
        public void Ship_CatchesNonWebException_AndThrowsExceptionOfSameType()
        {
            // Setup the ship request to throw an exception unrelated to a web request
            shippingRequest.Setup(r => r.Submit()).Throws(new ArgumentNullException());

            Assert.Throws<ArgumentNullException>(() => testObject.Ship(shipmentEntity));
        }

        [Fact]
        public void Ship_ThrowsFedExException_WhenOriginAddressHasMoreThanTwoLines()
        {
            shipmentEntity.OriginStreet1 = "street 1";
            shipmentEntity.OriginStreet2 = "street 2";
            shipmentEntity.OriginStreet3 = "street 3";

            Assert.Throws<FedExException>(() => testObject.Ship(shipmentEntity));
        }

        [Fact]
        public void Ship_WritesToLog_WhenOriginAddressHasMoreThanTwoLines()
        {
            try
            {
                shipmentEntity.ShipmentID = 12345;
                shipmentEntity.OriginStreet1 = "street 1";
                shipmentEntity.OriginStreet2 = "street 2";
                shipmentEntity.OriginStreet3 = "street 3";

                testObject.Ship(shipmentEntity);
            }
            catch (FedExException)
            {
                // just catch the exception so we can verify the log was written to
            }
            finally
            {
                // Hard-code the string and shipment ID since we're setting up the shipment entity above
                mock.Mock<ILog>().Verify(l => l.Error("Shipment ID 12345 cannot have three lines in the From Street Address."), Times.Once());
            }
        }

        [Fact]
        public void Ship_ThrowsFedExException_WhenShipAddressHasMoreThanTwoLines()
        {
            shipmentEntity.ShipStreet1 = "street 1";
            shipmentEntity.ShipStreet2 = "street 2";
            shipmentEntity.ShipStreet3 = "street 3";

            Assert.Throws<FedExException>(() => testObject.Ship(shipmentEntity));
        }

        [Fact]
        public void Ship_WritesToLog_WhenShipAddressHasMoreThanTwoLines()
        {
            try
            {
                shipmentEntity.ShipmentID = 12345;
                shipmentEntity.ShipStreet1 = "street 1";
                shipmentEntity.ShipStreet2 = "street 2";
                shipmentEntity.ShipStreet3 = "street 3";

                testObject.Ship(shipmentEntity);
            }
            catch (FedExException)
            {
                // just catch the exception so we can verify the log was written to
            }
            finally
            {
                // Hard-code the string and shipment ID since we're setting up the shipment entity above
                mock.Mock<ILog>().Verify(l => l.Error("Shipment ID 12345 cannot have three lines in the To Street Address."), Times.Once());
            }
        }

        #region CloseGround Tests

        [Fact]
        public void CloseGround_DelegatesToRequestFactory_WhenCreatingGroundCloseRequest()
        {
            FedExAccountEntity account = new FedExAccountEntity();

            testObject.CloseGround(account);

            // Make sure the account provided to the method is the one used to create the request
            mock.Mock<IFedExRequestFactory>()
                .Verify(f => f.CreateGroundCloseRequest(account), Times.Once());
        }

        [Fact]
        public void CloseGround_DelegatesToRequest_ToSumbitCloseRequest()
        {
            FedExAccountEntity account = new FedExAccountEntity();

            testObject.CloseGround(account);

            groundCloseRequest.Verify(r => r.Submit(), Times.Once());
        }

        [Fact]
        public void CloseGround_DelegatesToResponse_ToProcess()
        {
            FedExAccountEntity account = new FedExAccountEntity();

            testObject.CloseGround(account);

            groundCloseResponse.Verify(r => r.Process(), Times.Once());
        }

        [Fact]
        public void CloseGround_WritesToLog_WhenResponseTypeIsNotFedExGroundCloseResponse()
        {
            // The request is configured to return a mocked ICarrierResponse, so there's nothing else
            // to do for the setup of this test
            FedExAccountEntity account = new FedExAccountEntity();

            testObject.CloseGround(account);

            mock.Mock<ILog>().Verify(l => l.Info(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void CloseGround_CloseEntityIsNull_WhenResponseTypeIsNotFedExGroundCloseResponse()
        {
            // The request is configured to return a mocked ICarrierResponse, so there's nothing else
            // to do for the setup of this test
            FedExAccountEntity account = new FedExAccountEntity();

            FedExEndOfDayCloseEntity closeEntity = testObject.CloseGround(account);

            Assert.Null(closeEntity);
        }

        [Fact]
        public void CloseGround_CloseEntityIsNotNull_WhenResponseTypeIsFedExGroundCloseResponse()
        {
            // This is a borderline integration test rather than unit test, since we're returning a "real" ground close response which will
            // be processed from the test object. Since it's the real ground request we have to have the knowledge of the inner workings of
            // the response and configure the reply to simulate the close entity being populated (i.e. the integration test characteristics).
            ShipWorks.Shipping.Carriers.FedEx.WebServices.Close.Notification[] notifications = new ShipWorks.Shipping.Carriers.FedEx.WebServices.Close.Notification[]
            {
                new ShipWorks.Shipping.Carriers.FedEx.WebServices.Close.Notification() { Code = "8" }
            };

            GroundCloseReply closeReply = new GroundCloseReply { Notifications = notifications, HighestSeverity = ShipWorks.Shipping.Carriers.FedEx.WebServices.Close.NotificationSeverityType.SUCCESS };

            FedExGroundCloseResponse closeResponse = new FedExGroundCloseResponse(new List<IFedExCloseResponseManipulator>(), closeReply, groundCloseRequest.Object);
            groundCloseRequest.Setup(r => r.Submit()).Returns(closeResponse);

            FedExAccountEntity account = new FedExAccountEntity();
            FedExEndOfDayCloseEntity closeEntity = testObject.CloseGround(account);
            testObject.CloseGround(account);

            Assert.NotNull(closeEntity);
        }

        [Fact]
        public void CloseGround_CatchesSoapException_AndThrowsFedExSoapException()
        {
            groundCloseRequest.Setup(r => r.Submit()).Throws(new SoapException());

            FedExAccountEntity account = new FedExAccountEntity();
            Assert.Throws<FedExSoapCarrierException>(() => testObject.CloseGround(account));
        }

        [Fact]
        public void CloseGround_CatchesCarrierException_AndThrowsFedExException()
        {
            groundCloseRequest.Setup(r => r.Submit()).Throws(new CarrierException());

            FedExAccountEntity account = new FedExAccountEntity();
            Assert.Throws<FedExException>(() => testObject.CloseGround(account));
        }

        [Fact]
        public void CloseGround_CatchesNonWebException_AndThrowsExceptionOfSameType()
        {
            // Setup the ground request to throw an exception unrelated to a web request
            groundCloseRequest.Setup(r => r.Submit()).Throws(new ArgumentNullException());

            FedExAccountEntity account = new FedExAccountEntity();
            Assert.Throws<ArgumentNullException>(() => testObject.CloseGround(account));
        }

        [Fact]
        public void CloseGround_CatchesWebException_AndThrowsFedExException()
        {
            // Setup the ground request to throw a "web-request-type" of exception
            groundCloseRequest.Setup(r => r.Submit()).Throws(new TimeoutException("This is slow"));

            FedExAccountEntity account = new FedExAccountEntity();
            Assert.Throws<FedExException>(() => testObject.CloseGround(account));
        }

        #endregion CloseGround Tests

        #region CloseSmartPost Tests

        [Fact]
        public void CloseSmartPost_DelegatesToRequestFactory_WhenCreatingSmartPostCloseRequest()
        {
            FedExAccountEntity account = new FedExAccountEntity();

            testObject.CloseSmartPost(account);

            // Make sure the account provided to the method is the one used to create the request
            mock.Mock<IFedExRequestFactory>()
                .Verify(f => f.CreateSmartPostCloseRequest(account), Times.Once());
        }

        [Fact]
        public void CloseSmartPost_DelegatesToRequest_ToSumbitCloseRequest()
        {
            FedExAccountEntity account = new FedExAccountEntity();

            testObject.CloseSmartPost(account);

            smartPostCloseRequest.Verify(r => r.Submit(), Times.Once());
        }

        [Fact]
        public void CloseSmartPost_DelegatesToResponse_ToProcess()
        {
            FedExAccountEntity account = new FedExAccountEntity();

            testObject.CloseSmartPost(account);

            smartPostCloseResponse.Verify(r => r.Process(), Times.Once());
        }

        [Fact]
        public void CloseSmartPost_WritesToLog_WhenResponseTypeIsNotFedExSmartPostCloseResponse()
        {
            // The request is configured to return a mocked ICarrierResponse, so there's nothing else
            // to do for the setup of this test
            FedExAccountEntity account = new FedExAccountEntity();

            testObject.CloseSmartPost(account);

            mock.Mock<ILog>().Verify(l => l.Info(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void CloseSmartPost_CloseEntityIsNull_WhenResponseTypeIsNotFedExSmartPostCloseResponse()
        {
            // The request is configured to return a mocked ICarrierResponse, so there's nothing else
            // to do for the setup of this test
            FedExAccountEntity account = new FedExAccountEntity();

            FedExEndOfDayCloseEntity closeEntity = testObject.CloseSmartPost(account);

            Assert.Null(closeEntity);
        }

        [Fact]
        public void CloseSmartPost_CloseEntityIsNotNull_WhenResponseTypeIsFedExSmartPostCloseResponse()
        {
            // This is a borderline integration test rather than unit test, since we're returning a "real" smartPost close response which will
            // be processed from the test object. Since it's the real smartPost request we have to have the knowledge of the inner workings of
            // the response and configure the reply to simulate the close entity being populated (i.e. the integration test characteristics).
            ShipWorks.Shipping.Carriers.FedEx.WebServices.Close.Notification[] notifications = new ShipWorks.Shipping.Carriers.FedEx.WebServices.Close.Notification[]
            {
                new ShipWorks.Shipping.Carriers.FedEx.WebServices.Close.Notification() { Code = "8" }
            };

            SmartPostCloseReply closeReply = new SmartPostCloseReply { Notifications = notifications, HighestSeverity = ShipWorks.Shipping.Carriers.FedEx.WebServices.Close.NotificationSeverityType.SUCCESS };

            FedExSmartPostCloseResponse closeResponse = new FedExSmartPostCloseResponse(new List<IFedExCloseResponseManipulator>(), closeReply, smartPostCloseRequest.Object);
            smartPostCloseRequest.Setup(r => r.Submit()).Returns(closeResponse);

            FedExAccountEntity account = new FedExAccountEntity();
            FedExEndOfDayCloseEntity closeEntity = testObject.CloseSmartPost(account);
            testObject.CloseSmartPost(account);

            Assert.NotNull(closeEntity);
        }

        [Fact]
        public void CloseSmartPost_CatchesSoapException_AndThrowsFedExSoapException()
        {
            smartPostCloseRequest.Setup(r => r.Submit()).Throws(new SoapException());

            FedExAccountEntity account = new FedExAccountEntity();
            Assert.Throws<FedExSoapCarrierException>(() => testObject.CloseSmartPost(account));
        }


        [Fact]
        public void CloseSmartPost_CatchesCarrierException_AndThrowsFedExException()
        {
            smartPostCloseRequest.Setup(r => r.Submit()).Throws(new CarrierException());

            FedExAccountEntity account = new FedExAccountEntity();
            Assert.Throws<FedExException>(() => testObject.CloseSmartPost(account));
        }

        [Fact]
        public void CloseSmartPost_CatchesNonWebException_AndThrowsExceptionOfSameType()
        {
            // Setup the smartPost request to throw an exception unrelated to a web request
            smartPostCloseRequest.Setup(r => r.Submit()).Throws(new ArgumentNullException());

            FedExAccountEntity account = new FedExAccountEntity();
            Assert.Throws<ArgumentNullException>(() => testObject.CloseSmartPost(account));
        }

        [Fact]
        public void CloseSmartPost_CatchesWebException_AndThrowsFedExException()
        {
            // Setup the smartPost request to throw a "web-request-type" of exception
            smartPostCloseRequest.Setup(r => r.Submit()).Throws(new TimeoutException("This is slow"));

            FedExAccountEntity account = new FedExAccountEntity();
            Assert.Throws<FedExException>(() => testObject.CloseSmartPost(account));
        }


        #endregion CloseSmartPost Tests

        #region RegisterAccount Tests

        [Fact]
        public void RegisterAccount_DelegatesToRepository_ForShippingSettings()
        {
            FedExAccountEntity account = new FedExAccountEntity();

            // Setup the repository method so a null value is not returned
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity());

            testObject.RegisterAccount(account);

            settingsRepository.Verify(r => r.GetShippingSettings(), Times.Once());
        }

        [Fact]
        public void RegisterAccount_DelegatesToRequestFactoryForRegistrationRequest_WhenFedExUsernameIsNull()
        {
            FedExAccountEntity account = new FedExAccountEntity();

            // Setup the repository to return shipping settings with a null username for this test
            ShippingSettingsEntity shippingSettings = new ShippingSettingsEntity { FedExUsername = null };
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(shippingSettings);

            testObject.RegisterAccount(account);

            mock.Mock<IFedExRequestFactory>()
                .Verify(f => f.CreateRegisterCspUserRequest(account), Times.Once());
        }

        [Fact]
        public void RegisterAccount_SubmitsRegistrationRequest_WhenFedExUsernameIsNull()
        {
            FedExAccountEntity account = new FedExAccountEntity();

            // Setup the repository to return shipping settings with a null username for this test
            ShippingSettingsEntity shippingSettings = new ShippingSettingsEntity { FedExUsername = null };
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(shippingSettings);

            testObject.RegisterAccount(account);

            registrationRequest.Verify(r => r.Submit(), Times.Once());
        }

        [Fact]
        public void RegisterAccount_ProcessesRegistrationResponse_WhenFedExUsernameIsNull()
        {
            FedExAccountEntity account = new FedExAccountEntity();

            // Setup the repository to return shipping settings with a null username for this test
            ShippingSettingsEntity shippingSettings = new ShippingSettingsEntity { FedExUsername = null };
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(shippingSettings);

            testObject.RegisterAccount(account);

            registrationResponse.Verify(r => r.Process(), Times.Once());
        }

        [Fact]
        public void RegisterAccount_DoesNotDelegateToRequestFactoryForRegistrationRequest_WhenFedExUsernameIsNotNull()
        {
            FedExAccountEntity account = new FedExAccountEntity();

            // Setup the repository to return shipping settings with a null username for this test
            ShippingSettingsEntity shippingSettings = new ShippingSettingsEntity { FedExUsername = "username" };
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(shippingSettings);

            testObject.RegisterAccount(account);

            mock.Mock<IFedExRequestFactory>()
                .Verify(f => f.CreateRegisterCspUserRequest(account), Times.Never());
        }

        [Fact]
        public void RegisterAccount_DoesNotSubmitRegistrationRequest_WhenFedExUsernameIsNotNull()
        {
            FedExAccountEntity account = new FedExAccountEntity();

            // Setup the repository to return shipping settings with a null username for this test
            ShippingSettingsEntity shippingSettings = new ShippingSettingsEntity { FedExUsername = "username" };
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(shippingSettings);

            testObject.RegisterAccount(account);

            registrationRequest.Verify(r => r.Submit(), Times.Never());
        }

        [Fact]
        public void RegisterAccount_DoesNotProcessRegistrationResponse_WhenFedExUsernameIsNotNull()
        {
            FedExAccountEntity account = new FedExAccountEntity();

            // Setup the repository to return shipping settings with a null username for this test
            ShippingSettingsEntity shippingSettings = new ShippingSettingsEntity { FedExUsername = "username" };
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(shippingSettings);

            testObject.RegisterAccount(account);

            registrationResponse.Verify(r => r.Process(), Times.Never());
        }

        [Fact]
        public void RegisterAccount_DelegatesToRequestFactoryForSubscriptionRequest()
        {
            FedExAccountEntity account = new FedExAccountEntity();

            // Setup the repository to return shipping settings with a null username for this test
            ShippingSettingsEntity shippingSettings = new ShippingSettingsEntity { FedExUsername = "username" };
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(shippingSettings);

            testObject.RegisterAccount(account);

            // make sure the factory is called with the account provided to the register method
            mock.Mock<IFedExRequestFactory>()
                .Verify(f => f.CreateSubscriptionRequest(account), Times.Once());
        }

        [Fact]
        public void RegisterAccount_SubmitsSubscriptionRequest()
        {
            FedExAccountEntity account = new FedExAccountEntity();

            // Setup the repository to return shipping settings with a null username for this test
            ShippingSettingsEntity shippingSettings = new ShippingSettingsEntity { FedExUsername = "username" };
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(shippingSettings);

            testObject.RegisterAccount(account);

            subscriptionRequest.Verify(r => r.Submit(), Times.Once());
        }

        [Fact]
        public void RegisterAccount_ProcessesSubscriptionResponse()
        {
            FedExAccountEntity account = new FedExAccountEntity();

            // Setup the repository to return shipping settings with a null username for this test
            ShippingSettingsEntity shippingSettings = new ShippingSettingsEntity { FedExUsername = "username" };
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(shippingSettings);

            testObject.RegisterAccount(account);

            subscriptionResponse.Verify(r => r.Process(), Times.Once());
        }

        [Fact]
        public void RegisterAccount_CatchesSoapException_AndThrowsFedExSoapException()
        {
            // Just throw a soap exception
            settingsRepository.Setup(r => r.GetShippingSettings()).Throws(new SoapException());

            Assert.Throws<FedExSoapCarrierException>(() => testObject.RegisterAccount(new FedExAccountEntity()));
        }

        [Fact]
        public void RegisterAccount_WritesToLog_WhenSoapExceptionIsCaught()
        {
            // Just throw a soap exception
            settingsRepository.Setup(r => r.GetShippingSettings()).Throws(new SoapException());

            Assert.Throws<FedExSoapCarrierException>(() => testObject.RegisterAccount(new FedExAccountEntity()));
            mock.Mock<ILog>().Verify(l => l.Error(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void RegisterAccount_CatchesCarrierException_AndThrowsFedExException()
        {
            // Just throw a carrier exception
            settingsRepository.Setup(r => r.GetShippingSettings()).Throws(new CarrierException());

            Assert.Throws<FedExException>(() => testObject.RegisterAccount(new FedExAccountEntity()));
        }

        [Fact]
        public void RegisterAccount_WritesToLog_WhenCarrierExceptionIsCaught()
        {
            try
            {
                // Just throw a soap exception
                settingsRepository.Setup(r => r.GetShippingSettings()).Throws(new CarrierException());

                testObject.RegisterAccount(new FedExAccountEntity());
            }
            catch (FedExException)
            { }

            finally
            {
                mock.Mock<ILog>().Verify(l => l.Error(It.IsAny<string>()), Times.Once());
            }
        }

        [Fact]
        public void RegisterAccount_CatchesWebException_AndThrowsFedExException()
        {
            // Just throw a web-related exception
            settingsRepository.Setup(r => r.GetShippingSettings()).Throws(new TimeoutException("this is really slow"));

            Assert.Throws<FedExException>(() => testObject.RegisterAccount(new FedExAccountEntity()));
        }

        [Fact]
        public void RegisterAccount_WritesToLog_WhenWebExceptionIsCaught()
        {
            try
            {
                // Just throw a soap exception
                settingsRepository.Setup(r => r.GetShippingSettings()).Throws(new TimeoutException("this is slow"));

                testObject.RegisterAccount(new FedExAccountEntity());
            }
            catch (FedExException)
            { }

            finally
            {
                mock.Mock<ILog>().Verify(l => l.Error(It.IsAny<string>()), Times.Once());
            }
        }

        [Fact]
        public void RegisterAccount_CatchesNonWebException_AndThrowsExceptionOfSameType()
        {
            // Just throw a non-web exception
            settingsRepository.Setup(r => r.GetShippingSettings()).Throws(new ArgumentNullException());

            Assert.Throws<ArgumentNullException>(() => testObject.RegisterAccount(new FedExAccountEntity()));
        }


        [Fact]
        public void RegisterAccount_WritesToLog_WhenNonWebExceptionIsCaught()
        {
            try
            {
                // Just throw a soap exception
                settingsRepository.Setup(r => r.GetShippingSettings()).Throws(new ArgumentNullException());

                testObject.RegisterAccount(new FedExAccountEntity());
            }
            catch (ArgumentNullException)
            { }

            finally
            {
                mock.Mock<ILog>().Verify(l => l.Error(It.IsAny<string>()), Times.Once());
            }
        }

        #endregion RegisterAccount Tests

        #region GetRates Tests

        [Fact]
        public void GetRates_WritesWarningToLog_WhenCertificateRequestReturnsNone()
        {
            certificateRequest.Setup(r => r.Submit()).Returns(CertificateSecurityLevel.None);

            try
            {
                testObject.GetRates(shipmentEntity, new TrustingCertificateInspector());
            }
            catch (FedExException)
            { }

            mock.Mock<ILog>().Verify(l => l.Warn("The FedEx certificate did not pass inspection and could not be trusted."));
        }

        [Fact]
        public void GetRates_ThrowsFedExException_WhenCertificateRequestReturnsNone()
        {
            certificateRequest.Setup(r => r.Submit()).Returns(CertificateSecurityLevel.None);
            Assert.Throws<FedExException>(() => testObject.GetRates(shipmentEntity, new TrustingCertificateInspector()));
        }

        [Fact]
        public void GetRates_WritesWarningToLog_WhenCertificateRequestReturnsSpoofed()
        {
            certificateRequest.Setup(r => r.Submit()).Returns(CertificateSecurityLevel.Spoofed);

            try
            {
                testObject.GetRates(shipmentEntity, new TrustingCertificateInspector());
            }
            catch (FedExException)
            { }

            mock.Mock<ILog>().Verify(l => l.Warn("The FedEx certificate did not pass inspection and could not be trusted."));
        }

        [Fact]
        public void GetRates_ThrowsFedExException_WhenCertificateRequestReturnsSpoofed()
        {
            certificateRequest.Setup(r => r.Submit()).Returns(CertificateSecurityLevel.Spoofed);
            Assert.Throws<FedExException>(() => testObject.GetRates(shipmentEntity, new TrustingCertificateInspector()));
        }

        [Fact]
        public void GetRates_PerformsVersionCapture_WhenCertificateRequestReturnsTrusted()
        {
            certificateRequest.Setup(r => r.Submit()).Returns(CertificateSecurityLevel.Trusted);

            testObject.GetRates(shipmentEntity, new TrustingCertificateInspector());

            Assert.True(testObject.HasDoneVersionCapture);
        }


        [Fact]
        public void GetRates_PerformsVersionCapture()
        {
            testObject.GetRates(shipmentEntity, new TrustingCertificateInspector());

            // Kind of a weak test due to the fact that the version capture is static, but we're
            // really just interested in the fact that the version capture has been performed
            Assert.True(testObject.HasDoneVersionCapture);
        }

        [Fact]
        public void GetRates_ThrowsFedExException_WhenOriginStreet3IsNotEmpty()
        {
            shipmentEntity.OriginStreet3 = "desk";

            Assert.Throws<FedExException>(() => testObject.GetRates(shipmentEntity, new TrustingCertificateInspector()));
        }

        [Fact]
        public void GetRates_ThrowsFedExException_WhenShipStreet3IsNotEmpty()
        {
            shipmentEntity.ShipStreet3 = "desk";

            Assert.Throws<FedExException>(() => testObject.GetRates(shipmentEntity, new TrustingCertificateInspector()));
        }

        [Fact]
        public void GetRates_DelegatesToRequestFactory()
        {
            testObject.GetRates(shipmentEntity, new TrustingCertificateInspector());

            mock.Mock<IFedExRequestFactory>()
                .Verify(f => f.CreateRateRequest(), Times.Exactly(3));
        }

        [Fact]
        public void GetRates_DelegatesToRequest_ToSubmitRateRequest()
        {
            testObject.GetRates(shipmentEntity, new TrustingCertificateInspector());

            rateRequest.Verify(r => r.Submit(shipmentEntity));
            rateRequest.Verify(r => r.Submit(shipmentEntity, FedExRateRequestOptions.OneRate));
            rateRequest.Verify(r => r.Submit(shipmentEntity, FedExRateRequestOptions.SmartPost));
        }

        [Fact]
        public void GetRates_ProcessesResponse()
        {
            testObject.GetRates(shipmentEntity, new TrustingCertificateInspector());

            rateResponse.Verify(r => r.Process(), Times.Exactly(3));
        }

        [Fact]
        public void GetRates_AssignsTransitTime_WhenDeliveryTimeStampSpecifiedIsTrue()
        {
            DateTime shipDate = DateTime.Now;

            // Setup the delivery date to be four days from now
            shipmentEntity.ShipDate = shipDate;

            nativeRateReply.RateReplyDetails[0].DeliveryTimestampSpecified = true;
            nativeRateReply.RateReplyDetails[0].DeliveryTimestamp = shipDate.AddDays(4);

            RateGroup rates = testObject.GetRates(shipmentEntity, new TrustingCertificateInspector());

            Assert.Equal($"4 ({shipDate.AddDays(4).ToString("dddd h:mm tt")})", rates.Rates.First().Days);
        }

        [Fact]
        public void GetRates_AssignsTransitTime_WhenDeliveryTimeStampSpecifiedIsFalse_AndTransitTimeSpecifiedIsTrue()
        {
            shipmentEntity.ShipDate = new DateTime(2017, 3, 21);

            // Setup the transit time to be eleven days
            nativeRateReply.RateReplyDetails[0].DeliveryTimestampSpecified = false;
            nativeRateReply.RateReplyDetails[0].TransitTime = TransitTimeType.ELEVEN_DAYS;
            nativeRateReply.RateReplyDetails[0].TransitTimeSpecified = true;

            mock.Mock<IFedExRateResponse>()
                .Setup(x => x.Process())
                .Returns(nativeRateReply);

            RateGroup rates = testObject.GetRates(shipmentEntity, new TrustingCertificateInspector());

            Assert.StartsWith("11", rates.Rates.First().Days);
        }

        [Fact]
        public void GetRates_TransitTimeIsEmpty_WhenDeliveryTimeStampSpecifiedIsFalse_AndTransitTimeSpecifiedIsFalse()
        {
            // Setup the transit time to be eleven days
            nativeRateReply.RateReplyDetails[0].DeliveryTimestampSpecified = false;
            nativeRateReply.RateReplyDetails[0].TransitTime = TransitTimeType.ELEVEN_DAYS;
            nativeRateReply.RateReplyDetails[0].TransitTimeSpecified = false;

            RateGroup rates = testObject.GetRates(shipmentEntity, new TrustingCertificateInspector());

            Assert.Equal(string.Empty, rates.Rates.First().Days);
        }

        [Fact]
        public void GetRates_GrabsSecondRateWhenItIsActualRateType()
        {
            // Setup the native request to have a priority freight rate
            nativeRateReply = new RateReply
            {
                RateReplyDetails = new[]
                {
                    new RateReplyDetail()
                    {
                        ActualRateTypeSpecified = true,
                        ActualRateType = ReturnedRateType.PAYOR_ACCOUNT_SHIPMENT,

                        RatedShipmentDetails = new RatedShipmentDetail[]
                        {
                            new RatedShipmentDetail
                            {
                                ShipmentRateDetail = new ShipmentRateDetail
                                {
                                    RateTypeSpecified = true,
                                    RateType = ReturnedRateType.PAYOR_ACCOUNT_PACKAGE,

                                    TotalNetCharge = new Money { Amount = 40.12M },
                                    RatedWeightMethod = RatedWeightMethod.ACTUAL,
                                    RatedWeightMethodSpecified = true,
                                    TotalBillingWeight = new Weight { Value = 0.1M },
                                    TotalDimWeight = new Weight { Value = 0.1M }
                                }
                            },
                            new RatedShipmentDetail
                            {
                                ShipmentRateDetail = new ShipmentRateDetail
                                {
                                    RateTypeSpecified = true,
                                    RateType = ReturnedRateType.PAYOR_ACCOUNT_SHIPMENT,

                                    TotalNetCharge = new Money { Amount = 42.42M },
                                    RatedWeightMethod = RatedWeightMethod.ACTUAL,
                                    RatedWeightMethodSpecified = true,
                                    TotalBillingWeight = new Weight { Value = 0.1M },
                                    TotalDimWeight = new Weight { Value = 0.1M }
                                }
                            }
                        },
                        ServiceType = ServiceType.FEDEX_GROUND
                    }
                }
            };

            // Setup the requests to return the native rate reply
            rateResponse.Setup(r => r.Process()).Returns(nativeRateReply);

            RateGroup rates = testObject.GetRates(shipmentEntity, new TrustingCertificateInspector());

            // We should get rates back (priority overnight for basic, smart post, and One Rate)
            Assert.Equal(42.42M, rates.Rates.First().AmountOrDefault);
        }

        [Fact]
        public void GetRates_RemovesPriorityFreightRates()
        {
            // Setup the native request to have a priority freight rate
            nativeRateReply = new RateReply
            {
                RateReplyDetails = new RateReplyDetail[]
                {
                    new RateReplyDetail()
                    {
                        ActualRateTypeSpecified = true,
                        ActualRateType = ReturnedRateType.PAYOR_ACCOUNT_PACKAGE,

                        RatedShipmentDetails = new RatedShipmentDetail[]
                        {
                            new RatedShipmentDetail
                            {
                                ShipmentRateDetail = new ShipmentRateDetail
                                {
                                    RateTypeSpecified = true,
                                    RateType = ReturnedRateType.PAYOR_ACCOUNT_PACKAGE,

                                    TotalNetCharge = new Money { Amount = 40.12M },
                                    RatedWeightMethod = RatedWeightMethod.ACTUAL,
                                    RatedWeightMethodSpecified = true,
                                    TotalBillingWeight = new Weight { Value = 0.1M },
                                    TotalDimWeight = new Weight { Value = 0.1M }
                                }
                            }
                        },
                        ServiceType = ServiceType.FEDEX_FREIGHT_PRIORITY
                    },
                    new RateReplyDetail()
                    {
                        ActualRateTypeSpecified = true,
                        ActualRateType = ReturnedRateType.PAYOR_ACCOUNT_PACKAGE,

                        RatedShipmentDetails = new RatedShipmentDetail[]
                        {
                            new RatedShipmentDetail
                            {
                                ShipmentRateDetail = new ShipmentRateDetail
                                {
                                    RateTypeSpecified = true,
                                    RateType = ReturnedRateType.PAYOR_ACCOUNT_PACKAGE,

                                    TotalNetCharge = new Money { Amount = 40.12M },
                                    RatedWeightMethod = RatedWeightMethod.ACTUAL,
                                    RatedWeightMethodSpecified = true,
                                    TotalBillingWeight = new Weight { Value = 0.1M },
                                    TotalDimWeight = new Weight { Value = 0.1M }
                                }
                            }
                        },
                        ServiceType = ServiceType.PRIORITY_OVERNIGHT
                    }
                }
            };

            // Setup the requests to return the native rate reply
            rateResponse.Setup(r => r.Process()).Returns(nativeRateReply);

            RateGroup rates = testObject.GetRates(shipmentEntity, new TrustingCertificateInspector());

            // We should get rates back (priority overnight for basic, smart post, and One Rate)
            Assert.Equal(3, rates.Rates.Count);
        }

        [Fact]
        public void GetRates_RemovesEconomyFreightRates()
        {
            // Setup the native request to have an economy freight rate
            nativeRateReply = new RateReply
            {
                RateReplyDetails = new RateReplyDetail[]
                {
                    new RateReplyDetail()
                    {
                        ActualRateTypeSpecified = true,
                        ActualRateType = ReturnedRateType.PAYOR_ACCOUNT_PACKAGE,

                        RatedShipmentDetails = new RatedShipmentDetail[]
                        {
                            new RatedShipmentDetail
                            {
                                ShipmentRateDetail = new ShipmentRateDetail
                                {
                                    RateTypeSpecified = true,
                                    RateType = ReturnedRateType.PAYOR_ACCOUNT_PACKAGE,
                                    TotalNetCharge = new Money { Amount = 40.12M },
                                    RatedWeightMethod = RatedWeightMethod.ACTUAL,
                                    RatedWeightMethodSpecified = true
                                }
                            }
                        },
                        ServiceType = ServiceType.FEDEX_FREIGHT_ECONOMY
                    },
                    new RateReplyDetail()
                    {
                        ActualRateTypeSpecified = true,
                        ActualRateType = ReturnedRateType.PAYOR_ACCOUNT_PACKAGE,

                        RatedShipmentDetails = new RatedShipmentDetail[]
                        {
                            new RatedShipmentDetail
                            {
                                ShipmentRateDetail = new ShipmentRateDetail
                                {
                                    RateTypeSpecified = true,
                                    RateType = ReturnedRateType.PAYOR_ACCOUNT_PACKAGE,

                                    TotalNetCharge = new Money { Amount = 40.12M },
                                    RatedWeightMethod = RatedWeightMethod.ACTUAL,
                                    RatedWeightMethodSpecified = true
                                }
                            }
                        },
                        ServiceType = ServiceType.PRIORITY_OVERNIGHT
                    }
                }
            };

            // Setup the requests to return the native rate reply
            rateResponse.Setup(r => r.Process()).Returns(nativeRateReply);

            RateGroup rates = testObject.GetRates(shipmentEntity, new TrustingCertificateInspector());

            // We should get rates back (priority overnight for basic, smart post, and One Rate)
            Assert.Equal(3, rates.Rates.Count);
        }

        [Fact]
        public void GetRates_SetsAmountToTotalNetCharge()
        {
            nativeRateReply.RateReplyDetails[0].RatedShipmentDetails[0].ShipmentRateDetail.TotalNetCharge.Amount = 43.85M;

            RateGroup rates = testObject.GetRates(shipmentEntity, new TrustingCertificateInspector());

            Assert.Equal(43.85M, rates.Rates.First().AmountOrDefault);
        }

        [Fact]
        public void GetRates_RateGroupCount_MatchesRateReplyDetailsCount()
        {
            nativeRateReply.RateReplyDetails[0].RatedShipmentDetails[0].ShipmentRateDetail.TotalNetCharge.Amount = 43.85M;

            RateGroup rates = testObject.GetRates(shipmentEntity, new TrustingCertificateInspector());

            // Multiply by two to account for the basic rate, smart post rates, and One Rate rates
            Assert.Equal(nativeRateReply.RateReplyDetails.Length * 3, rates.Rates.Count);
        }

        [Fact]
        public void GetRates_CreatesSmartPostRequest_WithRequestFactory()
        {
            testObject.GetRates(shipmentEntity, new TrustingCertificateInspector());

            rateRequest.Verify(x => x.Submit(shipmentEntity, FedExRateRequestOptions.SmartPost));
        }

        [Fact]
        public void GetRates_CreatesOneRateRequest_WithRequestFactory()
        {
            testObject.GetRates(shipmentEntity, new TrustingCertificateInspector());

            rateRequest.Verify(x => x.Submit(shipmentEntity, FedExRateRequestOptions.OneRate));
        }

        [Fact]
        public void GetRates_CatchesSoapException_AndThrowsFedExSoapException()
        {
            // Just throw a soap exception
            rateRequest.Setup(r => r.Submit(AnyIShipment)).Throws(new SoapException());

            Assert.Throws<FedExSoapCarrierException>(() => testObject.GetRates(shipmentEntity, new TrustingCertificateInspector()));
        }

        [Fact]
        public void GetRates_WritesToLog_WhenSoapExceptionIsCaught()
        {
            try
            {
                // Just throw a soap exception
                rateRequest.Setup(r => r.Submit(AnyIShipment)).Throws(new SoapException());

                testObject.GetRates(shipmentEntity, new TrustingCertificateInspector());
            }
            catch (FedExSoapCarrierException)
            { }

            finally
            {
                mock.Mock<ILog>().Verify(l => l.Error(It.IsAny<string>()), Times.Once());
            }
        }

        [Fact]
        public void GetRates_CatchesCarrierException_AndThrowsFedExException()
        {
            // Just throw a carrier exception
            rateRequest.Setup(r => r.Submit(AnyIShipment)).Throws(new CarrierException());

            Assert.Throws<FedExException>(() => testObject.GetRates(shipmentEntity, new TrustingCertificateInspector()));
        }

        [Fact]
        public void GetRates_WritesToLog_WhenCarrierExceptionIsCaught()
        {
            try
            {
                // Just throw a soap exception
                rateRequest.Setup(r => r.Submit(AnyIShipment)).Throws(new CarrierException());

                testObject.GetRates(shipmentEntity, new TrustingCertificateInspector());
            }
            catch (FedExException)
            { }

            finally
            {
                mock.Mock<ILog>().Verify(l => l.Error(It.IsAny<string>()), Times.Once());
            }
        }

        [Fact]
        public void GetRates_CatchesWebException_AndThrowsFedExException()
        {
            // Just throw a web-related exception
            rateRequest.Setup(r => r.Submit(AnyIShipment)).Throws(new TimeoutException("this is really slow"));

            Assert.Throws<FedExException>(() => testObject.GetRates(shipmentEntity, new TrustingCertificateInspector()));
        }

        [Fact]
        public void GetRates_WritesToLog_WhenWebExceptionIsCaught()
        {
            try
            {
                // Just throw a soap exception
                rateRequest.Setup(r => r.Submit(AnyIShipment)).Throws(new TimeoutException("this is slow"));

                testObject.GetRates(shipmentEntity, new TrustingCertificateInspector());
            }
            catch (FedExException)
            { }

            finally
            {
                mock.Mock<ILog>().Verify(l => l.Error(It.IsAny<string>()), Times.Once());
            }
        }

        [Fact]
        public void GetRates_CatchesNonWebException_AndThrowsExceptionOfSameType()
        {
            // Just throw a non-web exception
            rateRequest.Setup(r => r.Submit(AnyIShipment)).Throws(new ArgumentNullException());

            Assert.Throws<ArgumentNullException>(() => testObject.GetRates(shipmentEntity, new TrustingCertificateInspector()));
        }

        [Fact]
        public void GetRates_WritesToLog_WhenNonWebExceptionIsCaught()
        {
            try
            {
                // Just throw a soap exception
                rateRequest.Setup(r => r.Submit(AnyIShipment)).Throws(new ArgumentNullException());

                testObject.GetRates(shipmentEntity, new TrustingCertificateInspector());
            }
            catch (ArgumentNullException)
            { }

            finally
            {
                mock.Mock<ILog>().Verify(l => l.Error(It.IsAny<string>()), Times.Once());
            }
        }

        [Fact]
        public void GetRates_DoesNotThrowException_WhenSmartPostRatesCatchesFedExException()
        {
            rateRequest
                .Setup(x => x.Submit(AnyShipment, FedExRateRequestOptions.SmartPost))
                .Throws<FedExException>();

            testObject.GetRates(shipmentEntity, new TrustingCertificateInspector());
        }

        [Fact]
        public void GetRates_WritesWarningToLog_WhenSmartPostRatesCatchesFedExException()
        {
            rateRequest
                .Setup(x => x.Submit(AnyShipment, FedExRateRequestOptions.SmartPost))
                .Throws<FedExException>();

            testObject.GetRates(shipmentEntity, new TrustingCertificateInspector());

            mock.Mock<ILog>().Verify(l => l.Warn(It.Is<string>(s => s.Contains("Error getting SmartPost rates"))), Times.Once());
        }

        [Fact]
        public void GetRates_DoesNotThrowException_WhenSmartPostRatesCatchesFedExApiException()
        {
            rateRequest
                .Setup(x => x.Submit(AnyShipment, FedExRateRequestOptions.SmartPost))
                .Throws(new FedExApiCarrierException(new Notification[0]));

            testObject.GetRates(shipmentEntity, new TrustingCertificateInspector());
        }

        [Fact]
        public void GetRates_WritesWarningToLog_WhenSmartPostRatesCatchesFedExApiException()
        {
            rateRequest
                .Setup(x => x.Submit(AnyShipment, FedExRateRequestOptions.SmartPost))
                .Throws(new FedExApiCarrierException(new Notification[0]));

            testObject.GetRates(shipmentEntity, new TrustingCertificateInspector());

            mock.Mock<ILog>().Verify(l => l.Warn(It.Is<string>(s => s.Contains("Error getting SmartPost rates"))), Times.Once());
        }

        #endregion GetRates Tests
    }
}
