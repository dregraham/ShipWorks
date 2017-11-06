using System;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    public class FedExSmartPostManipulatorTest
    {
        private readonly ProcessShipmentRequest processShipmentRequest;
        private readonly ShipmentEntity shipment;
        private readonly FedExSmartPostManipulator testObject;

        public FedExSmartPostManipulatorTest()
        {
            AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            shipment = Create.Shipment().AsFedEx().Build();
            shipment.FedEx.Service = (int) FedExServiceType.SmartPost;
            shipment.FedEx.SmartPostHubID = "5531";

            FedExAccountEntity account = new FedExAccountEntity { AccountNumber = "1234", MeterNumber = "45453", SmartPostHubList = "<Root><HubID>5531</HubID></Root>" };
            mock.Mock<IFedExSettingsRepository>()
                .Setup(x => x.GetAccountReadOnly(AnyIShipment))
                .Returns(account);

            processShipmentRequest = new ProcessShipmentRequest();

            testObject = mock.Create<FedExSmartPostManipulator>();
        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(null, new ProcessShipmentRequest(), 0));
        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenProcessShipmentRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(new ShipmentEntity(), null, 0));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenSmartPostHubIDIsBlank()
        {
            shipment.FedEx.SmartPostHubID = string.Empty;

            Assert.Throws<CarrierException>(() => testObject.Manipulate(shipment, processShipmentRequest, 0));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenSmartPostIndiciaTypeIsInvalid()
        {
            shipment.FedEx.SmartPostIndicia = 239955;
            Assert.Throws<CarrierException>(() => testObject.Manipulate(shipment, processShipmentRequest, 0));
        }

        [Fact]
        public void Manipulate_AccountsForNullRequestedShipment()
        {
            // Setup the test by configuring the native request to have a null requested shipment property and re-initialize
            // the carrier request with the updated native request
            processShipmentRequest.RequestedShipment = null;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // The requested shipment property should be created now
            Assert.NotNull(processShipmentRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_TotalInsuredValueIsNull()
        {
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // The requested shipment property should be created now
            Assert.Null(processShipmentRequest.RequestedShipment.TotalInsuredValue);
        }

        [Fact]
        public void Manipulate_SmartPostShipmentDetailIsNull_WhenShipmentTypeIsNotSmartPost()
        {
            shipment.FedEx.Service = (int) FedExServiceType.FedEx1DayFreight;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Null(processShipmentRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_SmartPostHubIDsMatch()
        {
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(shipment.FedEx.SmartPostHubID, processShipmentRequest.RequestedShipment.SmartPostDetail.HubId);
        }

        [Fact]
        public void Manipulate_SmartPostIndiciaTypesCorrect_WhenConverted()
        {
            shipment.FedEx.SmartPostIndicia = (int) FedExSmartPostIndicia.BoundPrintedMatter;
            testObject.Manipulate(shipment, processShipmentRequest, 0);
            Assert.Equal(SmartPostIndiciaType.PRESORTED_BOUND_PRINTED_MATTER, processShipmentRequest.RequestedShipment.SmartPostDetail.Indicia);
            Assert.Equal(true, processShipmentRequest.RequestedShipment.SmartPostDetail.IndiciaSpecified);

            shipment.FedEx.SmartPostIndicia = (int) FedExSmartPostIndicia.MediaMail;
            testObject.Manipulate(shipment, processShipmentRequest, 0);
            Assert.Equal(SmartPostIndiciaType.MEDIA_MAIL, processShipmentRequest.RequestedShipment.SmartPostDetail.Indicia);
            Assert.Equal(true, processShipmentRequest.RequestedShipment.SmartPostDetail.IndiciaSpecified);

            shipment.FedEx.SmartPostIndicia = (int) FedExSmartPostIndicia.ParcelReturn;
            testObject.Manipulate(shipment, processShipmentRequest, 0);
            Assert.Equal(SmartPostIndiciaType.PARCEL_RETURN, processShipmentRequest.RequestedShipment.SmartPostDetail.Indicia);
            Assert.Equal(true, processShipmentRequest.RequestedShipment.SmartPostDetail.IndiciaSpecified);

            shipment.FedEx.SmartPostIndicia = (int) FedExSmartPostIndicia.ParcelSelect;
            testObject.Manipulate(shipment, processShipmentRequest, 0);
            Assert.Equal(SmartPostIndiciaType.PARCEL_SELECT, processShipmentRequest.RequestedShipment.SmartPostDetail.Indicia);
            Assert.Equal(true, processShipmentRequest.RequestedShipment.SmartPostDetail.IndiciaSpecified);

            shipment.FedEx.SmartPostIndicia = (int) FedExSmartPostIndicia.PresortedStandard;
            testObject.Manipulate(shipment, processShipmentRequest, 0);
            Assert.Equal(SmartPostIndiciaType.PRESORTED_STANDARD, processShipmentRequest.RequestedShipment.SmartPostDetail.Indicia);
            Assert.Equal(true, processShipmentRequest.RequestedShipment.SmartPostDetail.IndiciaSpecified);
        }

        [Fact]
        public void Manipulate_SmartPostEndorsementsCorrect_WhenConverted()
        {
            shipment.FedEx.SmartPostEndorsement = (int) FedExSmartPostEndorsement.AddressCorrection;
            testObject.Manipulate(shipment, processShipmentRequest, 0);
            Assert.Equal(SmartPostAncillaryEndorsementType.ADDRESS_CORRECTION, processShipmentRequest.RequestedShipment.SmartPostDetail.AncillaryEndorsement);
            Assert.Equal(true, processShipmentRequest.RequestedShipment.SmartPostDetail.AncillaryEndorsementSpecified);

            shipment.FedEx.SmartPostEndorsement = (int) FedExSmartPostEndorsement.ChangeService;
            testObject.Manipulate(shipment, processShipmentRequest, 0);
            Assert.Equal(SmartPostAncillaryEndorsementType.CHANGE_SERVICE, processShipmentRequest.RequestedShipment.SmartPostDetail.AncillaryEndorsement);
            Assert.Equal(true, processShipmentRequest.RequestedShipment.SmartPostDetail.AncillaryEndorsementSpecified);

            shipment.FedEx.SmartPostEndorsement = (int) FedExSmartPostEndorsement.ForwardingService;
            testObject.Manipulate(shipment, processShipmentRequest, 0);
            Assert.Equal(SmartPostAncillaryEndorsementType.FORWARDING_SERVICE, processShipmentRequest.RequestedShipment.SmartPostDetail.AncillaryEndorsement);
            Assert.Equal(true, processShipmentRequest.RequestedShipment.SmartPostDetail.AncillaryEndorsementSpecified);

            shipment.FedEx.SmartPostEndorsement = (int) FedExSmartPostEndorsement.LeaveIfNoResponse;
            testObject.Manipulate(shipment, processShipmentRequest, 0);
            Assert.Equal(SmartPostAncillaryEndorsementType.CARRIER_LEAVE_IF_NO_RESPONSE, processShipmentRequest.RequestedShipment.SmartPostDetail.AncillaryEndorsement);
            Assert.Equal(true, processShipmentRequest.RequestedShipment.SmartPostDetail.AncillaryEndorsementSpecified);

            shipment.FedEx.SmartPostEndorsement = (int) FedExSmartPostEndorsement.ReturnService;
            testObject.Manipulate(shipment, processShipmentRequest, 0);
            Assert.Equal(SmartPostAncillaryEndorsementType.RETURN_SERVICE, processShipmentRequest.RequestedShipment.SmartPostDetail.AncillaryEndorsement);
            Assert.Equal(true, processShipmentRequest.RequestedShipment.SmartPostDetail.AncillaryEndorsementSpecified);

            shipment.FedEx.SmartPostEndorsement = (int) FedExSmartPostEndorsement.None;
            testObject.Manipulate(shipment, processShipmentRequest, 0);
            Assert.Equal(false, processShipmentRequest.RequestedShipment.SmartPostDetail.AncillaryEndorsementSpecified);
        }

        [Fact]
        public void Manipulate_SmartPostCustomerManifestIdIsNull_WhenSmartPostCustomerManifestIsNull()
        {
            shipment.FedEx.SmartPostCustomerManifest = null;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Null(processShipmentRequest.RequestedShipment.SmartPostDetail.CustomerManifestId);
        }

        [Fact]
        public void Manipulate_SmartPostCustomerManifestIdMatchesValue_WhenSmartPostCustomerManifestPresent()
        {
            shipment.FedEx.SmartPostCustomerManifest = "asdf1234";

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal("asdf1234", processShipmentRequest.RequestedShipment.SmartPostDetail.CustomerManifestId);
        }
    }
}
