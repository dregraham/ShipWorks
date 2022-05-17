using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Pdf;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.ShipEngine.DTOs;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.UI;

namespace ShipWorks.Shipping.Carriers.Ups.ShipEngine
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
        /// Save Ups specific label data to the shipment
        /// </summary>
        protected override void SaveLabelInfoToEntity(ShipmentEntity shipment, Label label)
        {
            base.SaveLabelInfoToEntity(shipment, label);

            shipment.Ups.PublishedCharges = shipment.ShipmentCost;
            shipment.Ups.NegotiatedRate = true;
            shipment.BilledType = (int) BilledType.Unknown;
            shipment.TrackingStatus = TrackingStatus.Pending;

            UpsShipmentEntity upsShipment = shipment.Ups;

            for (int i = 0; i < label.Packages.Count; i++)
            {
                LabelPackage labelPackage = label.Packages[i];
                UpsPackageEntity upsPackage = upsShipment.Packages[i];

                upsPackage.TrackingNumber = labelPackage.TrackingNumber;
            }
        }

        /// <summary>
        /// Save the Shipengine label ID to the Ups Shipment
        /// </summary>
        protected override void SaveShipEngineLabelID(ShipmentEntity shipment, Label label) =>
            shipment.Ups.ShipEngineLabelID = label.LabelId;
    }
}
