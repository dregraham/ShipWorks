using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using Address = ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.Address;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    /// <summary>
    /// An implementation of the IFedExShipRequestManipulator that will manipulate the label specification of a
    /// IFedExNativeShipmentRequest.
    /// </summary>
    public class FedExLtlFreightLabelSpecificationManipulator : IFedExShipRequestManipulator
    {
        private readonly IFedExSettingsRepository settingsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExLabelSpecificationManipulator" /> class.
        /// </summary>
        public FedExLtlFreightLabelSpecificationManipulator(IFedExSettingsRepository settingsRepository)
        {
            this.settingsRepository = settingsRepository;
        }

        /// <summary>
        /// Should the manipulator be applied
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, int sequenceNumber) => FedExUtility.IsFreightLtlService(shipment.FedEx.Service);

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public GenericResult<ProcessShipmentRequest> Manipulate(IShipmentEntity shipment, ProcessShipmentRequest request, int sequenceNumber)
        {
            InitializeRequest(request);

            //Fetch the latest shipping settings from the repository
            ShippingSettingsEntity shippingSettings = settingsRepository.GetShippingSettings();

            // All we need to do is mask the account data and configure the type of label being generated
            LabelSpecification labelSpecification = new LabelSpecification();
            MaskAccountData(shippingSettings, labelSpecification, shipment);

            return ConfigureLabelType(shipment, shippingSettings, labelSpecification)
                .Map(() =>
                {
                    // Add alcohol label request if needed
                    AddAlcoholRegulatoryLabels(labelSpecification, shipment);

                    // Everything is ready to go
                    request.RequestedShipment.LabelSpecification = labelSpecification;

                    return request;
                });
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        private void InitializeRequest(ProcessShipmentRequest request) =>
            request.Ensure(x => x.RequestedShipment);

        /// <summary>
        /// Configures the label type (format, thermal/image, size, etc.).
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <param name="shippingSettings">The shipping settings.</param>
        /// <param name="labelSpecification">The label specification.</param>
        private Result ConfigureLabelType(IShipmentEntity shipmentEntity, ShippingSettingsEntity shippingSettings, LabelSpecification labelSpecification)
        {
            labelSpecification.LabelFormatType = LabelFormatType.FEDEX_FREIGHT_STRAIGHT_BILL_OF_LADING;

            return ConfigureImageAndStockType(labelSpecification)
                .Map(() =>
                {
                    AddPrintedLabelOrigin(shipmentEntity, labelSpecification);
                });
        }

        /// <summary>
        /// Configure label image and stock type
        /// </summary>
        private Result ConfigureImageAndStockType(LabelSpecification labelSpecification)
        {
            // Define the type of image label and the size/medium
            labelSpecification.ImageType = ShippingDocumentImageType.PDF;
            labelSpecification.ImageTypeSpecified = true;
            labelSpecification.LabelStockType = LabelStockType.PAPER_LETTER;
            labelSpecification.LabelStockTypeSpecified = true;

            return Result.FromSuccess();
        }

        /// <summary>
        /// Adds the printed label origin. This is needed for SmartPost's return address. 
        /// </summary>
        private static void AddPrintedLabelOrigin(IShipmentEntity shipmentEntity, LabelSpecification labelSpecification)
        {
            PersonAdapter person = shipmentEntity.OriginPerson;

            labelSpecification.PrintedLabelOrigin = new ContactAndAddress()
            {
                Contact = new Contact()
                {
                    CompanyName = person.Company,
                    PersonName = person.UnparsedName,
                    PhoneNumber = person.Phone
                },
                Address = new Address()
                {
                    StreetLines = person.StreetLines,
                    City = person.City,
                    StateOrProvinceCode = person.StateProvCode,
                    PostalCode = person.PostalCode,
                    CountryCode = person.AdjustedCountryCode(ShipmentTypeCode.FedEx)
                }
            };
        }

        /// <summary>
        /// Masks the account data based on the shipping settings.
        /// </summary>
        /// <param name="shippingSettings">The shipping settings.</param>
        /// <param name="labelSpecification">The label specification.</param>
        /// <param name="shipment">The shipment.</param>
        private void MaskAccountData(ShippingSettingsEntity shippingSettings, LabelSpecification labelSpecification, IShipmentEntity shipment)
        {
            List<LabelMaskableDataType> maskableDataTypes = new List<LabelMaskableDataType>();

            bool isInternational = !ShipmentTypeManager.GetType(shipment).IsDomestic(shipment);

            if (shippingSettings.FedExMaskAccount)
            {
                if (isInternational)
                {
                    // Three fields must be applied to mask the account data for international shipments
                    maskableDataTypes.AddRange(new List<LabelMaskableDataType>()
                    {
                        LabelMaskableDataType.SHIPPER_ACCOUNT_NUMBER,
                        LabelMaskableDataType.TRANSPORTATION_CHARGES_PAYOR_ACCOUNT_NUMBER,
                        LabelMaskableDataType.DUTIES_AND_TAXES_PAYOR_ACCOUNT_NUMBER,
                        LabelMaskableDataType.CUSTOMS_VALUE
                    });
                }
                else
                {
                    // We just need to supply the shipper account number on domestic shipments
                    maskableDataTypes.AddRange(new List<LabelMaskableDataType>()
                    {
                        LabelMaskableDataType.SHIPPER_ACCOUNT_NUMBER
                    });
                }
            }

            LabelMaskableDataType? maskableDataType = GetFedExLabelMaskableDataType(shipment.FedEx.MaskedData);
            if (maskableDataType.HasValue)
            {
                maskableDataTypes.AddRange(new List<LabelMaskableDataType>()
                    {
                        maskableDataType.Value
                    });
            }

            if (maskableDataTypes.Any())
            {
                CustomerSpecifiedLabelDetail detailDetail = new CustomerSpecifiedLabelDetail();
                labelSpecification.CustomerSpecifiedDetail = detailDetail;
                detailDetail.MaskedData = maskableDataTypes.ToArray();
            }
        }

        /// <summary>
        /// Given a MaskedDataType (as int) return corresponding value
        /// </summary>
        public static LabelMaskableDataType? GetFedExLabelMaskableDataType([Obfuscation(Exclude = true)] int? maskedDataType)
        {
            if (!maskedDataType.HasValue)
            {
                return null;
            }

            switch ((FedExMaskedDataType) maskedDataType.Value)
            {
                case FedExMaskedDataType.SecondaryBarcode:
                    return LabelMaskableDataType.SECONDARY_BARCODE;
                default:
                    throw new ArgumentOutOfRangeException(nameof(maskedDataType));
            }
        }

        /// <summary>
        /// Adds alcohol regulatory label if needed.
        /// </summary>
        /// <param name="labelSpecification">The label specification.</param>
        /// <param name="shipment">The shipment.</param>
        private static void AddAlcoholRegulatoryLabels(LabelSpecification labelSpecification, IShipmentEntity shipment)
        {
            bool hasAlcohol = shipment.FedEx.Packages.Any(p => p.ContainsAlcohol);

            if (hasAlcohol)
            {
                CustomerSpecifiedLabelDetail detailDetail = new CustomerSpecifiedLabelDetail();

                if (labelSpecification.CustomerSpecifiedDetail != null)
                {
                    detailDetail = labelSpecification.CustomerSpecifiedDetail;
                }

                List<CustomerSpecifiedLabelGenerationOptionType> generalOptions = new List<CustomerSpecifiedLabelGenerationOptionType>()
                {
                    CustomerSpecifiedLabelGenerationOptionType.CONTENT_ON_SUPPLEMENTAL_LABEL_ONLY
                };

                RegulatoryLabelContentDetail regulatoryLabelContentDetail = new RegulatoryLabelContentDetail()
                {
                    Type = RegulatoryLabelType.ALCOHOL_SHIPMENT_LABEL,
                    TypeSpecified = true,
                    GenerationOptions = generalOptions.ToArray()
                };

                List<RegulatoryLabelContentDetail> regulatoryLabelContentDetails = new List<RegulatoryLabelContentDetail>() { regulatoryLabelContentDetail };
                detailDetail.RegulatoryLabels = regulatoryLabelContentDetails.ToArray();

                labelSpecification.CustomerSpecifiedDetail = detailDetail;
            }
        }
    }
}
