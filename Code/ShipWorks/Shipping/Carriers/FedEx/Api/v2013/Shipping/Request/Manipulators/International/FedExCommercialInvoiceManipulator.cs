using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Ship;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using Interapptive.Shared.Business;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Shipping.Request.Manipulators.International
{
    /// <summary>
    /// An implementation of the ICarrierRequestManipulator interface that builds up the commercial
    /// invoice portion of the customs detail in the FedEx ProcessShipmentRequest object.
    /// </summary>
    public class FedExCommercialInvoiceManipulator : FedExShippingRequestManipulatorBase
    {
        private string shipmentCurrencyType;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExCommercialInvoiceManipulator" /> class.
        /// </summary>
        public FedExCommercialInvoiceManipulator()
            : this(new FedExSettings(new FedExSettingsRepository()))
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExCommercialInvoiceManipulator" /> class.
        /// </summary>
        /// <param name="fedExSettings">The fed ex settings.</param>
        public FedExCommercialInvoiceManipulator(FedExSettings fedExSettings)
            : base(fedExSettings)
        {
        }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public override void Manipulate(CarrierRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(request);

            // We can safely cast this since we've passed initialization
            ProcessShipmentRequest nativeRequest = request.NativeRequest as ProcessShipmentRequest;
            FedExShipmentEntity fedExShipment = request.ShipmentEntity.FedEx;

            shipmentCurrencyType = GetShipmentCurrencyType(fedExShipment.Shipment);

            if (fedExShipment.CommercialInvoice)
            {
                CustomsClearanceDetail customsDetail = GetCustomsDetail(nativeRequest);

                ConfigureInvoice(fedExShipment, customsDetail);
                ConfigureImporter(fedExShipment, customsDetail);

                customsDetail.InsuranceCharges = new Money
                    {
                        Amount = fedExShipment.CommercialInvoiceInsurance,
                        Currency = shipmentCurrencyType 
                    };
                nativeRequest.RequestedShipment.CustomsClearanceDetail = customsDetail;
            }

        }

        /// <summary>
        /// Configures the importer of the customs detail based on the shipment data.
        /// </summary>
        /// <param name="fedExShipment">The FedEx shipment.</param>
        /// <param name="customsDetail">The customs detail.</param>
        private void ConfigureImporter(FedExShipmentEntity fedExShipment, CustomsClearanceDetail customsDetail)
        {
            if (fedExShipment.ImporterOfRecord)
            {
                Party importer = new Party();
                PersonAdapter importerPerson = new PersonAdapter(fedExShipment, "Importer");

                importer.Address = FedExApiCore.CreateAddress<Address>(importerPerson);
                importer.Contact = FedExApiCore.CreateContact<Contact>(importerPerson);
                importer.AccountNumber = fedExShipment.ImporterAccount;
                importer.Tins = new TaxpayerIdentification[] { new TaxpayerIdentification { Number = fedExShipment.ImporterTIN } };

                customsDetail.ImporterOfRecord = importer;
            }
        }

        /// <summary>
        /// Configures the commercial invoice of the customs detail based on the shipment data.
        /// </summary>
        /// <param name="fedExShipment">The FedEx shipment.</param>
        /// <param name="customsDetail">The customs detail.</param>
        private void ConfigureInvoice(FedExShipmentEntity fedExShipment, CustomsClearanceDetail customsDetail)
        {
            CommercialInvoice invoice = new CommercialInvoice();
            invoice.TermsOfSale = GetApiTermsOfSale((FedExTermsOfSale) fedExShipment.CommercialInvoiceTermsOfSale);
            invoice.TermsOfSaleSpecified = true;

            invoice.Purpose = GetApiCommercialInvoicePurpose((FedExCommercialInvoicePurpose) fedExShipment.CommercialInvoicePurpose);
            invoice.PurposeSpecified = true;
            
            invoice.Comments = new string[] {fedExShipment.CommercialInvoiceComments};

            invoice.FreightCharge = new Money
                {
                    Amount = fedExShipment.CommercialInvoiceFreight,
                    Currency = shipmentCurrencyType 
                };
            invoice.TaxesOrMiscellaneousCharge = new Money 
                { 
                    Amount = fedExShipment.CommercialInvoiceOther,
                    Currency = shipmentCurrencyType 
                };

            if (!string.IsNullOrEmpty(fedExShipment.CommercialInvoiceReference))
            {
                // Only add a customer reference if a value is provided
                invoice.CustomerReferences = new CustomerReference[]
                {
                    new CustomerReference {CustomerReferenceType = CustomerReferenceType.CUSTOMER_REFERENCE, Value = fedExShipment.CommercialInvoiceReference}
                };
            }

            customsDetail.CommercialInvoice = invoice;
        }


        /// <summary>
        /// Gets the customs detail from the request or creates a new one if the one on the request is null.
        /// </summary>
        /// <param name="nativeRequest">The native request.</param>
        /// <returns>A CustomsClearanceDetail object.</returns>
        private CustomsClearanceDetail GetCustomsDetail(ProcessShipmentRequest nativeRequest)
        {
            return nativeRequest.RequestedShipment.CustomsClearanceDetail ?? new CustomsClearanceDetail();
        }

        /// <summary>
        /// Get the FedEx API value that corresponds to our internal value
        /// </summary>
        private TermsOfSaleType GetApiTermsOfSale(FedExTermsOfSale termsOfSale)
        {
            switch (termsOfSale)
            {
                case FedExTermsOfSale.FOB_or_FCA: return TermsOfSaleType.FOB_OR_FCA;
                case FedExTermsOfSale.CFR_or_CPT: return TermsOfSaleType.CFR_OR_CPT;
                case FedExTermsOfSale.CIF_or_CIP: return TermsOfSaleType.CIF_OR_CIP;
                case FedExTermsOfSale.EXW: return TermsOfSaleType.EXW;
                case FedExTermsOfSale.DDP: return TermsOfSaleType.DDP;
                case FedExTermsOfSale.DDU: return TermsOfSaleType.DDU;
            }

            throw new InvalidOperationException("Invalid FedEx TermsOfSale: " + termsOfSale);
        }

        /// <summary>
        /// Get the FedEx API value that corresponds to our internal value
        /// </summary>
        private PurposeOfShipmentType GetApiCommercialInvoicePurpose(FedExCommercialInvoicePurpose purpose)
        {
            switch (purpose)
            {
                case FedExCommercialInvoicePurpose.Sold: return PurposeOfShipmentType.SOLD;
                case FedExCommercialInvoicePurpose.NotSold: return PurposeOfShipmentType.NOT_SOLD;
                case FedExCommercialInvoicePurpose.Gift: return PurposeOfShipmentType.GIFT;
                case FedExCommercialInvoicePurpose.Sample: return PurposeOfShipmentType.SAMPLE;
                case FedExCommercialInvoicePurpose.Personal: return PurposeOfShipmentType.PERSONAL_EFFECTS;
                case FedExCommercialInvoicePurpose.Repair: return PurposeOfShipmentType.REPAIR_AND_RETURN;
            }

            throw new InvalidOperationException("Invalid FedEx Commercial Invoice Purpose: " + purpose);
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        private void InitializeRequest(CarrierRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            // The native FedEx request type should be a ProcessShipmentRequest
            ProcessShipmentRequest nativeRequest = request.NativeRequest as ProcessShipmentRequest;
            if (nativeRequest == null)
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }

            // Make sure the RequestedShipment is there
            if (nativeRequest.RequestedShipment == null)
            {
                // We'll be manipulating properties of the requested shipment, so make sure it's been created
                nativeRequest.RequestedShipment = new RequestedShipment();
            }
        }
    }
}
