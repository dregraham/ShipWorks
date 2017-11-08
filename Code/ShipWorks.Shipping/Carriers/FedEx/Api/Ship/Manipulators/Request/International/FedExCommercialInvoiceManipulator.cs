using System;
using System.Collections.Generic;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using Address = ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.Address;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request.International
{
    /// <summary>
    /// An implementation of the ICarrierRequestManipulator interface that builds up the commercial
    /// invoice portion of the customs detail in the FedEx IFedExNativeShipmentRequest object.
    /// </summary>
    public class FedExCommercialInvoiceManipulator : IFedExShipRequestManipulator
    {
        private readonly IFedExSettingsRepository settings;
        private string shipmentCurrencyType;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExCommercialInvoiceManipulator(IFedExSettingsRepository settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// Does this manipulator apply to this shipment
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, int sequenceNumber)
        {
            return shipment.FedEx.CommercialInvoice && !new FedExShipmentType().IsDomestic(shipment);
        }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        public GenericResult<ProcessShipmentRequest> Manipulate(IShipmentEntity shipment, ProcessShipmentRequest request, int sequenceNumber)
        {
            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(shipment, request);

            // We can safely cast this since we've passed initialization
            IFedExShipmentEntity fedExShipment = shipment.FedEx;

            shipmentCurrencyType = FedExSettings.GetCurrencyTypeApiValue(shipment, () => settings.GetAccountReadOnly(shipment));

            CustomsClearanceDetail customsDetail = request.RequestedShipment.CustomsClearanceDetail;

            return ConfigureInvoice(fedExShipment, customsDetail)
                .Map(() =>
                {
                    ConfigureImporter(fedExShipment, customsDetail);

                    customsDetail.InsuranceCharges = new Money
                    {
                        Amount = fedExShipment.CommercialInvoiceInsurance,
                        Currency = shipmentCurrencyType
                    };
                    request.RequestedShipment.CustomsClearanceDetail = customsDetail;

                    ConfigureEtd(fedExShipment, request);
                })
                .Map(() => request);
        }

        /// <summary>
        /// Add Etd fields
        /// </summary>
        private static void ConfigureEtd(IFedExShipmentEntity fedExShipment, IFedExNativeShipmentRequest request)
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
            if (request.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes != null)
            {
                shipmentSpecialServiceTypes.AddRange(request.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);
            }
            shipmentSpecialServiceTypes.Add(ShipmentSpecialServiceType.ELECTRONIC_TRADE_DOCUMENTS);

            ConfigureCustomsShippingDocumentSpecs(request);

            request.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = shipmentSpecialServiceTypes.ToArray();
            request.RequestedShipment.SpecialServicesRequested.EtdDetail = etdDetail;
        }

        /// <summary>
        /// Add ShippingDocumentSpecification if needed
        /// </summary>
        private static void ConfigureCustomsShippingDocumentSpecs(IFedExNativeShipmentRequest request)
        {
            ShippingDocumentSpecification shippingDocumentSpecification = request.RequestedShipment.ShippingDocumentSpecification;

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

            request.RequestedShipment.ShippingDocumentSpecification = shippingDocumentSpecification;
        }

        /// <summary>
        /// Configures the importer of the customs detail based on the shipment data.
        /// </summary>
        /// <param name="fedExShipment">The FedEx shipment.</param>
        /// <param name="customsDetail">The customs detail.</param>
        private void ConfigureImporter(IFedExShipmentEntity fedExShipment, CustomsClearanceDetail customsDetail)
        {
            if (fedExShipment.ImporterOfRecord)
            {
                Party importer = new Party();
                PersonAdapter importerPerson = fedExShipment.ImporterPerson;

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
        private Result ConfigureInvoice(IFedExShipmentEntity fedExShipment, CustomsClearanceDetail customsDetail)
        {
            CommercialInvoice invoice = new CommercialInvoice();
            invoice.PurposeSpecified = true;

            invoice.Comments = new string[] { fedExShipment.CommercialInvoiceComments };

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

            return GenericResult.FromSuccess(customsDetail)
                .Do(_ =>
                    GetApiTermsOfSale((FedExTermsOfSale) fedExShipment.CommercialInvoiceTermsOfSale)
                        .Do(x => invoice.TermsOfSale = x))
                .Do(_ =>
                    GetApiCommercialInvoicePurpose((FedExCommercialInvoicePurpose) fedExShipment.CommercialInvoicePurpose)
                        .Do(x => invoice.Purpose = x))
                .Do(x => x.CommercialInvoice = invoice);
        }

        /// <summary>
        /// Get the FedEx API value that corresponds to our internal value
        /// </summary>
        private GenericResult<string> GetApiTermsOfSale(FedExTermsOfSale termsOfSale)
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

            return new InvalidOperationException("Invalid FedEx TermsOfSale: " + termsOfSale);
        }

        /// <summary>
        /// Get the FedEx API value that corresponds to our internal value
        /// </summary>
        private GenericResult<PurposeOfShipmentType> GetApiCommercialInvoicePurpose(FedExCommercialInvoicePurpose purpose)
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

            return new InvalidOperationException("Invalid FedEx Commercial Invoice Purpose: " + purpose);
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        private void InitializeRequest(IShipmentEntity shipment, ProcessShipmentRequest request)
        {
            request.Ensure(r => r.RequestedShipment)
                .Ensure(rs => rs.SpecialServicesRequested);
            request.RequestedShipment.Ensure(x => x.CustomsClearanceDetail);
        }
    }
}
