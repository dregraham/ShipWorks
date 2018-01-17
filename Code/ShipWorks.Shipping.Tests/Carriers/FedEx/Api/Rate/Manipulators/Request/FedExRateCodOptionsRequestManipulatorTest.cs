using System;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    public class FedExRateCodOptionsRequestManipulatorTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly ShipmentEntity shipment;

        public FedExRateCodOptionsRequestManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            shipment = Create.Shipment().AsFedEx(f => f.WithPackage()).Build();
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void ShouldApply_ReturnsValue_DependingOnCodEnabled(bool enabled, bool expected)
        {
            shipment.FedEx.CodEnabled = enabled;
            var testObject = mock.Create<FedExRateCodOptionsManipulator>();

            var result = testObject.ShouldApply(shipment, FedExRateRequestOptions.None);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Manipulate_SetsDetails_WhenShipmentIsGround()
        {
            shipment.FedEx.Service = (int) FedExServiceType.FedExGround;
            var testObject = mock.Create<FedExRateCodOptionsManipulator>();

            var result = testObject.Manipulate(shipment, new RateRequest());
            var specialServices = result.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested;

            Assert.NotNull(specialServices.CodDetail);
            Assert.Contains(PackageSpecialServiceType.COD, specialServices.SpecialServiceTypes);
        }

        [Fact]
        public void Manipulate_SetsDetails_WhenShipmentIsNotGround()
        {
            shipment.FedEx.Service = (int) FedExServiceType.FedExNextDayAfternoon;
            var testObject = mock.Create<FedExRateCodOptionsManipulator>();

            var result = testObject.Manipulate(shipment, new RateRequest());
            var specialServices = result.RequestedShipment.SpecialServicesRequested;

            Assert.NotNull(specialServices.CodDetail);
            Assert.Contains(ShipmentSpecialServiceType.COD, specialServices.SpecialServiceTypes);
        }

        [Fact]
        public void Manipulate_AddsBasicDetails()
        {
            shipment.FedEx.Service = (int) FedExServiceType.FedExNextDayAfternoon;
            shipment.FedEx.CodPaymentType = (int) FedExCodPaymentType.Secured;

            shipment.FedEx.CodStreet1 = "Foo";
            shipment.FedEx.CodStreet2 = "Bar";
            shipment.FedEx.CodStreet3 = "Baz";
            shipment.FedEx.CodCity = "St. Louis";
            shipment.FedEx.CodStateProvCode = "MO";
            shipment.FedEx.CodPostalCode = "63102";
            shipment.FedEx.CodCountryCode = "US";

            shipment.FedEx.CodFirstName = "John";
            shipment.FedEx.CodLastName = "Doe";
            shipment.FedEx.CodCompany = "Foo Company";
            shipment.FedEx.CodPhone = "314-555-1234";

            var testObject = mock.Create<FedExRateCodOptionsManipulator>();

            var result = testObject.Manipulate(shipment, new RateRequest());
            var specialServices = result.RequestedShipment.SpecialServicesRequested;

            Assert.Equal(CodCollectionType.GUARANTEED_FUNDS, specialServices.CodDetail.CollectionType);
            Assert.True(specialServices.CodDetail.CollectionTypeSpecified);

            var party = specialServices.CodDetail.CodRecipient;
            Assert.Equal(new[] { "Foo", "Bar", "Baz" }, party.Address.StreetLines);
            Assert.Equal("St. Louis", party.Address.City);
            Assert.Equal("MO", party.Address.StateOrProvinceCode);
            Assert.Equal("63102", party.Address.PostalCode);
            Assert.Equal("US", party.Address.CountryCode);

            Assert.Equal("John Doe", party.Contact.PersonName);
            Assert.Equal("Foo Company", party.Contact.CompanyName);
            Assert.Empty(party.Contact.EMailAddress);
            Assert.Equal("314-555-1234", party.Contact.PhoneNumber);
            Assert.Null(party.Contact.PhoneExtension);
            Assert.Empty(party.Contact.FaxNumber);
        }

        [Theory]
        [InlineData(true, RateTypeBasisType.LIST)]
        [InlineData(false, RateTypeBasisType.ACCOUNT)]
        public void Manipulate_AddsFreightDetail_WhenAddFreightIsEnabled(bool useListRates, RateTypeBasisType expectedBasisType)
        {
            mock.Mock<IFedExSettingsRepository>()
                .SetupGet(x => x.UseListRates)
                .Returns(useListRates);

            shipment.FedEx.CodAddFreight = true;
            shipment.FedEx.CodChargeBasis = (int) CodAddTransportationChargeBasisType.NET_CHARGE;

            var testObject = mock.Create<FedExRateCodOptionsManipulator>();

            var result = testObject.Manipulate(shipment, new RateRequest());
            var detail = result.RequestedShipment.SpecialServicesRequested.CodDetail.AddTransportationChargesDetail;

            Assert.Equal(CodAddTransportationChargeBasisType.NET_CHARGE, detail.ChargeBasis);
            Assert.True(detail.ChargeBasisSpecified);
            Assert.Equal(expectedBasisType, detail.RateTypeBasis);
            Assert.True(detail.RateTypeBasisSpecified);
            Assert.Equal(ChargeBasisLevelType.CURRENT_PACKAGE, detail.ChargeBasisLevel);
            Assert.True(detail.ChargeBasisLevelSpecified);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
