using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Business;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Stores.Platforms.Amazon.WebServices.Associates;
using Address = ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.Address;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    /// <summary>
    /// An implementation of the ICarrierRequestManipulator that will manipulate the label specification of a
    /// IFedExNativeShipmentRequest.
    /// </summary>
    public class FedExLabelSpecificationManipulator : FedExShippingRequestManipulatorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FedExLabelSpecificationManipulator" /> class.
        /// </summary>
        public FedExLabelSpecificationManipulator()
            : this(new FedExSettings(new FedExSettingsRepository()))
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExLabelSpecificationManipulator" /> class.
        /// </summary>
        /// <param name="fedExSettings">The fed ex settings.</param>
        public FedExLabelSpecificationManipulator(FedExSettings fedExSettings)
            : base(fedExSettings)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExLabelSpecificationManipulator" /> class.
        /// </summary>
        /// <param name="settingsRepository">The settings repository.</param>
        public FedExLabelSpecificationManipulator(ICarrierSettingsRepository settingsRepository) : base(settingsRepository)
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

            //Fetch the latest shipping settings from the repository
            ShippingSettingsEntity shippingSettings = SettingsRepository.GetShippingSettings();

            // All we need to do is mask the account data and configure the type of label being generated
            LabelSpecification labelSpecification = new LabelSpecification();            
            MaskAccountData(shippingSettings, labelSpecification, request.ShipmentEntity);
            ConfigureLabelType(request.ShipmentEntity, shippingSettings, labelSpecification);

            // Add alcohol label request if needed
            AddAlcoholRegulatoryLabels(labelSpecification, request.ShipmentEntity);

            // Everything is ready to go
            nativeRequest.RequestedShipment.LabelSpecification = labelSpecification;
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

            if (nativeRequest.RequestedShipment == null)
            {
                // We'll be manipulating properties of the requested shipment, so make sure it's been created
                nativeRequest.RequestedShipment = new RequestedShipment();
            }

            // We'll also initialize properties on the the shipment entity if needed (this is sort of a unique
            // case as we'll be setting the thermal type on the shipment entity
            if (request.ShipmentEntity.FedEx == null)
            {
                request.ShipmentEntity.FedEx = new FedExShipmentEntity();
            }

            if (request.ShipmentEntity.FedEx.Shipment == null)
            {
                request.ShipmentEntity.FedEx.Shipment = new ShipmentEntity();
            }
        }

        /// <summary>
        /// Configures the label type (format, thermal/image, size, etc.).
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <param name="shippingSettings">The shipping settings.</param>
        /// <param name="labelSpecification">The label specification.</param>
        private void ConfigureLabelType(ShipmentEntity shipmentEntity, ShippingSettingsEntity shippingSettings, LabelSpecification labelSpecification)
        {
            labelSpecification.LabelFormatType = LabelFormatType.COMMON2D;

            if (shipmentEntity.RequestedLabelFormat != (int) ThermalLanguage.None)
            {
                // Setup the properties for a thermal label generated by FedEx
                ConfigureThermalLabel(shipmentEntity, shippingSettings, labelSpecification);
            }
            else
            {
                // We're generating an image label; this is a little unique in that the manipulator is setting a property 
                // on the shipment to record whether we downloaded a standard label or themal label
                shipmentEntity.FedEx.Shipment.ActualLabelFormat = null;

                // Define the type of image label and the size/medium
                labelSpecification.ImageType = ShippingDocumentImageType.PNG;
                labelSpecification.LabelStockType = LabelStockType.PAPER_4X6;
            }

            AddPrintedLabelOrigin(shipmentEntity, labelSpecification);

            labelSpecification.ImageTypeSpecified = true;
            labelSpecification.LabelStockTypeSpecified = true;
        }

        /// <summary>
        /// Adds the printed label origin. This is needed for SmartPost's return address. 
        /// </summary>
        private static void AddPrintedLabelOrigin(ShipmentEntity shipmentEntity, LabelSpecification labelSpecification)
        {
            if (shipmentEntity.FedEx.Service == (int) FedExServiceType.SmartPost)
            {
                if (!shipmentEntity.ReturnShipment)
                {
                    PersonAdapter person = new PersonAdapter(shipmentEntity, "Origin");

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
        }

        /// <summary>
        /// Configures the thermal label properties of the label specification..
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <param name="shippingSettings">The shipping settings.</param>
        /// <param name="labelSpecification">The label specification.</param>
        private void ConfigureThermalLabel(ShipmentEntity shipmentEntity, ShippingSettingsEntity shippingSettings, LabelSpecification labelSpecification)
        {
            // A little unique in this manipulator is setting a property on the shipment to record whether we
            // downloaded a standard label or themal label
            shipmentEntity.FedEx.Shipment.ActualLabelFormat = shipmentEntity.RequestedLabelFormat;

            // Thermal type
            labelSpecification.ImageType = GetFedExApiThermalType((ThermalLanguage)shipmentEntity.RequestedLabelFormat);

            // Has a doc-tab
            if (shippingSettings.FedExThermalDocTab)
            {
                if (shippingSettings.FedExThermalDocTabType == (int)ThermalDocTabType.Leading)
                {
                    labelSpecification.LabelStockType = LabelStockType.STOCK_4X675_LEADING_DOC_TAB;
                }
                else
                {
                    labelSpecification.LabelStockType = LabelStockType.STOCK_4X675_TRAILING_DOC_TAB;
                }
            }
            else
            {
                labelSpecification.LabelStockType = LabelStockType.STOCK_4X6;
            }
        }

        /// <summary>
        /// Get the FedEx API value for our internal thermal type
        /// </summary>
        private ShippingDocumentImageType GetFedExApiThermalType(ThermalLanguage thermalType)
        {
            switch (thermalType)
            {
                case ThermalLanguage.EPL: return ShippingDocumentImageType.EPL2;
                case ThermalLanguage.ZPL: return ShippingDocumentImageType.ZPLII;
            }

            throw new InvalidOperationException("Invalid FedEx thermal type " + thermalType);
        }

        /// <summary>
        /// Masks the account data based on the shipping settings.
        /// </summary>
        /// <param name="shippingSettings">The shipping settings.</param>
        /// <param name="labelSpecification">The label specification.</param>
        /// <param name="shipment">The shipment.</param>
        private void MaskAccountData(ShippingSettingsEntity shippingSettings, LabelSpecification labelSpecification, ShipmentEntity shipment)
        {
            bool isInternational = !ShipmentTypeManager.GetType(shipment).IsDomestic(shipment);

            if (shippingSettings.FedExMaskAccount)
            {
                CustomerSpecifiedLabelDetail detailDetail = new CustomerSpecifiedLabelDetail();

                if (isInternational)
                {
                    // Three fields must be applied to mask the account data for international shipments
                    detailDetail.MaskedData = new LabelMaskableDataType[]
                    {
                        LabelMaskableDataType.SHIPPER_ACCOUNT_NUMBER, 
                        LabelMaskableDataType.TRANSPORTATION_CHARGES_PAYOR_ACCOUNT_NUMBER, 
                        LabelMaskableDataType.DUTIES_AND_TAXES_PAYOR_ACCOUNT_NUMBER,
                        LabelMaskableDataType.CUSTOMS_VALUE
                    };
                }
                else
                {
                    // We just need to supply the shipper account number on domestic shipments
                    detailDetail.MaskedData = new LabelMaskableDataType[] { LabelMaskableDataType.SHIPPER_ACCOUNT_NUMBER };
                }
                

                labelSpecification.CustomerSpecifiedDetail = detailDetail;
            }
        }

        /// <summary>
        /// Adds alcohol regulatory label if needed.
        /// </summary>
        /// <param name="labelSpecification">The label specification.</param>
        /// <param name="shipment">The shipment.</param>
        private static void AddAlcoholRegulatoryLabels(LabelSpecification labelSpecification, ShipmentEntity shipment)
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
