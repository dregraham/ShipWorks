using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Shipping.ShipEngine.DTOs;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP
{
    /// <summary>
    /// Label data that has been downloaded from a carrier
    /// </summary>
    [Component(RegistrationType.Self)]
    public class AmazonSFPDownloadedLabelData : ShipEngineDownloadedLabelData
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonSFPDownloadedLabelData(ShipmentEntity shipment,
            Label label,
            IDataResourceManager resourceManager,
            IShipEngineResourceDownloader resourceDownloader)
            : base(shipment, label, resourceManager, resourceDownloader)
        {
        }

        /// <summary>
        /// Save the label info to the shipment
        /// </summary>
        protected override void SaveLabelInfoToEntity(ShipmentEntity shipment, Label label)
        {
            base.SaveLabelInfoToEntity(shipment, label);

            shipment.AmazonSFP.AmazonUniqueShipmentID = label.ShipmentId;
            shipment.AmazonSFP.CarrierName = label.CarrierCode ?? string.Empty;
            shipment.AmazonSFP.ShippingServiceName = label.ServiceCode ?? string.Empty;
            shipment.ActualLabelFormat = shipment.AmazonSFP.RequestedLabelFormat == (int) ThermalLanguage.None ? 
                (int?) null : 
                shipment.AmazonSFP.RequestedLabelFormat;
        }

        /// <summary>
        /// Save the ShipEngine label ID to the Amazon SFP shipment
        /// </summary>
        protected override void SaveShipEngineLabelID(ShipmentEntity shipment, Label label)
        {
            shipment.AmazonSFP.ShipEngineLabelID = label.LabelId;
        }
    }
}
