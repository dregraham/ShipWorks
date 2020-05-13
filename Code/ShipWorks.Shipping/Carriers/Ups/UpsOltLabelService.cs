using System;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.Ups;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// LabelService for Ups Online Tools
    /// </summary>
    public class UpsOltLabelService : UpsLabelService
    {
        private readonly IUpsShipmentValidatorFactory upsShipmentValidatorFactory;
        private readonly IUpsLabelClientFactory labelClientFactory;
        private readonly IIndex<ShipmentTypeCode, ICarrierSettingsRepository> settingsRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsOltLabelService(IUpsShipmentValidatorFactory upsShipmentValidatorFactory,
            IUpsLabelClientFactory labelClientFactory,
            IIndex<ShipmentTypeCode, ICarrierSettingsRepository> settingsRepository)
            : base(labelClientFactory, settingsRepository)
        {
            this.upsShipmentValidatorFactory = upsShipmentValidatorFactory;
            this.labelClientFactory = labelClientFactory;
            this.settingsRepository = settingsRepository;
        }

        /// <summary>
        /// Creates a label for Ups Online Tools
        /// </summary>
        public override Task<TelemetricResult<IDownloadedLabelData>> Create(ShipmentEntity shipment)
        {
            try
            {
                // Call the base class for setting default values as needed based on the service/package type of the shipment
                base.Create(shipment);

                Result validationResult = upsShipmentValidatorFactory.Create(shipment).ValidateShipment(shipment);
                if (validationResult.Failure)
                {
                    throw new ShippingException(validationResult.Message);
                }

                UpsServicePackageTypeSetting.Validate(shipment);
                
                return labelClientFactory.GetClient(shipment).GetLabel(shipment);
            }
            catch (UpsApiException ex)
            {
                string message = ex.Message;

                // find the "XML document is well formed but not valid" error
                if (ex.ErrorCode == "10002" &&
                    shipment.ReturnShipment &&
                    !string.IsNullOrEmpty(ex.ErrorLocation) &&
                    string.Equals(ex.ErrorLocation, "ShipmentConfirmRequest/Shipment/Package/Description", StringComparison.OrdinalIgnoreCase))
                {
                    message = "The return shipment's Contents is required.";
                }

                throw new ShippingException(message, ex);
            }
            catch (CarrierException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }
    }
}
