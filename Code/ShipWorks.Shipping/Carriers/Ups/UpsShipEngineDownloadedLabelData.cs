using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipEngine.ApiClient.Model;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ShipEngine;

namespace ShipWorks.Shipping.Carriers.Ups
{
    /// <summary>
    /// Save UPS ShipEngine label data
    /// </summary>
    [Component(RegistrationType.Self)]
    public class UpsShipEngineDownloadedLabelData : ShipEngineDownloadedLabelData
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UpsShipEngineDownloadedLabelData(ShipmentEntity shipment,
            Label label,
            IDataResourceManager resourceManager,
            IShipEngineResourceDownloader resourceDownloader)
            : base(shipment, label, resourceManager, resourceDownloader)
        {
        }

        /// <summary>
        /// Save the Shipengine label ID to the Ups Shipment
        /// </summary>
        protected override void SaveShipEngineLabelID(ShipmentEntity shipment, Label label) =>
            shipment.Ups.ShipEngineLabelID = label.LabelId;
    }
}
