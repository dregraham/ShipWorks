using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Enums;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping;

namespace ShipWorks.Tests.Integration.MSTest.Fixtures.Shipping.Carriers.FedEx
{
    public class FedExUSExpressInternationalFixture : FedExInternationalPrototypeFixture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FedExUSExpressInternationalFixture" /> class.
        /// </summary>
        public FedExUSExpressInternationalFixture()
            : base()
        {
            
        }

        // Need to check where to assign these three values
        public string ShipperTinType { get; set; }
        public string ShipperTinsNumber { get; set; }
        public string PackageLineItemDimensionUnits { get; set; }

        public string InternationalControlledExportDetailType { get; set; }
        public string InternationalControlledExportDetailForeignTradeZoneCode { get; set; }
        public string InternationalControlledExportDetailEntryNumber { get; set; }
        public string InternationalControlledExportDetailLicenseOrPermitNumber { get; set; }
        public string InternationalControlledExportDetailLicenseOrPermitExpirationDate { get; set; }

        public string TrafficInArmsLicenseOrExemptionNumber { get; set; }


        public string BrokerType { get; set; }
        public string BrokerAccountNumber { get; set; }
        public string BrokerTinType { get; set; }
        public string BrokerTinsNumber { get; set; }
        public string BrokerContactId { get; set; }
        public string BrokerPersonName { get; set; }
        public string BrokerTitle { get; set; }
        public string BrokerCompanyName { get; set; }
        public string BrokerPhoneNumber { get; set; }
        public string BrokerPhoneExtension { get; set; }
        public string BrokerEMailAddress { get; set; }
        public string BrokerStreetLines { get; set; }
        public string BrokerCity { get; set; }
        public string BrokerStateOrProvinceCode { get; set; }
        public string BrokerPostalCode { get; set; }
        public string BrokerCountryCode { get; set; }

        public string CustomsOptionType { get; set; }
        public string CustomsOptionDescription { get; set; }

        public string CustomsRecipientIdType { get; set; }
        public string CustomsRecipientIdValue { get; set; }

        public string CustomsClearanceInsuranceCurrency { get; set; }
        public string CustomsClearanceInsuranceAmount { get; set; }

        public string CommercialInvoiceComments { get; set; }
        public string CommercialInvoiceFreightChargeCurrency { get; set; }
        public string CommercialInvoiceFreightChargeAmount { get; set; }
        public string CommercialInvoiceTaxesCurrency { get; set; }
        public string CommercialInvoiceTaxesAmount { get; set; }
        public string CommercialInvoicePurpose { get; set; }
        public string CommercialInvoiceCustomerReferenceType { get; set; }
        public string CommercialInvoiceCustomerReferenceValue { get; set; }
        public string CommercialInvoiceTermsOfSale { get; set; }

        public string ExportDetailB13AFilingOption { get; set; }
        public string ExportDetailExportComplianceStatement { get; set; }


        
        /// <summary>
        /// Creates the shipment.
        /// </summary>
        /// <returns></returns>
        public override ShipmentEntity CreateShipment()
        {
            ShipmentEntity shipment = base.CreateShipment();

            shipment.FedEx.TrafficInArmsLicenseNumber = TrafficInArmsLicenseOrExemptionNumber;
            shipment.FedEx.WeightUnitType = ShipmentWeightUnits.ToLower() == "lb" ? (int) WeightUnitOfMeasure.Pounds : (int) WeightUnitOfMeasure.Kilograms;


            SetupCustomsData(shipment);

            SetupCommercialInvoice(shipment);
            SetupBroker(shipment);
            SetupExportDetail(shipment);

            SetupRecipientIdentification(shipment);

            return shipment;
        }

        private void SetupExportDetail(ShipmentEntity shipment)
        {
            if ((!string.IsNullOrEmpty(SpecialServiceType1) && SpecialServiceType1.ToLower() == "international_controlled_export_service") ||
                (!string.IsNullOrEmpty(SpecialServiceType2) && SpecialServiceType2.ToLower() == "international_controlled_export_service"))
            {
                // Hard code as this is the only export type in the tests
                shipment.FedEx.IntlExportDetailType = (int) FedExInternationalControlledExportType.Dea236;
                shipment.FedEx.IntlExportDetailEntryNumber = InternationalControlledExportDetailEntryNumber;
                shipment.FedEx.IntlExportDetailForeignTradeZoneCode = InternationalControlledExportDetailForeignTradeZoneCode;
                shipment.FedEx.IntlExportDetailLicenseOrPermitExpirationDate = DateTime.Parse(InternationalControlledExportDetailLicenseOrPermitExpirationDate);
                shipment.FedEx.IntlExportDetailLicenseOrPermitNumber = InternationalControlledExportDetailLicenseOrPermitNumber;                
            }
        }

