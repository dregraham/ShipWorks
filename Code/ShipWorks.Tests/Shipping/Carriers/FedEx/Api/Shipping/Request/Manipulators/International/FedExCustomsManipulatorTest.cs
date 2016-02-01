using System;
using System.Collections.Generic;
using Interapptive.Shared.Enums;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx;
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
        private Mock<ICustomsRequired> customsRequired;

        public FedExCustomsManipulatorTest()
        {
            // Create a ProcessShipmentRequest type and set the properties the manipulator is interested in
            nativeRequest = new ProcessShipmentRequest()
            {
                RequestedShipment = new RequestedShipment()
                {
                    Recipient = new Party()
                    {
                        Tins = new TaxpayerIdentification[1] { new TaxpayerIdentification() }
                    }
                }
            };

            // Create our default shipment entity and initialize the properties our test object will be accessing
            shipmentEntity = new ShipmentEntity()
            {
                ShipCountryCode = "NZ",
                OriginCountryCode = "CA",
                CustomsValue = 123.45M,
                FedEx = new FedExShipmentEntity() { PayorDutiesAccount = "987", PayorTransportName = "Transport Name", PayorDutiesType = (int)FedExPayorType.Sender, CustomsRecipientTIN = "5468" }
            };

            shipmentEntity.CustomsItems.Add(new ShipmentCustomsItemEntity { Description = "item one", Quantity = 4, Weight = 2.3, UnitValue = 15.36M, HarmonizedCode = "Harmonized 1", UnitPriceAmount = 51.36M, NumberOfPieces = 4 });
            shipmentEntity.CustomsItems.Add(new ShipmentCustomsItemEntity { Description = "item two", Quantity = 64, Weight = 112.3, UnitValue = 98.52M, HarmonizedCode = "Harmonized 2", UnitPriceAmount = 114.57M, NumberOfPieces = 12 });
            shipmentEntity.CustomsItems.Add(new ShipmentCustomsItemEntity { Description = "item three", Quantity = 342, Weight = 92.3, UnitValue = 6.67M, HarmonizedCode = "Harmonized 3", UnitPriceAmount = 14.06M, NumberOfPieces = 25 });


            fedExAccount = new FedExAccountEntity { AccountNumber = "123", CountryCode = "US", LastName = "Doe", FirstName = "John" };


            // Setup the carrier request's NativeRequest property to return the ProcessShipmentRequest object
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            carrierRequest.Setup(r => r.CarrierAccountEntity).Returns(fedExAccount);

            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>())).Returns(fedExAccount);

            customsRequired = new Mock<ICustomsRequired>();
            customsRequired
                .Setup(c => c.IsCustomsRequired(It.IsAny<ShipmentEntity>()))
                .Returns(true);

            testObject = new FedExCustomsManipulator(new FedExSettings(settingsRepository.Object), customsRequired.Object);
        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(null));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull()
        {
            // Setup the native request to be null
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, null);

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotProcessShipmentRequest()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new object());

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_AccountsForRequestedShipment()
        {
            // Setup the test by configuring the native request to have a null requested shipment property 
            nativeRequest.RequestedShipment = null;

            testObject.Manipulate(carrierRequest.Object);

            // The requested shipment property should be created now
            Assert.NotNull(nativeRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_AccountsForRecipient()
        {
            // Setup the test by configuring the native request to have a null recipient property 
            nativeRequest.RequestedShipment.Recipient = null;

            testObject.Manipulate(carrierRequest.Object);

            // The recipient property should be created now
            Assert.NotNull(nativeRequest.RequestedShipment.Recipient);
        }

        [Fact]
        public void Manipulate_CustomsDetailIsNull_WhenCustomsNotRequired()
        {
            customsRequired
                .Setup(c => c.IsCustomsRequired(It.IsAny<ShipmentEntity>()))
                .Returns(false);

            testObject.Manipulate(carrierRequest.Object);

            Assert.Null(nativeRequest.RequestedShipment.CustomsClearanceDetail);
        }

        [Fact]
        public void Manipulate_CustomsValueAmountIsShipmentCustomsValue()
        {
            testObject.Manipulate(carrierRequest.Object);

            // The amount on the shipment is 123.45
            Assert.Equal(123.45M, nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsValue.Amount);
        }

        [Fact]
        public void Manipulate_CustomsCurrencyValidIsUSD()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal("USD", nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsValue.Currency);
        }

        [Fact]
        public void Manipulate_DocumentContentIsDocumentsOnly_WhenFedExCustomsDocumentsOnlyIsTrue()
        {
            shipmentEntity.FedEx.CustomsDocumentsOnly = true;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(InternationalDocumentContentType.DOCUMENTS_ONLY, nativeRequest.RequestedShipment.CustomsClearanceDetail.DocumentContent);
        }

        [Fact]
        public void Manipulate_DocumentContentIsNonDocuments_WhenFedExCustomsDocumentsOnlyIsFalse()
        {
            shipmentEntity.FedEx.CustomsDocumentsOnly = false;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(InternationalDocumentContentType.NON_DOCUMENTS, nativeRequest.RequestedShipment.CustomsClearanceDetail.DocumentContent);
        }


        #region Commodities Tests

        [Fact]
        public void Manipulate_CommoditiesAddsAllShipmentCustomItems_WhenFedExCustomsDocumentsOnlyIsFalse()
        {
            shipmentEntity.FedEx.CustomsDocumentsOnly = false;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(shipmentEntity.CustomsItems.Count, nativeRequest.RequestedShipment.CustomsClearanceDetail.Commodities.Length);
        }

        [Fact]
        public void Manipulate_CommoditiesContainDescriptionFromShipmentCustomItems_WhenFedExCustomsDocumentsOnlyIsFalse()
        {
            shipmentEntity.FedEx.CustomsDocumentsOnly = false;

            testObject.Manipulate(carrierRequest.Object);

            for (int i = 0; i < shipmentEntity.CustomsItems.Count; i++)
            {
                Assert.Equal(shipmentEntity.CustomsItems[i].Description, nativeRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].Description);
            }
        }

        [Fact]
        public void Manipulate_CommoditiesContainQuantityFromShipmentCustomItems_WhenFedExCustomsDocumentsOnlyIsFalse()
        {
            shipmentEntity.FedEx.CustomsDocumentsOnly = false;

            testObject.Manipulate(carrierRequest.Object);

            for (int i = 0; i < shipmentEntity.CustomsItems.Count; i++)
            {
                Assert.Equal(shipmentEntity.CustomsItems[i].Quantity, (double)nativeRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].Quantity);
            }
        }

        [Fact]
        public void Manipulate_CommoditiesContainQuantityUnitsIsEA_WhenFedExCustomsDocumentsOnlyIsFalse()
        {
            shipmentEntity.FedEx.CustomsDocumentsOnly = false;

            testObject.Manipulate(carrierRequest.Object);

            for (int i = 0; i < shipmentEntity.CustomsItems.Count; i++)
            {
                Assert.Equal("EA", nativeRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].QuantityUnits);
            }
        }

        [Fact]
        public void Manipulate_CommoditiesContainNumberofPiecesFromFedExShipmentCustomItem_WhenFedExCustomsDocumentsOnlyIsFalse()
        {
            shipmentEntity.FedEx.CustomsDocumentsOnly = false;

            testObject.Manipulate(carrierRequest.Object);

            for (int i = 0; i < shipmentEntity.CustomsItems.Count; i++)
            {
                Assert.Equal(shipmentEntity.CustomsItems[i].NumberOfPieces.ToString(), nativeRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].NumberOfPieces);
            }
        }

        [Fact]
        public void Manipulate_CommoditiesWeightsFromShipmentCustomItem_WhenFedExCustomsDocumentsOnlyIsFalse()
        {
            shipmentEntity.FedEx.CustomsDocumentsOnly = false;

            testObject.Manipulate(carrierRequest.Object);

            for (int i = 0; i < shipmentEntity.CustomsItems.Count; i++)
            {
                Assert.Equal((decimal)shipmentEntity.CustomsItems[i].Weight, nativeRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].Weight.Value);
            }
        }

        [Fact]
        public void Manipulate_CommoditiesWeightsIsPounds_WhenFedExCustomsDocumentsOnlyIsFalse_AndCustomItemWeightIsPounds()
        {
            shipmentEntity.FedEx.CustomsDocumentsOnly = false;
            shipmentEntity.FedEx.WeightUnitType = (int)WeightUnitOfMeasure.Pounds;

            testObject.Manipulate(carrierRequest.Object);

            for (int i = 0; i < shipmentEntity.CustomsItems.Count; i++)
            {
                Assert.Equal(WeightUnits.LB, nativeRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].Weight.Units);
            }
        }

        [Fact]
        public void Manipulate_CommoditiesWeightsIsKilograms_WhenFedExCustomsDocumentsOnlyIsFalse_AndCustomItemWeightIsKilograms()
        {
            shipmentEntity.FedEx.CustomsDocumentsOnly = false;
            shipmentEntity.FedEx.WeightUnitType = (int)WeightUnitOfMeasure.Kilograms;

            testObject.Manipulate(carrierRequest.Object);

            for (int i = 0; i < shipmentEntity.CustomsItems.Count; i++)
            {
                Assert.Equal(WeightUnits.KG, nativeRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].Weight.Units);
            }
        }

        [Fact]
        public void Manipulate_ThrowsInvalidOperationException_WhenFedExCustomsDocumentsOnlyIsFalse_AndShipmentWeightTypeIsUnknown()
        {
            shipmentEntity.FedEx.CustomsDocumentsOnly = false;
            shipmentEntity.FedEx.WeightUnitType = 42;

            Assert.Throws<InvalidOperationException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_CommoditiesUnitPriceFromFedExShipmentCustomItem_WhenFedExCustomsDocumentsOnlyIsFalse()
        {
            shipmentEntity.FedEx.CustomsDocumentsOnly = false;

            testObject.Manipulate(carrierRequest.Object);

            for (int i = 0; i < shipmentEntity.CustomsItems.Count; i++)
            {
                Assert.Equal(shipmentEntity.CustomsItems[i].UnitPriceAmount, nativeRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].UnitPrice.Amount);
            }
        }

        [Fact]
        public void Manipulate_CommoditiesUnitPriceCurrencyIsUSD_WhenFedExCustomsDocumentsOnlyIsFalse_AndCurrencyIsUSD()
        {
            shipmentEntity.FedEx.CustomsDocumentsOnly = false;

            testObject.Manipulate(carrierRequest.Object);

            for (int i = 0; i < shipmentEntity.CustomsItems.Count; i++)
            {
                Assert.Equal("USD", nativeRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].UnitPrice.Currency);
            }
        }

        [Fact]
        public void Manipulate_CommoditiesUnitPriceCurrencyIsCAD_WhenFedExCustomsDocumentsOnlyIsFalse_AndCurrencyIsCAD()
        {
            shipmentEntity.FedEx.CustomsDocumentsOnly = false;
            fedExAccount.CountryCode = "CA";

            testObject.Manipulate(carrierRequest.Object);

            for (int i = 0; i < shipmentEntity.CustomsItems.Count; i++)
            {
                Assert.Equal("CAD", nativeRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].UnitPrice.Currency);
            }
        }

        [Fact]
        public void Manipulate_CommoditiesCountryOfManufacture_IsShipmentCustomItemCountryOfOrigin_WhenFedExCustomsDocumentsOnlyIsFalse()
        {
            shipmentEntity.FedEx.CustomsDocumentsOnly = false;

            testObject.Manipulate(carrierRequest.Object);

            for (int i = 0; i < shipmentEntity.CustomsItems.Count; i++)
            {
                Assert.Equal(shipmentEntity.CustomsItems[i].CountryOfOrigin, nativeRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].CountryOfManufacture);
            }
        }

        [Fact]
        public void Manipulate_CommoditiesHarmonizedCode_IsShipmentCustomItemHarmonized_WhenFedExCustomsDocumentsOnlyIsFalse()
        {
            shipmentEntity.FedEx.CustomsDocumentsOnly = false;

            testObject.Manipulate(carrierRequest.Object);

            for (int i = 0; i < shipmentEntity.CustomsItems.Count; i++)
            {
                Assert.Equal(shipmentEntity.CustomsItems[i].HarmonizedCode, nativeRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].HarmonizedCode);
            }
        }

        [Fact]
        public void Manipulate_CommoditiesCustomsValueCurrencyIsUSD_WhenFedExCustomsDocumentsOnlyIsFalse()
        {
            shipmentEntity.FedEx.CustomsDocumentsOnly = false;

            testObject.Manipulate(carrierRequest.Object);

            for (int i = 0; i < shipmentEntity.CustomsItems.Count; i++)
            {
                Assert.Equal("USD", nativeRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].CustomsValue.Currency);
            }
        }

        [Fact]
        public void Manipulate_CommoditiesCustomsValueFromShipmentCustomItem_WhenFedExCustomsDocumentsOnlyIsFalse()
        {
            shipmentEntity.FedEx.CustomsDocumentsOnly = false;

            testObject.Manipulate(carrierRequest.Object);

            for (int i = 0; i < shipmentEntity.CustomsItems.Count; i++)
            {
                Assert.Equal(shipmentEntity.CustomsItems[i].UnitValue, nativeRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].CustomsValue.Amount);
            }
        }

        #endregion Commodities Tests


        #region NAFTA Tests

        [Fact]
        public void Manipulate_NaftaDetailIsNull_WhenNaftaIsNotEnabled()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = false;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.Null(commodity.NaftaDetail);
            }
        }

        [Fact]
        public void Manipulate_RegulatoryControlIsNull_WhenNaftaIsNotEnabled()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = false;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Null(customsDetail.RegulatoryControls);
        }


        [Fact]
        public void Manipulate_RegulatoryControlArrayHasSizeOne_WhenNaftaIsEnabled()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal(1, customsDetail.RegulatoryControls.Length);
        }

        [Fact]
        public void Manipulate_AddsNaftaAsRegulatoryControl_WhenNaftaIsEnabled()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal(RegulatoryControlType.NAFTA, customsDetail.RegulatoryControls[0]);
        }

        [Fact]
        public void Manipulate_NaftaDetailIsNotNull_WhenNaftaIsEnabled()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.NotNull(commodity.NaftaDetail);
            }
        }

        [Fact]
        public void Manipulate_NetCostMethodIsNO_WhenNaftaIsEnabled_AndNetCostIsNotCalculated()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;
            shipmentEntity.FedEx.CustomsNaftaNetCostMethod = (int)FedExNaftaNetCostMethod.NotCalculated;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.Equal(NaftaNetCostMethodCode.NO, commodity.NaftaDetail.NetCostMethod);
            }
        }

        [Fact]
        public void Manipulate_NetCostMethodIsNC_WhenNaftaIsEnabled_AndNetCostIsCalculated()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;
            shipmentEntity.FedEx.CustomsNaftaNetCostMethod = (int)FedExNaftaNetCostMethod.NetCostMethod;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.Equal(NaftaNetCostMethodCode.NC, commodity.NaftaDetail.NetCostMethod);
            }
        }

        [Fact]
        public void Manipulate_ThrowsInvalidOperationException_WhenNaftaIsEnabled_AndNetCostIsInvalid()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;
            shipmentEntity.FedEx.CustomsNaftaNetCostMethod = 54;

            Assert.Throws<InvalidOperationException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_NetCostMethodIsSpecified_WhenNaftaIsEnabled()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;
            shipmentEntity.FedEx.CustomsNaftaNetCostMethod = (int)FedExNaftaNetCostMethod.NetCostMethod;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.True(commodity.NaftaDetail.NetCostMethodSpecified);
            }
        }


        [Fact]
        public void Manipulate_PreferenceCriterionIsA_WhenNaftaIsEnable()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;
            shipmentEntity.FedEx.CustomsNaftaPreferenceType = (int)FedExNaftaPreferenceCriteria.A;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.Equal(NaftaPreferenceCriterionCode.A, commodity.NaftaDetail.PreferenceCriterion);
            }
        }

        [Fact]
        public void Manipulate_PreferenceCriterionIsB_WhenNaftaIsEnable()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;
            shipmentEntity.FedEx.CustomsNaftaPreferenceType = (int)FedExNaftaPreferenceCriteria.B;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.Equal(NaftaPreferenceCriterionCode.B, commodity.NaftaDetail.PreferenceCriterion);
            }
        }

        [Fact]
        public void Manipulate_PreferenceCriterionIsC_WhenNaftaIsEnable()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;
            shipmentEntity.FedEx.CustomsNaftaPreferenceType = (int)FedExNaftaPreferenceCriteria.C;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.Equal(NaftaPreferenceCriterionCode.C, commodity.NaftaDetail.PreferenceCriterion);
            }
        }

        [Fact]
        public void Manipulate_PreferenceCriterionIsD_WhenNaftaIsEnable()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;
            shipmentEntity.FedEx.CustomsNaftaPreferenceType = (int)FedExNaftaPreferenceCriteria.D;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.Equal(NaftaPreferenceCriterionCode.D, commodity.NaftaDetail.PreferenceCriterion);
            }
        }

        [Fact]
        public void Manipulate_PreferenceCriterionIsE_WhenNaftaIsEnable()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;
            shipmentEntity.FedEx.CustomsNaftaPreferenceType = (int)FedExNaftaPreferenceCriteria.E;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.Equal(NaftaPreferenceCriterionCode.E, commodity.NaftaDetail.PreferenceCriterion);
            }
        }

        [Fact]
        public void Manipulate_PreferenceCriterionIsF_WhenNaftaIsEnable()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;
            shipmentEntity.FedEx.CustomsNaftaPreferenceType = (int)FedExNaftaPreferenceCriteria.F;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.Equal(NaftaPreferenceCriterionCode.F, commodity.NaftaDetail.PreferenceCriterion);
            }
        }

        [Fact]
        public void Manipulate_ThrowsInvalidOperationException_WhenNaftaIsEnable_AndPreferenceIsInvalid()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;
            shipmentEntity.FedEx.CustomsNaftaPreferenceType = 40;

            Assert.Throws<InvalidOperationException>(() => testObject.Manipulate(carrierRequest.Object));
        }


        [Fact]
        public void Manipulate_PreferenceCriterionIsSpecified_WhenNaftaIsEnable()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;
            shipmentEntity.FedEx.CustomsNaftaPreferenceType = (int)FedExNaftaPreferenceCriteria.F;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.True(commodity.NaftaDetail.PreferenceCriterionSpecified);
            }
        }


        [Fact]
        public void Manipulate_ProducerDeterminationIsYes_WhenNaftaIsEnable_AndCodeIsProducerOfCommodity()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;
            shipmentEntity.FedEx.CustomsNaftaDeterminationCode = (int)FedExNaftaDeterminationCode.ProducerOfCommodity;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.Equal(NaftaProducerDeterminationCode.YES, commodity.NaftaDetail.ProducerDetermination);
            }
        }

        [Fact]
        public void Manipulate_ProducerDeterminationIsNo1_WhenNaftaIsEnable_AndCodeIsNotProducerKnowledge()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;
            shipmentEntity.FedEx.CustomsNaftaDeterminationCode = (int)FedExNaftaDeterminationCode.NotProducerKnowledgeOfCommodity;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.Equal(NaftaProducerDeterminationCode.NO_1, commodity.NaftaDetail.ProducerDetermination);
            }
        }

        [Fact]
        public void Manipulate_ProducerDeterminationIsNo2_WhenNaftaIsEnable_AndCodeIsNotProducerStatement()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;
            shipmentEntity.FedEx.CustomsNaftaDeterminationCode = (int)FedExNaftaDeterminationCode.NotProducerWrittenStatement;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.Equal(NaftaProducerDeterminationCode.NO_2, commodity.NaftaDetail.ProducerDetermination);
            }
        }

        [Fact]
        public void Manipulate_ProducerDeterminationIsNo3_WhenNaftaIsEnable_AndCodeIsNotProducerSignedCertificate()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;
            shipmentEntity.FedEx.CustomsNaftaDeterminationCode = (int)FedExNaftaDeterminationCode.NotProducerSignedCertificate;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.Equal(NaftaProducerDeterminationCode.NO_3, commodity.NaftaDetail.ProducerDetermination);
            }
        }

        [Fact]
        public void Manipulate_ThrowsInvalidOperationException_WhenNaftaIsEnable_AndProducerIsInvalid()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;
            shipmentEntity.FedEx.CustomsNaftaPreferenceType = 13;

            Assert.Throws<InvalidOperationException>(() => testObject.Manipulate(carrierRequest.Object));
        }


        [Fact]
        public void Manipulate_ProducerDeterminationIsSpecified_WhenNaftaIsEnable()
        {
            shipmentEntity.FedEx.CustomsNaftaEnabled = true;
            shipmentEntity.FedEx.CustomsNaftaPreferenceType = (int)FedExNaftaPreferenceCriteria.F;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.True(commodity.NaftaDetail.ProducerDeterminationSpecified);
            }
        }



        #endregion NAFTA Tests


        #region Payment Detail Tests

        [Fact]
        public void Manipulate_CustomsDutiesPaymentIsNotNull()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.NotNull(nativeRequest.RequestedShipment.CustomsClearanceDetail.DutiesPayment);
        }


        [Fact]
        public void Manipulate_UsesFedExAccountNumber_WhenPayorDutiesTypeIsSender()
        {
            // Setup the fedex shipment payor type for the test
            shipmentEntity.FedEx.PayorDutiesType = (int)FedExPayorType.Sender;

            testObject.Manipulate((carrierRequest.Object));

            Payment dutiesPayment = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.CustomsClearanceDetail.DutiesPayment;
            Assert.Equal(fedExAccount.AccountNumber, dutiesPayment.Payor.ResponsibleParty.AccountNumber);
        }

        [Fact]
        public void Manipulate_UsesFedExAccountCountryCode_WhenPayorDutiesTypeIsSender()
        {
            // Setup the fedex shipment payor type for the test
            shipmentEntity.FedEx.PayorDutiesType = (int)FedExPayorType.Sender;

            testObject.Manipulate((carrierRequest.Object));

            Payment dutiesPayment = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.CustomsClearanceDetail.DutiesPayment;
            Assert.Equal(fedExAccount.CountryCode, dutiesPayment.Payor.ResponsibleParty.Address.CountryCode);
        }

        [Fact]
        public void Manipulate_UsesFedExAccountNameAsContactName_WhenPayorDutiesTypeIsSender()
        {
            // Setup the fedex shipment payor type for the test
            shipmentEntity.FedEx.PayorDutiesType = (int)FedExPayorType.Sender;

            testObject.Manipulate((carrierRequest.Object));

            Payment dutiesPayment = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.CustomsClearanceDetail.DutiesPayment;
            Assert.Equal(fedExAccount.FirstName + " " + fedExAccount.LastName, dutiesPayment.Payor.ResponsibleParty.Contact.PersonName);
        }


        [Fact]
        public void Manipulate_SetsPaymentTypeToRecipient()
        {
            // Setup the fedex shipment payor type for the test
            shipmentEntity.FedEx.PayorDutiesType = (int)FedExPayorType.Recipient;

            testObject.Manipulate((carrierRequest.Object));

            Payment dutiesPayment = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.CustomsClearanceDetail.DutiesPayment;
            Assert.Equal(PaymentType.RECIPIENT, dutiesPayment.PaymentType);
        }

        [Fact]
        public void Manipulate_UsesPayorDutiesCountryCode_WhenPayorDutiesTypeIsRecipient()
        {
            // Setup the fedex shipment payor type for the test
            shipmentEntity.FedEx.PayorDutiesType = (int)FedExPayorType.Recipient;
            shipmentEntity.FedEx.PayorDutiesCountryCode = "CA";

            testObject.Manipulate((carrierRequest.Object));

            Payment dutiesPayment = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.CustomsClearanceDetail.DutiesPayment;
            Assert.Equal("CA", dutiesPayment.Payor.ResponsibleParty.Address.CountryCode);
        }

        [Fact]
        public void Manipulate_UsesFedExShipmentPayorDutiesAccount_WhenPayorDutiesTypeIsRecipient()
        {
            // Setup the fedex shipment payor type for the test
            shipmentEntity.FedEx.PayorDutiesType = (int)FedExPayorType.Recipient;

            testObject.Manipulate((carrierRequest.Object));

            Payment dutiesPayment = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.CustomsClearanceDetail.DutiesPayment;
            Assert.Equal(shipmentEntity.FedEx.PayorDutiesAccount, dutiesPayment.Payor.ResponsibleParty.AccountNumber);
        }

        [Fact]
        public void Manipulate_UsesPayorDutiesName_WhenPayorDutiesTypeIsRecepient()
        {
            shipmentEntity.FedEx.PayorDutiesType = (int)FedExPayorType.Recipient;
            shipmentEntity.FedEx.PayorDutiesName = "Peter Gibbons";

            testObject.Manipulate(carrierRequest.Object);

            Payment dutiesPayment = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.CustomsClearanceDetail.DutiesPayment;
            Assert.Equal("Peter Gibbons", dutiesPayment.Payor.ResponsibleParty.Contact.PersonName);
        }

        [Fact]
        public void Manipulate_SetsPaymentTypeToThirdParty()
        {
            // Setup the fedex shipment payor type for the test
            shipmentEntity.FedEx.PayorDutiesType = (int)FedExPayorType.ThirdParty;

            testObject.Manipulate((carrierRequest.Object));

            Payment dutiesPayment = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.CustomsClearanceDetail.DutiesPayment;
            Assert.Equal(PaymentType.THIRD_PARTY, dutiesPayment.PaymentType);
        }

        [Fact]
        public void Manipulate_UsesFedExShipmentPayorDutiesAccount_WhenPayorDutiesTypeIsThirdParty()
        {
            // Setup the fedex shipment payor type for the test
            shipmentEntity.FedEx.PayorDutiesType = (int)FedExPayorType.ThirdParty;

            testObject.Manipulate((carrierRequest.Object));

            Payment dutiesPayment = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.CustomsClearanceDetail.DutiesPayment;
            Assert.Equal(shipmentEntity.FedEx.PayorDutiesAccount, dutiesPayment.Payor.ResponsibleParty.AccountNumber);
        }

        [Fact]
        public void Manipulate_UsesPayorDutiesName_WhenPayorDutiesTypeIsThirdParty()
        {
            shipmentEntity.FedEx.PayorDutiesType = (int)FedExPayorType.ThirdParty;
            shipmentEntity.FedEx.PayorDutiesName = "Michael Bolton";

            testObject.Manipulate(carrierRequest.Object);

            Payment dutiesPayment = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.CustomsClearanceDetail.DutiesPayment;
            Assert.Equal("Michael Bolton", dutiesPayment.Payor.ResponsibleParty.Contact.PersonName);
        }

        [Fact]
        public void Manipulate_UsesPayorDutiesCountryCode_WhenPayorDutiesTypeIsThirdParty()
        {
            // Setup the fedex shipment payor type for the test
            shipmentEntity.FedEx.PayorDutiesType = (int)FedExPayorType.ThirdParty;
            shipmentEntity.FedEx.PayorDutiesCountryCode = "UK";

            testObject.Manipulate((carrierRequest.Object));

            Payment dutiesPayment = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.CustomsClearanceDetail.DutiesPayment;
            Assert.Equal("UK", dutiesPayment.Payor.ResponsibleParty.Address.CountryCode);
        }

        [Fact]
        public void Manipulate_SetsPaymentTypeToCollect()
        {
            // Setup the fedex shipment payor type for the test
            shipmentEntity.FedEx.PayorDutiesType = (int)FedExPayorType.Collect;

            testObject.Manipulate((carrierRequest.Object));

            Payment dutiesPayment = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.CustomsClearanceDetail.DutiesPayment;
            Assert.Equal(PaymentType.COLLECT, dutiesPayment.PaymentType);
        }

        [Fact]
        public void Manipulate_ContactPersonNameIsNullOrEmpty_WhenPayorDutiesTypeIsCollect()
        {
            shipmentEntity.FedEx.PayorDutiesType = (int)FedExPayorType.Collect;

            testObject.Manipulate(carrierRequest.Object);

            Payment dutiesPayment = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.CustomsClearanceDetail.DutiesPayment;

            Assert.Null(dutiesPayment.Payor);
        }

        [Fact]
        public void Manipulate_AccountNumberIsNullOrEmpty_WhenPayorDutiesTypeIsCollect()
        {
            shipmentEntity.FedEx.PayorDutiesType = (int)FedExPayorType.Collect;

            testObject.Manipulate(carrierRequest.Object);

            Payment dutiesPayment = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.CustomsClearanceDetail.DutiesPayment;

            Assert.Null(dutiesPayment.Payor);
        }

        [Fact]
        public void Manipulate_ThrowsInvalidOperationException_ForUnknownPayorType()
        {
            // Setup the fedex shipment payor type for the test by setting the type to an unsupported value
            shipmentEntity.FedEx.PayorDutiesType = 23;

            Assert.Throws<InvalidOperationException>(() => testObject.Manipulate((carrierRequest.Object)));
        }

        #endregion Payment Detail Tests


        #region Tax Payer Tests

        [Fact]
        public void Manipulate_RecipientTinsIsNotNull()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.NotNull(nativeRequest.RequestedShipment.Recipient.Tins);
        }

        [Fact]
        public void Manipulate_RecipientTinsCountIsOne()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(1, nativeRequest.RequestedShipment.Recipient.Tins.Length);
        }

        [Fact]
        public void Manipulate_RecipientTinsCountIsOne_WhenTinsIsInitiallyNull()
        {
            nativeRequest.RequestedShipment.Recipient.Tins = null;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(1, nativeRequest.RequestedShipment.Recipient.Tins.Length);
        }

        [Fact]
        public void Manipulate_RecipientTinsCountIsOne_WhenTinsCountIsInitiallyZero()
        {
            nativeRequest.RequestedShipment.Recipient.Tins = new TaxpayerIdentification[0];

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(1, nativeRequest.RequestedShipment.Recipient.Tins.Length);
        }

        [Fact]
        public void Manipulate_RecipientTinsNumber_IsShipmentCustomerRecipientTin()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(shipmentEntity.FedEx.CustomsRecipientTIN, nativeRequest.RequestedShipment.Recipient.Tins[0].Number);
        }

        [Fact]
        public void Manipulate_RecipientTinsType_IsPersonalState()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(TinType.PERSONAL_STATE, nativeRequest.RequestedShipment.Recipient.Tins[0].TinType);
        }


        #endregion Tax Payer Tests

        [Fact]
        public void Manipulate_ExportDetailIsNotNull()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.NotNull(nativeRequest.RequestedShipment.CustomsClearanceDetail.ExportDetail);
        }

        [Fact]
        public void Manipulate_ExportDetailsNullWhenShippingFromUSInternationaly()
        {
            shipmentEntity.ShipCountryCode = "CA";
            shipmentEntity.OriginCountryCode = "US";

            testObject.Manipulate(carrierRequest.Object);

            Assert.False(nativeRequest.RequestedShipment.CustomsClearanceDetail.ExportDetail.B13AFilingOptionSpecified);
        }

        [Fact]
        public void Manipulate_ExportDetailsNullWhenShippingFromCaToUs()
        {
            shipmentEntity.ShipCountryCode = "US";
            shipmentEntity.OriginCountryCode = "CA";

            testObject.Manipulate(carrierRequest.Object);

            Assert.False(nativeRequest.RequestedShipment.CustomsClearanceDetail.ExportDetail.B13AFilingOptionSpecified);
        }

        [Fact]
        public void Manipulate_B13FilingOptionSpecifiedIsTrue()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.True(nativeRequest.RequestedShipment.CustomsClearanceDetail.ExportDetail.B13AFilingOptionSpecified);
        }

        [Fact]
        public void Manipulate_B13FilingOptionSpecifiedIsNotRequired()
        {
            shipmentEntity.FedEx.CustomsExportFilingOption = (int)FedExCustomsExportFilingOption.NotRequired;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(B13AFilingOptionType.NOT_REQUIRED, nativeRequest.RequestedShipment.CustomsClearanceDetail.ExportDetail.B13AFilingOption);
        }

        [Fact]
        public void Manipulate_B13FilingOptionSpecifiedIsManuallyAttached()
        {
            shipmentEntity.FedEx.CustomsExportFilingOption = (int)FedExCustomsExportFilingOption.ManuallyAttached;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(B13AFilingOptionType.MANUALLY_ATTACHED, nativeRequest.RequestedShipment.CustomsClearanceDetail.ExportDetail.B13AFilingOption);
        }

        [Fact]
        public void Manipulate_B13FilingOptionSpecifiedIsFiledElectronically()
        {
            shipmentEntity.FedEx.CustomsExportFilingOption = (int)FedExCustomsExportFilingOption.FiledElectonically;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(B13AFilingOptionType.FILED_ELECTRONICALLY, nativeRequest.RequestedShipment.CustomsClearanceDetail.ExportDetail.B13AFilingOption);
        }

        [Fact]
        public void Manipulate_B13FilingOptionSpecifiedIsSummaryReporting()
        {
            shipmentEntity.FedEx.CustomsExportFilingOption = (int)FedExCustomsExportFilingOption.SummaryReporting;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(B13AFilingOptionType.SUMMARY_REPORTING, nativeRequest.RequestedShipment.CustomsClearanceDetail.ExportDetail.B13AFilingOption);
        }

        [Fact]
        public void Manipulate_ComplianceStatementIsNull_WhenAESEEIIsEmptyStringTest()
        {
            shipmentEntity.FedEx.CustomsAESEEI = string.Empty;

            testObject.Manipulate(carrierRequest.Object);

            Assert.NotEqual(string.Empty, nativeRequest.RequestedShipment.CustomsClearanceDetail.ExportDetail.ExportComplianceStatement);
        }

        [Fact]
        public void Manipulate_ComplianceStatementIsNotEmpty()
        {
            shipmentEntity.FedEx.CustomsAESEEI = "NO EEI 30.2(d)(2)";

            testObject.Manipulate(carrierRequest.Object);

            Assert.NotEqual(string.Empty, nativeRequest.RequestedShipment.CustomsClearanceDetail.ExportDetail.ExportComplianceStatement);
        }

        [Fact]
        public void Manipulate_RecipientCustomsIdIsNull_WhenIdentificationTypeIsNone()
        {
            shipmentEntity.FedEx.CustomsRecipientIdentificationType = (int)FedExCustomsRecipientIdentificationType.None;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Null(nativeRequest.RequestedShipment.CustomsClearanceDetail.RecipientCustomsId);
        }

        [Fact]
        public void Manipulate_RecipientCustomsIdTypeIsPassport_WhenIdentificationTypeIsPassport()
        {
            shipmentEntity.FedEx.CustomsRecipientIdentificationType = (int)FedExCustomsRecipientIdentificationType.Passport;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(RecipientCustomsIdType.PASSPORT, nativeRequest.RequestedShipment.CustomsClearanceDetail.RecipientCustomsId.Type);
        }

        [Fact]
        public void Manipulate_RecipientCustomsIdTypeSpecifiedIsTrue_WhenIdentificationTypeIsPassport()
        {
            shipmentEntity.FedEx.CustomsRecipientIdentificationType = (int)FedExCustomsRecipientIdentificationType.Passport;

            testObject.Manipulate(carrierRequest.Object);

            Assert.True(nativeRequest.RequestedShipment.CustomsClearanceDetail.RecipientCustomsId.TypeSpecified);
        }

        [Fact]
        public void Manipulate_RecipientCustomsIdValue_WhenIdentificationTypeIsPassport()
        {
            shipmentEntity.FedEx.CustomsRecipientIdentificationType = (int)FedExCustomsRecipientIdentificationType.Passport;
            shipmentEntity.FedEx.CustomsRecipientIdentificationValue = "123456";

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal("123456", nativeRequest.RequestedShipment.CustomsClearanceDetail.RecipientCustomsId.Value);
        }

        [Fact]
        public void Manipulate_RecipientCustomsIdTypeIsIndividual_WhenIdentificationTypeIsIndividual()
        {
            shipmentEntity.FedEx.CustomsRecipientIdentificationType = (int)FedExCustomsRecipientIdentificationType.Individual;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(RecipientCustomsIdType.INDIVIDUAL, nativeRequest.RequestedShipment.CustomsClearanceDetail.RecipientCustomsId.Type);
        }

        [Fact]
        public void Manipulate_RecipientCustomsIdTypeSpecifiedIsTrue_WhenIdentificationTypeIsIndividual()
        {
            shipmentEntity.FedEx.CustomsRecipientIdentificationType = (int)FedExCustomsRecipientIdentificationType.Individual;

            testObject.Manipulate(carrierRequest.Object);

            Assert.True(nativeRequest.RequestedShipment.CustomsClearanceDetail.RecipientCustomsId.TypeSpecified);
        }

        [Fact]
        public void Manipulate_RecipientCustomsIdValue_WhenIdentificationTypeIsIndividual()
        {
            shipmentEntity.FedEx.CustomsRecipientIdentificationType = (int)FedExCustomsRecipientIdentificationType.Individual;
            shipmentEntity.FedEx.CustomsRecipientIdentificationValue = "123456";

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal("123456", nativeRequest.RequestedShipment.CustomsClearanceDetail.RecipientCustomsId.Value);
        }

        [Fact]
        public void Manipulate_RecipientCustomsIdTypeIsCompany_WhenIdentificationTypeIsCompany()
        {
            shipmentEntity.FedEx.CustomsRecipientIdentificationType = (int)FedExCustomsRecipientIdentificationType.Company;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(RecipientCustomsIdType.COMPANY, nativeRequest.RequestedShipment.CustomsClearanceDetail.RecipientCustomsId.Type);
        }

        [Fact]
        public void Manipulate_RecipientCustomsIdTypeSpecifiedIsTrue_WhenIdentificationTypeIsCompany()
        {
            shipmentEntity.FedEx.CustomsRecipientIdentificationType = (int)FedExCustomsRecipientIdentificationType.Company;

            testObject.Manipulate(carrierRequest.Object);

            Assert.True(nativeRequest.RequestedShipment.CustomsClearanceDetail.RecipientCustomsId.TypeSpecified);
        }

        [Fact]
        public void Manipulate_RecipientCustomsIdValue_WhenIdentificationTypeIsCompany()
        {
            shipmentEntity.FedEx.CustomsRecipientIdentificationType = (int)FedExCustomsRecipientIdentificationType.Company;
            shipmentEntity.FedEx.CustomsRecipientIdentificationValue = "123456";

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal("123456", nativeRequest.RequestedShipment.CustomsClearanceDetail.RecipientCustomsId.Value);
        }

        [Fact]
        public void Manipulate_ThrowsInvalidOperationException_WhenIdentificationTypeIsNotRecognized()
        {
            shipmentEntity.FedEx.CustomsRecipientIdentificationType = 53;

            Assert.Throws<InvalidOperationException>(() => testObject.Manipulate(carrierRequest.Object));
        }


        #region Customs Options Tests

        [Fact]
        public void Manipulate_CustomOptionsIsNull_WhenOptionTypeIsNone()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.None;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Null(nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeIsCourtesyReturnLabel()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.CourtesyReturnLabel;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(CustomsOptionType.COURTESY_RETURN_LABEL, nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Type);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeSpecifiedIsTrue_WhenTypeIsCourtesyReturnLabel()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.CourtesyReturnLabel;

            testObject.Manipulate(carrierRequest.Object);

            Assert.True(nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.TypeSpecified);
        }

        [Fact]
        public void Manipulate_CustomOptionDescription_WhenTypeIsCourtesyReturnLabel()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.CourtesyReturnLabel;
            shipmentEntity.FedEx.CustomsOptionsDesription = "option description";

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal("option description", nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Description);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeIsExhibitionTradeShow()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.ExhibitionTradeShow;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(CustomsOptionType.EXHIBITION_TRADE_SHOW, nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Type);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeSpecifiedIsTrue_WhenTypeIsExhibitionTradeShow()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.ExhibitionTradeShow;

            testObject.Manipulate(carrierRequest.Object);

            Assert.True(nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.TypeSpecified);
        }

        [Fact]
        public void Manipulate_CustomOptionDescription_WhenTypeIsExhibitionTradeShow()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.ExhibitionTradeShow;
            shipmentEntity.FedEx.CustomsOptionsDesription = "option description";

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal("option description", nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Description);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeIsFaultyItem()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.FaultyItem;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(CustomsOptionType.FAULTY_ITEM, nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Type);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeSpecifiedIsTrue_WhenTypeIsFaultyItem()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.FaultyItem;

            testObject.Manipulate(carrierRequest.Object);

            Assert.True(nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.TypeSpecified);
        }

        [Fact]
        public void Manipulate_CustomOptionDescription_WhenTypeIsFaultyItem()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.FaultyItem;
            shipmentEntity.FedEx.CustomsOptionsDesription = "option description";

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal("option description", nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Description);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeIsFollowingRepair()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.FollowingRepair;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(CustomsOptionType.FOLLOWING_REPAIR, nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Type);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeSpecifiedIsTrue_WhenTypeIsFollowingRepair()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.FollowingRepair;

            testObject.Manipulate(carrierRequest.Object);

            Assert.True(nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.TypeSpecified);
        }

        [Fact]
        public void Manipulate_CustomOptionDescription_WhenTypeIsFollowingRepair()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.FollowingRepair;
            shipmentEntity.FedEx.CustomsOptionsDesription = "option description";

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal("option description", nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Description);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeIsForRepair()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.ForRepair;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(CustomsOptionType.FOR_REPAIR, nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Type);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeSpecifiedIsTrue_WhenTypeIsForRepair()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.ForRepair;

            testObject.Manipulate(carrierRequest.Object);

            Assert.True(nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.TypeSpecified);
        }

        [Fact]
        public void Manipulate_CustomOptionDescription_WhenTypeIsForRepair()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.ForRepair;
            shipmentEntity.FedEx.CustomsOptionsDesription = "option description";

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal("option description", nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Description);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeIsItemForLoan()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.ItemForLoan;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(CustomsOptionType.ITEM_FOR_LOAN, nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Type);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeSpecifiedIsTrue_WhenTypeIsItemForLoan()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.ItemForLoan;

            testObject.Manipulate(carrierRequest.Object);

            Assert.True(nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.TypeSpecified);
        }

        [Fact]
        public void Manipulate_CustomOptionDescription_WhenTypeIsItemForLoan()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.ItemForLoan;
            shipmentEntity.FedEx.CustomsOptionsDesription = "option description";

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal("option description", nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Description);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeIsOther()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.Other;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(CustomsOptionType.OTHER, nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Type);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeSpecifiedIsTrue_WhenTypeIsOther()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.Other;

            testObject.Manipulate(carrierRequest.Object);

            Assert.True(nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.TypeSpecified);
        }

        [Fact]
        public void Manipulate_CustomOptionDescription_WhenTypeIsOther()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.Other;
            shipmentEntity.FedEx.CustomsOptionsDesription = "option description";

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal("option description", nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Description);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeIsRejected()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.Rejected;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(CustomsOptionType.REJECTED, nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Type);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeSpecifiedIsTrue_WhenTypeIsRejected()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.Rejected;

            testObject.Manipulate(carrierRequest.Object);

            Assert.True(nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.TypeSpecified);
        }

        [Fact]
        public void Manipulate_CustomOptionDescription_WhenTypeIsRejected()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.Rejected;
            shipmentEntity.FedEx.CustomsOptionsDesription = "option description";

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal("option description", nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Description);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeIsReplacement()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.Replacement;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(CustomsOptionType.REPLACEMENT, nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Type);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeSpecifiedIsTrue_WhenTypeIsReplacement()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.Replacement;

            testObject.Manipulate(carrierRequest.Object);

            Assert.True(nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.TypeSpecified);
        }

        [Fact]
        public void Manipulate_CustomOptionDescription_WhenTypeIsReplacement()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.Replacement;
            shipmentEntity.FedEx.CustomsOptionsDesription = "option description";

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal("option description", nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Description);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeIsTrial()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.Trial;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(CustomsOptionType.TRIAL, nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Type);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeSpecifiedIsTrue_WhenTypeIsTrial()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.Trial;

            testObject.Manipulate(carrierRequest.Object);

            Assert.True(nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.TypeSpecified);
        }

        [Fact]
        public void Manipulate_CustomOptionDescription_WhenTypeIsTrial()
        {
            shipmentEntity.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.Trial;
            shipmentEntity.FedEx.CustomsOptionsDesription = "option description";

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal("option description", nativeRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Description);
        }

        [Fact]
        public void Manipulate_ThrowsInvalidOperationException_WhenTypeIsNotRecognized()
        {
            shipmentEntity.FedEx.CustomsOptionsType = 32;

            Assert.Throws<InvalidOperationException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        #endregion Customs Options Tests
    }
}
