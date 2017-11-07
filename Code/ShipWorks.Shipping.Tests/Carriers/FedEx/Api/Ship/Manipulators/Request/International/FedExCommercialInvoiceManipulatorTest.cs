using System;
using Autofac.Extras.Moq;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shared;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Ship.Manipulators.Request.International
{
    public class FedExCommercialInvoiceManipulatorTest
    {
        private readonly FedExAccountEntity fedExAccount;
        private readonly ProcessShipmentRequest processShipmentRequest;
        private readonly ShipmentEntity shipment;
        private readonly FedExCommercialInvoiceManipulator testObject;
        private readonly AutoMock mock;

        public FedExCommercialInvoiceManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            
            shipment = new ShipmentEntity
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

            processShipmentRequest = new ProcessShipmentRequest()
            {
                RequestedShipment = new RequestedShipment()
                {
                    CustomsClearanceDetail = new CustomsClearanceDetail()
                }
            };

            fedExAccount = new FedExAccountEntity { AccountNumber = "123", CountryCode = "US", LastName = "Doe", FirstName = "John" };
            mock.Mock<IFedExSettingsRepository>()
                .Setup(x => x.GetAccountReadOnly(AnyIShipment))
                .Returns(fedExAccount);

            testObject = mock.Create<FedExCommercialInvoiceManipulator>();
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

        [Fact]
        public void Manipulate_AccountsForNullRequestedShipment()
        {
            // setup the test by setting the requested shipment to null
            processShipmentRequest.RequestedShipment = null;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.NotNull(processShipmentRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_AccountsForNullSpecialServices()
        {
            // setup the test by setting the requested shipment to null
            processShipmentRequest.RequestedShipment.SpecialServicesRequested = null;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.NotNull(processShipmentRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_CustomClearanceDetailsIsNotNull_WhenCommercialInvoiceIsTrue()
        {
            shipment.FedEx.CommercialInvoice = true;
            processShipmentRequest.RequestedShipment.CustomsClearanceDetail = null;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Make sure that the customs detail gets added back to the request
            Assert.NotNull(processShipmentRequest.RequestedShipment.CustomsClearanceDetail);
        }

        [Fact]
        public void Manipulate_CustomClearanceDetailsIsNull_WhenCommercialInvoiceIsFalse()
        {
            shipment.FedEx.CommercialInvoice = false;
            processShipmentRequest.RequestedShipment.CustomsClearanceDetail = null;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // The customs detail should not have been initialized
            Assert.Null(processShipmentRequest.RequestedShipment.CustomsClearanceDetail);
        }

        [Fact]
        public void Manipulate_CommercialInvoiceIsNotNull()
        {
            shipment.FedEx.CommercialInvoice = true;
            shipment.FedEx.CommercialInvoiceTermsOfSale = (int)FedExTermsOfSale.FOB_or_FCA;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.NotNull(customsDetail.CommercialInvoice);
        }

        [Fact]
        public void Manipulate_TermsOfSaleIsFca()
        {
            shipment.FedEx.CommercialInvoice = true;
            shipment.FedEx.CommercialInvoiceTermsOfSale = (int)FedExTermsOfSale.FOB_or_FCA;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal("FCA", customsDetail.CommercialInvoice.TermsOfSale);
        }

        [Fact]
        public void Manipulate_TermsOfSaleIsCptOrCf()
        {
            shipment.FedEx.CommercialInvoice = true;
            shipment.FedEx.CommercialInvoiceTermsOfSale = (int)FedExTermsOfSale.CFR_or_CPT;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal("CPT/C&F", customsDetail.CommercialInvoice.TermsOfSale);
        }

        [Fact]
        public void Manipulate_TermsOfSaleIsCipOrCif()
        {
            shipment.FedEx.CommercialInvoice = true;
            shipment.FedEx.CommercialInvoiceTermsOfSale = (int)FedExTermsOfSale.CIF_or_CIP;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal("CIP/CIF", customsDetail.CommercialInvoice.TermsOfSale);
        }

        [Fact]
        public void Manipulate_TermsOfSaleIsEXW()
        {
            shipment.FedEx.CommercialInvoice = true;
            shipment.FedEx.CommercialInvoiceTermsOfSale = (int)FedExTermsOfSale.EXW;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal("EXW", customsDetail.CommercialInvoice.TermsOfSale);
        }

        [Fact]
        public void Manipulate_TermsOfSaleIsDDP()
        {
            shipment.FedEx.CommercialInvoice = true;
            shipment.FedEx.CommercialInvoiceTermsOfSale = (int)FedExTermsOfSale.DDP;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal("DDP", customsDetail.CommercialInvoice.TermsOfSale);
        }

        [Fact]
        public void Manipulate_TermsOfSaleIsDDU()
        {
            shipment.FedEx.CommercialInvoice = true;
            shipment.FedEx.CommercialInvoiceTermsOfSale = (int)FedExTermsOfSale.DDU;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal("DDU", customsDetail.CommercialInvoice.TermsOfSale);
        }

        [Fact]
        public void Manipulate_ThrowsInvalidOperationException_WhenTermsOfSaleIsNotRecognized()
        {
            shipment.FedEx.CommercialInvoice = true;
            shipment.FedEx.CommercialInvoiceTermsOfSale = 112;

            Assert.Throws<InvalidOperationException>(() => testObject.Manipulate(shipment, processShipmentRequest, 0));
        }

        [Fact]
        public void Manipulate_PurposeIsSold()
        {
            shipment.FedEx.CommercialInvoice = true;
            shipment.FedEx.CommercialInvoicePurpose = (int)FedExCommercialInvoicePurpose.Sold;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal(PurposeOfShipmentType.SOLD, customsDetail.CommercialInvoice.Purpose);
        }

        [Fact]
        public void Manipulate_PurposeIsNotSold()
        {
            shipment.FedEx.CommercialInvoice = true;
            shipment.FedEx.CommercialInvoicePurpose = (int)FedExCommercialInvoicePurpose.NotSold;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal(PurposeOfShipmentType.NOT_SOLD, customsDetail.CommercialInvoice.Purpose);
        }

        [Fact]
        public void Manipulate_PurposeIsGift()
        {
            shipment.FedEx.CommercialInvoice = true;
            shipment.FedEx.CommercialInvoicePurpose = (int)FedExCommercialInvoicePurpose.Gift;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal(PurposeOfShipmentType.GIFT, customsDetail.CommercialInvoice.Purpose);
        }

        [Fact]
        public void Manipulate_PurposeIsSample()
        {
            shipment.FedEx.CommercialInvoice = true;
            shipment.FedEx.CommercialInvoicePurpose = (int)FedExCommercialInvoicePurpose.Sample;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal(PurposeOfShipmentType.SAMPLE, customsDetail.CommercialInvoice.Purpose);
        }

        [Fact]
        public void Manipulate_PurposeIsPersonalEffects()
        {
            shipment.FedEx.CommercialInvoice = true;
            shipment.FedEx.CommercialInvoicePurpose = (int)FedExCommercialInvoicePurpose.Personal;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal(PurposeOfShipmentType.PERSONAL_EFFECTS, customsDetail.CommercialInvoice.Purpose);
        }

        [Fact]
        public void Manipulate_PurposeIsRepairAndReturn()
        {
            shipment.FedEx.CommercialInvoice = true;
            shipment.FedEx.CommercialInvoicePurpose = (int)FedExCommercialInvoicePurpose.Repair;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal(PurposeOfShipmentType.REPAIR_AND_RETURN, customsDetail.CommercialInvoice.Purpose);
        }

        [Fact]
        public void Manipulate_ThrowsInvalidOperationException_WhenPurposeIsNotRecognized()
        {
            shipment.FedEx.CommercialInvoice = true;
            shipment.FedEx.CommercialInvoicePurpose = 54;

            Assert.Throws<InvalidOperationException>(() => testObject.Manipulate(shipment, processShipmentRequest, 0));
        }

        [Fact]
        public void Manipulate_PurposeSpecifiedIsTrue_WhenCommercialInvoiceIsTrue()
        {
            shipment.FedEx.CommercialInvoice = true;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.True(customsDetail.CommercialInvoice.PurposeSpecified);
        }


        [Fact]
        public void Manipulate_InvoiceCommentsHasOneElement()
        {
            shipment.FedEx.CommercialInvoice = true;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal(1, customsDetail.CommercialInvoice.Comments.Length);
        }

        [Fact]
        public void Manipulate_InvoiceComments()
        {
            shipment.FedEx.CommercialInvoice = true;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal(shipment.FedEx.CommercialInvoiceComments, customsDetail.CommercialInvoice.Comments[0]);
        }

        [Fact]
        public void Manipulate_FreightChargeCurrencyIsUSD()
        {
            shipment.FedEx.CommercialInvoice = true;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal("USD", customsDetail.CommercialInvoice.FreightCharge.Currency);
        }

        [Fact]
        public void Manipulate_FreightChargeAmount()
        {
            shipment.FedEx.CommercialInvoice = true;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal(shipment.FedEx.CommercialInvoiceFreight, customsDetail.CommercialInvoice.FreightCharge.Amount);
        }

        [Fact]
        public void Manipulate_TaxCurrencyIsUSD()
        {
            shipment.FedEx.CommercialInvoice = true;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal("USD", customsDetail.CommercialInvoice.TaxesOrMiscellaneousCharge.Currency);
        }

        [Fact]
        public void Manipulate_TaxAmount()
        {
            shipment.FedEx.CommercialInvoice = true;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal(shipment.FedEx.CommercialInvoiceOther, customsDetail.CommercialInvoice.TaxesOrMiscellaneousCharge.Amount);
        }

        [Fact]
        public void Manipulate_InvoiceCustomerReferenceIsNull_WhenReferenceIsEmptyString()
        {
            shipment.FedEx.CommercialInvoice = true;
            shipment.FedEx.CommercialInvoiceReference = string.Empty;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Null(customsDetail.CommercialInvoice.CustomerReferences);
        }

        [Fact]
        public void Manipulate_InvoiceCustomerReferenceIsNull_WhenReferenceIsNull()
        {
            shipment.FedEx.CommercialInvoice = true;
            shipment.FedEx.CommercialInvoiceReference = null;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Null(customsDetail.CommercialInvoice.CustomerReferences);
        }

        [Fact]
        public void Manipulate_InvoiceCustomerReferenceIsNotNull_WhenReferenceIsProvided()
        {
            shipment.FedEx.CommercialInvoice = true;
            shipment.FedEx.CommercialInvoiceReference = "reference";

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.NotNull(customsDetail.CommercialInvoice.CustomerReferences);
        }

        [Fact]
        public void Manipulate_InvoiceCustomerReferenceSizeIsOne_WhenReferenceIsProvided()
        {
            shipment.FedEx.CommercialInvoice = true;
            shipment.FedEx.CommercialInvoiceReference = "reference";

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal(1, customsDetail.CommercialInvoice.CustomerReferences.Length);
        }

        [Fact]
        public void Manipulate_InvoiceCustomerReferenceIsCustomerReferenceType_WhenReferenceIsProvided()
        {
            shipment.FedEx.CommercialInvoice = true;
            shipment.FedEx.CommercialInvoiceReference = "reference";

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal(CustomerReferenceType.CUSTOMER_REFERENCE, customsDetail.CommercialInvoice.CustomerReferences[0].CustomerReferenceType);
        }

        [Fact]
        public void Manipulate_InvoiceCustomerReferenceValue_WhenReferenceIsProvided()
        {
            shipment.FedEx.CommercialInvoice = true;
            shipment.FedEx.CommercialInvoiceReference = "reference";

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal("reference", customsDetail.CommercialInvoice.CustomerReferences[0].Value);
        }

        [Fact]
        public void Manipulate_ImporterIsNotNull_WhenCommercialInvoiceAndImporterRecordAreTrue()
        {
            shipment.FedEx.CommercialInvoice = true;
            shipment.FedEx.ImporterOfRecord = true;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.NotNull(customsDetail.ImporterOfRecord);
        }

        [Fact]
        public void Manipulate_ImporterAddressIsNotNull_WhenCommercialInvoiceAndImporterRecordAreTrue()
        {
            shipment.FedEx.CommercialInvoice = true;
            shipment.FedEx.ImporterOfRecord = true;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Just need to check for null since adding the address is deferred to another object
            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.NotNull(customsDetail.ImporterOfRecord.Address);
        }

        [Fact]
        public void Manipulate_ImporterContactIsNotNull_WhenCommercialInvoiceAndImporterRecordAreTrue()
        {
            shipment.FedEx.CommercialInvoice = true;
            shipment.FedEx.ImporterOfRecord = true;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Just need to check for null since adding the address is deferred to another object
            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.NotNull(customsDetail.ImporterOfRecord.Contact);
        }

        [Fact]
        public void Manipulate_ImporterAccountNumber_WhenCommercialInvoiceAndImporterRecordAreTrue()
        {
            shipment.FedEx.CommercialInvoice = true;
            shipment.FedEx.ImporterOfRecord = true;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Just need to check for null since adding the address is deferred to another object
            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal(shipment.FedEx.ImporterAccount, customsDetail.ImporterOfRecord.AccountNumber);
        }

        [Fact]
        public void Manipulate_ImporterTINsIsNotNull_WhenCommercialInvoiceAndImporterRecordAreTrue()
        {
            shipment.FedEx.CommercialInvoice = true;
            shipment.FedEx.ImporterOfRecord = true;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.NotNull(customsDetail.ImporterOfRecord.Tins);
        }

        [Fact]
        public void Manipulate_ImporterTINsSizeisOne_WhenCommercialInvoiceAndImporterRecordAreTrue()
        {
            shipment.FedEx.CommercialInvoice = true;
            shipment.FedEx.ImporterOfRecord = true;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal(1, customsDetail.ImporterOfRecord.Tins.Length);
        }

        [Fact]
        public void Manipulate_ImporterTINNumber_WhenCommercialInvoiceAndImporterRecordAreTrue()
        {
            shipment.FedEx.CommercialInvoice = true;
            shipment.FedEx.ImporterOfRecord = true;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal(shipment.FedEx.ImporterTIN, customsDetail.ImporterOfRecord.Tins[0].Number);
        }

        [Fact]
        public void Manipulate_InsuranceChargeIsNotNull_WhenCommercialInvoiceIsTrue()
        {
            shipment.FedEx.CommercialInvoice = true;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.NotNull(customsDetail.InsuranceCharges);
        }

        [Fact]
        public void Manipulate_InsuranceChargeCurrencyIsUSD_WhenCommercialInvoiceIsTrue()
        {
            shipment.FedEx.CommercialInvoice = true;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal("USD", customsDetail.InsuranceCharges.Currency);
        }

        [Fact]
        public void Manipulate_InsuranceChargeAmount_WhenCommercialInvoiceIsTrue()
        {
            shipment.FedEx.CommercialInvoice = true;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            CustomsClearanceDetail customsDetail = processShipmentRequest.RequestedShipment.CustomsClearanceDetail;
            Assert.Equal(shipment.FedEx.CommercialInvoiceInsurance, customsDetail.InsuranceCharges.Amount);
        }


    }
}
