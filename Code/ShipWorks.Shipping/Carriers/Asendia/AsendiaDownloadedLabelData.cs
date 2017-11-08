using ShipWorks.Data.Model.EntityClasses;
using ShipEngine.ApiClient.Model;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data;
using ShipWorks.Shipping.ShipEngine;

namespace ShipWorks.Shipping.Carriers.Asendia
{
    /// <summary>
    /// Save Asendia label data
    /// </summary>
    [Component(RegistrationType.Self)]
    public class AsendiaDownloadedLabelData : ShipEngineDownloadedLabelData
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AsendiaDownloadedLabelData(ShipmentEntity shipment,
            Label label,
            IDataResourceManager resourceManager,
            IShipEngineResourceDownloader resourceDownloader) 
            : base(shipment, label, resourceManager, resourceDownloader)
        {
        }

        /// <summary>
        /// Save the ShipEngine label ID to the Asendia shipment
        /// </summary>
        protected override void SaveShipEngineLabelID(ShipmentEntity shipment, Label label)
        {
            shipment.Asendia.ShipEngineLabelID = label.LabelId;
        }
    }
}
