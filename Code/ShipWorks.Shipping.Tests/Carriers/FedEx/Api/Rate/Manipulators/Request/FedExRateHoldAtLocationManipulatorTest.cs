using System;
using System.Linq;
using Autofac.Extras.Moq;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    public class FedExRateHoldAtLocationManipulatorTest : IDisposable
    {
        readonly AutoMock mock;

        public FedExRateHoldAtLocationManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(true, true)]
        public void ShouldApply_ReturnsAppropriateValue_ForHoldAtLocationSetting(bool enabled, bool expected)
        {
            var shipment = Create.Shipment().AsFedEx(f => f.Set(x => x.FedExHoldAtLocationEnabled, enabled)).Build();
            var testObject = mock.Create<FedExRateHoldAtLocationManipulator>();

            var result = testObject.ShouldApply(shipment, FedExRateRequestOptions.None);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Manipulate_AddsHoldAtLocationToServices_WhenRequestIsEmpty()
        {
            var shipment = Create.Shipment().AsFedEx().Build();
            var testObject = mock.Create<FedExRateHoldAtLocationManipulator>();

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Contains(ShipmentSpecialServiceType.HOLD_AT_LOCATION,
                result.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);
        }

        [Fact]
        public void Manipulate_AddsHoldAtLocationToServices_WhenSpecialServicesExist()
        {
            var rateRequest = new RateRequest();
            var services = rateRequest.Ensure(x => x.RequestedShipment)
                .Ensure(x => x.SpecialServicesRequested);
            services.SpecialServiceTypes = new[] { ShipmentSpecialServiceType.COD };

            var shipment = Create.Shipment().AsFedEx().Build();
            var testObject = mock.Create<FedExRateHoldAtLocationManipulator>();

            var result = testObject.Manipulate(shipment, rateRequest);

            var serviceTypes = result.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes;
            Assert.Contains(ShipmentSpecialServiceType.HOLD_AT_LOCATION, serviceTypes);
            Assert.Contains(ShipmentSpecialServiceType.COD, serviceTypes);
        }

        [Theory]
        [InlineData("Foo", null, null)]
        [InlineData("Foo", "Bar", null)]
        [InlineData("Foo", null, "Baz")]
        [InlineData("Foo", "Bar", "Baz")]
        public void Manipulate_SetsStreetToMaxTwoElements_RegardlessOfStreetCount(string street1, string street2, string street3)
        {
            var shipment = Create.Shipment().AsFedEx().Build();
            shipment.FedEx.HoldStreet1 = street1;
            shipment.FedEx.HoldStreet2 = street2;
            shipment.FedEx.HoldStreet3 = street3;

            var testObject = mock.Create<FedExRateHoldAtLocationManipulator>();

            var result = testObject.Manipulate(shipment, new RateRequest());

            var expectedLines = new[] { street1, street2 }.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            Assert.Equal(expectedLines,
                result.RequestedShipment.SpecialServicesRequested.HoldAtLocationDetail.LocationContactAndAddress.Address.StreetLines);
        }

        [Fact]
        public void Manipulate_SetsAddressDetails_WhenRequestIsEmpty()
        {
            var shipment = Create.Shipment().AsFedEx().Build();
            shipment.FedEx.HoldCity = "St. Louis";
            shipment.FedEx.HoldStateOrProvinceCode = "MO";
            shipment.FedEx.HoldPostalCode = "63102";
            shipment.FedEx.HoldCountryCode = "US";
            shipment.FedEx.HoldUrbanizationCode = "URB";

            shipment.FedEx.HoldContactId = "ABC123";
            shipment.FedEx.HoldCompanyName = "Foo Company";
            shipment.FedEx.HoldEmailAddress = "foo@example.com";
            shipment.FedEx.HoldFaxNumber = "314-555-9999";
            shipment.FedEx.HoldPagerNumber = "314-555-8888";
            shipment.FedEx.HoldPersonName = "John Doe";
            shipment.FedEx.HoldPhoneNumber = "314-555-1234";
            shipment.FedEx.HoldPhoneExtension = "x123";
            shipment.FedEx.HoldTitle = "boss";

            var testObject = mock.Create<FedExRateHoldAtLocationManipulator>();

            var result = testObject.Manipulate(shipment, new RateRequest());

            var address = result.RequestedShipment.SpecialServicesRequested.HoldAtLocationDetail.LocationContactAndAddress.Address;
            Assert.Equal("St. Louis", address.City);
            Assert.Equal("MO", address.StateOrProvinceCode);
            Assert.Equal("63102", address.PostalCode);
            Assert.Equal("US", address.CountryCode);
            Assert.Equal("URB", address.UrbanizationCode);

            var contact = result.RequestedShipment.SpecialServicesRequested.HoldAtLocationDetail.LocationContactAndAddress.Contact;
            Assert.Equal("ABC123", contact.ContactId);
            Assert.Equal("Foo Company", contact.CompanyName);
            Assert.Equal("foo@example.com", contact.EMailAddress);
            Assert.Equal("314-555-9999", contact.FaxNumber);
            Assert.Equal("314-555-8888", contact.PagerNumber);
            Assert.Equal("John Doe", contact.PersonName);
            Assert.Equal("314-555-1234", contact.PhoneNumber);
            Assert.Equal("x123", contact.PhoneExtension);
            Assert.Equal("boss", contact.Title);
        }

        [Fact]
        public void Manipulate_SetsLocationType_WhenLocationTypeHasValue()
        {
            var shipment = Create.Shipment()
                .AsFedEx(f => f.Set(x => x.HoldLocationType, (int) FedExLocationType.FEDEX_OFFICE))
                .Build();

            var testObject = mock.Create<FedExRateHoldAtLocationManipulator>();

            var result = testObject.Manipulate(shipment, new RateRequest());

            var detail = result.RequestedShipment.SpecialServicesRequested.HoldAtLocationDetail;
            Assert.Equal(FedExLocationType.FEDEX_OFFICE, detail.LocationType);
            Assert.True(detail.LocationTypeSpecified);
        }

        [Fact]
        public void Manipulate_DoesNotSetLocationType_WhenLocationTypeIsNull()
        {
            var shipment = Create.Shipment().AsFedEx().DoNotSetDefaults().Build();
            var testObject = mock.Create<FedExRateHoldAtLocationManipulator>();

            var result = testObject.Manipulate(shipment, new RateRequest());

            var detail = result.RequestedShipment.SpecialServicesRequested.HoldAtLocationDetail;
            Assert.Equal(FedExLocationType.FEDEX_EXPRESS_STATION, detail.LocationType);  // This is set by default
            Assert.False(detail.LocationTypeSpecified);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
