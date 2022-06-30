using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Shipping.ShipEngine.DTOs;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP.Platform
    {
    /// <summary>
    /// Save Amazon Buy Shipping label data
    /// </summary>
    [Component(RegistrationType.Self)]
    public class AmazonSfpShipEngineDownloadedLabelData : ShipEngineDownloadedLabelData
    {
        private readonly IAmazonSFPServiceTypeRepository amazonSfpServiceTypeRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonSfpShipEngineDownloadedLabelData(ShipmentEntity shipment,
            Label label,
            IDataResourceManager resourceManager,
            IShipEngineResourceDownloader resourceDownloader,
            IAmazonSFPServiceTypeRepository amazonSfpServiceTypeRepository)
            : base(shipment, label, resourceManager, resourceDownloader)
        {
            this.amazonSfpServiceTypeRepository = amazonSfpServiceTypeRepository;
        }

        /// <summary>
        /// Save the ShipEngine label ID to the Amazon Buy Shipping shipment
        /// </summary>
        protected override void SaveShipEngineLabelID(ShipmentEntity shipment, Label label)
        {
            shipment.AmazonSFP.ShipEngineLabelID = label.LabelId;
        }

        /// <summary>
        /// Save the label info to the shipment
        /// </summary>
        protected override void SaveLabelInfoToEntity(ShipmentEntity shipment, Label label)
        {
            base.SaveLabelInfoToEntity(shipment, label);

            var serviceType = amazonSfpServiceTypeRepository.Find(label.ServiceCode);
            var carrierName = amazonSfpServiceTypeRepository.GetCarrierName(label.ServiceCode);

            shipment.AmazonSFP.AmazonUniqueShipmentID = label.ShipmentId;
            shipment.AmazonSFP.CarrierName = carrierName ?? string.Empty;
            shipment.AmazonSFP.ShippingServiceName = serviceType.Description ?? string.Empty;
            shipment.ActualLabelFormat = shipment.AmazonSFP.RequestedLabelFormat == (int) ThermalLanguage.None ?
                (int?) null :
                shipment.AmazonSFP.RequestedLabelFormat;
        }
    }
}
