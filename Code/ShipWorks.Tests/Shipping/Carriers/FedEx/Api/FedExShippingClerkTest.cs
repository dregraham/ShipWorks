using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services.Protocols;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Close.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.Close.Response.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Api.PackageMovement.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Close;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping;
using log4net;
using Notification = ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate.Notification;
using ServiceType = ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate.ServiceType;
using Interapptive.Shared.Net;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Enums;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api
{
    [TestClass]
    public class FedExShippingClerkTest
    {
        private FedExShippingClerk testObject;

        private Mock<ICarrierSettingsRepository> settingsRepository;
        private Mock<ICertificateInspector> certificateInspector;
        private Mock<ICertificateRequest> certificateRequest;

        private Mock<IFedExRequestFactory> requestFactory;
        private Mock<ILog> log;

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

        private Mock<CarrierRequest> rateRequest;
        private Mock<ICarrierResponse> rateResponse;
        private RateReply nativeRateReply;

        private PostalCodeInquiryReply reply;

        private Mock<ILabelRepository> labelRepository;

        private ShipmentEntity shipmentEntity;

        [TestInitialize]
        public void Initialize()
        {
            log = new Mock<ILog>();
            log.Setup(l => l.Info(It.IsAny<string>()));
            log.Setup(l => l.Error(It.IsAny<string>()));

            settingsRepository = new Mock<ICarrierSettingsRepository>();
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
            settingsRepository.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>())).Returns(new FedExAccountEntity() { MeterNumber = "123" });

            certificateInspector = new Mock<ICertificateInspector>();
            certificateInspector.Setup(i => i.Inspect(It.IsAny<ICertificateRequest>())).Returns(CertificateSecurityLevel.Trusted);

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

            rateResponse = new Mock<ICarrierResponse>();
            rateResponse.Setup(r => r.Process());
            rateResponse.Setup(r => r.NativeResponse).Returns(nativeRateReply);

            rateRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), null);
            rateRequest.Setup(r => r.Submit()).Returns(rateResponse.Object);



            requestFactory = new Mock<IFedExRequestFactory>();
            requestFactory.Setup(f => f.CreatePackageMovementRequest(It.IsAny<ShipmentEntity>(), It.IsAny<FedExAccountEntity>())).Returns(packageMovementRequest.Object);
            requestFactory.Setup(f => f.CreateShipRequest(It.IsAny<ShipmentEntity>())).Returns(shippingRequest.Object);
            requestFactory.Setup(f => f.CreateVersionCaptureRequest(It.IsAny<ShipmentEntity>(), It.IsAny<string>(), It.IsAny<FedExAccountEntity>())).Returns(versionCaptureRequest.Object);
            requestFactory.Setup(f => f.CreateGroundCloseRequest(It.IsAny<FedExAccountEntity>())).Returns(groundCloseRequest.Object);
            requestFactory.Setup(f => f.CreateSmartPostCloseRequest(It.IsAny<FedExAccountEntity>())).Returns(smartPostCloseRequest.Object);
            requestFactory.Setup(f => f.CreateRegisterCspUserRequest(It.IsAny<FedExAccountEntity>())).Returns(registrationRequest.Object);
            requestFactory.Setup(f => f.CreateSubscriptionRequest(It.IsAny<FedExAccountEntity>())).Returns(subscriptionRequest.Object);
            requestFactory.Setup(f => f.CreateRateRequest(It.IsAny<ShipmentEntity>(), null)).Returns(rateRequest.Object);
            requestFactory.Setup(f => f.CreateRateRequest(It.IsAny<ShipmentEntity>(), It.IsAny<List<ICarrierRequestManipulator>>())).Returns(rateRequest.Object);
            requestFactory.Setup(f => f.CreateCertificateRequest(It.IsAny<ICertificateInspector>())).Returns(certificateRequest.Object);

            labelRepository = new Mock<ILabelRepository>();
            labelRepository.Setup(f => f.ClearReferences(It.IsAny<ShipmentEntity>()));

            shipmentEntity = BuildFedExShipmentEntity.SetupBaseShipmentEntity();
            shipmentEntity.FedEx.SmartPostHubID = "5571";

            Mock<IExcludedServiceTypeRepository> excludedServiceTypeRepository = new Mock<IExcludedServiceTypeRepository>();
            excludedServiceTypeRepository.Setup(x => x.GetExcludedServiceTypes(It.IsAny<ShipmentType>()))
                .Returns(new List<ExcludedServiceTypeEntity> { new ExcludedServiceTypeEntity((int) ShipmentTypeCode.FedEx, (int) FedExServiceType.FedExGround) });

            // Force our test object to perform version capture when called.
            testObject = new FedExShippingClerk(settingsRepository.Object, certificateInspector.Object, requestFactory.Object, log.Object, true, labelRepository.Object, excludedServiceTypeRepository.Object);
        }

        [TestMethod]
        public void PerformVersionCapture_WritesToLog_Test()
        {
            testObject.PerformVersionCapture(new ShipmentEntity());

            // The string should indicate the capture was forced since our test object was configured as such
            log.Verify(l => l.Info("Performing FedEx version capture (forced)"), Times.Once());
        }

        [TestMethod]
        public void PerformVersionCapture_DelegatesToFactory_ToCreatePackageMovementRequest_ForEachFedExAccountTest()
        {
            testObject.PerformVersionCapture(new ShipmentEntity());

            // Our repository is setup to return 3 accounts
            requestFactory.Verify(f => f.CreatePackageMovementRequest(It.IsAny<ShipmentEntity>(), It.IsAny<FedExAccountEntity>()), Times.Exactly(3));
        }

        [TestMethod]
        public void PerformVersionCapture_DelegatesToPackageMovementRequest_ForEachFedExAccount_Test()
        {
            testObject.PerformVersionCapture(new ShipmentEntity());

            // Our repository is setup to return 3 accounts
            packageMovementRequest.Verify(r => r.Submit(), Times.Exactly(3));
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void PerformVersionCapture_ThrowsFedExException_WhenMovementResponseIsNull_Test()
        {
            // Setup the request to return a null value
            packageMovementRequest.Setup(r => r.Submit()).Returns((ICarrierResponse) null);

            testObject.PerformVersionCapture(new ShipmentEntity());
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void PerformVersionCapture_ThrowsFedExException_WhenUnexpectedResponseTypeIsReturned_Test()
        {
            // Setup the request to return a null value
            packageMovementRequest.Setup(r => r.Submit()).Returns(new FedExShipResponse(null, null, null, null, null));

            testObject.PerformVersionCapture(new ShipmentEntity());
        }

        [TestMethod]
        public void PerformVersionCapture_DelegatesToFactory_ToCreateVersionCaptureRequest_ForEachFedExAccount_Test()
        {
            testObject.PerformVersionCapture(new ShipmentEntity());

            // Our repository is setup to return 3 accounts
            requestFactory.Verify(f => f.CreateVersionCaptureRequest(It.IsAny<ShipmentEntity>(), It.IsAny<string>(), It.IsAny<FedExAccountEntity>()), Times.Exactly(3));
        }

        [TestMethod]
        public void PerformVersionCapture_DelegatesToVersionCaptureRequest_ForEachFedExAccount_Test()
        {
            testObject.PerformVersionCapture(new ShipmentEntity());

            // Our repository is setup to return 3 accounts
            versionCaptureRequest.Verify(r => r.Submit(), Times.Exactly(3));
        }

        [TestMethod]
        public void PerformVersionCapture_SetsVersionCaptureFlagToTrue_WithActiveFedExAccounts_Test()
        {
            testObject.PerformVersionCapture(new ShipmentEntity());

            Assert.IsTrue(testObject.HasDoneVersionCapture);
        }


        [TestMethod]
        public void PerformVersionCapture_SetsVersionCaptureFlagToTrue_WithoutActiveFedExAccounts_Test()
        {
            // Setup the repositor to return accounts that are not active
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
            Assert.IsTrue(testObject.HasDoneVersionCapture);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void Ship_ThrowsFedExException_WhenFedExAccountIsNull_Test()
        {
            // Create the shipment and setup the repository to return a null account for this test
            settingsRepository.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>())).Returns<FedExAccountEntity>(null);

            testObject.Ship(shipmentEntity);
        }

        [TestMethod]
        public void Ship_WritesToErrorLog_WhenFedExAccountIsNull_Test()
        {
            try
            {
                // Create the shipment and setup the repository to return a null account for this test
                shipmentEntity.ShipmentID = 1001;
                settingsRepository.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>())).Returns<FedExAccountEntity>(null);

                testObject.Ship(shipmentEntity);
            }
                // catch the exception that is thrown so we can verify the correct message gets logged
            catch (FedExException)
            {}
            finally
            {
                // Hard code the shipment IDn the expected error message since it was setup in the shipment above
                const string expectedErrorMessage = "Shipment ID 1001 does not have a FedEx account selected. Select a valid FedEx account that is available in ShipWorks.";
                log.Verify(l => l.Error(expectedErrorMessage), Times.Once());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void Ship_ThrowsFedExException_WhenFedExAccountIsPending2xMigration_Test()
        {
            // Create the shipment and setup the repository to return an account that needs to be 
            // migrated for this test (indicated by the meter number)
            settingsRepository.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>())).Returns(new FedExAccountEntity() { MeterNumber = string.Empty });

            testObject.Ship(shipmentEntity);
        }

        [TestMethod]
        public void Ship_WritesToErrorLog_WhenFedExAccountIsPending2xMigration_Test()
        {
            try
            {
                // Create the shipment and setup the repository to return an account that needs to be 
                // migrated for this test (indicated by the meter number)
                settingsRepository.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>())).Returns(new FedExAccountEntity() { AccountNumber = "123ABC", MeterNumber = string.Empty });

                testObject.Ship(shipmentEntity);
            }
                // catch the exception that is thrown so we can verify the correct message gets logged
            catch (FedExException)
            {}
            finally
            {
                // Hard code the account number in the expected error message since it is being mocked
                const string expectedErrorMessage = "Attempt to use a FedEx account migrated from ShipWorks 2 that has not been configured for ShipWorks 3. The FedEx account (account number 123ABC) needs to be configured for ShipWorks3.";
                log.Verify(l => l.Error(expectedErrorMessage), Times.Once());
            }
        }

        [TestMethod]
        public void Ship_DelegatesToRequestFactory_WhenCreatingShipRequest_Test()
        {
            testObject.Ship(shipmentEntity);

            requestFactory.Verify(f => f.CreateShipRequest(shipmentEntity), Times.Exactly(2));
        }

        [TestMethod]
        public void Ship_SubmitsShippingRequest_Test()
        {
            testObject.Ship(shipmentEntity);

            shippingRequest.Verify(r => r.Submit(), Times.Exactly(2));
        }

        [TestMethod]
        public void Ship_PerformsVersionCapture_Test()
        {
            testObject.Ship(shipmentEntity);

            // Kind of a weak test due to the fact that the version capture is static, but we're
            // really just interested in the fact that the version capture has been performed
            Assert.IsTrue(testObject.HasDoneVersionCapture);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExSoapCarrierException))]
        public void Ship_CatchesSoapException_AndThrowsFedExSoapException_Test()
        {
            // Setup the ship request to throw a soap exception
            shippingRequest.Setup(r => r.Submit()).Throws(new SoapException("Catch me!", XmlQualifiedName.Empty));

            testObject.Ship(shipmentEntity);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void Ship_CatchesCarrierException_AndThrowsFedExException_Test()
        {
            // Setup the ship request to throw an exception unrelated to a web request
            shippingRequest.Setup(r => r.Submit()).Throws(new CarrierException());

            testObject.Ship(shipmentEntity);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void Ship_CatchesWebException_AndThrowsFedExException_Test()
        {
            // Setup the ship request to throw a "web-request-type" of exception 
            shippingRequest.Setup(r => r.Submit()).Throws(new TimeoutException("This is slow"));

            testObject.Ship(shipmentEntity);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ship_CatchesNonWebException_AndThrowsExceptionOfSameType_Test()
        {
            // Setup the ship request to throw an exception unrelated to a web request
            shippingRequest.Setup(r => r.Submit()).Throws(new ArgumentNullException());

            testObject.Ship(shipmentEntity);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void Ship_ThrowsFedExException_WhenOriginAddressHasMoreThanTwoLines_Test()
        {
            shipmentEntity.OriginStreet1 = "street 1";
            shipmentEntity.OriginStreet2 = "street 2";
            shipmentEntity.OriginStreet3 = "street 3";

            testObject.Ship(shipmentEntity);
        }

        [TestMethod]
        public void Ship_WritesToLog_WhenOriginAddressHasMoreThanTwoLines_Test()
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
                log.Verify(l => l.Error("Shipment ID 12345 cannot have three lines in the From Street Address."), Times.Once());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void Ship_ThrowsFedExException_WhenShipAddressHasMoreThanTwoLines_Test()
        {
            shipmentEntity.ShipStreet1 = "street 1";
            shipmentEntity.ShipStreet2 = "street 2";
            shipmentEntity.ShipStreet3 = "street 3";

            testObject.Ship(shipmentEntity);
        }

        [TestMethod]
        public void Ship_WritesToLog_WhenShipAddressHasMoreThanTwoLines_Test()
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
                log.Verify(l => l.Error("Shipment ID 12345 cannot have three lines in the To Street Address."), Times.Once());
            }
        }

        #region CloseGround Tests

        [TestMethod]
        public void CloseGround_DelegatesToRequestFactory_WhenCreatingGroundCloseRequest_Test()
        {
            FedExAccountEntity account = new FedExAccountEntity();

            testObject.CloseGround(account);

            // Make sure the account provided to the method is the one used to create the request
            requestFactory.Verify(f => f.CreateGroundCloseRequest(account), Times.Once());
        }

        [TestMethod]
        public void CloseGround_DelegatesToRequest_ToSumbitCloseRequest_Test()
        {
            FedExAccountEntity account = new FedExAccountEntity();

            testObject.CloseGround(account);

            groundCloseRequest.Verify(r => r.Submit(), Times.Once());
        }

        [TestMethod]
        public void CloseGround_DelegatesToResponse_ToProcess_Test()
        {
            FedExAccountEntity account = new FedExAccountEntity();

            testObject.CloseGround(account);

            groundCloseResponse.Verify(r => r.Process(), Times.Once());
        }

        [TestMethod]
        public void CloseGround_WritesToLog_WhenResponseTypeIsNotFedExGroundCloseResponse_Test()
        {
            // The request is configured to return a mocked ICarrierResponse, so there's nothing else
            // to do for the setup of this test
            FedExAccountEntity account = new FedExAccountEntity();

            testObject.CloseGround(account);

            log.Verify(l => l.Info(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CloseGround_CloseEntityIsNull_WhenResponseTypeIsNotFedExGroundCloseResponse_Test()
        {
            // The request is configured to return a mocked ICarrierResponse, so there's nothing else
            // to do for the setup of this test
            FedExAccountEntity account = new FedExAccountEntity();

            FedExEndOfDayCloseEntity closeEntity = testObject.CloseGround(account);

            Assert.IsNull(closeEntity);
        }

        [TestMethod]
        public void CloseGround_CloseEntityIsNotNull_WhenResponseTypeIsFedExGroundCloseResponse_Test()
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

            Assert.IsNotNull(closeEntity);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExSoapCarrierException))]
        public void CloseGround_CatchesSoapException_AndThrowsFedExSoapException_Test()
        {
            groundCloseRequest.Setup(r => r.Submit()).Throws(new SoapException());

            FedExAccountEntity account = new FedExAccountEntity();
            testObject.CloseGround(account);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void CloseGround_CatchesCarrierException_AndThrowsFedExException_Test()
        {
            groundCloseRequest.Setup(r => r.Submit()).Throws(new CarrierException());

            FedExAccountEntity account = new FedExAccountEntity();
            testObject.CloseGround(account);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CloseGround_CatchesNonWebException_AndThrowsExceptionOfSameType_Test()
        {
            // Setup the ground request to throw an exception unrelated to a web request
            groundCloseRequest.Setup(r => r.Submit()).Throws(new ArgumentNullException());

            FedExAccountEntity account = new FedExAccountEntity();
            testObject.CloseGround(account);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void CloseGround_CatchesWebException_AndThrowsFedExException_Test()
        {
            // Setup the ground request to throw a "web-request-type" of exception 
            groundCloseRequest.Setup(r => r.Submit()).Throws(new TimeoutException("This is slow"));

            FedExAccountEntity account = new FedExAccountEntity();
            testObject.CloseGround(account);
        }

        #endregion CloseGround Tests


        #region CloseSmartPost Tests

        [TestMethod]
        public void CloseSmartPost_DelegatesToRequestFactory_WhenCreatingSmartPostCloseRequest_Test()
        {
            FedExAccountEntity account = new FedExAccountEntity();

            testObject.CloseSmartPost(account);

            // Make sure the account provided to the method is the one used to create the request
            requestFactory.Verify(f => f.CreateSmartPostCloseRequest(account), Times.Once());
        }

        [TestMethod]
        public void CloseSmartPost_DelegatesToRequest_ToSumbitCloseRequest_Test()
        {
            FedExAccountEntity account = new FedExAccountEntity();

            testObject.CloseSmartPost(account);

            smartPostCloseRequest.Verify(r => r.Submit(), Times.Once());
        }

        [TestMethod]
        public void CloseSmartPost_DelegatesToResponse_ToProcess_Test()
        {
            FedExAccountEntity account = new FedExAccountEntity();

            testObject.CloseSmartPost(account);

            smartPostCloseResponse.Verify(r => r.Process(), Times.Once());
        }

        [TestMethod]
        public void CloseSmartPost_WritesToLog_WhenResponseTypeIsNotFedExSmartPostCloseResponse_Test()
        {
            // The request is configured to return a mocked ICarrierResponse, so there's nothing else
            // to do for the setup of this test
            FedExAccountEntity account = new FedExAccountEntity();

            testObject.CloseSmartPost(account);

            log.Verify(l => l.Info(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CloseSmartPost_CloseEntityIsNull_WhenResponseTypeIsNotFedExSmartPostCloseResponse_Test()
        {
            // The request is configured to return a mocked ICarrierResponse, so there's nothing else
            // to do for the setup of this test
            FedExAccountEntity account = new FedExAccountEntity();

            FedExEndOfDayCloseEntity closeEntity = testObject.CloseSmartPost(account);

            Assert.IsNull(closeEntity);
        }

        [TestMethod]
        public void CloseSmartPost_CloseEntityIsNotNull_WhenResponseTypeIsFedExSmartPostCloseResponse_Test()
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

            Assert.IsNotNull(closeEntity);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExSoapCarrierException))]
        public void CloseSmartPost_CatchesSoapException_AndThrowsFedExSoapException_Test()
        {
            smartPostCloseRequest.Setup(r => r.Submit()).Throws(new SoapException());

            FedExAccountEntity account = new FedExAccountEntity();
            testObject.CloseSmartPost(account);
        }


        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void CloseSmartPost_CatchesCarrierException_AndThrowsFedExException_Test()
        {
            smartPostCloseRequest.Setup(r => r.Submit()).Throws(new CarrierException());

            FedExAccountEntity account = new FedExAccountEntity();
            testObject.CloseSmartPost(account);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CloseSmartPost_CatchesNonWebException_AndThrowsExceptionOfSameType_Test()
        {
            // Setup the smartPost request to throw an exception unrelated to a web request
            smartPostCloseRequest.Setup(r => r.Submit()).Throws(new ArgumentNullException());

            FedExAccountEntity account = new FedExAccountEntity();
            testObject.CloseSmartPost(account);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void CloseSmartPost_CatchesWebException_AndThrowsFedExException_Test()
        {
            // Setup the smartPost request to throw a "web-request-type" of exception 
            smartPostCloseRequest.Setup(r => r.Submit()).Throws(new TimeoutException("This is slow"));

            FedExAccountEntity account = new FedExAccountEntity();
            testObject.CloseSmartPost(account);
        }


        #endregion CloseSmartPost Tests


        #region RegisterAccount Tests

        [TestMethod]
        public void RegisterAccount_DelegatesToRepository_ForShippingSettings_Test()
        {
            FedExAccountEntity account = new FedExAccountEntity();

            // Setup the repository method so a null value is not returned
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity());

            testObject.RegisterAccount(account);

            settingsRepository.Verify(r => r.GetShippingSettings(), Times.Once());
        }

        [TestMethod]
        public void RegisterAccount_DelegatesToRequestFactoryForRegistrationRequest_WhenFedExUsernameIsNull_Test()
        {
            FedExAccountEntity account = new FedExAccountEntity();

            // Setup the repository to return shipping settings with a null username for this test
            ShippingSettingsEntity shippingSettings = new ShippingSettingsEntity { FedExUsername = null };
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(shippingSettings);

            testObject.RegisterAccount(account);

            requestFactory.Verify(f => f.CreateRegisterCspUserRequest(account), Times.Once());
        }

        [TestMethod]
        public void RegisterAccount_SubmitsRegistrationRequest_WhenFedExUsernameIsNull_Test()
        {
            FedExAccountEntity account = new FedExAccountEntity();

            // Setup the repository to return shipping settings with a null username for this test
            ShippingSettingsEntity shippingSettings = new ShippingSettingsEntity { FedExUsername = null };
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(shippingSettings);

            testObject.RegisterAccount(account);

            registrationRequest.Verify(r => r.Submit(), Times.Once());
        }

        [TestMethod]
        public void RegisterAccount_ProcessesRegistrationResponse_WhenFedExUsernameIsNull_Test()
        {
            FedExAccountEntity account = new FedExAccountEntity();

            // Setup the repository to return shipping settings with a null username for this test
            ShippingSettingsEntity shippingSettings = new ShippingSettingsEntity { FedExUsername = null };
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(shippingSettings);

            testObject.RegisterAccount(account);

            registrationResponse.Verify(r => r.Process(), Times.Once());
        }

        [TestMethod]
        public void RegisterAccount_DoesNotDelegateToRequestFactoryForRegistrationRequest_WhenFedExUsernameIsNotNull_Test()
        {
            FedExAccountEntity account = new FedExAccountEntity();

            // Setup the repository to return shipping settings with a null username for this test
            ShippingSettingsEntity shippingSettings = new ShippingSettingsEntity { FedExUsername = "username" };
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(shippingSettings);

            testObject.RegisterAccount(account);

            requestFactory.Verify(f => f.CreateRegisterCspUserRequest(account), Times.Never());
        }

        [TestMethod]
        public void RegisterAccount_DoesNotSubmitRegistrationRequest_WhenFedExUsernameIsNotNull_Test()
        {
            FedExAccountEntity account = new FedExAccountEntity();

            // Setup the repository to return shipping settings with a null username for this test
            ShippingSettingsEntity shippingSettings = new ShippingSettingsEntity { FedExUsername = "username" };
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(shippingSettings);

            testObject.RegisterAccount(account);

            registrationRequest.Verify(r => r.Submit(), Times.Never());
        }

        [TestMethod]
        public void RegisterAccount_DoesNotProcessRegistrationResponse_WhenFedExUsernameIsNotNull_Test()
        {
            FedExAccountEntity account = new FedExAccountEntity();

            // Setup the repository to return shipping settings with a null username for this test
            ShippingSettingsEntity shippingSettings = new ShippingSettingsEntity { FedExUsername = "username" };
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(shippingSettings);

            testObject.RegisterAccount(account);

            registrationResponse.Verify(r => r.Process(), Times.Never());
        }

        [TestMethod]
        public void RegisterAccount_DelegatesToRequestFactoryForSubscriptionRequest_Test()
        {
            FedExAccountEntity account = new FedExAccountEntity();

            // Setup the repository to return shipping settings with a null username for this test
            ShippingSettingsEntity shippingSettings = new ShippingSettingsEntity { FedExUsername = "username" };
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(shippingSettings);

            testObject.RegisterAccount(account);

            // make sure the factory is called with the account provided to the register method
            requestFactory.Verify(f => f.CreateSubscriptionRequest(account), Times.Once());
        }

        [TestMethod]
        public void RegisterAccount_SubmitsSubscriptionRequest_Test()
        {
            FedExAccountEntity account = new FedExAccountEntity();

            // Setup the repository to return shipping settings with a null username for this test
            ShippingSettingsEntity shippingSettings = new ShippingSettingsEntity { FedExUsername = "username" };
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(shippingSettings);

            testObject.RegisterAccount(account);

            subscriptionRequest.Verify(r => r.Submit(), Times.Once());
        }

        [TestMethod]
        public void RegisterAccount_ProcessesSubscriptionResponse_Test()
        {
            FedExAccountEntity account = new FedExAccountEntity();

            // Setup the repository to return shipping settings with a null username for this test
            ShippingSettingsEntity shippingSettings = new ShippingSettingsEntity { FedExUsername = "username" };
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(shippingSettings);

            testObject.RegisterAccount(account);

            subscriptionResponse.Verify(r => r.Process(), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(FedExSoapCarrierException))]
        public void RegisterAccount_CatchesSoapException_AndThrowsFedExSoapException_Test()
        {
            // Just throw a soap exception
            settingsRepository.Setup(r => r.GetShippingSettings()).Throws(new SoapException());

            testObject.RegisterAccount(new FedExAccountEntity());
        }

        [TestMethod]
        public void RegisterAccount_WritesToLog_WhenSoapExceptionIsCaught_Test()
        {
            try
            {
                // Just throw a soap exception
                settingsRepository.Setup(r => r.GetShippingSettings()).Throws(new SoapException());

                testObject.RegisterAccount(new FedExAccountEntity());
            }
            catch (FedExSoapCarrierException)
            {}

            finally
            {
                log.Verify(l => l.Error(It.IsAny<string>()), Times.Once());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void RegisterAccount_CatchesCarrierException_AndThrowsFedExException_Test()
        {
            // Just throw a carrier exception
            settingsRepository.Setup(r => r.GetShippingSettings()).Throws(new CarrierException());

            testObject.RegisterAccount(new FedExAccountEntity());
        }

        [TestMethod]
        public void RegisterAccount_WritesToLog_WhenCarrierExceptionIsCaught_Test()
        {
            try
            {
                // Just throw a soap exception
                settingsRepository.Setup(r => r.GetShippingSettings()).Throws(new CarrierException());

                testObject.RegisterAccount(new FedExAccountEntity());
            }
            catch (FedExException)
            {}

            finally
            {
                log.Verify(l => l.Error(It.IsAny<string>()), Times.Once());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void RegisterAccount_CatchesWebException_AndThrowsFedExException_Test()
        {
            // Just throw a web-related exception
            settingsRepository.Setup(r => r.GetShippingSettings()).Throws(new TimeoutException("this is really slow"));

            testObject.RegisterAccount(new FedExAccountEntity());
        }

        [TestMethod]
        public void RegisterAccount_WritesToLog_WhenWebExceptionIsCaught_Test()
        {
            try
            {
                // Just throw a soap exception
                settingsRepository.Setup(r => r.GetShippingSettings()).Throws(new TimeoutException("this is slow"));

                testObject.RegisterAccount(new FedExAccountEntity());
            }
            catch (FedExException)
            {}

            finally
            {
                log.Verify(l => l.Error(It.IsAny<string>()), Times.Once());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterAccount_CatchesNonWebException_AndThrowsExceptionOfSameType_Test()
        {
            // Just throw a non-web exception
            settingsRepository.Setup(r => r.GetShippingSettings()).Throws(new ArgumentNullException());

            testObject.RegisterAccount(new FedExAccountEntity());
        }


        [TestMethod]
        public void RegisterAccount_WritesToLog_WhenNonWebExceptionIsCaught_Test()
        {
            try
            {
                // Just throw a soap exception
                settingsRepository.Setup(r => r.GetShippingSettings()).Throws(new ArgumentNullException());

                testObject.RegisterAccount(new FedExAccountEntity());
            }
            catch (ArgumentNullException)
            {}

            finally
            {
                log.Verify(l => l.Error(It.IsAny<string>()), Times.Once());
            }
        }

        #endregion RegisterAccount Tests

        #region GetRates Tests

        [TestMethod]
        public void GetRates_WritesWarningToLog_WhenCertificateRequestReturnsNone_Test()
        {
            certificateRequest.Setup(r => r.Submit()).Returns(CertificateSecurityLevel.None);

            try
            {
                testObject.GetRates(shipmentEntity);
            }
            catch (FedExException)
            {}

            log.Verify(l => l.Warn("The FedEx certificate did not pass inspection and could not be trusted."));
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void GetRates_ThrowsFedExException_WhenCertificateRequestReturnsNone_Test()
        {
            certificateRequest.Setup(r => r.Submit()).Returns(CertificateSecurityLevel.None);
            testObject.GetRates(shipmentEntity);
        }

        [TestMethod]
        public void GetRates_WritesWarningToLog_WhenCertificateRequestReturnsSpoofed_Test()
        {
            certificateRequest.Setup(r => r.Submit()).Returns(CertificateSecurityLevel.Spoofed);

            try
            {
                testObject.GetRates(shipmentEntity);
            }
            catch (FedExException)
            {}

            log.Verify(l => l.Warn("The FedEx certificate did not pass inspection and could not be trusted."));
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void GetRates_ThrowsFedExException_WhenCertificateRequestReturnsSpoofed_Test()
        {
            certificateRequest.Setup(r => r.Submit()).Returns(CertificateSecurityLevel.Spoofed);
            testObject.GetRates(shipmentEntity);
        }

        [TestMethod]
        public void GetRates_PerformsVersionCapture_WhenCertificateRequestReturnsTrusted_Test()
        {
            certificateRequest.Setup(r => r.Submit()).Returns(CertificateSecurityLevel.Trusted);

            testObject.GetRates(shipmentEntity);

            Assert.IsTrue(testObject.HasDoneVersionCapture);
        }


        [TestMethod]
        public void GetRates_PerformsVersionCapture_Test()
        {
            testObject.GetRates(shipmentEntity);

            // Kind of a weak test due to the fact that the version capture is static, but we're
            // really just interested in the fact that the version capture has been performed
            Assert.IsTrue(testObject.HasDoneVersionCapture);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void GetRates_ThrowsFedExException_WhenOriginStreet3IsNotEmpty_Test()
        {
            shipmentEntity.OriginStreet3 = "desk";

            testObject.GetRates(shipmentEntity);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void GetRates_ThrowsFedExException_WhenShipStreet3IsNotEmpty_Test()
        {
            shipmentEntity.ShipStreet3 = "desk";

            testObject.GetRates(shipmentEntity);
        }

        [TestMethod]
        public void GetRates_DelegatesToRequestFactory_Test()
        {
            testObject.GetRates(shipmentEntity);

            // Check that the rate request was created with a null value
            //for the specialized manipulators - this is for obtaining the "basic rates"
            requestFactory.Verify(f => f.CreateRateRequest(shipmentEntity, null), Times.Once());
        }

        [TestMethod]
        public void GetRates_DelegatesToRequest_ToSubmitRateRequest_Test()
        {
            testObject.GetRates(shipmentEntity);

            rateRequest.Verify(r => r.Submit(), Times.Exactly(3));
        }

        [TestMethod]
        public void GetRates_ProcessesResponse_Test()
        {
            testObject.GetRates(shipmentEntity);

            rateResponse.Verify(r => r.Process(), Times.Exactly(3));
        }

        [TestMethod]
        public void GetRates_AssignsTransitTime_WhenDeliveryTimeStampSpecifiedIsTrue_Test()
        {
            // Setup the delivery date to be four days from now
            shipmentEntity.ShipDate = DateTime.Now;

            nativeRateReply.RateReplyDetails[0].DeliveryTimestampSpecified = true;
            nativeRateReply.RateReplyDetails[0].DeliveryTimestamp = DateTime.Now.AddDays(4);

            RateGroup rates = testObject.GetRates(shipmentEntity);

            Assert.AreEqual("4", rates.Rates.First().Days);
        }

        [TestMethod]
        public void GetRates_AssignsTransitTime_WhenDeliveryTimeStampSpecifiedIsFalse_AndTransitTimeSpecifiedIsTrue_Test()
        {
            // Setup the transit time to be eleven days
            nativeRateReply.RateReplyDetails[0].DeliveryTimestampSpecified = false;
            nativeRateReply.RateReplyDetails[0].TransitTime = TransitTimeType.ELEVEN_DAYS;
            nativeRateReply.RateReplyDetails[0].TransitTimeSpecified = true;

            RateGroup rates = testObject.GetRates(shipmentEntity);

            Assert.AreEqual("11", rates.Rates.First().Days);
        }

        [TestMethod]
        public void GetRates_TransitTimeIsEmpty_WhenDeliveryTimeStampSpecifiedIsFalse_AndTransitTimeSpecifiedIsFalse_Test()
        {
            // Setup the transit time to be eleven days
            nativeRateReply.RateReplyDetails[0].DeliveryTimestampSpecified = false;
            nativeRateReply.RateReplyDetails[0].TransitTime = TransitTimeType.ELEVEN_DAYS;
            nativeRateReply.RateReplyDetails[0].TransitTimeSpecified = false;

            RateGroup rates = testObject.GetRates(shipmentEntity);

            Assert.AreEqual(string.Empty, rates.Rates.First().Days);
        }

        [TestMethod]
        public void GetRates_GrabsSecondRateWhenItIsActualRateType_Test()
        {
            // Setup the native request to have a prioity freight rate
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
            rateResponse.Setup(r => r.NativeResponse).Returns(nativeRateReply);
            
            RateGroup rates = testObject.GetRates(shipmentEntity);

            // We should get rates back (priority overnight for basic, smart post, and One Rate)
            Assert.AreEqual(42.42M, rates.Rates.First().Amount);
        }

        [TestMethod]
        public void GetRates_RemovesPriorityFreightRates_Test()
        {
            // Setup the native request to have a prioity freight rate
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
            rateResponse.Setup(r => r.NativeResponse).Returns(nativeRateReply);


            RateGroup rates = testObject.GetRates(shipmentEntity);

            // We should get rates back (priority overnight for basic, smart post, and One Rate)
            Assert.AreEqual(3, rates.Rates.Count);
        }

        [TestMethod]
        public void GetRates_RemovesEconomyFreightRates_Test()
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
            rateResponse.Setup(r => r.NativeResponse).Returns(nativeRateReply);


            RateGroup rates = testObject.GetRates(shipmentEntity);

            // We should get rates back (priority overnight for basic, smart post, and One Rate)
            Assert.AreEqual(3, rates.Rates.Count);
        }



        [TestMethod]
        public void GetRates_SetsAmountToTotalNetCharge_Test()
        {
            nativeRateReply.RateReplyDetails[0].RatedShipmentDetails[0].ShipmentRateDetail.TotalNetCharge.Amount = 43.85M;

            RateGroup rates = testObject.GetRates(shipmentEntity);

            Assert.AreEqual(43.85M, rates.Rates.First().Amount);
        }

        [TestMethod]
        public void GetRates_RateGroupCount_MatchesRateReplyDetailsCount_Test()
        {
            nativeRateReply.RateReplyDetails[0].RatedShipmentDetails[0].ShipmentRateDetail.TotalNetCharge.Amount = 43.85M;

            RateGroup rates = testObject.GetRates(shipmentEntity);

            // Multiply by two to account for the basic rate, smart post rates, and One Rate rates
            Assert.AreEqual(nativeRateReply.RateReplyDetails.Length*3, rates.Rates.Count);
        }


        [TestMethod]
        public void GetRates_CreatesSmartPostRequest_WithRequestFactory_Test()
        {
            testObject.GetRates(shipmentEntity);

            // Check that a rate request was created that supplied a list of specialized manipulators 
            // containing one manipulator that was the smart post manipulator
            requestFactory.Verify(f => f.CreateRateRequest(shipmentEntity, It.Is<List<ICarrierRequestManipulator>>
                (l => l != null && l.Count == 1 && l[0].GetType() == typeof(FedExRateSmartPostManipulator))), Times.Once());
        }

        [TestMethod]
        public void GetRates_CreatesOneRateRequest_WithRequestFactory_Test()
        {
            testObject.GetRates(shipmentEntity);

            // Check that a rate request was created that supplied a list of specialized manipulators 
            // containing one manipulator that was the One Rate manipulator
            requestFactory.Verify(f => f.CreateRateRequest(shipmentEntity, It.Is<List<ICarrierRequestManipulator>>
                (l => l != null && l.Count == 1 && l[0].GetType() == typeof(FedExRateOneRateManipulator))), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(FedExSoapCarrierException))]
        public void GetRates_CatchesSoapException_AndThrowsFedExSoapException_Test()
        {
            // Just throw a soap exception
            rateRequest.Setup(r => r.Submit()).Throws(new SoapException());

            testObject.GetRates(shipmentEntity);
        }

        [TestMethod]
        public void GetRates_WritesToLog_WhenSoapExceptionIsCaught_Test()
        {
            try
            {
                // Just throw a soap exception
                rateRequest.Setup(r => r.Submit()).Throws(new SoapException());

                testObject.GetRates(shipmentEntity);
            }
            catch (FedExSoapCarrierException)
            {}

            finally
            {
                log.Verify(l => l.Error(It.IsAny<string>()), Times.Once());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void GetRates_CatchesCarrierException_AndThrowsFedExException_Test()
        {
            // Just throw a carrier exception
            rateRequest.Setup(r => r.Submit()).Throws(new CarrierException());

            testObject.GetRates(shipmentEntity);
        }

        [TestMethod]
        public void GetRates_WritesToLog_WhenCarrierExceptionIsCaught_Test()
        {
            try
            {
                // Just throw a soap exception
                rateRequest.Setup(r => r.Submit()).Throws(new CarrierException());

                testObject.GetRates(shipmentEntity);
            }
            catch (FedExException)
            {}

            finally
            {
                log.Verify(l => l.Error(It.IsAny<string>()), Times.Once());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void GetRates_CatchesWebException_AndThrowsFedExException_Test()
        {
            // Just throw a web-related exception
            rateRequest.Setup(r => r.Submit()).Throws(new TimeoutException("this is really slow"));

            testObject.GetRates(shipmentEntity);
        }

        [TestMethod]
        public void GetRates_WritesToLog_WhenWebExceptionIsCaught_Test()
        {
            try
            {
                // Just throw a soap exception
                rateRequest.Setup(r => r.Submit()).Throws(new TimeoutException("this is slow"));

                testObject.GetRates(shipmentEntity);
            }
            catch (FedExException)
            {}

            finally
            {
                log.Verify(l => l.Error(It.IsAny<string>()), Times.Once());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetRates_CatchesNonWebException_AndThrowsExceptionOfSameType_Test()
        {
            // Just throw a non-web exception
            rateRequest.Setup(r => r.Submit()).Throws(new ArgumentNullException());

            testObject.GetRates(shipmentEntity);
        }


        [TestMethod]
        public void GetRates_WritesToLog_WhenNonWebExceptionIsCaught_Test()
        {
            try
            {
                // Just throw a soap exception
                rateRequest.Setup(r => r.Submit()).Throws(new ArgumentNullException());

                testObject.GetRates(shipmentEntity);
            }
            catch (ArgumentNullException)
            {}

            finally
            {
                log.Verify(l => l.Error(It.IsAny<string>()), Times.Once());
            }
        }

        [TestMethod]
        public void GetRates_DoesNotThrowException_WhenSmartPostRatesCatchesFedExException_Test()
        {
            // Just throw an error when the request is created using the specialized 
            // manipulators (i.e. the smart post rate request). Setup the call to create
            // the "basic" rates as well, so the exception is not thrown when a null value is supplied
            requestFactory.Setup(f => f.CreateRateRequest(It.IsAny<ShipmentEntity>(), It.IsAny<List<ICarrierRequestManipulator>>())).Throws(new FedExException());
            requestFactory.Setup(f => f.CreateRateRequest(It.IsAny<ShipmentEntity>(), null)).Returns(rateRequest.Object);

            testObject.GetRates(shipmentEntity);
        }

        [TestMethod]
        public void GetRates_WritesWarningToLog_WhenSmartPostRatesCatchesFedExException_Test()
        {
            // Just throw an error when the request is created using the specialized 
            // manipulators (i.e. the smart post rate request). Setup the call to create
            // the "basic" rates as well, so the exception is not thrown when a null value is supplied
            requestFactory.Setup(f => f.CreateRateRequest(It.IsAny<ShipmentEntity>(), It.IsAny<List<ICarrierRequestManipulator>>())).Throws(new FedExException());
            requestFactory.Setup(f => f.CreateRateRequest(It.IsAny<ShipmentEntity>(), null)).Returns(rateRequest.Object);

            testObject.GetRates(shipmentEntity);

            log.Verify(l => l.Warn(It.Is<string>(s => s.Contains("Error getting SmartPost rates"))), Times.Once());
        }

        [TestMethod]
        public void GetRates_DoesNotThrowException_WhenSmartPostRatesCatchesFedExApiException_Test()
        {
            // Just throw an error when the request is created using the specialized 
            // manipulators (i.e. the smart post rate request). Setup the call to create
            // the "basic" rates as well, so the exception is not thrown when a null value is supplied
            Notification[] notifications = new Notification[1] { new Notification { Message = "message" } };

            requestFactory.Setup(f => f.CreateRateRequest(It.IsAny<ShipmentEntity>(), It.IsAny<List<ICarrierRequestManipulator>>())).Throws(new FedExApiCarrierException(notifications));
            requestFactory.Setup(f => f.CreateRateRequest(It.IsAny<ShipmentEntity>(), null)).Returns(rateRequest.Object);

            testObject.GetRates(shipmentEntity);
        }

        [TestMethod]
        public void GetRates_WritesWarningToLog_WhenSmartPostRatesCatchesFedExApiException_Test()
        {
            // Just throw an error when the request is created using the specialized 
            // manipulators (i.e. the smart post rate request). Setup the call to create
            // the "basic" rates as well, so the exception is not thrown when a null value is supplied
            Notification[] notifications = new Notification[1] { new Notification { Message = "message" } };

            requestFactory.Setup(f => f.CreateRateRequest(It.IsAny<ShipmentEntity>(), It.IsAny<List<ICarrierRequestManipulator>>())).Throws(new FedExApiCarrierException(notifications));
            requestFactory.Setup(f => f.CreateRateRequest(It.IsAny<ShipmentEntity>(), null)).Returns(rateRequest.Object);

            testObject.GetRates(shipmentEntity);

            log.Verify(l => l.Warn(It.Is<string>(s => s.Contains("Error getting SmartPost rates"))), Times.Once());
        }

        #endregion GetRates Tests
    }
}
