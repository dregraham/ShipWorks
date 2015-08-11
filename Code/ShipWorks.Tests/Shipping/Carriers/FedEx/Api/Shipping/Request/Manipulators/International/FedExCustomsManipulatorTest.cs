using System;
using System.Collections.Generic;
using Interapptive.Shared.Enums;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators.International;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators.International
{
    public class FedExCustomsManipulatorTest
    {
        private FedExCustomsManipulator testObject;
        private Mock<CarrierRequest> carrierRequest;
        private Mock<ICarrierSettingsRepository> settingsRepository;
        private ProcessShipmentRequest nativeRequest;
        private ShipmentEntity shipmentEntity;
        private FedExAccountEntity fedExAccount;

        [TestInitialize]
        public void Initialize()
        {
            // Create a ProcessShipmentRequest type and set the properties the manipulator is interested in
            nativeRequest = new ProcessShipmentRequest()
            {
                RequestedShipment = new RequestedShipment()
                {
                    Recipient = new Party()
                    {
                        Tins = new TaxpayerIdentification[1] {new TaxpayerIdentification()}
                    }
                }
            };

            // Create our default shipment entity and initialize the properties our test object will be accessing
            shipmentEntity = new ShipmentEntity()
            {
                ShipCountryCode = "NZ",
                OriginCountryCode = "CA",
                CustomsValue = 123.45M,
                FedEx = new FedExShipmentEntity() {PayorDutiesAccount = "987", PayorTransportName = "Transport Name", PayorDutiesType = (int) FedExPayorType.Sender, CustomsRecipientTIN = "5468"}
            };

            shipmentEntity.CustomsItems.Add(new ShipmentCustomsItemEntity { Description = "item one", Quantity = 4, Weight = 2.3, UnitValue = 15.36M, HarmonizedCode = "Harmonized 1", UnitPriceAmount = 51.36M, NumberOfPieces = 4 });
            shipmentEntity.CustomsItems.Add(new ShipmentCustomsItemEntity { Description = "item two", Quantity = 64, Weight = 112.3, UnitValue = 98.52M, HarmonizedCode = "Harmonized 2", UnitPriceAmount = 114.57M, NumberOfPieces = 12 });
            shipmentEntity.CustomsItems.Add(new ShipmentCustomsItemEntity { Description = "item three", Quantity = 342, Weight = 92.3, UnitValue = 6.67M, HarmonizedCode = "Harmonized 3", UnitPriceAmount = 14.06M, NumberOfPieces = 25 });


            fedExAccount = new FedExAccountEntity {AccountNumber = "123", CountryCode = "US", LastName = "Doe", FirstName = "John"};


            // Setup the carrier request's NativeRequest property to return the ProcessShipmentRequest object
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            carrierRequest.Setup(r => r.CarrierAccountEntity).Returns(fedExAccount);

            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>())).Returns(fedExAccount);

            testObject = new FedExCustomsManipulator(new FedExSettings(settingsRepository.Object));
        }

        [Fact]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull_Test()
        {
            testObject.Manipulate(null);
        }

        [Fact]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull_Test()
        {
            // Setup the native request to be null
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, null);

            testObject.Manipulate(carrierRequest.Object);
        }

        [Fact]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotProcessShipmentRequest_Test()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new object());

            testObject.Manipulate(carrierRequest.Object);
        }

        [Fact]
        public void Manipulate_AccountsForRequestedShipment_Test()
        {
            // Setup the test by configuring the native request to have a null requested shipment property 
            nativeRequest.RequestedShipment = null;

            testObject.Manipulate(carrierRequest.Object);

            // The requested shipment property should be created now
            Assert.IsNotNull(nativeRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_AccountsForRecipient_Test()
        {
            // Setup the test by configuring the native request to have a null recipient property 
            nativeRequest.RequestedShipment.Recipient = null;

            testObject.Manipulate(carrierRequest.Object);

            // The recipient property should be created now
            Assert.IsNotNull(nativeRequest.RequestedShipment.Recipient);
        }

        [Fact]
        public void Manipulate_CustomsDetailIsNull_WhenShipCountryCodeAndOriginCountryCodeAreUS_Test()
        {
            shipmentEntity.ShipCountryCode = "US";
            shipmentEntity.OriginCountryCode = "US";

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNull(nativeRequest.RequestedShipment.CustomsClearanceDetail);
        }

        [Fact]
        public void Manipulate_CustomsDetailIsNull_WhenShipCountryCodeAndOriginCountryCodeAreCA_Test()
        {
            // Test that "US" isn't hard-coded in the manipulator
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.OriginCountryCode = "CA";

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNull(nativeRequest.RequestedShipment.CustomsClearanceDetail);
        }

        [Fact]
        public void Manipulate_CustomsValueAmountIsShipmentCustomsValue_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // The amount on the shipment is 123.45
            Assert.AreEqual(123.45M, nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsValue.Amount);
        }

        [Fact]
        public void Manipulate_CustomsCurrencyValidIsUSD_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual("USD", nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsValue.Currency);
        }

        [Fact]
        public void Manipulate_DocumentContentIsDocumentsOnly_WhenFedExCustomsDocumentsOnlyIsTrue_Test()
        {
            shipmentEntity.FedEx.CustomsDocumentsOnly = true;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(InternationalDocumentContentType.DOCUMENTS_ONLY, nativeRequest.RequestedShipment.CustomsClearanceDetail.DocumentContent);
        }

        [Fact]
        public void Manipulate_DocumentContentIsNonDocuments_WhenFedExCustomsDocumentsOnlyIsFalse_Test()
        {
            shipmentEntity.FedEx.CustomsDocumentsOnly = false;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(InternationalDocumentContentType.NON_DOCUMENTS, nativeRequest.RequestedShipment.CustomsClearanceDetail.DocumentContent);
        }


        #region Commodities Tests
        
        [Fact]
        public void Manipulate_CommoditiesAddsAllShipmentCustomItems_WhenFedExCustomsDocumentsOnlyIsFalse_Test()
        {
            shipmentEntity.FedEx.CustomsDocumentsOnly = false;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(shipmentEntity.CustomsItems.Count, nativeRequest.RequestedShipment.CustomsClearanceDetail.Commodities.Length);
        }

        [Fact]
        public void Manipulate_CommoditiesContainDescriptionFromShipmentCustomItems_WhenFedExCustomsDocumentsOnlyIsFalse_Test()
        {
            shipmentEntity.FedEx.CustomsDocumentsOnly = false;

            testObject.Manipulate(carrierRequest.Object);

            for (int i = 0; i < shipmentEntity.CustomsItems.Count; i++)
            {
                Assert.AreEqual(shipmentEntity.CustomsItems[i].Description, nativeRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].Description);
            }
        }

        [Fact]
        public void Manipulate_CommoditiesContainQuantityFromShipmentCustomItems_WhenFedExCustomsDocumentsOnlyIsFalse_Test()
        {
            shipmentEntity.FedEx.CustomsDocumentsOnly = false;

            testObject.Manipulate(carrierRequest.Object);

            for (int i = 0; i < shipmentEntity.CustomsItems.Count; i++)
            {
                Assert.AreEqual(shipmentEntity.CustomsItems[i].Quantity, (double)nativeRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].Quantity);
            }
        }

        [Fact]
        public void Manipulate_CommoditiesContainQuantityUnitsIsEA_WhenFedExCustomsDocumentsOnlyIsFalse_Test()
        {
            shipmentEntity.FedEx.CustomsDocumentsOnly = false;

            testObject.Manipulate(carrierRequest.Object);

            for (int i = 0; i < shipmentEntity.CustomsItems.Count; i++)
            {
                Assert.AreEqual("EA", nativeRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].QuantityUnits);
            }
        }

        [Fact]
        public void Manipulate_CommoditiesContainNumberofPiecesFromFedExShipmentCustomItem_WhenFedExCustomsDocumentsOnlyIsFalse_Test()
        {
            shipmentEntity.FedEx.CustomsDocumentsOnly = false;

            testObject.Manipulate(carrierRequest.Object);

            for (int i = 0; i < shipmentEntity.CustomsItems.Count; i++)
            {
                Assert.AreEqual(shipmentEntity.CustomsItems[i].NumberOfPieces.ToString(), nativeRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].NumberOfPieces);
            }
        }

        [Fact]
        public void Manipulate_CommoditiesWeightsFromShipmentCustomItem_WhenFedExCustomsDocumentsOnlyIsFalse_Test()
        {
            shipmentEntity.FedEx.CustomsDocumentsOnly = false;

            testObject.Manipulate(carrierRequest.Object);

            for (int i = 0; i < shipmentEntity.CustomsItems.Count; i++)
            {
                Assert.AreEqual((decimal)shipmentEntity.CustomsItems[i].Weight, nativeRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].Weight.Value);
            }
        }

        [Fact]
        public void Manipulate_CommoditiesWeightsIsPounds_WhenFedExCustomsDocumentsOnlyIsFalse_AndCustomItemWeightIsPounds_Test()
        {
            shipmentEntity.FedEx.CustomsDocumentsOnly = false;
            shipmentEntity.FedEx.WeightUnitType = (int) WeightUnitOfMeasure.Pounds;

            testObject.Manipulate(carrierRequest.Object);

            for (int i = 0; i < shipmentEntity.CustomsItems.Count; i++)
            {
                Assert.AreEqual(WeightUnits.LB, nativeRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].Weight.Units);
            }
        }

        [Fact]
        public void Manipulate_CommoditiesWeightsIsKilograms_WhenFedExCustomsDocumentsOnlyIsFalse_AndCustomItemWeightIsKilograms_Test()
        {
            shipmentEntity.FedEx.CustomsDocumentsOnly = false;
            shipmentEntity.FedEx.WeightUnitType = (int)WeightUnitOfMeasure.Kilograms;

            testObject.Manipulate(carrierRequest.Object);

            for (int i = 0; i < shipmentEntity.CustomsItems.Count; i++)
            {
                Assert.AreEqual(WeightUnits.KG, nativeRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].Weight.Units);
            }
        }

        [Fact]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Manipulate_ThrowsInvalidOperationException_WhenFedExCustomsDocumentsOnlyIsFalse_AndShipmentWeightTypeIsUnknown_Test()
        {
            shipmentEntity.FedEx.CustomsDocumentsOnly = false;
            shipmentEntity.FedEx.WeightUnitType = 42;

            testObject.Manipulate(carrierRequest.Object);
        }

        [Fact]
        public void Manipulate_CommoditiesUnitPriceFromFedExShipmentCustomItem_WhenFedExCustomsDocumentsOnlyIsFalse_Test()
        {
            shipmentEntity.FedEx.CustomsDocumentsOnly = false;
            
            testObject.Manipulate(carrierRequest.Object);

            for (int i = 0; i < shipmentEntity.CustomsItems.Count; i++)
            {
                Assert.AreEqual(shipmentEntity.CustomsItems[i].UnitPriceAmount, nativeRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].UnitPrice.Amount);
            }
        }

        [Fact]
        public void Manipulate_CommoditiesUnitPriceCurrencyIsUSD_WhenFedExCustomsDocumentsOnlyIsFalse_AndCurrencyIsUSD_Test()
        {
            shipmentEntity.FedEx.CustomsDocumentsOnly = false;

            testObject.Manipulate(carrierRequest.Object);

            for (int i = 0; i < shipmentEntity.CustomsItems.Count; i++)
            {
                Assert.AreEqual("USD", nativeRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].UnitPrice.Currency);
            }
        }

        [Fact]
        public void Manipulate_CommoditiesUnitPriceCurrencyIsCAD_WhenFedExCustomsDocumentsOnlyIsFalse_AndCurrencyIsCAD_Test()
        {
            shipmentEntity.FedEx.CustomsDocumentsOnly = false;
            fedExAccount.CountryCode = "CA";

            testObject.Manipulate(carrierRequest.Object);

            for (int i = 0; i < shipmentEntity.CustomsItems.Count; i++)
            {
                Assert.AreEqual("CAD", nativeRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].UnitPrice.Currency);
            }
        }

        [Fact]
        public void Manipulate_CommoditiesCountryOfManufacture_IsShipmentCustomItemCountryOfOrigin_WhenFedExCustomsDocumentsOnlyIsFalse_Test()
        {
            shipmentEntity.FedEx.CustomsDocumentsOnly = false;

            testObject.Manipulate(carrierRequest.Object);

            for (int i = 0; i < shipmentEntity.CustomsItems.Count; i++)
            {
                Assert.AreEqual(shipmentEntity.CustomsItems[i].CountryOfOrigin, nativeRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].CountryOfManufacture);
            }
        }

        [Fact]
        public void Manipulate_CommoditiesHarmonizedCode_IsShipmentCustomItemHarmonized_WhenFedExCustomsDocumentsOnlyIsFalse_Test()
        {
            shipmentEntity.FedEx.CustomsDocumentsOnly = false;

            testObject.Manipulate(carrierRequest.Object);

            for (int i = 0; i < shipmentEntity.CustomsItems.Count; i++)
            {
                Assert.AreEqual(shipmentEntity.CustomsItems[i].HarmonizedCode, nativeRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].HarmonizedCode);
            }
        }

        [Fact]
        public void Manipulate_CommoditiesCustomsValueCurrencyIsUSD_WhenFedExCustomsDocumentsOnlyIsFalse_Test()
        {
            shipmentEntity.FedEx.CustomsDocumentsOnly = false;

            testObject.Manipulate(carrierRequest.Object);

            for (int i = 0; i < shipmentEntity.CustomsItems.Count; i++)
            {
                Assert.AreEqual("USD", nativeRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].CustomsValue.Currency);
            }
        }

        [Fact]
        public void Manipulate_CommoditiesCustomsValueFromShipmentCustomItem_WhenFedExCustomsDocumentsOnlyIsFalse_Test()
        {
            shipmentEntity.FedEx.CustomsDocumentsOnly = false;

            testObject.Manipulate(carrierRequest.Object);

            for (int i = 0; i < shipmentEntity.CustomsItems.Count; i++)
            {
                Assert.AreEqual(shipmentEntity.CustomsItems[i].UnitValue, nativeRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].CustomsValue.Amount);
            }
        }

        #endregion Commodities Tests


        #region NAFTA Tests

        [Fact]
        public void Manipulate_NaftaDetailIsNull_WhenNaftaIsNotEnabled_Test()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = false;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.IsNull(commodity.NaftaDetail);
            }
        }

        [Fact]
        public void Manipulate_RegulatoryControlIsNull_WhenNaftaIsNotEnabled_Test()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = false;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.IsNull(customsDetail.RegulatoryControls);
        }


        [Fact]
        public void Manipulate_RegulatoryControlArrayHasSizeOne_WhenNaftaIsEnabled_Test()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.AreEqual(1, customsDetail.RegulatoryControls.Length);
        }

        [Fact]
        public void Manipulate_AddsNaftaAsRegulatoryControl_WhenNaftaIsEnabled_Test()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.AreEqual(RegulatoryControlType.NAFTA, customsDetail.RegulatoryControls[0]);
        }

        [Fact]
        public void Manipulate_NaftaDetailIsNotNull_WhenNaftaIsEnabled_Test()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.IsNotNull(commodity.NaftaDetail);
            }
        }

        [Fact]
        public void Manipulate_NetCostMethodIsNO_WhenNaftaIsEnabled_AndNetCostIsNotCalculated_Test()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;
            shipmentEntity.FedEx.CustomsNaftaNetCostMethod = (int) FedExNaftaNetCostMethod.NotCalculated;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.AreEqual(NaftaNetCostMethodCode.NO, commodity.NaftaDetail.NetCostMethod);
            }
        }

        [Fact]
        public void Manipulate_NetCostMethodIsNC_WhenNaftaIsEnabled_AndNetCostIsCalculated_Test()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;
            shipmentEntity.FedEx.CustomsNaftaNetCostMethod = (int)FedExNaftaNetCostMethod.NetCostMethod;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.AreEqual(NaftaNetCostMethodCode.NC, commodity.NaftaDetail.NetCostMethod);
            }
        }

        [Fact]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Manipulate_ThrowsInvalidOperationException_WhenNaftaIsEnabled_AndNetCostIsInvalid_Test()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;
            shipmentEntity.FedEx.CustomsNaftaNetCostMethod = 54;

            testObject.Manipulate(carrierRequest.Object);
        }

        [Fact]
        public void Manipulate_NetCostMethodIsSpecified_WhenNaftaIsEnabled_Test()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;
            shipmentEntity.FedEx.CustomsNaftaNetCostMethod = (int)FedExNaftaNetCostMethod.NetCostMethod;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.IsTrue(commodity.NaftaDetail.NetCostMethodSpecified);
            }
        }


        [Fact]
        public void Manipulate_PreferenceCriterionIsA_WhenNaftaIsEnable_Test()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;
            shipmentEntity.FedEx.CustomsNaftaPreferenceType = (int)FedExNaftaPreferenceCriteria.A;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.AreEqual(NaftaPreferenceCriterionCode.A, commodity.NaftaDetail.PreferenceCriterion);
            }
        }

        [Fact]
        public void Manipulate_PreferenceCriterionIsB_WhenNaftaIsEnable_Test()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;
            shipmentEntity.FedEx.CustomsNaftaPreferenceType = (int)FedExNaftaPreferenceCriteria.B;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.AreEqual(NaftaPreferenceCriterionCode.B, commodity.NaftaDetail.PreferenceCriterion);
            }
        }

        [Fact]
        public void Manipulate_PreferenceCriterionIsC_WhenNaftaIsEnable_Test()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;
            shipmentEntity.FedEx.CustomsNaftaPreferenceType = (int)FedExNaftaPreferenceCriteria.C;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.AreEqual(NaftaPreferenceCriterionCode.C, commodity.NaftaDetail.PreferenceCriterion);
            }
        }

        [Fact]
        public void Manipulate_PreferenceCriterionIsD_WhenNaftaIsEnable_Test()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;
            shipmentEntity.FedEx.CustomsNaftaPreferenceType = (int)FedExNaftaPreferenceCriteria.D;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.AreEqual(NaftaPreferenceCriterionCode.D, commodity.NaftaDetail.PreferenceCriterion);
            }
        }

        [Fact]
        public void Manipulate_PreferenceCriterionIsE_WhenNaftaIsEnable_Test()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;
            shipmentEntity.FedEx.CustomsNaftaPreferenceType = (int)FedExNaftaPreferenceCriteria.E;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.AreEqual(NaftaPreferenceCriterionCode.E, commodity.NaftaDetail.PreferenceCriterion);
            }
        }

        [Fact]
        public void Manipulate_PreferenceCriterionIsF_WhenNaftaIsEnable_Test()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;
            shipmentEntity.FedEx.CustomsNaftaPreferenceType = (int)FedExNaftaPreferenceCriteria.F;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.AreEqual(NaftaPreferenceCriterionCode.F, commodity.NaftaDetail.PreferenceCriterion);
            }
        }

        [Fact]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Manipulate_ThrowsInvalidOperationException_WhenNaftaIsEnable_AndPreferenceIsInvalid_Test()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;
            shipmentEntity.FedEx.CustomsNaftaPreferenceType = 40;

            testObject.Manipulate(carrierRequest.Object);
        }


        [Fact]
        public void Manipulate_PreferenceCriterionIsSpecified_WhenNaftaIsEnable_Test()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;
            shipmentEntity.FedEx.CustomsNaftaPreferenceType = (int)FedExNaftaPreferenceCriteria.F;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.IsTrue(commodity.NaftaDetail.PreferenceCriterionSpecified);
            }
        }


        [Fact]
        public void Manipulate_ProducerDeterminationIsYes_WhenNaftaIsEnable_AndCodeIsProducerOfCommodity_Test()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;
            shipmentEntity.FedEx.CustomsNaftaDeterminationCode = (int) FedExNaftaDeterminationCode.ProducerOfCommodity;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.AreEqual(NaftaProducerDeterminationCode.YES, commodity.NaftaDetail.ProducerDetermination);
            }
        }

        [Fact]
        public void Manipulate_ProducerDeterminationIsNo1_WhenNaftaIsEnable_AndCodeIsNotProducerKnowledge_Test()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;
            shipmentEntity.FedEx.CustomsNaftaDeterminationCode = (int)FedExNaftaDeterminationCode.NotProducerKnowledgeOfCommodity;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.AreEqual(NaftaProducerDeterminationCode.NO_1, commodity.NaftaDetail.ProducerDetermination);
            }
        }

        [Fact]
        public void Manipulate_ProducerDeterminationIsNo2_WhenNaftaIsEnable_AndCodeIsNotProducerStatement_Test()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;
            shipmentEntity.FedEx.CustomsNaftaDeterminationCode = (int)FedExNaftaDeterminationCode.NotProducerWrittenStatement;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.AreEqual(NaftaProducerDeterminationCode.NO_2, commodity.NaftaDetail.ProducerDetermination);
            }
        }

        [Fact]
        public void Manipulate_ProducerDeterminationIsNo3_WhenNaftaIsEnable_AndCodeIsNotProducerSignedCertificate_Test()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;
            shipmentEntity.FedEx.CustomsNaftaDeterminationCode = (int)FedExNaftaDeterminationCode.NotProducerSignedCertificate;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.AreEqual(NaftaProducerDeterminationCode.NO_3, commodity.NaftaDetail.ProducerDetermination);
            }
        }

        [Fact]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Manipulate_ThrowsInvalidOperationException_WhenNaftaIsEnable_AndProducerIsInvalid_Test()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;
            shipmentEntity.FedEx.CustomsNaftaPreferenceType = 13;

            testObject.Manipulate(carrierRequest.Object);
        }


        [Fact]
        public void Manipulate_ProducerDeterminationIsSpecified_WhenNaftaIsEnable_Test()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;
            shipmentEntity.FedEx.CustomsNaftaPreferenceType = (int)FedExNaftaPreferenceCriteria.F;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.IsTrue(commodity.NaftaDetail.ProducerDeterminationSpecified);
            }
        }



        #endregion NAFTA Tests
        

        #region Payment Detail Tests

        [Fact]
        public void Manipulate_CustomsDutiesPaymentIsNotNull_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNotNull(nativeRequest.RequestedShipment.CustomsClearanceDetail.DutiesPayment);
        }


        [Fact]
        public void Manipulate_UsesFedExAccountNumber_WhenPayorDutiesTypeIsSender_Test()
        {
            // Setup the fedex shipment payor type for the test
            shipmentEntity.FedEx.PayorDutiesType = (int)FedExPayorType.Sender;

            testObject.Manipulate((carrierRequest.Object));

            Payment dutiesPayment = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.CustomsClearanceDetail.DutiesPayment;
            Assert.AreEqual(fedExAccount.AccountNumber, dutiesPayment.Payor.ResponsibleParty.AccountNumber);
        }

        [Fact]
        public void Manipulate_UsesFedExAccountCountryCode_WhenPayorDutiesTypeIsSender_Test()
        {
            // Setup the fedex shipment payor type for the test
            shipmentEntity.FedEx.PayorDutiesType = (int)FedExPayorType.Sender;

            testObject.Manipulate((carrierRequest.Object));

            Payment dutiesPayment = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.CustomsClearanceDetail.DutiesPayment;
            Assert.AreEqual(fedExAccount.CountryCode, dutiesPayment.Payor.ResponsibleParty.Address.CountryCode);
        }

        [Fact]
        public void Manipulate_UsesFedExAccountNameAsContactName_WhenPayorDutiesTypeIsSender_Test()
        {
            // Setup the fedex shipment payor type for the test
            shipmentEntity.FedEx.PayorDutiesType = (int)FedExPayorType.Sender;

            testObject.Manipulate((carrierRequest.Object));

            Payment dutiesPayment = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.CustomsClearanceDetail.DutiesPayment;
            Assert.AreEqual(fedExAccount.FirstName + " " + fedExAccount.LastName, dutiesPayment.Payor.ResponsibleParty.Contact.PersonName);
        }


        [Fact]
        public void Manipulate_SetsPaymentTypeToRecipient_Test()
        {
            // Setup the fedex shipment payor type for the test
            shipmentEntity.FedEx.PayorDutiesType = (int)FedExPayorType.Recipient;

            testObject.Manipulate((carrierRequest.Object));

            Payment dutiesPayment = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.CustomsClearanceDetail.DutiesPayment;
            Assert.AreEqual(PaymentType.RECIPIENT, dutiesPayment.PaymentType);
        }

        [Fact]
        public void Manipulate_UsesPayorDutiesCountryCode_WhenPayorDutiesTypeIsRecipient_Test()
        {
            // Setup the fedex shipment payor type for the test
            shipmentEntity.FedEx.PayorDutiesType = (int)FedExPayorType.Recipient;
            shipmentEntity.FedEx.PayorDutiesCountryCode = "CA";

            testObject.Manipulate((carrierRequest.Object));

            Payment dutiesPayment = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.CustomsClearanceDetail.DutiesPayment;
            Assert.AreEqual("CA", dutiesPayment.Payor.ResponsibleParty.Address.CountryCode);
        }

        [Fact]
        public void Manipulate_UsesFedExShipmentPayorDutiesAccount_WhenPayorDutiesTypeIsRecipient_Test()
        {
            // Setup the fedex shipment payor type for the test
            shipmentEntity.FedEx.PayorDutiesType = (int)FedExPayorType.Recipient;

            testObject.Manipulate((carrierRequest.Object));

            Payment dutiesPayment = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.CustomsClearanceDetail.DutiesPayment;
            Assert.AreEqual(shipmentEntity.FedEx.PayorDutiesAccount, dutiesPayment.Payor.ResponsibleParty.AccountNumber);
        }

        [Fact]
        public void Manipulate_UsesPayorDutiesName_WhenPayorDutiesTypeIsRecepient_Test()
        {
            shipmentEntity.FedEx.PayorDutiesType = (int)FedExPayorType.Recipient;
            shipmentEntity.FedEx.PayorDutiesName = "Peter Gibbons";

            testObject.Manipulate(carrierRequest.Object);

            Payment dutiesPayment = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.CustomsClearanceDetail.DutiesPayment;
            Assert.AreEqual("Peter Gibbons", dutiesPayment.Payor.ResponsibleParty.Contact.PersonName);
        }

        [Fact]
        public void Manipulate_SetsPaymentTypeToThirdParty_Test()
        {
            // Setup the fedex shipment payor type for the test
            shipmentEntity.FedEx.PayorDutiesType = (int)FedExPayorType.ThirdParty;

            testObject.Manipulate((carrierRequest.Object));

            Payment dutiesPayment = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.CustomsClearanceDetail.DutiesPayment;
            Assert.AreEqual(PaymentType.THIRD_PARTY, dutiesPayment.PaymentType);
        }

        [Fact]
        public void Manipulate_UsesFedExShipmentPayorDutiesAccount_WhenPayorDutiesTypeIsThirdParty_Test()
        {
            // Setup the fedex shipment payor type for the test
            shipmentEntity.FedEx.PayorDutiesType = (int)FedExPayorType.ThirdParty;

            testObject.Manipulate((carrierRequest.Object));

            Payment dutiesPayment = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.CustomsClearanceDetail.DutiesPayment;
            Assert.AreEqual(shipmentEntity.FedEx.PayorDutiesAccount, dutiesPayment.Payor.ResponsibleParty.AccountNumber);
        }

        [Fact]
        public void Manipulate_UsesPayorDutiesName_WhenPayorDutiesTypeIsThirdParty_Test()
        {
            shipmentEntity.FedEx.PayorDutiesType = (int)FedExPayorType.ThirdParty;
            shipmentEntity.FedEx.PayorDutiesName = "Michael Bolton";

            testObject.Manipulate(carrierRequest.Object);

            Payment dutiesPayment = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.CustomsClearanceDetail.DutiesPayment;
            Assert.AreEqual("Michael Bolton", dutiesPayment.Payor.ResponsibleParty.Contact.PersonName);
        }

        [Fact]
        public void Manipulate_UsesPayorDutiesCountryCode_WhenPayorDutiesTypeIsThirdParty_Test()
        {
            // Setup the fedex shipment payor type for the test
            shipmentEntity.FedEx.PayorDutiesType = (int)FedExPayorType.ThirdParty;
            shipmentEntity.FedEx.PayorDutiesCountryCode = "UK";

            testObject.Manipulate((carrierRequest.Object));

            Payment dutiesPayment = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.CustomsClearanceDetail.DutiesPayment;
            Assert.AreEqual("UK", dutiesPayment.Payor.ResponsibleParty.Address.CountryCode);
        }

        [Fact]
        public void Manipulate_SetsPaymentTypeToCollect_Test()
        {
            // Setup the fedex shipment payor type for the test
            shipmentEntity.FedEx.PayorDutiesType = (int)FedExPayorType.Collect;

            testObject.Manipulate((carrierRequest.Object));

            Payment dutiesPayment = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.CustomsClearanceDetail.DutiesPayment;
            Assert.AreEqual(PaymentType.COLLECT, dutiesPayment.PaymentType);
        }

        [Fact]
        public void Manipulate_ContactPersonNameIsNullOrEmpty_WhenPayorDutiesTypeIsCollect_Test()
        {
            shipmentEntity.FedEx.PayorDutiesType = (int)FedExPayorType.Collect;

            testObject.Manipulate(carrierRequest.Object);

            Payment dutiesPayment = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.CustomsClearanceDetail.DutiesPayment;

            Assert.IsNull(dutiesPayment.Payor);
        }

        [Fact]
        public void Manipulate_AccountNumberIsNullOrEmpty_WhenPayorDutiesTypeIsCollect_Test()
        {
            shipmentEntity.FedEx.PayorDutiesType = (int)FedExPayorType.Collect;

            testObject.Manipulate(carrierRequest.Object);

            Payment dutiesPayment = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.CustomsClearanceDetail.DutiesPayment;

            Assert.IsNull(dutiesPayment.Payor);
        }

        [Fact]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Manipulate_ThrowsInvalidOperationException_ForUnknownPayorType_Test()
        {
            // Setup the fedex shipment payor type for the test by setting the type to an unsupported value
            shipmentEntity.FedEx.PayorDutiesType = 23;

            testObject.Manipulate((carrierRequest.Object));
        }

        #endregion Payment Detail Tests


        #region Tax Payer Tests

        [Fact]
        public void Manipulate_RecipientTinsIsNotNull_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNotNull(nativeRequest.RequestedShipment.Recipient.Tins);
        }

        [Fact]
        public void Manipulate_RecipientTinsCountIsOne_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(1, nativeRequest.RequestedShipment.Recipient.Tins.Length);
        }

        [Fact]
        public void Manipulate_RecipientTinsCountIsOne_WhenTinsIsInitiallyNull_Test()
        {
            nativeRequest.RequestedShipment.Recipient.Tins = null;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(1, nativeRequest.RequestedShipment.Recipient.Tins.Length);
        }

        [Fact]
        public void Manipulate_RecipientTinsCountIsOne_WhenTinsCountIsInitiallyZero_Test()
        {
            nativeRequest.RequestedShipment.Recipient.Tins = new TaxpayerIdentification[0];

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(1, nativeRequest.RequestedShipment.Recipient.Tins.Length);
        }

        [Fact]
        public void Manipulate_RecipientTinsNumber_IsShipmentCustomerRecipientTin_Test()
        {   
            testObject.Manipulate(carrierRequest.Object);
            
            Assert.AreEqual(shipmentEntity.FedEx.CustomsRecipientTIN, nativeRequest.RequestedShipment.Recipient.Tins[0].Number);
        }

        [Fact]
        public void Manipulate_RecipientTinsType_IsPersonalState_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(TinType.PERSONAL_STATE, nativeRequest.RequestedShipment.Recipient.Tins[0].TinType);
        }


        #endregion Tax Payer Tests

        [Fact]
        public void Manipulate_ExportDetailIsNotNull_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNotNull(nativeRequest.RequestedShipment.CustomsClearanceDetail.ExportDetail);
        }

        [Fact]
        public void Manipulate_ExportDetailsNullWhenShippingFromUSInternationaly_Test()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.OriginCountryCode = "US";

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsFalse(nativeRequest.RequestedShipment.CustomsClearanceDetail.ExportDetail.B13AFilingOptionSpecified);
        }

        [Fact]
        public void Manipulate_ExportDetailsNullWhenShippingFromCaToUs_Test()
        {
            shipmentEntity.ShipCountryCode = "US";
            shipmentEntity.OriginCountryCode = "CA";

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsFalse(nativeRequest.RequestedShipment.CustomsClearanceDetail.ExportDetail.B13AFilingOptionSpecified);
        }

        [Fact]
        public void Manipulate_B13FilingOptionSpecifiedIsTrue_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.RequestedShipment.CustomsClearanceDetail.ExportDetail.B13AFilingOptionSpecified);
        }

        [Fact]
        public void Manipulate_B13FilingOptionSpecifiedIsNotRequired_Test()
        {
            shipmentEntity.FedEx.CustomsExportFilingOption = (int) FedExCustomsExportFilingOption.NotRequired;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(B13AFilingOptionType.NOT_REQUIRED, nativeRequest.RequestedShipment.CustomsClearanceDetail.ExportDetail.B13AFilingOption);
        }

        [Fact]
        public void Manipulate_B13FilingOptionSpecifiedIsManuallyAttached_Test()
        {
            shipmentEntity.FedEx.CustomsExportFilingOption = (int)FedExCustomsExportFilingOption.ManuallyAttached;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(B13AFilingOptionType.MANUALLY_ATTACHED, nativeRequest.RequestedShipment.CustomsClearanceDetail.ExportDetail.B13AFilingOption);
        }

        [Fact]
        public void Manipulate_B13FilingOptionSpecifiedIsFiledElectronically_Test()
        {
            shipmentEntity.FedEx.CustomsExportFilingOption = (int)FedExCustomsExportFilingOption.FiledElectonically;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(B13AFilingOptionType.FILED_ELECTRONICALLY, nativeRequest.RequestedShipment.CustomsClearanceDetail.ExportDetail.B13AFilingOption);
        }

        [Fact]
        public void Manipulate_B13FilingOptionSpecifiedIsSummaryReporting_Test()
        {
            shipmentEntity.FedEx.CustomsExportFilingOption = (int)FedExCustomsExportFilingOption.SummaryReporting;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(B13AFilingOptionType.SUMMARY_REPORTING, nativeRequest.RequestedShipment.CustomsClearanceDetail.ExportDetail.B13AFilingOption);
        }

        [Fact]
        public void Manipulate_ComplianceStatementIsNull_WhenAESEEIIsEmptyStringTest()
        {
            shipmentEntity.FedEx.CustomsAESEEI = string.Empty;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreNotEqual(string.Empty, nativeRequest.RequestedShipment.CustomsClearanceDetail.ExportDetail.ExportComplianceStatement);
        }
        
        [Fact]
        public void Manipulate_ComplianceStatementIsNotEmpty_Test()
        {
            shipmentEntity.FedEx.CustomsAESEEI = "NO EEI 30.2(d)(2)";

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreNotEqual(string.Empty, nativeRequest.RequestedShipment.CustomsClearanceDetail.ExportDetail.ExportComplianceStatement);
        }        

        [Fact]
        public void Manipulate_RecipientCustomsIdIsNull_WhenIdentificationTypeIsNone_Test()
        {
            shipmentEntity.FedEx.CustomsRecipientIdentificationType = (int) FedExCustomsRecipientIdentificationType.None;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNull(nativeRequest.RequestedShipment.CustomsClearanceDetail.RecipientCustomsId);
        }

        [Fact]
        public void Manipulate_RecipientCustomsIdTypeIsPassport_WhenIdentificationTypeIsPassport_Test()
        {
            shipmentEntity.FedEx.CustomsRecipientIdentificationType = (int)FedExCustomsRecipientIdentificationType.Passport;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(RecipientCustomsIdType.PASSPORT, nativeRequest.RequestedShipment.CustomsClearanceDetail.RecipientCustomsId.Type);
        }

        [Fact]
        public void Manipulate_RecipientCustomsIdTypeSpecifiedIsTrue_WhenIdentificationTypeIsPassport_Test()
        {
            shipmentEntity.FedEx.CustomsRecipientIdentificationType = (int)FedExCustomsRecipientIdentificationType.Passport;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.RequestedShipment.CustomsClearanceDetail.RecipientCustomsId.TypeSpecified);
        }

        [Fact]
        public void Manipulate_RecipientCustomsIdValue_WhenIdentificationTypeIsPassport_Test()
        {
            shipmentEntity.FedEx.CustomsRecipientIdentificationType = (int)FedExCustomsRecipientIdentificationType.Passport;
            shipmentEntity.FedEx.CustomsRecipientIdentificationValue = "123456";

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual("123456", nativeRequest.RequestedShipment.CustomsClearanceDetail.RecipientCustomsId.Value);
        }

        [Fact]
        public void Manipulate_RecipientCustomsIdTypeIsIndividual_WhenIdentificationTypeIsIndividual_Test()
        {
            shipmentEntity.FedEx.CustomsRecipientIdentificationType = (int)FedExCustomsRecipientIdentificationType.Individual;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(RecipientCustomsIdType.INDIVIDUAL, nativeRequest.RequestedShipment.CustomsClearanceDetail.RecipientCustomsId.Type);
        }

        [Fact]
        public void Manipulate_RecipientCustomsIdTypeSpecifiedIsTrue_WhenIdentificationTypeIsIndividual_Test()
        {
            shipmentEntity.FedEx.CustomsRecipientIdentificationType = (int)FedExCustomsRecipientIdentificationType.Individual;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.RequestedShipment.CustomsClearanceDetail.RecipientCustomsId.TypeSpecified);
        }

        [Fact]
        public void Manipulate_RecipientCustomsIdValue_WhenIdentificationTypeIsIndividual_Test()
        {
            shipmentEntity.FedEx.CustomsRecipientIdentificationType = (int)FedExCustomsRecipientIdentificationType.Individual;
            shipmentEntity.FedEx.CustomsRecipientIdentificationValue = "123456";

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual("123456", nativeRequest.RequestedShipment.CustomsClearanceDetail.RecipientCustomsId.Value);
        }

        [Fact]
        public void Manipulate_RecipientCustomsIdTypeIsCompany_WhenIdentificationTypeIsCompany_Test()
        {
            shipmentEntity.FedEx.CustomsRecipientIdentificationType = (int)FedExCustomsRecipientIdentificationType.Company;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(RecipientCustomsIdType.COMPANY, nativeRequest.RequestedShipment.CustomsClearanceDetail.RecipientCustomsId.Type);
        }

        [Fact]
        public void Manipulate_RecipientCustomsIdTypeSpecifiedIsTrue_WhenIdentificationTypeIsCompany_Test()
        {
            shipmentEntity.FedEx.CustomsRecipientIdentificationType = (int)FedExCustomsRecipientIdentificationType.Company;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.RequestedShipment.CustomsClearanceDetail.RecipientCustomsId.TypeSpecified);
        }

        [Fact]
        public void Manipulate_RecipientCustomsIdValue_WhenIdentificationTypeIsCompany_Test()
        {
            shipmentEntity.FedEx.CustomsRecipientIdentificationType = (int)FedExCustomsRecipientIdentificationType.Company;
            shipmentEntity.FedEx.CustomsRecipientIdentificationValue = "123456";

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual("123456", nativeRequest.RequestedShipment.CustomsClearanceDetail.RecipientCustomsId.Value);
        }

        [Fact]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Manipulate_ThrowsInvalidOperationException_WhenIdentificationTypeIsNotRecognized_Test()
        {
            shipmentEntity.FedEx.CustomsRecipientIdentificationType = 53;

            testObject.Manipulate(carrierRequest.Object);
        }

        
        #region Customs Options Tests

        [Fact]
        public void Manipulate_CustomOptionsIsNull_WhenOptionTypeIsNone_Test()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int) FedExCustomsOptionType.None;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNull(nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeIsCourtesyReturnLabel_Test()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.CourtesyReturnLabel;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(CustomsOptionType.COURTESY_RETURN_LABEL, nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Type);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeSpecifiedIsTrue_WhenTypeIsCourtesyReturnLabel_Test()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.CourtesyReturnLabel;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.TypeSpecified);
        }

        [Fact]
        public void Manipulate_CustomOptionDescription_WhenTypeIsCourtesyReturnLabel_Test()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.CourtesyReturnLabel;
            shipmentEntity.FedEx.CustomsOptionsDesription = "option description";

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual("option description", nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Description);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeIsExhibitionTradeShow_Test()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.ExhibitionTradeShow;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(CustomsOptionType.EXHIBITION_TRADE_SHOW, nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Type);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeSpecifiedIsTrue_WhenTypeIsExhibitionTradeShow_Test()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.ExhibitionTradeShow;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.TypeSpecified);
        }

        [Fact]
        public void Manipulate_CustomOptionDescription_WhenTypeIsExhibitionTradeShow_Test()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.ExhibitionTradeShow;
            shipmentEntity.FedEx.CustomsOptionsDesription = "option description";

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual("option description", nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Description);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeIsFaultyItem_Test()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.FaultyItem;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(CustomsOptionType.FAULTY_ITEM, nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Type);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeSpecifiedIsTrue_WhenTypeIsFaultyItem_Test()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.FaultyItem;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.TypeSpecified);
        }

        [Fact]
        public void Manipulate_CustomOptionDescription_WhenTypeIsFaultyItem_Test()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.FaultyItem;
            shipmentEntity.FedEx.CustomsOptionsDesription = "option description";

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual("option description", nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Description);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeIsFollowingRepair_Test()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.FollowingRepair;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(CustomsOptionType.FOLLOWING_REPAIR, nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Type);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeSpecifiedIsTrue_WhenTypeIsFollowingRepair_Test()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.FollowingRepair;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.TypeSpecified);
        }

        [Fact]
        public void Manipulate_CustomOptionDescription_WhenTypeIsFollowingRepair_Test()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.FollowingRepair;
            shipmentEntity.FedEx.CustomsOptionsDesription = "option description";

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual("option description", nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Description);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeIsForRepair_Test()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.ForRepair;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(CustomsOptionType.FOR_REPAIR, nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Type);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeSpecifiedIsTrue_WhenTypeIsForRepair_Test()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.ForRepair;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.TypeSpecified);
        }

        [Fact]
        public void Manipulate_CustomOptionDescription_WhenTypeIsForRepair_Test()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.ForRepair;
            shipmentEntity.FedEx.CustomsOptionsDesription = "option description";

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual("option description", nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Description);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeIsItemForLoan_Test()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.ItemForLoan;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(CustomsOptionType.ITEM_FOR_LOAN, nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Type);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeSpecifiedIsTrue_WhenTypeIsItemForLoan_Test()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.ItemForLoan;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.TypeSpecified);
        }

        [Fact]
        public void Manipulate_CustomOptionDescription_WhenTypeIsItemForLoan_Test()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.ItemForLoan;
            shipmentEntity.FedEx.CustomsOptionsDesription = "option description";

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual("option description", nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Description);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeIsOther_Test()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.Other;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(CustomsOptionType.OTHER, nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Type);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeSpecifiedIsTrue_WhenTypeIsOther_Test()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.Other;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.TypeSpecified);
        }

        [Fact]
        public void Manipulate_CustomOptionDescription_WhenTypeIsOther_Test()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.Other;
            shipmentEntity.FedEx.CustomsOptionsDesription = "option description";

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual("option description", nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Description);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeIsRejected_Test()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.Rejected;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(CustomsOptionType.REJECTED, nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Type);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeSpecifiedIsTrue_WhenTypeIsRejected_Test()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.Rejected;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.TypeSpecified);
        }

        [Fact]
        public void Manipulate_CustomOptionDescription_WhenTypeIsRejected_Test()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.Rejected;
            shipmentEntity.FedEx.CustomsOptionsDesription = "option description";

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual("option description", nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Description);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeIsReplacement_Test()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.Replacement;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(CustomsOptionType.REPLACEMENT, nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Type);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeSpecifiedIsTrue_WhenTypeIsReplacement_Test()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.Replacement;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.TypeSpecified);
        }

        [Fact]
        public void Manipulate_CustomOptionDescription_WhenTypeIsReplacement_Test()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.Replacement;
            shipmentEntity.FedEx.CustomsOptionsDesription = "option description";

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual("option description", nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Description);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeIsTrial_Test()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.Trial;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(CustomsOptionType.TRIAL, nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Type);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeSpecifiedIsTrue_WhenTypeIsTrial_Test()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.Trial;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.TypeSpecified);
        }

        [Fact]
        public void Manipulate_CustomOptionDescription_WhenTypeIsTrial_Test()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.Trial;
            shipmentEntity.FedEx.CustomsOptionsDesription = "option description";

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual("option description", nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Description);
        }

        [Fact]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Manipulate_ThrowsInvalidOperationException_WhenTypeIsNotRecognized_Test()
        {
            shipmentEntity.FedEx.CustomsOptionsType = 32;

            testObject.Manipulate(carrierRequest.Object);
        }

        #endregion Customs Options Tests
    }
}
