using System;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    public class FedExRateSmartPostManipulatorTest
    {
        private FedExRateSmartPostManipulator testObject;
        private readonly AutoMock mock;
        private ShipmentEntity shipment;

        public FedExRateSmartPostManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            shipment = new ShipmentEntity
            {
                FedEx = new FedExShipmentEntity
                {
                    SmartPostHubID = "5015",
                    SmartPostIndicia = (int) FedExSmartPostIndicia.MediaMail,
                    SmartPostEndorsement = (int) FedExSmartPostEndorsement.ReturnService
                }
            };

            testObject = mock.Create<FedExRateSmartPostManipulator>();
        }

        [Theory]
        [InlineData("Foo", FedExRateRequestOptions.SmartPost, true)]
        [InlineData("Foo", FedExRateRequestOptions.ExpressFreight | FedExRateRequestOptions.SmartPost, true)]
        [InlineData("", FedExRateRequestOptions.SmartPost, false)]
        [InlineData("Foo", FedExRateRequestOptions.ExpressFreight, false)]
        [InlineData("Foo", FedExRateRequestOptions.None, false)]
        [InlineData("Foo", FedExRateRequestOptions.OneRate, false)]
        public void ShouldApply_ReturnsAppropriateValue_ForInputs(string hubID, FedExRateRequestOptions option, bool expected)
        {
            shipment.FedEx.SmartPostHubID = hubID;
            var result = testObject.ShouldApply(shipment, option);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ShouldApply_ReturnsFalse_WhenFedExShipmentIsNull()
        {
            shipment.FedEx = null;

            var result = testObject.ShouldApply(shipment, FedExRateRequestOptions.SmartPost);

            Assert.False(result);
        }

        [Fact]
        public void Manipulate_AccountsForNullRequestedShipment()
        {
            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.NotNull(result.RequestedShipment);
        }

        [Fact]
        public void Manipulate_AccountsForNullPackageLineItems()
        {
            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.NotNull(result.RequestedShipment.RequestedPackageLineItems);
            Assert.Equal(0, result.RequestedShipment.RequestedPackageLineItems.Length);
        }

        [Fact]
        public void Manipulate_AccountsForNullTotalInsuredValue()
        {
            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.NotNull(result.RequestedShipment.TotalInsuredValue);
        }

        [Fact]
        public void Manipulate_AssignsSmartPostDetail()
        {
            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.NotNull(result.RequestedShipment.SmartPostDetail);
        }

        [Fact]
        public void Manipulate_SetsSmartHubId()
        {
            var result = testObject.Manipulate(shipment, new RateRequest());

            // Since this is delegated to another object that is a static utility class, we
            // can check that hub ID has a non-empty string
            Assert.False(string.IsNullOrEmpty(result.RequestedShipment.SmartPostDetail.HubId));
        }

        [Fact]
        public void Manipulate_IndiciaTypeIsParcelSelect()
        {
            shipment.FedEx.SmartPostIndicia = (int) FedExSmartPostIndicia.ParcelSelect;

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Equal(SmartPostIndiciaType.PARCEL_SELECT, result.RequestedShipment.SmartPostDetail.Indicia);
        }

        [Fact]
        public void Manipulate_IndiciaTypeIsMediaMail()
        {
            shipment.FedEx.SmartPostIndicia = (int) FedExSmartPostIndicia.MediaMail;

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Equal(SmartPostIndiciaType.MEDIA_MAIL, result.RequestedShipment.SmartPostDetail.Indicia);
        }

        [Fact]
        public void Manipulate_IndiciaTypeIsPresortedBoundPrintedMatter()
        {
            shipment.FedEx.SmartPostIndicia = (int) FedExSmartPostIndicia.BoundPrintedMatter;

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Equal(SmartPostIndiciaType.PRESORTED_BOUND_PRINTED_MATTER, result.RequestedShipment.SmartPostDetail.Indicia);
        }

        [Fact]
        public void Manipulate_IndiciaTypeIsPresortedStandard()
        {
            shipment.FedEx.SmartPostIndicia = (int) FedExSmartPostIndicia.PresortedStandard;

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Equal(SmartPostIndiciaType.PRESORTED_STANDARD, result.RequestedShipment.SmartPostDetail.Indicia);
        }

        [Fact]
        public void Manipulate_ThrowsInvalidOperationException_WhenUnrecognizedIndiciaTypeIsProvided()
        {
            shipment.FedEx.SmartPostIndicia = 97;

            Assert.Throws<InvalidOperationException>(() => testObject.Manipulate(shipment, new RateRequest()));
        }

        [Fact]
        public void Manipulate_IndiciaSpecifiedIsTrue()
        {
            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.True(result.RequestedShipment.SmartPostDetail.IndiciaSpecified);
        }

        [Fact]
        public void Manipulate_AncillaryEndorsementIsAddressCorrection()
        {
            shipment.FedEx.SmartPostEndorsement = (int) FedExSmartPostEndorsement.AddressCorrection;

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Equal(SmartPostAncillaryEndorsementType.ADDRESS_CORRECTION, result.RequestedShipment.SmartPostDetail.AncillaryEndorsement);
        }

        [Fact]
        public void Manipulate_AncillaryEndorsementSpecifiedIsTrue_WhenEndorsementTypeIsAddressCorrection()
        {
            shipment.FedEx.SmartPostEndorsement = (int) FedExSmartPostEndorsement.AddressCorrection;

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.True(result.RequestedShipment.SmartPostDetail.AncillaryEndorsementSpecified);
        }

        [Fact]
        public void Manipulate_AncillaryEndorsementIsChangeService()
        {
            shipment.FedEx.SmartPostEndorsement = (int) FedExSmartPostEndorsement.ChangeService;

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Equal(SmartPostAncillaryEndorsementType.CHANGE_SERVICE, result.RequestedShipment.SmartPostDetail.AncillaryEndorsement);
        }

        [Fact]
        public void Manipulate_AncillaryEndorsementSpecifiedIsTrue_WhenEndorsementTypeIsChangeService()
        {
            shipment.FedEx.SmartPostEndorsement = (int) FedExSmartPostEndorsement.ChangeService;

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.True(result.RequestedShipment.SmartPostDetail.AncillaryEndorsementSpecified);
        }

        [Fact]
        public void Manipulate_AncillaryEndorsementIsForwardingService()
        {
            shipment.FedEx.SmartPostEndorsement = (int) FedExSmartPostEndorsement.ForwardingService;

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Equal(SmartPostAncillaryEndorsementType.FORWARDING_SERVICE, result.RequestedShipment.SmartPostDetail.AncillaryEndorsement);
        }

        [Fact]
        public void Manipulate_AncillaryEndorsementSpecifiedIsTrue_WhenEndorsementTypeIsForwardingService()
        {
            shipment.FedEx.SmartPostEndorsement = (int) FedExSmartPostEndorsement.ForwardingService;

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.True(result.RequestedShipment.SmartPostDetail.AncillaryEndorsementSpecified);
        }

        [Fact]
        public void Manipulate_AncillaryEndorsementIsLeaveIfNoResponse()
        {
            shipment.FedEx.SmartPostEndorsement = (int) FedExSmartPostEndorsement.LeaveIfNoResponse;

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Equal(SmartPostAncillaryEndorsementType.CARRIER_LEAVE_IF_NO_RESPONSE, result.RequestedShipment.SmartPostDetail.AncillaryEndorsement);
        }

        [Fact]
        public void Manipulate_AncillaryEndorsementSpecifiedIsTrue_WhenEndorsementTypeIsLeaveIfNoResponse()
        {
            shipment.FedEx.SmartPostEndorsement = (int) FedExSmartPostEndorsement.LeaveIfNoResponse;

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.True(result.RequestedShipment.SmartPostDetail.AncillaryEndorsementSpecified);
        }

        [Fact]
        public void Manipulate_AncillaryEndorsementIsReturnService()
        {
            shipment.FedEx.SmartPostEndorsement = (int) FedExSmartPostEndorsement.ReturnService;

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Equal(SmartPostAncillaryEndorsementType.RETURN_SERVICE, result.RequestedShipment.SmartPostDetail.AncillaryEndorsement);
        }

        [Fact]
        public void Manipulate_AncillaryEndorsementSpecifiedIsTrue_WhenEndorsementTypeIsReturnService()
        {
            shipment.FedEx.SmartPostEndorsement = (int) FedExSmartPostEndorsement.ReturnService;

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.True(result.RequestedShipment.SmartPostDetail.AncillaryEndorsementSpecified);
        }

        [Fact]
        public void Manipulate_AncillaryEndorsementSpecifiedIsFalse_WhenEndorsementTypeIsNone()
        {
            shipment.FedEx.SmartPostEndorsement = (int) FedExSmartPostEndorsement.None;

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.False(result.RequestedShipment.SmartPostDetail.AncillaryEndorsementSpecified);
        }

        [Fact]
        public void Manipulate_ThrowsInvalidOperationException_WhenUnrecognizedEndorsementIsProvided()
        {
            shipment.FedEx.SmartPostEndorsement = 97;

            Assert.Throws<InvalidOperationException>(() => testObject.Manipulate(shipment, new RateRequest()));
        }

        [Fact]
        public void Manipulate_ShipmentTotalInsuredValueIsZero()
        {
            var request = new RateRequest();
            request.Ensure(x => x.RequestedShipment).Ensure(x => x.TotalInsuredValue).Amount = 43;

            var result = testObject.Manipulate(shipment, request);

            Assert.Equal(0, result.RequestedShipment.TotalInsuredValue.Amount);
        }

        [Fact]
        public void Manipulate_EachPackageTotalInsuredAmountIsZero()
        {
            // Create a few packages in the FedEx shipment to work with
            shipment.FedEx.Packages.Add(new FedExPackageEntity() { DimsHeight = 2, DimsWidth = 2, DimsLength = 2 });
            shipment.FedEx.Packages.Add(new FedExPackageEntity() { DimsHeight = 2, DimsWidth = 2, DimsLength = 2 });
            shipment.FedEx.Packages.Add(new FedExPackageEntity() { DimsHeight = 2, DimsWidth = 2, DimsLength = 2 });

            var result = testObject.Manipulate(shipment, new RateRequest());

            for (int i = 0; i < shipment.FedEx.Packages.Count; i++)
            {
                Assert.Equal(0, result.RequestedShipment.RequestedPackageLineItems[i].InsuredValue.Amount);
            }
        }

        [Fact]
        public void Manipulate_ServiceTypeIsSmartPost()
        {
            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Equal(ServiceType.SMART_POST, result.RequestedShipment.ServiceType);
        }

        [Fact]
        public void Manipulate_ServiceTypeSpecifiedIsTrue()
        {
            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.True(result.RequestedShipment.ServiceTypeSpecified);
        }
    }
}
