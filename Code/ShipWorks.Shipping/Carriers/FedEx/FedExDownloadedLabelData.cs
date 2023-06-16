using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Shipping.ShipEngine.DTOs;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Label data that has been downloaded from a carrier
    /// </summary>
    [Component(RegistrationType.Self)]
    public class FedExDownloadedLabelData : ShipEngineDownloadedLabelData
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FedExDownloadedLabelData(ShipmentEntity shipment,
            Label label,
            IDataResourceManager resourceManager,
            IShipEngineResourceDownloader resourceDownloader) 
            : base(shipment, label, resourceManager, resourceDownloader)
        {
        }

        protected override void SaveLabelInfoToEntity(ShipmentEntity shipment, Label label)
        {
            base.SaveLabelInfoToEntity(shipment, label);
            FedExShipmentEntity fedExShipment = shipment.FedEx;

            for (int i = 0; i < label.Packages.Count; i++)
            {
                LabelPackage labelPackage = label.Packages[i];
                FedExPackageEntity fedExPackage = fedExShipment.Packages[i];

                fedExPackage.TrackingNumber = labelPackage.TrackingNumber;
            }
        }

        /// <summary>
        /// Save the ShipEngine label ID to the FedEx shipment
        /// </summary>
        protected override void SaveShipEngineLabelID(ShipmentEntity shipment, Label label)
        {
            shipment.FedEx.ShipEngineLabelId = label.LabelId;
        }
    }
}

