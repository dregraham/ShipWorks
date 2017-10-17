using ShipWorks.Data.Model.EntityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipEngine.ApiClient.Api;
using ShipEngine.ApiClient.Client;
using ShipEngine.ApiClient.Model;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data;
using static ShipEngine.ApiClient.Model.Label;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// Save Dhl label data
    /// </summary>
    [Component(RegistrationType.Self)]
    class DhlExpressDownloadedLabelData : IDownloadedLabelData
    {
        private readonly ShipmentEntity shipment;
        private readonly Label label;
        private readonly IDataResourceManager resourceManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlExpressDownloadedLabelData(ShipmentEntity shipment, Label label, IDataResourceManager resourceManager)
        {
            this.shipment = shipment;
            this.label = label;
            this.resourceManager = resourceManager;
        }
        
        /// <summary>
        /// Save the label data
        /// </summary>
        public void Save()
        {
            SaveLabelInfoToEntity(shipment, label);

            switch (label.LabelFormat)
            {
                case LabelFormatEnum.Pdf:
                    SavePdfLabel();
                    break;
                case LabelFormatEnum.Zpl:
                    SaveZplLabel();
                    break;
                default:
                case LabelFormatEnum.Png:
                    throw new DhlExpressException("DHL Express returned an unsupported label format.");
            }
        }

        /// <summary>
        /// Save the ZPL label
        /// </summary>
        private static void SaveZplLabel()
        {

        }

        /// <summary>
        /// Save the PDF label
        /// </summary>
        private static void SavePdfLabel()
        {

        }

        /// <summary>
        /// Save the label info to the shipment
        /// </summary>
        /// <param name="shipment"></param>
        /// <param name="label"></param>
        private static void SaveLabelInfoToEntity(ShipmentEntity shipment, Label label)
        {
            shipment.TrackingNumber = label.TrackingNumber;
            shipment.ShipmentCost = (decimal)label.ShipmentCost.Amount;
            shipment.DhlExpress.ShipEngineShipmentID = label.ShipmentId;
        }
    }
}
