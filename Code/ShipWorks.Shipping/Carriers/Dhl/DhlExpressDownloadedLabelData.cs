using ShipWorks.Data.Model.EntityClasses;
using System;
using ShipEngine.ApiClient.Model;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data;
using ShipWorks.Shipping.ShipEngine;
using static ShipEngine.ApiClient.Model.Label;
using ShipWorks.ApplicationCore.Logging;
using System.IO;
using ShipWorks.Common.IO.Hardware.Printers;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// Save Dhl label data
    /// </summary>
    [Component(RegistrationType.Self)]
    public class DhlExpressDownloadedLabelData : IDownloadedLabelData
    {
        private readonly ShipmentEntity shipment;
        private readonly Label label;
        private readonly IDataResourceManager resourceManager;
        private readonly IShipEngineResourceDownloader resourceDownloader;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlExpressDownloadedLabelData(ShipmentEntity shipment, 
            Label label, 
            IDataResourceManager resourceManager, 
            IShipEngineResourceDownloader resourceDownloader)
        {
            this.shipment = shipment;
            this.label = label;
            this.resourceManager = resourceManager;
            this.resourceDownloader = resourceDownloader;
        }
        
        /// <summary>
        /// Save the label data
        /// </summary>
        public void Save()
        {
            SaveLabelInfoToEntity(shipment, label);
            byte[] labelResource;

            try
            {
                labelResource = resourceDownloader.Download(new Uri(label.LabelDownload.Href), ApiLogSource.DHLExpress);
            }
            catch (ShipEngineException ex)
            {
                throw new DhlExpressException(ex.Message);
            }
            
            switch (label.LabelFormat)
            {
                case LabelFormatEnum.Pdf:
                    SavePdfLabel(labelResource);
                    break;
                case LabelFormatEnum.Zpl:
                    shipment.ActualLabelFormat = (int)ThermalLanguage.ZPL;
                    SaveZplLabel(labelResource);
                    break;
                default:
                case LabelFormatEnum.Png:
                    throw new DhlExpressException("DHL Express returned an unsupported label format.");
            }
        }

        /// <summary>
        /// Save the ZPL label
        /// </summary>
        private void SaveZplLabel(byte[] labelResource)
        {
            resourceManager.CreateFromBytes(labelResource, shipment.ShipmentID, "LabelPrimary");
        }

        /// <summary>
        /// Save the PDF label
        /// </summary>
        private void SavePdfLabel(byte[] labelResource)
        {
            using (MemoryStream pdfData = new MemoryStream(labelResource))
            {
                resourceManager.CreateFromPdf(pdfData, shipment.ShipmentID, i => i == 0 ? "LabelPrimary" : $"LabelPart{i}", (m) => m.ToArray());
            }
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
