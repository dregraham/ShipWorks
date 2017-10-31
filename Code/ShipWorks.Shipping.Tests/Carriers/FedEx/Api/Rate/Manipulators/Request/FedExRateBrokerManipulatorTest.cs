using System;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    public class FedExRateBrokerManipulatorTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly ShipmentEntity shipment;

        public FedExRateBrokerManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            shipment = Create.Shipment().AsFedEx().Build();
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void ShouldApply_ReturnsResult_BasedOnBrokerEnabled(bool enabled, bool expected)
        {
            shipment.FedEx.BrokerEnabled = enabled;
            var testObject = mock.Create<FedExRateBrokerManipulator>();

            var result = testObject.ShouldApply(shipment, FedExRateRequestOptions.None);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Manipulate_CreatesRequestedShipment_WhenItDoesNotExist()
        {
            var testObject = mock.Create<FedExRateBrokerManipulator>();

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.NotNull(result.RequestedShipment);
        }

        [Fact]
        public void Manipulate_DoesNotCreateRequestedShipment_WhenItAlreadyExists()
        {
            var requestedShipment = new RequestedShipment();
            var testObject = mock.Create<FedExRateBrokerManipulator>();

            var result = testObject.Manipulate(shipment, new RateRequest { RequestedShipment = requestedShipment });

            Assert.Same(requestedShipment, result.RequestedShipment);
        }

        [Fact]
        public void Manipulate_CreatesCustomsClearanceDetail_WhenItDoesNotExist()
        {
            var testObject = mock.Create<FedExRateBrokerManipulator>();

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.NotNull(result.RequestedShipment.CustomsClearanceDetail);
        }

        [Fact]
        public void Manipulate_DoesNotCreateCustomsClearanceDetail_WhenItAlreadyExists()
        {
            var customsClearanceDetail = new CustomsClearanceDetail();
            var testObject = mock.Create<FedExRateBrokerManipulator>();

            var result = testObject.Manipulate(shipment, new RateRequest { RequestedShipment = new RequestedShipment { CustomsClearanceDetail = customsClearanceDetail } });

            Assert.Same(customsClearanceDetail, result.RequestedShipment.CustomsClearanceDetail);
        }

        [Fact]
        public void Manipulate_CreatesSpecialServicesRequested_WhenItDoesNotExist()
        {
            var testObject = mock.Create<FedExRateBrokerManipulator>();

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.NotNull(result.RequestedShipment.SpecialServicesRequested);
        }

        [Fact]
        public void Manipulate_DoesNotCreateSpecialServicesRequested_WhenItAlreadyExists()
        {
            var specialServicesRequested = new ShipmentSpecialServicesRequested();
            var testObject = mock.Create<FedExRateBrokerManipulator>();

            var result = testObject.Manipulate(shipment, new RateRequest { RequestedShipment = new RequestedShipment { SpecialServicesRequested = specialServicesRequested } });

            Assert.Same(specialServicesRequested, result.RequestedShipment.SpecialServicesRequested);
        }

        [Fact]
        public void Manipulate_AddsBrokerToSpecialServiceTypes_WhenNoTypesHaveBeenAdded()
        {
            var testObject = mock.Create<FedExRateBrokerManipulator>();

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Contains(ShipmentSpecialServiceType.BROKER_SELECT_OPTION, result.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);
        }

        [Fact]
        public void Manipulate_AddsBrokerToSpecialServiceTypes_WhenOtherTypesHaveBeenAdded()
        {
            var request = new RateRequest
            {
                RequestedShipment = new RequestedShipment
                {
                    SpecialServicesRequested = new ShipmentSpecialServicesRequested
                    {
                        SpecialServiceTypes = new[] { ShipmentSpecialServiceType.DRY_ICE }
                    }
                }
            };
            var testObject = mock.Create<FedExRateBrokerManipulator>();

            var result = testObject.Manipulate(shipment, request);

            Assert.Contains(ShipmentSpecialServiceType.BROKER_SELECT_OPTION, result.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);
            Assert.Contains(ShipmentSpecialServiceType.DRY_ICE, result.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);
        }

        [Fact]
        public void Manipulate_AddsBrokerData_ToCustoms()
        {
            shipment.FedEx.BrokerStreet1 = "Foo";
            shipment.FedEx.BrokerStreet2 = "Bar";
            shipment.FedEx.BrokerStreet3 = "Baz";
            shipment.FedEx.BrokerCity = "St. Louis";
            shipment.FedEx.BrokerStateProvCode = "MO";
            shipment.FedEx.BrokerPostalCode = "63102";
            shipment.FedEx.BrokerCountryCode = "US";

            shipment.FedEx.BrokerFirstName = "John";
            shipment.FedEx.BrokerLastName = "Doe";
            shipment.FedEx.BrokerCompany = "Foo Company";
            shipment.FedEx.BrokerEmail = "foo@example.com";
            shipment.FedEx.BrokerPhone = "314-555-1234";
            shipment.FedEx.BrokerPhoneExtension = "x123";

            var testObject = mock.Create<FedExRateBrokerManipulator>();

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Equal(1, result.RequestedShipment.CustomsClearanceDetail.Brokers.Length);

            var broker = result.RequestedShipment.CustomsClearanceDetail.Brokers[0];
            Assert.Equal(BrokerType.IMPORT, broker.Type);
            Assert.True(broker.TypeSpecified);

            var details = result.RequestedShipment.CustomsClearanceDetail.Brokers[0].Broker;
            Assert.Equal(new[] { "Foo", "Bar", "Baz" }, details.Address.StreetLines);
            Assert.Equal("St. Louis", details.Address.City);
            Assert.Equal("MO", details.Address.StateOrProvinceCode);
            Assert.Equal("63102", details.Address.PostalCode);
            Assert.Equal("US", details.Address.CountryCode);

            Assert.Equal("John Doe", details.Contact.PersonName);
            Assert.Equal("Foo Company", details.Contact.CompanyName);
            Assert.Equal("foo@example.com", details.Contact.EMailAddress);
            Assert.Equal("314-555-1234", details.Contact.PhoneNumber);
            Assert.Equal("x123", details.Contact.PhoneExtension);
            Assert.Empty(details.Contact.FaxNumber);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
