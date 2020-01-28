using ShipWorks.Data.Model.EntityClasses;
using ShipEngine.ApiClient.Model;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data;
using ShipWorks.Shipping.ShipEngine;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// Save Dhl label data
    /// </summary>
    [Component(RegistrationType.Self)]
    public class DhlExpressDownloadedLabelData : ShipEngineDownloadedLabelData
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DhlExpressDownloadedLabelData(ShipmentEntity shipment, 
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
