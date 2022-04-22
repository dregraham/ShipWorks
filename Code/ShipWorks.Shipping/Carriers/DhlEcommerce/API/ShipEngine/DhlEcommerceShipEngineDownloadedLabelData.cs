using Interapptive.Shared.ComponentRegistration;
using ShipEngine.CarrierApi.Client.Model;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ShipEngine;

namespace ShipWorks.Shipping.Carriers.DhlEcommerce.API.ShipEngine
{
    /// <summary>
    /// Save Dhl label data
    /// </summary>
    [Component(RegistrationType.Self)]
    public class DhlEcommerceShipEngineDownloadedLabelData : ShipEngineDownloadedLabelData
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DhlEcommerceShipEngineDownloadedLabelData(ShipmentEntity shipment, 
            Label label, 
            IDataResourceManager resourceManager, 
            IShipEngineResourceDownloader resourceDownloader) 
            : base(shipment, label, resourceManager, resourceDownloader)
        {
        }

        /// <summary>
        /// Save the ShipEngine label ID to the DHL Ecommerce shipment
        /// </summary>
        protected override void SaveShipEngineLabelID(ShipmentEntity shipment, Label label)
        {
            shipment.DhlEcommerce.ShipEngineLabelID = label.LabelId;
        }
    }
}
