using System;
using System.Collections.Generic;
using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using Address = ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.Address;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators.International
{
    /// <summary>
    /// An implementation of the ICarrierRequestManipulator interface that builds up the commercial
    /// invoice portion of the customs detail in the FedEx IFedExNativeShipmentRequest object.
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
            IFedExNativeShipmentRequest nativeRequest = request.NativeRequest as IFedExNativeShipmentRequest;
            FedExShipmentEntity fedExShipment = request.ShipmentEntity.FedEx;

            shipmentCurrencyType = GetShipmentCurrencyType(fedExShipment.Shipment);

            if (fedExShipment.CommercialInvoice && !new FedExShipmentType().IsDomestic(request.ShipmentEntity))
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

                ConfigureEtd(fedExShipment, nativeRequest);
            }
        }

        /// <summary>
        /// Add Etd fields
        /// </summary>
        private static void ConfigureEtd(FedExShipmentEntity fedExShipment, IFedExNativeShipmentRequest nativeRequest)
        {
            // Return if the user chose no commercial invoice or not to file electronically.
            if (!fedExShipment.CommercialInvoice || !fedExShipment.CommercialInvoiceFileElectronically)
            {
                return;
            }

            // Only set the shipping document specification if we are not SmartPost
            if ((FedExServiceType) fedExShipment.Service == FedExServiceType.SmartPost)
            {
                return;
            }

            List<RequestedShippingDocumentType> requestedEtdDocTypes = new List<RequestedShippingDocumentType>() { RequestedShippingDocumentType.COMMERCIAL_INVOICE };

            EtdDetail etdDetail = new EtdDetail();
            etdDetail.RequestedDocumentCopies = requestedEtdDocTypes.ToArray();

            List<ShipmentSpecialServiceType> shipmentSpecialServiceTypes = new List<ShipmentSpecialServiceType>();
            if (nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes != null)
            {
                shipmentSpecialServiceTypes.AddRange(nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);
            }
            shipmentSpecialServiceTypes.Add(ShipmentSpecialServiceType.ELECTRONIC_TRADE_DOCUMENTS);

            ConfigureCustomsShippingDocumentSpecs(nativeRequest);

            nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = shipmentSpecialServiceTypes.ToArray();
            nativeRequest.RequestedShipment.SpecialServicesRequested.EtdDetail = etdDetail;
        }

        /// <summary>
        /// Add ShippingDocumentSpecification if needed
        /// </summary>
        private static void ConfigureCustomsShippingDocumentSpecs(IFedExNativeShipmentRequest nativeRequest)
        {
            ShippingDocumentSpecification shippingDocumentSpecification = nativeRequest.RequestedShipment.ShippingDocumentSpecification;

            // Make sure the ShippingDocumentSpecification is there
            if (shippingDocumentSpecification == null)
            {
                shippingDocumentSpecification = new ShippingDocumentSpecification();
            }

            // Make sure the ShippingDocumentTypes is there
            if (shippingDocumentSpecification.ShippingDocumentTypes == null)
            {
                shippingDocumentSpecification.ShippingDocumentTypes = new RequestedShippingDocumentType[0];
            }

            List<RequestedShippingDocumentType> shippingDocumentTypes = new List<RequestedShippingDocumentType>(shippingDocumentSpecification.ShippingDocumentTypes);
            shippingDocumentTypes.Add(RequestedShippingDocumentType.COMMERCIAL_INVOICE);
            shippingDocumentSpecification.ShippingDocumentTypes = shippingDocumentTypes.ToArray();
            
            CommercialInvoiceDetail commercialInvoiceDetail = shippingDocumentSpecification.CommercialInvoiceDetail;
            if (commercialInvoiceDetail == null)
            {
                commercialInvoiceDetail = new CommercialInvoiceDetail();
            }

            commercialInvoiceDetail.Format = new ShippingDocumentFormat()
            {
                ImageType = ShippingDocumentImageType.PDF,
                ImageTypeSpecified = true,
                StockType = ShippingDocumentStockType.PAPER_LETTER,
                StockTypeSpecified = true
            };

            shippingDocumentSpecification.CommercialInvoiceDetail = commercialInvoiceDetail;

            nativeRequest.RequestedShipment.ShippingDocumentSpecification = shippingDocumentSpecification;
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

                importer.Address = FedExRequestManipulatorUtilities.CreateAddress<Address>(importerPerson);
                importer.Contact = FedExRequestManipulatorUtilities.CreateContact<Contact>(importerPerson);
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
        private CustomsClearanceDetail GetCustomsDetail(IFedExNativeShipmentRequest nativeRequest)
        {
            return nativeRequest.RequestedShipment.CustomsClearanceDetail ?? new CustomsClearanceDetail();
        }

        /// <summary>
        /// Get the FedEx API value that corresponds to our internal value
        /// </summary>
        private string GetApiTermsOfSale(FedExTermsOfSale termsOfSale)
        {
            // TODO: We need to determine if the "or" types need to be split into individual types.
            // TODO: We need to determine the actual values for the rest.
            // TODO: May need to verify the DAP and DAT get added to the drop down, and determine if there's other info they need.
            switch (termsOfSale)
            {
                case FedExTermsOfSale.FOB_or_FCA: return "FCA";
                case FedExTermsOfSale.CFR_or_CPT: return "CPT/C&F";
                case FedExTermsOfSale.CIF_or_CIP: return "CIP/CIF";
                case FedExTermsOfSale.EXW: return "EXW";
                case FedExTermsOfSale.DDP: return "DDP";
                case FedExTermsOfSale.DDU: return "DDU";
                case FedExTermsOfSale.DAP: return "DAP";
                case FedExTermsOfSale.DAT: return "DAT";
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

            // The native FedEx request type should be a IFedExNativeShipmentRequest
            IFedExNativeShipmentRequest nativeRequest = request.NativeRequest as IFedExNativeShipmentRequest;
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
            // Make sure the RequestedShipment is there
            if (nativeRequest.RequestedShipment.SpecialServicesRequested == null)
            {
                // We'll be manipulating properties of the special services, so make sure it's been created
                nativeRequest.RequestedShipment.SpecialServicesRequested = new ShipmentSpecialServicesRequested();
            }

        }
    }
}
