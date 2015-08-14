using System;
using System.Collections.Generic;
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
    public class FedExCommercialInvoiceManipulatorTest
    {
        private FedExCommercialInvoiceManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private ProcessShipmentRequest nativeRequest;
        private ShipmentEntity shipmentEntity;
        private FedExAccountEntity fedExAccount;
        private Mock<ICarrierSettingsRepository> settingsRepository;

        public FedExCommercialInvoiceManipulatorTest()
        {
            shipmentEntity = new ShipmentEntity
            {
                FedEx = new FedExShipmentEntity
                {
                    CommercialInvoice = true,
                    CommercialInvoiceTermsOfSale = (int)FedExTermsOfSale.CFR_or_CPT,
                    CommercialInvoicePurpose = (int)FedExCommercialInvoicePurpose.Gift,
                    CommercialInvoiceComments = "some comments",
                    CommercialInvoiceFreight = 40.23M,
                    CommercialInvoiceOther = 84.20M,
                    CommercialInvoiceInsurance = 12.48M,

                    ImporterAccount = "123",
                    ImporterCity = "St. Louis",
                    ImporterCompany = "ACME",
                    ImporterCountryCode = "US",
                    ImporterFirstName = "Broker",
                    ImporterLastName = "McBrokerson",
                    ImporterPhone = "555-555-5555",
                    ImporterPostalCode = "63102",
                    ImporterStateProvCode = "MO",
                    ImporterStreet1 = "1 Memorial Drive",
                    ImporterStreet2 = "Suite 2000",
                },
                OriginCountryCode = "US",
                ShipCountryCode = "GB"
            };

            nativeRequest = new ProcessShipmentRequest()
            {
                RequestedShipment = new RequestedShipment()
                {
                    CustomsClearanceDetail = new CustomsClearanceDetail()
                }
            };

            fedExAccount = new FedExAccountEntity { AccountNumber = "123", CountryCode = "US", LastName = "Doe", FirstName = "John" };

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            carrierRequest.Setup(r => r.CarrierAccountEntity).Returns(fedExAccount);

            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>())).Returns(fedExAccount);

            testObject = new FedExCommercialInvoiceManipulator(new FedExSettings(settingsRepository.Object));
        }


        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull_Test()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(null));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull_Test()
        {
            // Setup the native request to be null
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, null);

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotProcessShipmentRequest_Test()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new object());

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_AccountsForNullRequestedShipment_Test()
        {
            // setup the test by setting the requested shipment to null
            nativeRequest.RequestedShipment = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.NotNull(nativeRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_AccountsForNullSpecialServices_Test()
        {
            // setup the test by setting the requested shipment to null
            nativeRequest.RequestedShipment.SpecialServicesRequested = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.NotNull(nativeRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_CustomClearanceDetailsIsNotNull_WhenCommercialInvoiceIsTrue_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = true;
            nativeRequest.RequestedShipment.CustomsClearanceDetail = null;

            testObject.Manipulate(carrierRequest.Object);

            // Make sure that the customs detail gets added back to the request
            Assert.NotNull(nativeRequest.RequestedShipment.CustomsClearanceDetail);
        }

        [Fact]
        public void Manipulate_CustomClearanceDetailsIsNull_WhenCommercialInvoiceIsFalse_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = false;
            nativeRequest.RequestedShipment.CustomsClearanceDetail = null;

            testObject.Manipulate(carrierRequest.Object);

            // The customs detail should not have been initialized
            Assert.Null(nativeRequest.RequestedShipment.CustomsClearanceDetail);
        }

        [Fact]
        public void Manipulate_CommercialInvoiceIsNotNull_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = true;
            shipmentEntity.FedEx.CommercialInvoiceTermsOfSale = (int)FedExTermsOfSale.FOB_or_FCA;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.NotNull(customsDetail.CommercialInvoice);
        }

        [Fact]
        public void Manipulate_TermsOfSaleIsFca_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = true;
            shipmentEntity.FedEx.CommercialInvoiceTermsOfSale = (int)FedExTermsOfSale.FOB_or_FCA;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal("FCA", customsDetail.CommercialInvoice.TermsOfSale);
        }

        [Fact]
        public void Manipulate_TermsOfSaleIsCptOrCf_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = true;
            shipmentEntity.FedEx.CommercialInvoiceTermsOfSale = (int)FedExTermsOfSale.CFR_or_CPT;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal("CPT/C&F", customsDetail.CommercialInvoice.TermsOfSale);
        }

        [Fact]
        public void Manipulate_TermsOfSaleIsCipOrCif_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = true;
            shipmentEntity.FedEx.CommercialInvoiceTermsOfSale = (int)FedExTermsOfSale.CIF_or_CIP;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal("CIP/CIF", customsDetail.CommercialInvoice.TermsOfSale);
        }

        [Fact]
        public void Manipulate_TermsOfSaleIsEXW_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = true;
            shipmentEntity.FedEx.CommercialInvoiceTermsOfSale = (int)FedExTermsOfSale.EXW;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal("EXW", customsDetail.CommercialInvoice.TermsOfSale);
        }

        [Fact]
        public void Manipulate_TermsOfSaleIsDDP_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = true;
            shipmentEntity.FedEx.CommercialInvoiceTermsOfSale = (int)FedExTermsOfSale.DDP;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal("DDP", customsDetail.CommercialInvoice.TermsOfSale);
        }

        [Fact]
        public void Manipulate_TermsOfSaleIsDDU_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = true;
            shipmentEntity.FedEx.CommercialInvoiceTermsOfSale = (int)FedExTermsOfSale.DDU;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal("DDU", customsDetail.CommercialInvoice.TermsOfSale);
        }

        [Fact]
        public void Manipulate_ThrowsInvalidOperationException_WhenTermsOfSaleIsNotRecognized_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = true;
            shipmentEntity.FedEx.CommercialInvoiceTermsOfSale = 112;

            Assert.Throws<InvalidOperationException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_PurposeIsSold_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = true;
            shipmentEntity.FedEx.CommercialInvoicePurpose = (int)FedExCommercialInvoicePurpose.Sold;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal(PurposeOfShipmentType.SOLD, customsDetail.CommercialInvoice.Purpose);
        }

        [Fact]
        public void Manipulate_PurposeIsNotSold_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = true;
            shipmentEntity.FedEx.CommercialInvoicePurpose = (int)FedExCommercialInvoicePurpose.NotSold;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal(PurposeOfShipmentType.NOT_SOLD, customsDetail.CommercialInvoice.Purpose);
        }

        [Fact]
        public void Manipulate_PurposeIsGift_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = true;
            shipmentEntity.FedEx.CommercialInvoicePurpose = (int)FedExCommercialInvoicePurpose.Gift;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal(PurposeOfShipmentType.GIFT, customsDetail.CommercialInvoice.Purpose);
        }

        [Fact]
        public void Manipulate_PurposeIsSample_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = true;
            shipmentEntity.FedEx.CommercialInvoicePurpose = (int)FedExCommercialInvoicePurpose.Sample;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal(PurposeOfShipmentType.SAMPLE, customsDetail.CommercialInvoice.Purpose);
        }

        [Fact]
        public void Manipulate_PurposeIsPersonalEffects_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = true;
            shipmentEntity.FedEx.CommercialInvoicePurpose = (int)FedExCommercialInvoicePurpose.Personal;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal(PurposeOfShipmentType.PERSONAL_EFFECTS, customsDetail.CommercialInvoice.Purpose);
        }

        [Fact]
        public void Manipulate_PurposeIsRepairAndReturn_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = true;
            shipmentEntity.FedEx.CommercialInvoicePurpose = (int)FedExCommercialInvoicePurpose.Repair;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal(PurposeOfShipmentType.REPAIR_AND_RETURN, customsDetail.CommercialInvoice.Purpose);
        }

        [Fact]
        public void Manipulate_ThrowsInvalidOperationException_WhenPurposeIsNotRecognized_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = true;
            shipmentEntity.FedEx.CommercialInvoicePurpose = 54;

            Assert.Throws<InvalidOperationException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_PurposeSpecifiedIsTrue_WhenCommercialInvoiceIsTrue_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = true;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.True(customsDetail.CommercialInvoice.PurposeSpecified);
        }


        [Fact]
        public void Manipulate_InvoiceCommentsHasOneElement_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = true;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal(1, customsDetail.CommercialInvoice.Comments.Length);
        }

        [Fact]
        public void Manipulate_InvoiceComments_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = true;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal(shipmentEntity.FedEx.CommercialInvoiceComments, customsDetail.CommercialInvoice.Comments[0]);
        }

        [Fact]
        public void Manipulate_FreightChargeCurrencyIsUSD_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = true;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal("USD", customsDetail.CommercialInvoice.FreightCharge.Currency);
        }

        [Fact]
        public void Manipulate_FreightChargeAmount_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = true;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal(shipmentEntity.FedEx.CommercialInvoiceFreight, customsDetail.CommercialInvoice.FreightCharge.Amount);
        }

        [Fact]
        public void Manipulate_TaxCurrencyIsUSD_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = true;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal("USD", customsDetail.CommercialInvoice.TaxesOrMiscellaneousCharge.Currency);
        }

        [Fact]
        public void Manipulate_TaxAmount_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = true;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal(shipmentEntity.FedEx.CommercialInvoiceOther, customsDetail.CommercialInvoice.TaxesOrMiscellaneousCharge.Amount);
        }

        [Fact]
        public void Manipulate_InvoiceCustomerReferenceIsNull_WhenReferenceIsEmptyString_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = true;
            shipmentEntity.FedEx.CommercialInvoiceReference = string.Empty;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Null(customsDetail.CommercialInvoice.CustomerReferences);
        }

        [Fact]
        public void Manipulate_InvoiceCustomerReferenceIsNull_WhenReferenceIsNull_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = true;
            shipmentEntity.FedEx.CommercialInvoiceReference = null;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Null(customsDetail.CommercialInvoice.CustomerReferences);
        }

        [Fact]
        public void Manipulate_InvoiceCustomerReferenceIsNotNull_WhenReferenceIsProvided_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = true;
            shipmentEntity.FedEx.CommercialInvoiceReference = "reference";

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.NotNull(customsDetail.CommercialInvoice.CustomerReferences);
        }

        [Fact]
        public void Manipulate_InvoiceCustomerReferenceSizeIsOne_WhenReferenceIsProvided_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = true;
            shipmentEntity.FedEx.CommercialInvoiceReference = "reference";

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal(1, customsDetail.CommercialInvoice.CustomerReferences.Length);
        }

        [Fact]
        public void Manipulate_InvoiceCustomerReferenceIsCustomerReferenceType_WhenReferenceIsProvided_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = true;
            shipmentEntity.FedEx.CommercialInvoiceReference = "reference";

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal(CustomerReferenceType.CUSTOMER_REFERENCE, customsDetail.CommercialInvoice.CustomerReferences[0].CustomerReferenceType);
        }

        [Fact]
        public void Manipulate_InvoiceCustomerReferenceValue_WhenReferenceIsProvided_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = true;
            shipmentEntity.FedEx.CommercialInvoiceReference = "reference";

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal("reference", customsDetail.CommercialInvoice.CustomerReferences[0].Value);
        }

        [Fact]
        public void Manipulate_ImporterIsNotNull_WhenCommercialInvoiceAndImporterRecordAreTrue_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = true;
            shipmentEntity.FedEx.ImporterOfRecord = true;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.NotNull(customsDetail.ImporterOfRecord);
        }

        [Fact]
        public void Manipulate_ImporterAddressIsNotNull_WhenCommercialInvoiceAndImporterRecordAreTrue_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = true;
            shipmentEntity.FedEx.ImporterOfRecord = true;

            testObject.Manipulate(carrierRequest.Object);

            // Just need to check for null since adding the address is deferred to another object
            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.NotNull(customsDetail.ImporterOfRecord.Address);
        }

        [Fact]
        public void Manipulate_ImporterContactIsNotNull_WhenCommercialInvoiceAndImporterRecordAreTrue_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = true;
            shipmentEntity.FedEx.ImporterOfRecord = true;

            testObject.Manipulate(carrierRequest.Object);

            // Just need to check for null since adding the address is deferred to another object
            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.NotNull(customsDetail.ImporterOfRecord.Contact);
        }

        [Fact]
        public void Manipulate_ImporterAccountNumber_WhenCommercialInvoiceAndImporterRecordAreTrue_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = true;
            shipmentEntity.FedEx.ImporterOfRecord = true;

            testObject.Manipulate(carrierRequest.Object);

            // Just need to check for null since adding the address is deferred to another object
            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal(shipmentEntity.FedEx.ImporterAccount, customsDetail.ImporterOfRecord.AccountNumber);
        }

        [Fact]
        public void Manipulate_ImporterTINsIsNotNull_WhenCommercialInvoiceAndImporterRecordAreTrue_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = true;
            shipmentEntity.FedEx.ImporterOfRecord = true;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.NotNull(customsDetail.ImporterOfRecord.Tins);
        }

        [Fact]
        public void Manipulate_ImporterTINsSizeisOne_WhenCommercialInvoiceAndImporterRecordAreTrue_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = true;
            shipmentEntity.FedEx.ImporterOfRecord = true;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal(1, customsDetail.ImporterOfRecord.Tins.Length);
        }

        [Fact]
        public void Manipulate_ImporterTINNumber_WhenCommercialInvoiceAndImporterRecordAreTrue_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = true;
            shipmentEntity.FedEx.ImporterOfRecord = true;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal(shipmentEntity.FedEx.ImporterTIN, customsDetail.ImporterOfRecord.Tins[0].Number);
        }

        [Fact]
        public void Manipulate_InsuranceChargeIsNotNull_WhenCommercialInvoiceIsTrue_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = true;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.NotNull(customsDetail.InsuranceCharges);
        }

        [Fact]
        public void Manipulate_InsuranceChargeCurrencyIsUSD_WhenCommercialInvoiceIsTrue_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = true;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal("USD", customsDetail.InsuranceCharges.Currency);
        }

        [Fact]
        public void Manipulate_InsuranceChargeAmount_WhenCommercialInvoiceIsTrue_Test()
        {
            shipmentEntity.FedEx.CommercialInvoice = true;

            testObject.Manipulate(carrierRequest.Object);

            CustomsClearanceDetail customsDetail = nativeRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal(shipmentEntity.FedEx.CommercialInvoiceInsurance, customsDetail.InsuranceCharges.Amount);
        }


    }
}
