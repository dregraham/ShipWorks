using System;
using System.Linq;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    public class FedExCodOptionsManipulatorTest
    {
        private FedExCodOptionsManipulator testObject;
        private readonly AutoMock mock;
        private ShipmentEntity shipment;
        private FedExAccountEntity fedExAccount;

        public FedExCodOptionsManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            shipment = new ShipmentEntity
            {
                ShipCountryCode = "US",
                OriginCountryCode = "US",
                FedEx = new FedExShipmentEntity
                {
                    CodEnabled = true,
                    CodAmount = 100.50M,
                    Service = (int) FedExServiceType.PriorityOvernight,
                    CodPaymentType = (int) FedExCodPaymentType.Secured,

                    CodCity = "St. Louis",
                    CodFirstName = "Samir",
                    CodLastName = "Nagahnagahnaworkhereanymore",
                    CodPhone = "555-555-5555",
                    CodPostalCode = "63102",
                    CodStreet1 = "1 Memorial Drive",
                    CodStreet2 = "Suite 2000",
                    CodStateProvCode = "MO",
                    CodCountryCode = "US",

                    CodTrackingNumber = "0123456789",
                    CodTrackingFormID = "9876"
                }
            };

            // Add a couple of packages to the shipment
            shipment.FedEx.Packages.Add(new FedExPackageEntity() { DimsHeight = 2, DimsWidth = 2, DimsLength = 2 });
            shipment.FedEx.Packages.Add(new FedExPackageEntity() { DimsHeight = 2, DimsWidth = 2, DimsLength = 2 });

            fedExAccount = new FedExAccountEntity { AccountNumber = "123", CountryCode = "US", LastName = "Doe", FirstName = "John" };

            testObject = mock.Create<FedExCodOptionsManipulator>();
        }

        [Fact]
        public void Manipulate_AccountsForNullRequestedShipment()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.NotNull(result.Value.RequestedShipment);
        }

        [Fact]
        public void Manipulate_AccountsForNullPackageLineItemArray()
        {
            // Change this to use a ground service so the package data is updated
            shipment.FedEx.Service = (int) FedExServiceType.GroundHomeDelivery;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.NotNull(result.Value.RequestedShipment.RequestedPackageLineItems);
        }

        [Fact]
        public void Manipulate_AccountsForNullSpecialServicesRequest()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.NotNull(result.Value.RequestedShipment.SpecialServicesRequested);
        }

        [Fact]
        public void Manipulate_AddsLineItem_WhenPackageLineItemArrayIsNull()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Equal(1, result.Value.RequestedShipment.RequestedPackageLineItems.Length);
            Assert.NotNull(result.Value.RequestedShipment.RequestedPackageLineItems[0]);
        }

        [Fact]
        public void Manipulate_AccountsForNullShipmentSpecialServicesRequested()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.NotNull(result.Value.RequestedShipment.SpecialServicesRequested);
        }

        [Fact]
        public void Manipulate_AccountsForNullShipmentCodDetail()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.NotNull(result.Value.RequestedShipment.SpecialServicesRequested.CodDetail);
        }

        [Fact]
        public void Manipulate_AccountsForNullPackageSpecialServicesRequested()
        {
            // Change this to use a ground service so the package data is updated
            shipment.FedEx.Service = (int) FedExServiceType.GroundHomeDelivery;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.NotNull(result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested);
        }

        [Fact]
        public void Manipulate_AccountsForNullPackageCodDetail()
        {
            // Change this to use a ground service so the package data is updated
            shipment.FedEx.Service = (int) FedExServiceType.GroundHomeDelivery;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.NotNull(result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.CodDetail);
        }

        [Fact]
        public void Manipulate_AddsCODSpecialServiceType()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            ShipmentSpecialServiceType[] serviceTypes = result.Value.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes;
            Assert.Equal(1, serviceTypes.Count(s => s == ShipmentSpecialServiceType.COD));
        }

        [Fact]
        public void Manipulate_SetsCurrencyToUSD_WhenRecipientCountryCodeIsUS_AndServiceIsNotGround()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            // Pull the COD detail out of the shipment
            CodDetail codDetail = result.Value.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.Equal("USD", codDetail.CodCollectionAmount.Currency);
        }

        [Fact]
        public void Manipulate_SetsCurrencyToShipCountryCurrency_WhenRecipientCountryCodeIsCA_AndServiceIsNotGround()
        {
            shipment.ShipCountryCode = "CA";

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            // Pull the COD detail out of the shipment
            CodDetail codDetail = result.Value.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.Equal("CAD", codDetail.CodCollectionAmount.Currency);
        }

        [Fact]
        public void Manipulate_CodAmountIsAtShipmentLevel_WhenServiceIsNotGround()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            // Pull the COD detail out of the line items - the COD amount should be calculated at the shipment level 
            // since this is a NOT ground shipment. The shipment was configured with COD amount of 100.50, so the COD
            // amount at the shipment level should be the full amount
            CodDetail codDetail = result.Value.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.Equal(100.5M, codDetail.CodCollectionAmount.Amount);
        }

        [Fact]
        public void Manipulate_SetsCurrencyToUSD_WhenRecipientCountryCodeIsUS_AndServiceIsFedExGround()
        {
            shipment.FedEx.Service = (int) FedExServiceType.FedExGround;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            // Pull the COD detail out of the line items
            CodDetail codDetail = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.CodDetail;
            Assert.Equal("USD", codDetail.CodCollectionAmount.Currency);
        }

        [Fact]
        public void Manipulate_SetsCurrencyToCAD_WhenRecipientCountryCodeIsCA_AndServiceIsFedExGround()
        {
            shipment.FedEx.Service = (int) FedExServiceType.FedExGround;
            shipment.ShipCountryCode = "CA";

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            // Pull the COD detail out of the line items
            CodDetail codDetail = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.CodDetail;
            Assert.Equal("CAD", codDetail.CodCollectionAmount.Currency);
        }

        [Fact]
        public void Manipulate_SetsCurrencyToCAD_WhenRecipientCountryCodeIsCA_AndServiceIsGroundHomeDelivery()
        {
            shipment.FedEx.Service = (int) FedExServiceType.GroundHomeDelivery;
            shipment.ShipCountryCode = "CA";

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            // Pull the COD detail out of the line items
            CodDetail codDetail = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.CodDetail;
            Assert.Equal("CAD", codDetail.CodCollectionAmount.Currency);
        }

        [Fact]
        public void Manipulate_SetsCurrencyToUSD_WhenRecipientCountryCodeIsUS_AndServiceIsGroundHomeDelivery()
        {
            shipment.FedEx.Service = (int) FedExServiceType.GroundHomeDelivery;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            // Pull the COD detail out of the line items
            CodDetail codDetail = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.CodDetail;
            Assert.Equal("USD", codDetail.CodCollectionAmount.Currency);
        }

        [Fact]
        public void Manipulate_DistributesAmountAcrossAllPackages_WhenServiceIsFedExGround()
        {
            shipment.FedEx.Service = (int) FedExServiceType.FedExGround;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            // Pull the COD detail out of the line items - the COD amount should be calculated at the package level 
            // since this is a ground shipment. The shipment was configured with COD amount of 100.50, so the COD
            // amount on the package should be 50.25
            CodDetail codDetail = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.CodDetail;
            Assert.Equal(50.25M, codDetail.CodCollectionAmount.Amount);
        }

        [Fact]
        public void Manipulate_DistributesAmountAcrossAllPackages_WhenServiceIsGroundHomeDelivery()
        {
            shipment.FedEx.Service = (int) FedExServiceType.FedExGround;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            // Pull the COD detail out of the line items - the COD amount should be calculated at the package level 
            // since this is a ground shipment. The shipment was configured with COD amount of 100.50, so the COD
            // amount on the package should be 50.25
            CodDetail codDetail = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.CodDetail;
            Assert.Equal(50.25M, codDetail.CodCollectionAmount.Amount);
        }

        [Fact]
        public void Manipulate_AssignsGuaranteedFundsPaymentType_WhenPaymentTypeIsSecured()
        {
            shipment.FedEx.CodPaymentType = (int) FedExCodPaymentType.Secured;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            CodDetail codDetail = result.Value.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.Equal(CodCollectionType.GUARANTEED_FUNDS, codDetail.CollectionType);
        }

        [Fact]
        public void Manipulate_AssignsCashPaymentType_WhenPaymentTypeIsUnsecured()
        {
            shipment.FedEx.CodPaymentType = (int) FedExCodPaymentType.Unsecured;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            CodDetail codDetail = result.Value.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.Equal(CodCollectionType.CASH, codDetail.CollectionType);
        }

        [Fact]
        public void Manipulate_AssignsAnyPaymentType_WhenPaymentTypeIsAny()
        {
            shipment.FedEx.CodPaymentType = (int) FedExCodPaymentType.Any;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            CodDetail codDetail = result.Value.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.Equal(CodCollectionType.ANY, codDetail.CollectionType);
        }

        [Fact]
        public void Manipulate_ThrowsInvalidOperationException_WhenInvalidPaymentTypeIsGiven()
        {
            shipment.FedEx.CodPaymentType = 45;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.True(result.Failure);
            Assert.IsAssignableFrom<InvalidOperationException>(result.Exception);
        }

        [Fact]
        public void Manipulate_AddsChargeBasisType_WhenFreightShipment()
        {
            shipment.FedEx.CodAddFreight = true;

            shipment.FedEx.CodChargeBasis = (int) CodAddTransportationChargeBasisType.NET_FREIGHT;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            CodDetail codDetail = result.Value.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.Equal(CodAddTransportationChargeBasisType.NET_FREIGHT, codDetail.AddTransportationChargesDetail.ChargeBasis);
        }

        [Fact]
        public void Manipulate_ChargeBasisIsSpecified_WhenFreightShipment()
        {
            shipment.FedEx.CodAddFreight = true;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            CodDetail codDetail = result.Value.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.True(codDetail.AddTransportationChargesDetail.ChargeBasisSpecified);
        }

        [Fact]
        public void Manipulate_RateBasisTypeIsList_WhenFreightShipment_AndUsingListRate()
        {
            shipment.FedEx.CodAddFreight = true;
            mock.Mock<IFedExSettingsRepository>().SetupGet(r => r.UseListRates).Returns(true);

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            CodDetail codDetail = result.Value.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.Equal(RateTypeBasisType.LIST, codDetail.AddTransportationChargesDetail.RateTypeBasis);
        }

        [Fact]
        public void Manipulate_RateBasisTypeIsAccount_WhenFreightShipment_AndNotUsingListRate()
        {
            shipment.FedEx.CodAddFreight = true;
            mock.Mock<IFedExSettingsRepository>().SetupGet(r => r.UseListRates).Returns(false);

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            CodDetail codDetail = result.Value.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.Equal(RateTypeBasisType.ACCOUNT, codDetail.AddTransportationChargesDetail.RateTypeBasis);
        }

        [Fact]
        public void Manipulate_RateBasisIsSpecified_WhenFreightShipment()
        {
            shipment.FedEx.CodAddFreight = true;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            CodDetail codDetail = result.Value.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.True(codDetail.AddTransportationChargesDetail.RateTypeBasisSpecified);
        }

        [Fact]
        public void Manipulate_ChargeBasisLevelIsCurrentPackage_WhenFreightShipment()
        {
            shipment.FedEx.CodAddFreight = true;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            CodDetail codDetail = result.Value.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.Equal(ChargeBasisLevelType.CURRENT_PACKAGE, codDetail.AddTransportationChargesDetail.ChargeBasisLevel);
        }

        [Fact]
        public void Manipulate_ChargeBasisLevelIsSpecified_WhenFreightShipment()
        {
            shipment.FedEx.CodAddFreight = true;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            CodDetail codDetail = result.Value.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.True(codDetail.AddTransportationChargesDetail.ChargeBasisLevelSpecified);
        }

        [Fact]
        public void Manipulate_AddTransportationDetailIsNull_WhenNotFreightShipment()
        {
            shipment.FedEx.CodAddFreight = false;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            CodDetail codDetail = result.Value.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.Null(codDetail.AddTransportationChargesDetail);
        }


        [Fact]
        public void Manipulate_AddsReceipient_WithPersonName()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            CodDetail codDetail = result.Value.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.Equal("Samir Nagahnagahnaworkhereanymore", codDetail.CodRecipient.Contact.PersonName);
        }

        [Fact]
        public void Manipulate_AddsReceipient_WithStreet1()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            CodDetail codDetail = result.Value.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.Equal("1 Memorial Drive", codDetail.CodRecipient.Address.StreetLines[0]);
        }

        [Fact]
        public void Manipulate_AddsReceipient_WithStreet2()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            CodDetail codDetail = result.Value.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.Equal("Suite 2000", codDetail.CodRecipient.Address.StreetLines[1]);
        }

        [Fact]
        public void Manipulate_AddsReceipient_WithCity()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            CodDetail codDetail = result.Value.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.Equal("St. Louis", codDetail.CodRecipient.Address.City);
        }

        [Fact]
        public void Manipulate_AddsReceipient_WithState()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            CodDetail codDetail = result.Value.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.Equal("MO", codDetail.CodRecipient.Address.StateOrProvinceCode);
        }

        [Fact]
        public void Manipulate_AddsReceipient_WithPostalCode()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            CodDetail codDetail = result.Value.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.Equal("63102", codDetail.CodRecipient.Address.PostalCode);
        }

        [Fact]
        public void Manipulate_AddsRecipient_WithUSCountryCode()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            CodDetail codDetail = result.Value.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.Equal("US", codDetail.CodRecipient.Address.CountryCode);
        }

        [Fact]
        public void Manipulate_AddsRecipient_WithAccountNumber_WhenCodAccountNumberIsNotEmpty()
        {
            shipment.FedEx.CodAccountNumber = "12345";

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            CodDetail codDetail = result.Value.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.Equal("12345", codDetail.CodRecipient.AccountNumber);
        }

        [Fact]
        public void Manipulate_AddsRecipient_WithoutAccountNumber_WhenCodAccountNumberIsEmpty()
        {
            shipment.FedEx.CodAccountNumber = string.Empty;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            CodDetail codDetail = result.Value.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.Null(codDetail.CodRecipient.AccountNumber);
        }

        [Fact]
        public void Manipulate_AddsRecipient_WithoutAccountNumber_WhenCodAccountNumberIsNull()
        {
            shipment.FedEx.CodAccountNumber = null;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            CodDetail codDetail = result.Value.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.Null(codDetail.CodRecipient.AccountNumber);
        }

        [Fact]
        public void Manipulate_AddsTrackingId_ToLastPackageInShipment()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 1);

            CodDetail codDetail = result.Value.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.Equal("0123456789", codDetail.ReturnTrackingId.TrackingNumber);
        }

        [Fact]
        public void Manipulate_AddsFormId_ToLastPackageInShipment()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 1);

            CodDetail codDetail = result.Value.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.Equal("9876", codDetail.ReturnTrackingId.FormId);
        }

        [Fact]
        public void Manipulate_DoesNotAddTrackingId_WhenCurrentPackageIsNotLastPackageInShipment()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            CodDetail codDetail = result.Value.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.Null(codDetail.ReturnTrackingId);
        }

        [Fact]
        public void Manipulate_DoesNotAddFormId_WhenCurrentPackageIsNotLastPackageInShipment()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            CodDetail codDetail = result.Value.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.Null(codDetail.ReturnTrackingId);
        }

        [Fact]
        public void Manipulate_TaxpayerIdentificationIsNotNull_WhenCodTaxIdIsProvided()
        {
            shipment.FedEx.CodTIN = "123";

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            CodDetail codDetail = result.Value.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.NotNull(codDetail.CodRecipient.Tins);
        }

        [Fact]
        public void Manipulate_TaxpayerIdentificationHasOneElement_WhenCodTaxIdIsProvided()
        {
            shipment.FedEx.CodTIN = "123";

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            CodDetail codDetail = result.Value.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.Equal(1, codDetail.CodRecipient.Tins.Length);
        }

        [Fact]
        public void Manipulate_TaxpayerIdentificationElementIsNotNull_WhenCodTaxIdIsProvided()
        {
            shipment.FedEx.CodTIN = "123";

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            CodDetail codDetail = result.Value.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.NotNull(codDetail.CodRecipient.Tins[0]);
        }

        [Fact]
        public void Manipulate_TaxpayerIdentificationIsNull_WhenCodTaxIdIsEmpty()
        {
            shipment.FedEx.CodTIN = string.Empty;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            CodDetail codDetail = result.Value.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.Null(codDetail.CodRecipient.Tins);
        }

        [Fact]
        public void Manipulate_TaxpayerIdentificationIsNull_WhenCodTaxIdIsNull()
        {
            shipment.FedEx.CodTIN = null;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            CodDetail codDetail = result.Value.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.Null(codDetail.CodRecipient.Tins);
        }

        [Fact]
        public void Manipulate_TaxNumber_MatchesCodTaxId()
        {
            shipment.FedEx.CodTIN = "123";

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            CodDetail codDetail = result.Value.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.Equal("123", codDetail.CodRecipient.Tins[0].Number);
        }

        [Fact]
        public void Manipulate_TinTypeIsPersonal_WhenCodTaxIdIsProvided()
        {
            shipment.FedEx.CodTIN = "123";

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            CodDetail codDetail = result.Value.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.Equal(TinType.PERSONAL_STATE, codDetail.CodRecipient.Tins[0].TinType);
        }
    }
}