        /// <summary>
        /// Setups the broker.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        private void SetupBroker(ShipmentEntity shipment)
        {
            if (!string.IsNullOrEmpty(BrokerAccountNumber))
            {
                // Only the test cases specifying a broker account have broker account info
                shipment.FedEx.BrokerEnabled = true;

                shipment.FedEx.BrokerAccount = BrokerAccountNumber;
                shipment.FedEx.BrokerCity = BrokerCity;
                shipment.FedEx.BrokerCompany = BrokerCompanyName;
                shipment.FedEx.BrokerCountryCode = BrokerCountryCode;
                shipment.FedEx.BrokerEmail = BrokerEMailAddress;

                List<string> names = ParseName(BrokerPersonName);
                shipment.FedEx.BrokerFirstName = names[0];
                shipment.FedEx.BrokerLastName = names[1];
                
                shipment.FedEx.BrokerPostalCode = BrokerPostalCode;
                shipment.FedEx.BrokerStateProvCode = BrokerStateOrProvinceCode;
                shipment.FedEx.BrokerStreet1 = BrokerStreetLines;

                shipment.FedEx.BrokerPhone = BrokerPhoneNumber;
                shipment.FedEx.BrokerPhoneExtension = BrokerPhoneExtension;

                // Don't need to worry about broker type since all test cases specify IMPORT (this is hard coded in the manipulator)
            }

        }

        
        /// <summary>
        /// Setups the recipient identification.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        protected virtual void SetupRecipientIdentification(ShipmentEntity shipment)
        {
            // All test cases have passport as the value
            shipment.FedEx.CustomsRecipientIdentificationType = (int) FedExCustomsRecipientIdentificationType.Passport;
            shipment.FedEx.CustomsRecipientIdentificationValue = CustomsRecipientIdValue;
        }

        /// <summary>
        /// Setups the customs data.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        private void SetupCustomsData(ShipmentEntity shipment)
        {
            // Set the filing options
            if (!string.IsNullOrEmpty(ExportDetailB13AFilingOption))
            {
                switch (ExportDetailB13AFilingOption.ToLower())
                {
                    case "not_required":
                        shipment.FedEx.CustomsExportFilingOption = (int) FedExCustomsExportFilingOption.NotRequired;
                        break;
                    case "manually_attached":
                        shipment.FedEx.CustomsExportFilingOption = (int) FedExCustomsExportFilingOption.ManuallyAttached;
                        break;
                    case "filed_electronically":
                        shipment.FedEx.CustomsExportFilingOption = (int) FedExCustomsExportFilingOption.FiledElectonically;
                        break;
                    case "summary_reporting":
                        shipment.FedEx.CustomsExportFilingOption = (int) FedExCustomsExportFilingOption.SummaryReporting;
                        break;
                }
            }

            shipment.FedEx.CustomsAESEEI = ExportDetailExportComplianceStatement;

           
            
            // TODO: setup the rest of the customs fields
            // CustomsClearanceInsuranceAmount

            if (!string.IsNullOrEmpty(CustomsOptionType))
            {
                // The only option type is Faulty item so just hard code the type here
                shipment.FedEx.CustomsOptionsType = (int) FedExCustomsOptionType.FaultyItem;
                shipment.FedEx.CustomsOptionsDesription = CustomsOptionDescription;
            }
            else
            {
                shipment.FedEx.CustomsOptionsType = (int) FedExCustomsOptionType.None;
            }
        }



        /// <summary>
        /// Sets the linear units.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        protected override void SetLinearUnits(ShipmentEntity shipment)
        {
            base.SetLinearUnits(shipment);

            if (!string.IsNullOrEmpty(PackageLineItemDimensionUnits))
            {
                if (PackageLineItemDimensionUnits.ToLower() == "cm")
                {
                    shipment.FedEx.LinearUnitType = (int)FedExLinearUnitOfMeasure.CM;
                }
                else
                {
                    shipment.FedEx.LinearUnitType = (int)FedExLinearUnitOfMeasure.IN;
                }
            }
        }

        /// <summary>
        /// Setups the commercial invoice.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        protected virtual void SetupCommercialInvoice(ShipmentEntity shipment)
        {
            // All test cases currently have commercial invoices
            shipment.FedEx.CommercialInvoice = true;

            shipment.FedEx.CommercialInvoiceComments = CommercialInvoiceComments;

            if (!string.IsNullOrEmpty(CommercialInvoiceFreightChargeAmount))
            {
                shipment.FedEx.CommercialInvoiceFreight = decimal.Parse(CommercialInvoiceFreightChargeAmount);
            }

            if (!string.IsNullOrEmpty(CommercialInvoiceTaxesAmount))
            {
                shipment.FedEx.CommercialInvoiceOther = decimal.Parse(CommercialInvoiceTaxesAmount);
            }

            // All test cases have sample as the purpose
            shipment.FedEx.CommercialInvoicePurpose = (int) FedExCommercialInvoicePurpose.Sample;
            shipment.FedEx.CommercialInvoiceReference = CommercialInvoiceCustomerReferenceValue;

            // Only non-empty item specifies terms of sale as EXW
            FedExTermsOfSale termsOfSale;
            if (FedExTermsOfSale.TryParse(CommercialInvoiceTermsOfSale, true, out termsOfSale))
            {
                shipment.FedEx.CommercialInvoiceTermsOfSale = (int) termsOfSale;
            }
            else
            {
                shipment.FedEx.CommercialInvoiceTermsOfSale = (int) FedExTermsOfSale.EXW;
            }

            if (!string.IsNullOrWhiteSpace(CustomsClearanceInsuranceAmount))
            {
                shipment.FedEx.CommercialInvoiceInsurance = decimal.Parse(CustomsClearanceInsuranceAmount);
            }
        }
    }
}
