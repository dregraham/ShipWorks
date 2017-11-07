using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.Hardware.Printers;
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
    /// An implementation of the ICarrierRequestManipulator that will manipulate the label specification of a
    /// IFedExNativeShipmentRequest.
    /// </summary>
    public class FedExLabelSpecificationManipulator : IFedExShipRequestManipulator
    {
        private readonly IFedExSettingsRepository settingsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExLabelSpecificationManipulator" /> class.
        /// </summary>
        public FedExLabelSpecificationManipulator(IFedExSettingsRepository settingsRepository)
        {
            this.settingsRepository = settingsRepository;
        }

        /// <summary>
        /// Should the manipulator be applied
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, int sequenceNumber) => true;

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
            labelSpecification.LabelFormatType = LabelFormatType.COMMON2D;

            return ConfigureImageAndStockType(shipmentEntity, shippingSettings, labelSpecification)
                .Map(() =>
                {
                    AddPrintedLabelOrigin(shipmentEntity, labelSpecification);

                    labelSpecification.ImageTypeSpecified = true;
                    labelSpecification.LabelStockTypeSpecified = true;
                });
        }

        /// <summary>
        /// Configure label image and stock type
        /// </summary>
        private Result ConfigureImageAndStockType(IShipmentEntity shipmentEntity, ShippingSettingsEntity shippingSettings, LabelSpecification labelSpecification)
        {
            if (shipmentEntity.RequestedLabelFormat != (int) ThermalLanguage.None)
            {
                // Setup the properties for a thermal label generated by FedEx
                return ConfigureThermalLabel(shipmentEntity, shippingSettings, labelSpecification);
            }
            else
            {
                // Define the type of image label and the size/medium
                labelSpecification.ImageType = ShippingDocumentImageType.PNG;
                labelSpecification.LabelStockType = LabelStockType.PAPER_4X6;

                return Result.FromSuccess();
            }
        }

        /// <summary>
        /// Adds the printed label origin. This is needed for SmartPost's return address. 
        /// </summary>
        private static void AddPrintedLabelOrigin(IShipmentEntity shipmentEntity, LabelSpecification labelSpecification)
        {
            if (shipmentEntity.FedEx.Service == (int) FedExServiceType.SmartPost && !shipmentEntity.ReturnShipment)
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
        }

        /// <summary>
        /// Configures the thermal label properties of the label specification..
        /// </summary>
        private Result ConfigureThermalLabel(IShipmentEntity shipmentEntity, IShippingSettingsEntity shippingSettings, LabelSpecification labelSpecification)
        {
            return GetFedExApiThermalType((ThermalLanguage) shipmentEntity.RequestedLabelFormat)
                .Map(x =>
                {
                    labelSpecification.ImageType = x;
                    labelSpecification.LabelStockType = GetLabelStockType(shippingSettings, labelSpecification);
                });
        }

        /// <summary>
        /// Get the label stock type
        /// </summary>
        private LabelStockType GetLabelStockType(IShippingSettingsEntity shippingSettings, LabelSpecification labelSpecification)
        {
            if (!shippingSettings.FedExThermalDocTab)
            {
                return LabelStockType.STOCK_4X6;
            }

            return shippingSettings.FedExThermalDocTabType == (int) ThermalDocTabType.Leading ?
                LabelStockType.STOCK_4X675_LEADING_DOC_TAB :
                LabelStockType.STOCK_4X675_TRAILING_DOC_TAB;
        }

        /// <summary>
        /// Get the FedEx API value for our internal thermal type
        /// </summary>
        private GenericResult<ShippingDocumentImageType> GetFedExApiThermalType(ThermalLanguage thermalType)
        {
            switch (thermalType)
            {
                case ThermalLanguage.EPL: return ShippingDocumentImageType.EPL2;
                case ThermalLanguage.ZPL: return ShippingDocumentImageType.ZPLII;
            }

            return new InvalidOperationException("Invalid FedEx thermal type " + thermalType);
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
