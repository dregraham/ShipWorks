using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping.ShipEngine.DTOs;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ShipEngine;

namespace ShipWorks.Shipping.Carriers.Dhl.API.ShipEngine
{
    /// <summary>
    /// Save Dhl label data
    /// </summary>
    [Component(RegistrationType.Self)]
    public class DhlExpressShipEngineDownloadedLabelData : ShipEngineDownloadedLabelData
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DhlExpressShipEngineDownloadedLabelData(ShipmentEntity shipment, 
            Label label, 
            IDataResourceManager resourceManager, 
            IShipEngineResourceDownloader resourceDownloader) 
            : base(shipment, label, resourceManager, resourceDownloader)
        {
        }

        /// <summary>
        /// Save the ShipEngine label ID to the DHL Express shipment
        /// </summary>
        protected override void SaveShipEngineLabelID(ShipmentEntity shipment, Label label)
        {
            shipment.DhlExpress.ShipEngineLabelID = label.LabelId;
        }
    }
}
