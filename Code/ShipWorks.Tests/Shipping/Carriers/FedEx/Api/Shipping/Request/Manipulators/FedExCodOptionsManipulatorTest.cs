using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    [TestClass]
    public class FedExCodOptionsManipulatorTest
    {
        private FedExCodOptionsManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private Mock<ICarrierSettingsRepository> settingsRepository;

        private ProcessShipmentRequest nativeRequest;
        private ShipmentEntity shipmentEntity;
        private FedExAccountEntity fedExAccount;

        [TestInitialize]
        public void Initialize()
        {
            shipmentEntity = new ShipmentEntity
            {
                ShipCountryCode =  "US",
                OriginCountryCode = "US",
                FedEx = new FedExShipmentEntity
                {
                    CodEnabled = true,
                    CodAmount = 100.50M,
                    Service = (int)FedExServiceType.PriorityOvernight,
                    CodPaymentType = (int)FedExCodPaymentType.Secured,

                    CodCity = "St. Louis",
                    CodFirstName = "Samir",
                    CodLastName = "Nagahnagahnaworkhereanymore",
                    CodPhone = "555-555-5555",
                    CodPostalCode = "63102",
                    CodStreet1 = "1 Memorial Drive",
                    CodStreet2 =  "Suite 2000",
                    CodStateProvCode = "MO",
                    CodCountryCode = "US",

                    CodTrackingNumber = "0123456789",
                    CodTrackingFormID = "9876"
                }
            };

            // Add a couple of packages to the shipment
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity() { DimsHeight = 2, DimsWidth = 2, DimsLength = 2 });
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity() { DimsHeight = 2, DimsWidth = 2, DimsLength = 2 });

            nativeRequest = new ProcessShipmentRequest()
            {
                RequestedShipment = new RequestedShipment()
                {
                    SpecialServicesRequested = new ShipmentSpecialServicesRequested
                    {
                        CodDetail = new CodDetail()
                    },

                    RequestedPackageLineItems = new RequestedPackageLineItem[]
                    {
                        new RequestedPackageLineItem
                        {
                            SpecialServicesRequested = new PackageSpecialServicesRequested
                            {
                                CodDetail = new CodDetail()
                            }
                        }
                    }
                }
            };

            fedExAccount = new FedExAccountEntity { AccountNumber = "123", CountryCode = "US", LastName = "Doe", FirstName = "John" };

            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.UseListRates).Returns(true);
            settingsRepository.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>())).Returns(fedExAccount);

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            carrierRequest.Setup(r => r.CarrierAccountEntity).Returns(fedExAccount);

            testObject = new FedExCodOptionsManipulator(settingsRepository.Object);
        }

        #region Tests for initializing the request and its properties

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull_Test()
        {
            testObject.Manipulate(null);
        }

        [TestMethod]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull_Test()
        {
            // Setup the native request to be null
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, null);

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotProcessShipmentRequest_Test()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new object());

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        public void Manipulate_AccountsForNullRequestedShipment_Test()
        {
            // setup the test by setting the requested shipment to null
            nativeRequest.RequestedShipment = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNotNull(nativeRequest.RequestedShipment);
        }

        [TestMethod]
        public void Manipulate_AccountsForNullPackageLineItemArray_Test()
        {
            // Change this to use a ground service so the package data is updated
            shipmentEntity.FedEx.Service = (int)FedExServiceType.GroundHomeDelivery;

            // setup the test by setting the line item array to null
            nativeRequest.RequestedShipment.RequestedPackageLineItems = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNotNull(nativeRequest.RequestedShipment.RequestedPackageLineItems);
        }

        [TestMethod]
        public void Manipulate_AccountsForNullSpecialServicesRequest_Test()
        {
            // setup the test by setting the line item array to null
            nativeRequest.RequestedShipment.SpecialServicesRequested = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNotNull(nativeRequest.RequestedShipment.SpecialServicesRequested);
        }

        [TestMethod]
        public void Manipulate_AddsLineItem_WhenPackageLineItemArrayIsNull_Test()
        {
            // setup the test by setting the line item array to null
            nativeRequest.RequestedShipment.RequestedPackageLineItems = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(1, nativeRequest.RequestedShipment.RequestedPackageLineItems.Length);
            Assert.IsNotNull(nativeRequest.RequestedShipment.RequestedPackageLineItems[0]);
        }

        [TestMethod]
        public void Manipulate_AccountsForNullShipmentSpecialServicesRequested_Test()
        {
            // setup the test by setting the special services requested property to null
            nativeRequest.RequestedShipment.SpecialServicesRequested = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNotNull(nativeRequest.RequestedShipment.SpecialServicesRequested);
        }

        [TestMethod]
        public void Manipulate_AccountsForNullShipmentCodDetail_Test()
        {
            // setup the test by setting the special services requested property to null
            nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNotNull(nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail);
        }

        [TestMethod]
        public void Manipulate_AccountsForNullPackageSpecialServicesRequested_Test()
        {
            // Change this to use a ground service so the package data is updated
            shipmentEntity.FedEx.Service = (int)FedExServiceType.GroundHomeDelivery;

            // setup the test by setting the special services requested property to null
            nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested = null;

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNotNull(nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested);
        }

        [TestMethod]
        public void Manipulate_AccountsForNullPackageCodDetail_Test()
        {
            // Change this to use a ground service so the package data is updated
            shipmentEntity.FedEx.Service = (int) FedExServiceType.GroundHomeDelivery;

            // setup the test by setting the special services requested property to null
            nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.CodDetail = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNotNull(nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.CodDetail);
        }

        #endregion Tests for initializing the request and its properties


        #region COD Amount Tests

        [TestMethod]
        public void Manipulate_AddsCODSpecialServiceType_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            ShipmentSpecialServiceType[] serviceTypes = nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes;
            Assert.AreEqual(1, serviceTypes.Count(s => s == ShipmentSpecialServiceType.COD));
        }

        [TestMethod]
        public void Manipulate_SetsCurrencyToUSD_WhenRecipientCountryCodeIsUS_AndServiceIsNotGround_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // Pull the COD detail out of the shipment
            CodDetail codDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.AreEqual("USD", codDetail.CodCollectionAmount.Currency);
        }

        [TestMethod]
        public void Manipulate_SetsCurrencyToShipCountryCurrency_WhenRecipientCountryCodeIsCA_AndServiceIsNotGround_Test()
        {
            shipmentEntity.ShipCountryCode = "CA";

            testObject.Manipulate(carrierRequest.Object);

            // Pull the COD detail out of the shipment
            CodDetail codDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.AreEqual("CAD", codDetail.CodCollectionAmount.Currency);
        }

        [TestMethod]
        public void Manipulate_CodAmountIsAtShipmentLevel_WhenServiceIsNotGround_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // Pull the COD detail out of the line items - the COD amount should be calcuated at the shipment level 
            // since this is a NOT ground shipment. The shipment was configured with COD amount of 100.50, so the COD
            // amount at the shipment level should be the full amount
            CodDetail codDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.AreEqual(100.5M, codDetail.CodCollectionAmount.Amount);
        }

        [TestMethod]
        public void Manipulate_SetsCurrencyToUSD_WhenRecipientCountryCodeIsUS_AndServiceIsFedExGround_Test()
        {
            shipmentEntity.FedEx.Service = (int) FedExServiceType.FedExGround;

            testObject.Manipulate(carrierRequest.Object);

            // Pull the COD detail out of the line items
            CodDetail codDetail = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.CodDetail;
            Assert.AreEqual("USD", codDetail.CodCollectionAmount.Currency);
        }

        [TestMethod]
        public void Manipulate_SetsCurrencyToCAD_WhenRecipientCountryCodeIsCA_AndServiceIsFedExGround_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
            shipmentEntity.ShipCountryCode = "CA";

            testObject.Manipulate(carrierRequest.Object);

            // Pull the COD detail out of the line items
            CodDetail codDetail = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.CodDetail;
            Assert.AreEqual("CAD", codDetail.CodCollectionAmount.Currency);
        }

        [TestMethod]
        public void Manipulate_SetsCurrencyToCAD_WhenRecipientCountryCodeIsCA_AndServiceIsGroundHomeDelivery_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.GroundHomeDelivery;
            shipmentEntity.ShipCountryCode = "CA";

            testObject.Manipulate(carrierRequest.Object);

            // Pull the COD detail out of the line items
            CodDetail codDetail = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.CodDetail;
            Assert.AreEqual("CAD", codDetail.CodCollectionAmount.Currency);
        }

        [TestMethod]
        public void Manipulate_SetsCurrencyToUSD_WhenRecipientCountryCodeIsUS_AndServiceIsGroundHomeDelivery_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.GroundHomeDelivery;

            testObject.Manipulate(carrierRequest.Object);

            // Pull the COD detail out of the line items
            CodDetail codDetail = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.CodDetail;
            Assert.AreEqual("USD", codDetail.CodCollectionAmount.Currency);
        }

        [TestMethod]
        public void Manipulate_DistributesAmountAcrossAllPackages_WhenServiceIsFedExGround_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;

            testObject.Manipulate(carrierRequest.Object);

            // Pull the COD detail out of the line items - the COD amount should be calcuated at the package level 
            // since this is a ground shipment. The shipment was configured with COD amount of 100.50, so the COD
            // amount on the package should be 50.25
            CodDetail codDetail = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.CodDetail;
            Assert.AreEqual(50.25M, codDetail.CodCollectionAmount.Amount);
        }

        [TestMethod]
        public void Manipulate_DistributesAmountAcrossAllPackages_WhenServiceIsGroundHomeDelivery_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;

            testObject.Manipulate(carrierRequest.Object);

            // Pull the COD detail out of the line items - the COD amount should be calcuated at the package level 
            // since this is a ground shipment. The shipment was configured with COD amount of 100.50, so the COD
            // amount on the package should be 50.25
            CodDetail codDetail = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.CodDetail;
            Assert.AreEqual(50.25M, codDetail.CodCollectionAmount.Amount);
        }

        #endregion COD Amount Tests

        [TestMethod]
        public void Manipulate_AssignsGuaranteedFundsPaymentType_WhenPaymentTypeIsSecured_Test()
        {
            shipmentEntity.FedEx.CodPaymentType = (int) FedExCodPaymentType.Secured;

            testObject.Manipulate(carrierRequest.Object);

            CodDetail codDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.AreEqual(CodCollectionType.GUARANTEED_FUNDS, codDetail.CollectionType);
        }

        [TestMethod]
        public void Manipulate_AssignsCashPaymentType_WhenPaymentTypeIsUnsecured_Test()
        {
            shipmentEntity.FedEx.CodPaymentType = (int)FedExCodPaymentType.Unsecured;

            testObject.Manipulate(carrierRequest.Object);

            CodDetail codDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.AreEqual(CodCollectionType.CASH, codDetail.CollectionType);
        }

        [TestMethod]
        public void Manipulate_AssignsAnyPaymentType_WhenPaymentTypeIsAny_Test()
        {
            shipmentEntity.FedEx.CodPaymentType = (int)FedExCodPaymentType.Any;

            testObject.Manipulate(carrierRequest.Object);

            CodDetail codDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.AreEqual(CodCollectionType.ANY, codDetail.CollectionType);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Manipulate_ThrowsInvalidOperationException_WhenInvalidPaymentTypeIsGiven_Test()
        {
            shipmentEntity.FedEx.CodPaymentType = 45;

            testObject.Manipulate(carrierRequest.Object);
        }


        [TestMethod]
        public void Manipulate_AddsChargeBasisType_WhenFreightShipment_Test()
        {
            shipmentEntity.FedEx.CodAddFreight = true;

            shipmentEntity.FedEx.CodChargeBasis = (int) CodAddTransportationChargeBasisType.NET_FREIGHT;

            testObject.Manipulate(carrierRequest.Object);

            CodDetail codDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.AreEqual(CodAddTransportationChargeBasisType.NET_FREIGHT, codDetail.AddTransportationChargesDetail.ChargeBasis);
        }

        [TestMethod]
        public void Manipulate_ChargeBasisIsSpecified_WhenFreightShipment_Test()
        {
            shipmentEntity.FedEx.CodAddFreight = true;

            testObject.Manipulate(carrierRequest.Object);

            CodDetail codDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.IsTrue(codDetail.AddTransportationChargesDetail.ChargeBasisSpecified);
        }

        [TestMethod]
        public void Manipulate_RateBasisTypeIsList_WhenFreightShipment_AndUsingListRate_Test()
        {
            shipmentEntity.FedEx.CodAddFreight = true;
            settingsRepository.Setup(r => r.UseListRates).Returns(true);

            testObject.Manipulate(carrierRequest.Object);

            CodDetail codDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.AreEqual(RateTypeBasisType.LIST, codDetail.AddTransportationChargesDetail.RateTypeBasis);
        }

        [TestMethod]
        public void Manipulate_RateBasisTypeIsAccount_WhenFreightShipment_AndNotUsingListRate_Test()
        {
            shipmentEntity.FedEx.CodAddFreight = true;
            settingsRepository.Setup(r => r.UseListRates).Returns(false);

            testObject.Manipulate(carrierRequest.Object);

            CodDetail codDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.AreEqual(RateTypeBasisType.ACCOUNT, codDetail.AddTransportationChargesDetail.RateTypeBasis);
        }

        [TestMethod]
        public void Manipulate_RateBasisIsSpecified_WhenFreightShipment_Test()
        {
            shipmentEntity.FedEx.CodAddFreight = true;

            testObject.Manipulate(carrierRequest.Object);

            CodDetail codDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.IsTrue(codDetail.AddTransportationChargesDetail.RateTypeBasisSpecified);
        }

        [TestMethod]
        public void Manipulate_ChargeBasisLevelIsCurrentPackage_WhenFreightShipment_Test()
        {
            shipmentEntity.FedEx.CodAddFreight = true;

            testObject.Manipulate(carrierRequest.Object);

            CodDetail codDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.AreEqual(ChargeBasisLevelType.CURRENT_PACKAGE, codDetail.AddTransportationChargesDetail.ChargeBasisLevel);
        }

        [TestMethod]
        public void Manipulate_ChargeBasisLevelIsSpecified_WhenFreightShipment_Test()
        {
            shipmentEntity.FedEx.CodAddFreight = true;

            testObject.Manipulate(carrierRequest.Object);

            CodDetail codDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.IsTrue(codDetail.AddTransportationChargesDetail.ChargeBasisLevelSpecified);
        }

        [TestMethod]
        public void Manipulate_AddTransportationDetailIsNull_WhenNotFreightShipment_Test()
        {
            shipmentEntity.FedEx.CodAddFreight = false;

            testObject.Manipulate(carrierRequest.Object);

            CodDetail codDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.IsNull(codDetail.AddTransportationChargesDetail);
        }


        [TestMethod]
        public void Manipulate_AddsReceipient_WithPersonName_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            CodDetail codDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.AreEqual("Samir Nagahnagahnaworkhereanymore", codDetail.CodRecipient.Contact.PersonName);
        }

        [TestMethod]
        public void Manipulate_AddsReceipient_WithStreet1_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            CodDetail codDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.AreEqual("1 Memorial Drive", codDetail.CodRecipient.Address.StreetLines[0]);
        }

        [TestMethod]
        public void Manipulate_AddsReceipient_WithStreet2_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            CodDetail codDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.AreEqual("Suite 2000", codDetail.CodRecipient.Address.StreetLines[1]);
        }

        [TestMethod]
        public void Manipulate_AddsReceipient_WithCity_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            CodDetail codDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.AreEqual("St. Louis", codDetail.CodRecipient.Address.City);
        }

        [TestMethod]
        public void Manipulate_AddsReceipient_WithState_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            CodDetail codDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.AreEqual("MO", codDetail.CodRecipient.Address.StateOrProvinceCode);
        }

        [TestMethod]
        public void Manipulate_AddsReceipient_WithPostalCode_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            CodDetail codDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.AreEqual("63102", codDetail.CodRecipient.Address.PostalCode);
        }

        [TestMethod]
        public void Manipulate_AddsRecipient_WithUSCountryCode_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            CodDetail codDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.AreEqual("US", codDetail.CodRecipient.Address.CountryCode);
        }

        [TestMethod]
        public void Manipulate_AddsRecipient_WithAccountNumber_WhenCodAccountNumberIsNotEmpty_Test()
        {
            shipmentEntity.FedEx.CodAccountNumber = "12345";

            testObject.Manipulate(carrierRequest.Object);

            CodDetail codDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.AreEqual("12345", codDetail.CodRecipient.AccountNumber);
        }

        [TestMethod]
        public void Manipulate_AddsRecipient_WithoutAccountNumber_WhenCodAccountNumberIsEmpty_Test()
        {
            shipmentEntity.FedEx.CodAccountNumber = string.Empty;

            testObject.Manipulate(carrierRequest.Object);

            CodDetail codDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.IsNull(codDetail.CodRecipient.AccountNumber);
        }

        [TestMethod]
        public void Manipulate_AddsRecipient_WithoutAccountNumber_WhenCodAccountNumberIsNull_Test()
        {
            shipmentEntity.FedEx.CodAccountNumber = null;

            testObject.Manipulate(carrierRequest.Object);

            CodDetail codDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.IsNull(codDetail.CodRecipient.AccountNumber);
        }

        [TestMethod]
        public void Manipulate_AddsTrackingId_ToLastPackageInShipment_Test()
        {
            // Setup the request so the sequence number indicates the last package is being processed
            // based on the shipment entity (sequence number is zero-based)
            carrierRequest.Object.SequenceNumber = shipmentEntity.FedEx.Packages.Count - 1;

            testObject.Manipulate(carrierRequest.Object);

            CodDetail codDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.AreEqual("0123456789", codDetail.ReturnTrackingId.TrackingNumber);
        }

        [TestMethod]
        public void Manipulate_AddsFormId_ToLastPackageInShipment_Test()
        {
            // Setup the request so the sequence number indicates the last package is being processed
            // based on the shipment entity (sequence number is zero-based)
            carrierRequest.Object.SequenceNumber = shipmentEntity.FedEx.Packages.Count - 1;

            testObject.Manipulate(carrierRequest.Object);

            CodDetail codDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.AreEqual("9876", codDetail.ReturnTrackingId.FormId);
        }

        [TestMethod]
        public void Manipulate_DoesNotAddTrackingId_WhenCurrentPackageIsNotLastPackageInShipment_Test()
        {
            // Setup the request so the sequence number to indicate that it is not the last package
            // based on the shipment entity (shipment is configured to have two packages in initialize method).
            // Sequence number is zero-based.
            carrierRequest.Object.SequenceNumber = 0;

            testObject.Manipulate(carrierRequest.Object);

            CodDetail codDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.IsNull(codDetail.ReturnTrackingId);
        }

        [TestMethod]
        public void Manipulate_DoesNotAddFormId_WhenCurrentPackageIsNotLastPackageInShipment_Test()
        {
            // Setup the request so the sequence number to indicate that it is not the last package
            // based on the shipment entity (shipment is configured to have two packages in initialize method)
            // Sequence number is zero-based.
            carrierRequest.Object.SequenceNumber = 0;

            testObject.Manipulate(carrierRequest.Object);

            CodDetail codDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.IsNull(codDetail.ReturnTrackingId);
        }

        [TestMethod]
        public void Manipulate_TaxpayerIdentificationIsNotNull_WhenCodTaxIdIsProvided_Test()
        {
            shipmentEntity.FedEx.CodTIN = "123";

            testObject.Manipulate(carrierRequest.Object);

            CodDetail codDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.IsNotNull(codDetail.CodRecipient.Tins);
        }

        [TestMethod]
        public void Manipulate_TaxpayerIdentificationHasOneElement_WhenCodTaxIdIsProvided_Test()
        {
            shipmentEntity.FedEx.CodTIN = "123";

            testObject.Manipulate(carrierRequest.Object);

            CodDetail codDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.AreEqual(1, codDetail.CodRecipient.Tins.Length);
        }

        [TestMethod]
        public void Manipulate_TaxpayerIdentificationElementIsNotNull_WhenCodTaxIdIsProvided_Test()
        {
            shipmentEntity.FedEx.CodTIN = "123";

            testObject.Manipulate(carrierRequest.Object);

            CodDetail codDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.IsNotNull(codDetail.CodRecipient.Tins[0]);
        }

        [TestMethod]
        public void Manipulate_TaxpayerIdentificationIsNull_WhenCodTaxIdIsEmpty_Test()
        {
            shipmentEntity.FedEx.CodTIN = string.Empty;

            testObject.Manipulate(carrierRequest.Object);

            CodDetail codDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.IsNull(codDetail.CodRecipient.Tins);
        }

        [TestMethod]
        public void Manipulate_TaxpayerIdentificationIsNull_WhenCodTaxIdIsNull_Test()
        {
            shipmentEntity.FedEx.CodTIN = null;

            testObject.Manipulate(carrierRequest.Object);

            CodDetail codDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.IsNull(codDetail.CodRecipient.Tins);
        }

        [TestMethod]
        public void Manipulate_TaxNumber_MatchesCodTaxId_Test()
        {
            shipmentEntity.FedEx.CodTIN = "123";

            testObject.Manipulate(carrierRequest.Object);

            CodDetail codDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.AreEqual("123", codDetail.CodRecipient.Tins[0].Number);
        }

        [TestMethod]
        public void Manipulate_TinTypeIsPersonal_WhenCodTaxIdIsProvided_Test()
        {
            shipmentEntity.FedEx.CodTIN = "123";

            testObject.Manipulate(carrierRequest.Object);

            CodDetail codDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.CodDetail;
            Assert.AreEqual(TinType.PERSONAL_STATE, codDetail.CodRecipient.Tins[0].TinType);
        }
    }
}
