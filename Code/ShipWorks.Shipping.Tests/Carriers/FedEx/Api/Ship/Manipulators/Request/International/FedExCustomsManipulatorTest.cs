using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Interapptive.Shared.Enums;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request.International;
using ShipWorks.Tests.Shared;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Ship.Manipulators.Request.International
{
    public class FedExCustomsManipulatorTest
    {
        private readonly ProcessShipmentRequest processShipmentRequest;
        private readonly ShipmentEntity shipment;
        private readonly FedExCustomsManipulator testObject;
        private Mock<ICarrierSettingsRepository> settingsRepository;
        private FedExAccountEntity fedExAccount;
        private Mock<ICustomsRequired> customsRequired;
        private readonly AutoMock mock;

        public FedExCustomsManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            // Create a ProcessShipmentRequest type and set the properties the manipulator is interested in
            processShipmentRequest = new ProcessShipmentRequest()
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
            shipment = new ShipmentEntity()
            {
                ShipCountryCode = "NZ",
                OriginCountryCode = "CA",
                CustomsValue = 123.45M,
                FedEx = new FedExShipmentEntity() { PayorDutiesAccount = "987", PayorTransportName = "Transport Name", PayorDutiesType = (int)FedExPayorType.Sender, CustomsRecipientTIN = "5468" }
            };

            shipment.CustomsItems.Add(new ShipmentCustomsItemEntity { Description = "item one", Quantity = 4, Weight = 2.3, UnitValue = 15.36M, HarmonizedCode = "Harmonized 1", UnitPriceAmount = 51.36M, NumberOfPieces = 4 });
            shipment.CustomsItems.Add(new ShipmentCustomsItemEntity { Description = "item two", Quantity = 64, Weight = 112.3, UnitValue = 98.52M, HarmonizedCode = "Harmonized 2", UnitPriceAmount = 114.57M, NumberOfPieces = 12 });
            shipment.CustomsItems.Add(new ShipmentCustomsItemEntity { Description = "item three", Quantity = 342, Weight = 92.3, UnitValue = 6.67M, HarmonizedCode = "Harmonized 3", UnitPriceAmount = 14.06M, NumberOfPieces = 25 });

            fedExAccount = new FedExAccountEntity { AccountNumber = "123", CountryCode = "US", LastName = "Doe", FirstName = "John" };
            mock.Mock<IFedExSettingsRepository>()
                .Setup(x => x.GetAccountReadOnly(AnyIShipment))
                .Returns(fedExAccount);

            settingsRepository = mock.Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>())).Returns(fedExAccount);

            customsRequired = mock.Mock<ICustomsRequired>();
            customsRequired
                .Setup(c => c.IsCustomsRequired(It.IsAny<ShipmentEntity>()))
                .Returns(true);

            testObject = mock.Create<FedExCustomsManipulator>();
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

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void ShouldApply_ReturnsCorrectValue(bool isCustomsRequired, bool expectedValue)
        {
            customsRequired = mock.Mock<ICustomsRequired>();
            customsRequired
                .Setup(c => c.IsCustomsRequired(It.IsAny<ShipmentEntity>()))
                .Returns(isCustomsRequired);

            Assert.Equal(expectedValue, testObject.ShouldApply(shipment, 0));
        }

        [Fact]
        public void Manipulate_AccountsForRequestedShipment()
        {
            // Setup the test by configuring the native request to have a null requested shipment property 
            processShipmentRequest.RequestedShipment = null;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // The requested shipment property should be created now
            Assert.NotNull(processShipmentRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_AccountsForRecipient()
        {
            // Setup the test by configuring the native request to have a null recipient property 
            processShipmentRequest.RequestedShipment.Recipient = null;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // The recipient property should be created now
            Assert.NotNull(processShipmentRequest.RequestedShipment.Recipient);
        }

        [Fact]
        public void Manipulate_CustomsDetailIsNull_WhenCustomsNotRequired()
        {
            customsRequired
                .Setup(c => c.IsCustomsRequired(It.IsAny<ShipmentEntity>()))
                .Returns(false);

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Null(processShipmentRequest.RequestedShipment.CustomsClearanceDetail);
        }

        [Fact]
        public void Manipulate_CustomsValueAmountIsShipmentCustomsValue()
        {
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // The amount on the shipment is 123.45
            Assert.Equal(123.45M, processShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsValue.Amount);
        }

        [Fact]
        public void Manipulate_CustomsCurrencyValidIsUSD()
        {
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal("USD", processShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsValue.Currency);
        }

        [Fact]
        public void Manipulate_DocumentContentIsDocumentsOnly_WhenFedExCustomsDocumentsOnlyIsTrue()
        {
            shipment.FedEx.CustomsDocumentsOnly = true;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(InternationalDocumentContentType.DOCUMENTS_ONLY, processShipmentRequest.RequestedShipment.CustomsClearanceDetail.DocumentContent);
        }

        [Fact]
        public void Manipulate_DocumentContentIsNonDocuments_WhenFedExCustomsDocumentsOnlyIsFalse()
        {
            shipment.FedEx.CustomsDocumentsOnly = false;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(InternationalDocumentContentType.NON_DOCUMENTS, processShipmentRequest.RequestedShipment.CustomsClearanceDetail.DocumentContent);
        }


        #region Commodities Tests

        [Fact]
        public void Manipulate_CommoditiesAddsAllShipmentCustomItems_WhenFedExCustomsDocumentsOnlyIsFalse()
        {
            shipment.FedEx.CustomsDocumentsOnly = false;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(shipment.CustomsItems.Count, processShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities.Length);
        }

        [Fact]
        public void Manipulate_CommoditiesContainDescriptionFromShipmentCustomItems_WhenFedExCustomsDocumentsOnlyIsFalse()
        {
            shipment.FedEx.CustomsDocumentsOnly = false;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            for (int i = 0; i < shipment.CustomsItems.Count; i++)
            {
                Assert.Equal(shipment.CustomsItems[i].Description, processShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].Description);
            }
        }

        [Fact]
        public void Manipulate_CommoditiesContainQuantityFromShipmentCustomItems_WhenFedExCustomsDocumentsOnlyIsFalse()
        {
            shipment.FedEx.CustomsDocumentsOnly = false;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            for (int i = 0; i < shipment.CustomsItems.Count; i++)
            {
                Assert.Equal(shipment.CustomsItems[i].Quantity, (double)processShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].Quantity);
            }
        }

        [Fact]
        public void Manipulate_CommoditiesContainQuantityUnitsIsEA_WhenFedExCustomsDocumentsOnlyIsFalse()
        {
            shipment.FedEx.CustomsDocumentsOnly = false;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            for (int i = 0; i < shipment.CustomsItems.Count; i++)
            {
                Assert.Equal("EA", processShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].QuantityUnits);
            }
        }

        [Fact]
        public void Manipulate_CommoditiesContainNumberofPiecesFromFedExShipmentCustomItem_WhenFedExCustomsDocumentsOnlyIsFalse()
        {
            shipment.FedEx.CustomsDocumentsOnly = false;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            for (int i = 0; i < shipment.CustomsItems.Count; i++)
            {
                Assert.Equal(shipment.CustomsItems[i].NumberOfPieces.ToString(), processShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].NumberOfPieces);
            }
        }

        [Fact]
        public void Manipulate_CommoditiesWeightsFromShipmentCustomItem_WhenFedExCustomsDocumentsOnlyIsFalse()
        {
            shipment.FedEx.CustomsDocumentsOnly = false;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            for (int i = 0; i < shipment.CustomsItems.Count; i++)
            {
                Assert.Equal((decimal)shipment.CustomsItems[i].Weight, processShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].Weight.Value);
            }
        }

        [Fact]
        public void Manipulate_CommoditiesWeightsIsPounds_WhenFedExCustomsDocumentsOnlyIsFalse_AndCustomItemWeightIsPounds()
        {
            shipment.FedEx.CustomsDocumentsOnly = false;
            shipment.FedEx.WeightUnitType = (int)WeightUnitOfMeasure.Pounds;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            for (int i = 0; i < shipment.CustomsItems.Count; i++)
            {
                Assert.Equal(WeightUnits.LB, processShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].Weight.Units);
            }
        }

        [Fact]
        public void Manipulate_CommoditiesWeightsIsKilograms_WhenFedExCustomsDocumentsOnlyIsFalse_AndCustomItemWeightIsKilograms()
        {
            shipment.FedEx.CustomsDocumentsOnly = false;
            shipment.FedEx.WeightUnitType = (int)WeightUnitOfMeasure.Kilograms;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            for (int i = 0; i < shipment.CustomsItems.Count; i++)
            {
                Assert.Equal(WeightUnits.KG, processShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].Weight.Units);
            }
        }

        [Fact]
        public void Manipulate_ThrowsInvalidOperationException_WhenFedExCustomsDocumentsOnlyIsFalse_AndShipmentWeightTypeIsUnknown()
        {
            shipment.FedEx.CustomsDocumentsOnly = false;
            shipment.FedEx.WeightUnitType = 42;

            Assert.Throws<InvalidOperationException>(() => testObject.Manipulate(shipment, processShipmentRequest, 0));
        }

        [Fact]
        public void Manipulate_CommoditiesUnitPriceFromFedExShipmentCustomItem_WhenFedExCustomsDocumentsOnlyIsFalse()
        {
            shipment.FedEx.CustomsDocumentsOnly = false;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            for (int i = 0; i < shipment.CustomsItems.Count; i++)
            {
                Assert.Equal(shipment.CustomsItems[i].UnitPriceAmount, processShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].UnitPrice.Amount);
            }
        }

        [Fact]
        public void Manipulate_CommoditiesUnitPriceCurrencyIsUSD_WhenFedExCustomsDocumentsOnlyIsFalse_AndCurrencyIsUSD()
        {
            shipment.FedEx.CustomsDocumentsOnly = false;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            for (int i = 0; i < shipment.CustomsItems.Count; i++)
            {
                Assert.Equal("USD", processShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].UnitPrice.Currency);
            }
        }

        [Fact]
        public void Manipulate_CommoditiesUnitPriceCurrencyIsCAD_WhenFedExCustomsDocumentsOnlyIsFalse_AndCurrencyIsCAD()
        {
            shipment.FedEx.CustomsDocumentsOnly = false;
            fedExAccount.CountryCode = "CA";

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            for (int i = 0; i < shipment.CustomsItems.Count; i++)
            {
                Assert.Equal("CAD", processShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].UnitPrice.Currency);
            }
        }

        [Fact]
        public void Manipulate_CommoditiesCountryOfManufacture_IsShipmentCustomItemCountryOfOrigin_WhenFedExCustomsDocumentsOnlyIsFalse()
        {
            shipment.FedEx.CustomsDocumentsOnly = false;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            for (int i = 0; i < shipment.CustomsItems.Count; i++)
            {
                Assert.Equal(shipment.CustomsItems[i].CountryOfOrigin, processShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].CountryOfManufacture);
            }
        }

        [Fact]
        public void Manipulate_CommoditiesHarmonizedCode_IsShipmentCustomItemHarmonized_WhenFedExCustomsDocumentsOnlyIsFalse()
        {
            shipment.FedEx.CustomsDocumentsOnly = false;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            for (int i = 0; i < shipment.CustomsItems.Count; i++)
            {
                Assert.Equal(shipment.CustomsItems[i].HarmonizedCode, processShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].HarmonizedCode);
            }
        }

        [Fact]
        public void Manipulate_CommoditiesCustomsValueCurrencyIsUSD_WhenFedExCustomsDocumentsOnlyIsFalse()
        {
            shipment.FedEx.CustomsDocumentsOnly = false;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            for (int i = 0; i < shipment.CustomsItems.Count; i++)
            {
                Assert.Equal("USD", processShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].CustomsValue.Currency);
            }
        }

        [Fact]
        public void Manipulate_CommoditiesCustomsValueFromShipmentCustomItem_WhenFedExCustomsDocumentsOnlyIsFalse()
        {
            shipment.FedEx.CustomsDocumentsOnly = false;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            for (int i = 0; i < shipment.CustomsItems.Count; i++)
            {
                Assert.Equal(shipment.CustomsItems[i].UnitValue, processShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities[i].CustomsValue.Amount);
            }
        }

        #endregion Commodities Tests


        #region NAFTA Tests

        [Fact]
        public void Manipulate_NaftaDetailIsNull_WhenNaftaIsNotEnabled()
        {
            shipment.FedEx.CustomsNaftaEnabled = false;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.Null(commodity.NaftaDetail);
            }
        }

        [Fact]
        public void Manipulate_RegulatoryControlIsNull_WhenNaftaIsNotEnabled()
        {
            shipment.FedEx.CustomsNaftaEnabled = false;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Null(customsDetail.RegulatoryControls);
        }


        [Fact]
        public void Manipulate_RegulatoryControlArrayHasSizeOne_WhenNaftaIsEnabled()
        {
            shipment.FedEx.CustomsNaftaEnabled = true;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal(1, customsDetail.RegulatoryControls.Length);
        }

        [Fact]
        public void Manipulate_AddsNaftaAsRegulatoryControl_WhenNaftaIsEnabled()
        {
            shipment.FedEx.CustomsNaftaEnabled = true;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal(RegulatoryControlType.NAFTA, customsDetail.RegulatoryControls[0]);
        }

        [Fact]
        public void Manipulate_NaftaDetailIsNotNull_WhenNaftaIsEnabled()
        {
            shipment.FedEx.CustomsNaftaEnabled = true;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.NotNull(commodity.NaftaDetail);
            }
        }

        [Fact]
        public void Manipulate_NetCostMethodIsNO_WhenNaftaIsEnabled_AndNetCostIsNotCalculated()
        {
            shipment.FedEx.CustomsNaftaEnabled = true;
            shipment.FedEx.CustomsNaftaNetCostMethod = (int)FedExNaftaNetCostMethod.NotCalculated;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.Equal(NaftaNetCostMethodCode.NO, commodity.NaftaDetail.NetCostMethod);
            }
        }

        [Fact]
        public void Manipulate_NetCostMethodIsNC_WhenNaftaIsEnabled_AndNetCostIsCalculated()
        {
            shipment.FedEx.CustomsNaftaEnabled = true;
            shipment.FedEx.CustomsNaftaNetCostMethod = (int)FedExNaftaNetCostMethod.NetCostMethod;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.Equal(NaftaNetCostMethodCode.NC, commodity.NaftaDetail.NetCostMethod);
            }
        }

        [Fact]
        public void Manipulate_ThrowsInvalidOperationException_WhenNaftaIsEnabled_AndNetCostIsInvalid()
        {
            shipment.FedEx.CustomsNaftaEnabled = true;
            shipment.FedEx.CustomsNaftaNetCostMethod = 54;

            Assert.Throws<InvalidOperationException>(() => testObject.Manipulate(shipment, processShipmentRequest, 0));
        }

        [Fact]
        public void Manipulate_NetCostMethodIsSpecified_WhenNaftaIsEnabled()
        {
            shipment.FedEx.CustomsNaftaEnabled = true;
            shipment.FedEx.CustomsNaftaNetCostMethod = (int)FedExNaftaNetCostMethod.NetCostMethod;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.True(commodity.NaftaDetail.NetCostMethodSpecified);
            }
        }


        [Fact]
        public void Manipulate_PreferenceCriterionIsA_WhenNaftaIsEnable()
        {
            shipment.FedEx.CustomsNaftaEnabled = true;
            shipment.FedEx.CustomsNaftaPreferenceType = (int)FedExNaftaPreferenceCriteria.A;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.Equal(NaftaPreferenceCriterionCode.A, commodity.NaftaDetail.PreferenceCriterion);
            }
        }

        [Fact]
        public void Manipulate_PreferenceCriterionIsB_WhenNaftaIsEnable()
        {
            shipment.FedEx.CustomsNaftaEnabled = true;
            shipment.FedEx.CustomsNaftaPreferenceType = (int)FedExNaftaPreferenceCriteria.B;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.Equal(NaftaPreferenceCriterionCode.B, commodity.NaftaDetail.PreferenceCriterion);
            }
        }

        [Fact]
        public void Manipulate_PreferenceCriterionIsC_WhenNaftaIsEnable()
        {
            shipment.FedEx.CustomsNaftaEnabled = true;
            shipment.FedEx.CustomsNaftaPreferenceType = (int)FedExNaftaPreferenceCriteria.C;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.Equal(NaftaPreferenceCriterionCode.C, commodity.NaftaDetail.PreferenceCriterion);
            }
        }

        [Fact]
        public void Manipulate_PreferenceCriterionIsD_WhenNaftaIsEnable()
        {
            shipment.FedEx.CustomsNaftaEnabled = true;
            shipment.FedEx.CustomsNaftaPreferenceType = (int)FedExNaftaPreferenceCriteria.D;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.Equal(NaftaPreferenceCriterionCode.D, commodity.NaftaDetail.PreferenceCriterion);
            }
        }

        [Fact]
        public void Manipulate_PreferenceCriterionIsE_WhenNaftaIsEnable()
        {
            shipment.FedEx.CustomsNaftaEnabled = true;
            shipment.FedEx.CustomsNaftaPreferenceType = (int)FedExNaftaPreferenceCriteria.E;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.Equal(NaftaPreferenceCriterionCode.E, commodity.NaftaDetail.PreferenceCriterion);
            }
        }

        [Fact]
        public void Manipulate_PreferenceCriterionIsF_WhenNaftaIsEnable()
        {
            shipment.FedEx.CustomsNaftaEnabled = true;
            shipment.FedEx.CustomsNaftaPreferenceType = (int)FedExNaftaPreferenceCriteria.F;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.Equal(NaftaPreferenceCriterionCode.F, commodity.NaftaDetail.PreferenceCriterion);
            }
        }

        [Fact]
        public void Manipulate_ThrowsInvalidOperationException_WhenNaftaIsEnable_AndPreferenceIsInvalid()
        {
            shipment.FedEx.CustomsNaftaEnabled = true;
            shipment.FedEx.CustomsNaftaPreferenceType = 40;

            Assert.Throws<InvalidOperationException>(() => testObject.Manipulate(shipment, processShipmentRequest, 0));
        }


        [Fact]
        public void Manipulate_PreferenceCriterionIsSpecified_WhenNaftaIsEnable()
        {
            shipment.FedEx.CustomsNaftaEnabled = true;
            shipment.FedEx.CustomsNaftaPreferenceType = (int)FedExNaftaPreferenceCriteria.F;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.True(commodity.NaftaDetail.PreferenceCriterionSpecified);
            }
        }


        [Fact]
        public void Manipulate_ProducerDeterminationIsYes_WhenNaftaIsEnable_AndCodeIsProducerOfCommodity()
        {
            shipment.FedEx.CustomsNaftaEnabled = true;
            shipment.FedEx.CustomsNaftaDeterminationCode = (int)FedExNaftaDeterminationCode.ProducerOfCommodity;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.Equal(NaftaProducerDeterminationCode.YES, commodity.NaftaDetail.ProducerDetermination);
            }
        }

        [Fact]
        public void Manipulate_ProducerDeterminationIsNo1_WhenNaftaIsEnable_AndCodeIsNotProducerKnowledge()
        {
            shipment.FedEx.CustomsNaftaEnabled = true;
            shipment.FedEx.CustomsNaftaDeterminationCode = (int)FedExNaftaDeterminationCode.NotProducerKnowledgeOfCommodity;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.Equal(NaftaProducerDeterminationCode.NO_1, commodity.NaftaDetail.ProducerDetermination);
            }
        }

        [Fact]
        public void Manipulate_ProducerDeterminationIsNo2_WhenNaftaIsEnable_AndCodeIsNotProducerStatement()
        {
            shipment.FedEx.CustomsNaftaEnabled = true;
            shipment.FedEx.CustomsNaftaDeterminationCode = (int)FedExNaftaDeterminationCode.NotProducerWrittenStatement;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.Equal(NaftaProducerDeterminationCode.NO_2, commodity.NaftaDetail.ProducerDetermination);
            }
        }

        [Fact]
        public void Manipulate_ProducerDeterminationIsNo3_WhenNaftaIsEnable_AndCodeIsNotProducerSignedCertificate()
        {
            shipment.FedEx.CustomsNaftaEnabled = true;
            shipment.FedEx.CustomsNaftaDeterminationCode = (int)FedExNaftaDeterminationCode.NotProducerSignedCertificate;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            foreach (Commodity commodity in customsDetail.Commodities)
            {
                Assert.Equal(NaftaProducerDeterminationCode.NO_3, commodity.NaftaDetail.ProducerDetermination);
            }
        }

        [Fact]
        public void Manipulate_ThrowsInvalidOperationException_WhenNaftaIsEnable_AndProducerIsInvalid()
        {
            shipment.FedEx.CustomsNaftaEnabled = true;
            shipment.FedEx.CustomsNaftaPreferenceType = 13;

            Assert.Throws<InvalidOperationException>(() => testObject.Manipulate(shipment, processShipmentRequest, 0));
        }


        [Fact]
        public void Manipulate_ProducerDeterminationIsSpecified_WhenNaftaIsEnable()
        {
            shipment.FedEx.CustomsNaftaEnabled = true;
            shipment.FedEx.CustomsNaftaPreferenceType = (int)FedExNaftaPreferenceCriteria.F;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
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
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.NotNull(processShipmentRequest.RequestedShipment.CustomsClearanceDetail.DutiesPayment);
        }


        [Fact]
        public void Manipulate_UsesFedExAccountNumber_WhenPayorDutiesTypeIsSender()
        {
            // Setup the fedex shipment payor type for the test
            shipment.FedEx.PayorDutiesType = (int)FedExPayorType.Sender;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Payment dutiesPayment = processShipmentRequest.RequestedShipment.CustomsClearanceDetail.DutiesPayment;
            Assert.Equal(fedExAccount.AccountNumber, dutiesPayment.Payor.ResponsibleParty.AccountNumber);
        }

        [Fact]
        public void Manipulate_UsesFedExAccountCountryCode_WhenPayorDutiesTypeIsSender()
        {
            // Setup the fedex shipment payor type for the test
            shipment.FedEx.PayorDutiesType = (int)FedExPayorType.Sender;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Payment dutiesPayment = processShipmentRequest.RequestedShipment.CustomsClearanceDetail.DutiesPayment;
            Assert.Equal(fedExAccount.CountryCode, dutiesPayment.Payor.ResponsibleParty.Address.CountryCode);
        }

        [Fact]
        public void Manipulate_UsesFedExAccountNameAsContactName_WhenPayorDutiesTypeIsSender()
        {
            // Setup the fedex shipment payor type for the test
            shipment.FedEx.PayorDutiesType = (int)FedExPayorType.Sender;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Payment dutiesPayment = processShipmentRequest.RequestedShipment.CustomsClearanceDetail.DutiesPayment;
            Assert.Equal(fedExAccount.FirstName + " " + fedExAccount.LastName, dutiesPayment.Payor.ResponsibleParty.Contact.PersonName);
        }


        [Fact]
        public void Manipulate_SetsPaymentTypeToRecipient()
        {
            // Setup the fedex shipment payor type for the test
            shipment.FedEx.PayorDutiesType = (int)FedExPayorType.Recipient;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Payment dutiesPayment = processShipmentRequest.RequestedShipment.CustomsClearanceDetail.DutiesPayment;
            Assert.Equal(PaymentType.RECIPIENT, dutiesPayment.PaymentType);
        }

        [Fact]
        public void Manipulate_UsesPayorDutiesCountryCode_WhenPayorDutiesTypeIsRecipient()
        {
            // Setup the fedex shipment payor type for the test
            shipment.FedEx.PayorDutiesType = (int)FedExPayorType.Recipient;
            shipment.FedEx.PayorDutiesCountryCode = "CA";

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Payment dutiesPayment = processShipmentRequest.RequestedShipment.CustomsClearanceDetail.DutiesPayment;
            Assert.Equal("CA", dutiesPayment.Payor.ResponsibleParty.Address.CountryCode);
        }

        [Fact]
        public void Manipulate_UsesFedExShipmentPayorDutiesAccount_WhenPayorDutiesTypeIsRecipient()
        {
            // Setup the fedex shipment payor type for the test
            shipment.FedEx.PayorDutiesType = (int)FedExPayorType.Recipient;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Payment dutiesPayment = processShipmentRequest.RequestedShipment.CustomsClearanceDetail.DutiesPayment;
            Assert.Equal(shipment.FedEx.PayorDutiesAccount, dutiesPayment.Payor.ResponsibleParty.AccountNumber);
        }

        [Fact]
        public void Manipulate_UsesPayorDutiesName_WhenPayorDutiesTypeIsRecepient()
        {
            shipment.FedEx.PayorDutiesType = (int)FedExPayorType.Recipient;
            shipment.FedEx.PayorDutiesName = "Peter Gibbons";

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Payment dutiesPayment = processShipmentRequest.RequestedShipment.CustomsClearanceDetail.DutiesPayment;
            Assert.Equal("Peter Gibbons", dutiesPayment.Payor.ResponsibleParty.Contact.PersonName);
        }

        [Fact]
        public void Manipulate_SetsPaymentTypeToThirdParty()
        {
            // Setup the fedex shipment payor type for the test
            shipment.FedEx.PayorDutiesType = (int)FedExPayorType.ThirdParty;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Payment dutiesPayment = processShipmentRequest.RequestedShipment.CustomsClearanceDetail.DutiesPayment;
            Assert.Equal(PaymentType.THIRD_PARTY, dutiesPayment.PaymentType);
        }

        [Fact]
        public void Manipulate_UsesFedExShipmentPayorDutiesAccount_WhenPayorDutiesTypeIsThirdParty()
        {
            // Setup the fedex shipment payor type for the test
            shipment.FedEx.PayorDutiesType = (int)FedExPayorType.ThirdParty;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Payment dutiesPayment = processShipmentRequest.RequestedShipment.CustomsClearanceDetail.DutiesPayment;
            Assert.Equal(shipment.FedEx.PayorDutiesAccount, dutiesPayment.Payor.ResponsibleParty.AccountNumber);
        }

        [Fact]
        public void Manipulate_UsesPayorDutiesName_WhenPayorDutiesTypeIsThirdParty()
        {
            shipment.FedEx.PayorDutiesType = (int)FedExPayorType.ThirdParty;
            shipment.FedEx.PayorDutiesName = "Michael Bolton";

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Payment dutiesPayment = processShipmentRequest.RequestedShipment.CustomsClearanceDetail.DutiesPayment;
            Assert.Equal("Michael Bolton", dutiesPayment.Payor.ResponsibleParty.Contact.PersonName);
        }

        [Fact]
        public void Manipulate_UsesPayorDutiesCountryCode_WhenPayorDutiesTypeIsThirdParty()
        {
            // Setup the fedex shipment payor type for the test
            shipment.FedEx.PayorDutiesType = (int)FedExPayorType.ThirdParty;
            shipment.FedEx.PayorDutiesCountryCode = "UK";

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Payment dutiesPayment = processShipmentRequest.RequestedShipment.CustomsClearanceDetail.DutiesPayment;
            Assert.Equal("UK", dutiesPayment.Payor.ResponsibleParty.Address.CountryCode);
        }

        [Fact]
        public void Manipulate_SetsPaymentTypeToCollect()
        {
            // Setup the fedex shipment payor type for the test
            shipment.FedEx.PayorDutiesType = (int)FedExPayorType.Collect;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Payment dutiesPayment = processShipmentRequest.RequestedShipment.CustomsClearanceDetail.DutiesPayment;
            Assert.Equal(PaymentType.COLLECT, dutiesPayment.PaymentType);
        }

        [Fact]
        public void Manipulate_ContactPersonNameIsNullOrEmpty_WhenPayorDutiesTypeIsCollect()
        {
            shipment.FedEx.PayorDutiesType = (int)FedExPayorType.Collect;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Payment dutiesPayment = processShipmentRequest.RequestedShipment.CustomsClearanceDetail.DutiesPayment;

            Assert.Null(dutiesPayment.Payor);
        }

        [Fact]
        public void Manipulate_AccountNumberIsNullOrEmpty_WhenPayorDutiesTypeIsCollect()
        {
            shipment.FedEx.PayorDutiesType = (int)FedExPayorType.Collect;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Payment dutiesPayment = processShipmentRequest.RequestedShipment.CustomsClearanceDetail.DutiesPayment;

            Assert.Null(dutiesPayment.Payor);
        }

        [Fact]
        public void Manipulate_ThrowsInvalidOperationException_ForUnknownPayorType()
        {
            // Setup the fedex shipment payor type for the test by setting the type to an unsupported value
            shipment.FedEx.PayorDutiesType = 23;

            Assert.Throws<InvalidOperationException>(() => testObject.Manipulate(shipment, processShipmentRequest, 0));
        }

        #endregion Payment Detail Tests


        #region Tax Payer Tests

        [Fact]
        public void Manipulate_RecipientTinsIsNotNull()
        {
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.NotNull(processShipmentRequest.RequestedShipment.Recipient.Tins);
        }

        [Fact]
        public void Manipulate_RecipientTinsCountIsOne()
        {
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(1, processShipmentRequest.RequestedShipment.Recipient.Tins.Length);
        }

        [Fact]
        public void Manipulate_RecipientTinsCountIsOne_WhenTinsIsInitiallyNull()
        {
            processShipmentRequest.RequestedShipment.Recipient.Tins = null;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(1, processShipmentRequest.RequestedShipment.Recipient.Tins.Length);
        }

        [Fact]
        public void Manipulate_RecipientTinsCountIsOne_WhenTinsCountIsInitiallyZero()
        {
            processShipmentRequest.RequestedShipment.Recipient.Tins = new TaxpayerIdentification[0];

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(1, processShipmentRequest.RequestedShipment.Recipient.Tins.Length);
        }

        [Fact]
        public void Manipulate_RecipientTinsNumber_IsShipmentCustomerRecipientTin()
        {
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(shipment.FedEx.CustomsRecipientTIN, processShipmentRequest.RequestedShipment.Recipient.Tins[0].Number);
        }

        [Fact]
        public void Manipulate_RecipientTinsType_IsPersonalState()
        {
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(TinType.PERSONAL_STATE, processShipmentRequest.RequestedShipment.Recipient.Tins[0].TinType);
        }


        #endregion Tax Payer Tests

        [Fact]
        public void Manipulate_ExportDetailIsNotNull()
        {
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.NotNull(processShipmentRequest.RequestedShipment.CustomsClearanceDetail.ExportDetail);
        }

        [Fact]
        public void Manipulate_ExportDetailsNullWhenShippingFromUSInternationaly()
        {
            shipment.ShipCountryCode = "CA";
            shipment.OriginCountryCode = "US";

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.False(processShipmentRequest.RequestedShipment.CustomsClearanceDetail.ExportDetail.B13AFilingOptionSpecified);
        }

        [Fact]
        public void Manipulate_ExportDetailsNullWhenShippingFromCaToUs()
        {
            shipment.ShipCountryCode = "US";
            shipment.OriginCountryCode = "CA";

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.False(processShipmentRequest.RequestedShipment.CustomsClearanceDetail.ExportDetail.B13AFilingOptionSpecified);
        }

        [Fact]
        public void Manipulate_B13FilingOptionSpecifiedIsTrue()
        {
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.True(processShipmentRequest.RequestedShipment.CustomsClearanceDetail.ExportDetail.B13AFilingOptionSpecified);
        }

        [Fact]
        public void Manipulate_B13FilingOptionSpecifiedIsNotRequired()
        {
            shipment.FedEx.CustomsExportFilingOption = (int)FedExCustomsExportFilingOption.NotRequired;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(B13AFilingOptionType.NOT_REQUIRED, processShipmentRequest.RequestedShipment.CustomsClearanceDetail.ExportDetail.B13AFilingOption);
        }

        [Fact]
        public void Manipulate_B13FilingOptionSpecifiedIsManuallyAttached()
        {
            shipment.FedEx.CustomsExportFilingOption = (int)FedExCustomsExportFilingOption.ManuallyAttached;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(B13AFilingOptionType.MANUALLY_ATTACHED, processShipmentRequest.RequestedShipment.CustomsClearanceDetail.ExportDetail.B13AFilingOption);
        }

        [Fact]
        public void Manipulate_B13FilingOptionSpecifiedIsFiledElectronically()
        {
            shipment.FedEx.CustomsExportFilingOption = (int)FedExCustomsExportFilingOption.FiledElectonically;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(B13AFilingOptionType.FILED_ELECTRONICALLY, processShipmentRequest.RequestedShipment.CustomsClearanceDetail.ExportDetail.B13AFilingOption);
        }

        [Fact]
        public void Manipulate_B13FilingOptionSpecifiedIsSummaryReporting()
        {
            shipment.FedEx.CustomsExportFilingOption = (int)FedExCustomsExportFilingOption.SummaryReporting;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(B13AFilingOptionType.SUMMARY_REPORTING, processShipmentRequest.RequestedShipment.CustomsClearanceDetail.ExportDetail.B13AFilingOption);
        }

        [Fact]
        public void Manipulate_ComplianceStatementIsNull_WhenAESEEIIsEmptyStringTest()
        {
            shipment.FedEx.CustomsAESEEI = string.Empty;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.NotEqual(string.Empty, processShipmentRequest.RequestedShipment.CustomsClearanceDetail.ExportDetail.ExportComplianceStatement);
        }

        [Fact]
        public void Manipulate_ComplianceStatementIsNotEmpty()
        {
            shipment.FedEx.CustomsAESEEI = "NO EEI 30.2(d)(2)";

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.NotEqual(string.Empty, processShipmentRequest.RequestedShipment.CustomsClearanceDetail.ExportDetail.ExportComplianceStatement);
        }

        [Fact]
        public void Manipulate_RecipientCustomsIdIsNull_WhenIdentificationTypeIsNone()
        {
            shipment.FedEx.CustomsRecipientIdentificationType = (int)FedExCustomsRecipientIdentificationType.None;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Null(processShipmentRequest.RequestedShipment.CustomsClearanceDetail.RecipientCustomsId);
        }

        [Fact]
        public void Manipulate_RecipientCustomsIdTypeIsPassport_WhenIdentificationTypeIsPassport()
        {
            shipment.FedEx.CustomsRecipientIdentificationType = (int)FedExCustomsRecipientIdentificationType.Passport;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(RecipientCustomsIdType.PASSPORT, processShipmentRequest.RequestedShipment.CustomsClearanceDetail.RecipientCustomsId.Type);
        }

        [Fact]
        public void Manipulate_RecipientCustomsIdTypeSpecifiedIsTrue_WhenIdentificationTypeIsPassport()
        {
            shipment.FedEx.CustomsRecipientIdentificationType = (int)FedExCustomsRecipientIdentificationType.Passport;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.True(processShipmentRequest.RequestedShipment.CustomsClearanceDetail.RecipientCustomsId.TypeSpecified);
        }

        [Fact]
        public void Manipulate_RecipientCustomsIdValue_WhenIdentificationTypeIsPassport()
        {
            shipment.FedEx.CustomsRecipientIdentificationType = (int)FedExCustomsRecipientIdentificationType.Passport;
            shipment.FedEx.CustomsRecipientIdentificationValue = "123456";

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal("123456", processShipmentRequest.RequestedShipment.CustomsClearanceDetail.RecipientCustomsId.Value);
        }

        [Fact]
        public void Manipulate_RecipientCustomsIdTypeIsIndividual_WhenIdentificationTypeIsIndividual()
        {
            shipment.FedEx.CustomsRecipientIdentificationType = (int)FedExCustomsRecipientIdentificationType.Individual;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(RecipientCustomsIdType.INDIVIDUAL, processShipmentRequest.RequestedShipment.CustomsClearanceDetail.RecipientCustomsId.Type);
        }

        [Fact]
        public void Manipulate_RecipientCustomsIdTypeSpecifiedIsTrue_WhenIdentificationTypeIsIndividual()
        {
            shipment.FedEx.CustomsRecipientIdentificationType = (int)FedExCustomsRecipientIdentificationType.Individual;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.True(processShipmentRequest.RequestedShipment.CustomsClearanceDetail.RecipientCustomsId.TypeSpecified);
        }

        [Fact]
        public void Manipulate_RecipientCustomsIdValue_WhenIdentificationTypeIsIndividual()
        {
            shipment.FedEx.CustomsRecipientIdentificationType = (int)FedExCustomsRecipientIdentificationType.Individual;
            shipment.FedEx.CustomsRecipientIdentificationValue = "123456";

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal("123456", processShipmentRequest.RequestedShipment.CustomsClearanceDetail.RecipientCustomsId.Value);
        }

        [Fact]
        public void Manipulate_RecipientCustomsIdTypeIsCompany_WhenIdentificationTypeIsCompany()
        {
            shipment.FedEx.CustomsRecipientIdentificationType = (int)FedExCustomsRecipientIdentificationType.Company;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(RecipientCustomsIdType.COMPANY, processShipmentRequest.RequestedShipment.CustomsClearanceDetail.RecipientCustomsId.Type);
        }

        [Fact]
        public void Manipulate_RecipientCustomsIdTypeSpecifiedIsTrue_WhenIdentificationTypeIsCompany()
        {
            shipment.FedEx.CustomsRecipientIdentificationType = (int)FedExCustomsRecipientIdentificationType.Company;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.True(processShipmentRequest.RequestedShipment.CustomsClearanceDetail.RecipientCustomsId.TypeSpecified);
        }

        [Fact]
        public void Manipulate_RecipientCustomsIdValue_WhenIdentificationTypeIsCompany()
        {
            shipment.FedEx.CustomsRecipientIdentificationType = (int)FedExCustomsRecipientIdentificationType.Company;
            shipment.FedEx.CustomsRecipientIdentificationValue = "123456";

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal("123456", processShipmentRequest.RequestedShipment.CustomsClearanceDetail.RecipientCustomsId.Value);
        }

        [Fact]
        public void Manipulate_ThrowsInvalidOperationException_WhenIdentificationTypeIsNotRecognized()
        {
            shipment.FedEx.CustomsRecipientIdentificationType = 53;

            Assert.Throws<InvalidOperationException>(() => testObject.Manipulate(shipment, processShipmentRequest, 0));
        }


        #region Customs Options Tests

        [Fact]
        public void Manipulate_CustomOptionsIsNull_WhenOptionTypeIsNone()
        {
            shipment.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.None;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Null(processShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeIsCourtesyReturnLabel()
        {
            shipment.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.CourtesyReturnLabel;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(CustomsOptionType.COURTESY_RETURN_LABEL, processShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Type);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeSpecifiedIsTrue_WhenTypeIsCourtesyReturnLabel()
        {
            shipment.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.CourtesyReturnLabel;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.True(processShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.TypeSpecified);
        }

        [Fact]
        public void Manipulate_CustomOptionDescription_WhenTypeIsCourtesyReturnLabel()
        {
            shipment.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.CourtesyReturnLabel;
            shipment.FedEx.CustomsOptionsDesription = "option description";

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal("option description", processShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Description);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeIsExhibitionTradeShow()
        {
            shipment.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.ExhibitionTradeShow;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(CustomsOptionType.EXHIBITION_TRADE_SHOW, processShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Type);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeSpecifiedIsTrue_WhenTypeIsExhibitionTradeShow()
        {
            shipment.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.ExhibitionTradeShow;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.True(processShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.TypeSpecified);
        }

        [Fact]
        public void Manipulate_CustomOptionDescription_WhenTypeIsExhibitionTradeShow()
        {
            shipment.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.ExhibitionTradeShow;
            shipment.FedEx.CustomsOptionsDesription = "option description";

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal("option description", processShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Description);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeIsFaultyItem()
        {
            shipment.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.FaultyItem;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(CustomsOptionType.FAULTY_ITEM, processShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Type);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeSpecifiedIsTrue_WhenTypeIsFaultyItem()
        {
            shipment.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.FaultyItem;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.True(processShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.TypeSpecified);
        }

        [Fact]
        public void Manipulate_CustomOptionDescription_WhenTypeIsFaultyItem()
        {
            shipment.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.FaultyItem;
            shipment.FedEx.CustomsOptionsDesription = "option description";

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal("option description", processShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Description);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeIsFollowingRepair()
        {
            shipment.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.FollowingRepair;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(CustomsOptionType.FOLLOWING_REPAIR, processShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Type);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeSpecifiedIsTrue_WhenTypeIsFollowingRepair()
        {
            shipment.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.FollowingRepair;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.True(processShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.TypeSpecified);
        }

        [Fact]
        public void Manipulate_CustomOptionDescription_WhenTypeIsFollowingRepair()
        {
            shipment.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.FollowingRepair;
            shipment.FedEx.CustomsOptionsDesription = "option description";

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal("option description", processShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Description);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeIsForRepair()
        {
            shipment.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.ForRepair;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(CustomsOptionType.FOR_REPAIR, processShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Type);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeSpecifiedIsTrue_WhenTypeIsForRepair()
        {
            shipment.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.ForRepair;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.True(processShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.TypeSpecified);
        }

        [Fact]
        public void Manipulate_CustomOptionDescription_WhenTypeIsForRepair()
        {
            shipment.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.ForRepair;
            shipment.FedEx.CustomsOptionsDesription = "option description";

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal("option description", processShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Description);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeIsItemForLoan()
        {
            shipment.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.ItemForLoan;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(CustomsOptionType.ITEM_FOR_LOAN, processShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Type);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeSpecifiedIsTrue_WhenTypeIsItemForLoan()
        {
            shipment.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.ItemForLoan;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.True(processShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.TypeSpecified);
        }

        [Fact]
        public void Manipulate_CustomOptionDescription_WhenTypeIsItemForLoan()
        {
            shipment.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.ItemForLoan;
            shipment.FedEx.CustomsOptionsDesription = "option description";

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal("option description", processShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Description);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeIsOther()
        {
            shipment.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.Other;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(CustomsOptionType.OTHER, processShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Type);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeSpecifiedIsTrue_WhenTypeIsOther()
        {
            shipment.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.Other;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.True(processShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.TypeSpecified);
        }

        [Fact]
        public void Manipulate_CustomOptionDescription_WhenTypeIsOther()
        {
            shipment.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.Other;
            shipment.FedEx.CustomsOptionsDesription = "option description";

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal("option description", processShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Description);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeIsRejected()
        {
            shipment.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.Rejected;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(CustomsOptionType.REJECTED, processShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Type);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeSpecifiedIsTrue_WhenTypeIsRejected()
        {
            shipment.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.Rejected;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.True(processShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.TypeSpecified);
        }

        [Fact]
        public void Manipulate_CustomOptionDescription_WhenTypeIsRejected()
        {
            shipment.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.Rejected;
            shipment.FedEx.CustomsOptionsDesription = "option description";

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal("option description", processShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Description);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeIsReplacement()
        {
            shipment.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.Replacement;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(CustomsOptionType.REPLACEMENT, processShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Type);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeSpecifiedIsTrue_WhenTypeIsReplacement()
        {
            shipment.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.Replacement;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.True(processShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.TypeSpecified);
        }

        [Fact]
        public void Manipulate_CustomOptionDescription_WhenTypeIsReplacement()
        {
            shipment.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.Replacement;
            shipment.FedEx.CustomsOptionsDesription = "option description";

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal("option description", processShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Description);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeIsTrial()
        {
            shipment.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.Trial;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(CustomsOptionType.TRIAL, processShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Type);
        }

        [Fact]
        public void Manipulate_CustomOptionTypeSpecifiedIsTrue_WhenTypeIsTrial()
        {
            shipment.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.Trial;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.True(processShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.TypeSpecified);
        }

        [Fact]
        public void Manipulate_CustomOptionDescription_WhenTypeIsTrial()
        {
            shipment.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.Trial;
            shipment.FedEx.CustomsOptionsDesription = "option description";

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal("option description", processShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Description);
        }

        [Fact]
        public void Manipulate_ThrowsInvalidOperationException_WhenTypeIsNotRecognized()
        {
            shipment.FedEx.CustomsOptionsType = 32;

            Assert.Throws<InvalidOperationException>(() => testObject.Manipulate(shipment, processShipmentRequest, 0));
        }

        #endregion Customs Options Tests
    }
}
