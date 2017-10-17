using ShipWorks.Data.Model.EntityClasses;
using System;
using System.Linq;
using ShipEngine.ApiClient.Model;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data;
using ShipWorks.Shipping.ShipEngine;
using static ShipEngine.ApiClient.Model.Label;
using ShipWorks.ApplicationCore.Logging;
using System.IO;
using System.Drawing;
using Interapptive.Shared.Imaging;
using System.Drawing.Imaging;
using ShipWorks.Common.IO.Hardware.Printers;

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
        private readonly IShipEngineResourceDownloader resourceDownloader;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlExpressDownloadedLabelData(ShipmentEntity shipment, Label label, IDataResourceManager resourceManager, IShipEngineResourceDownloader resourceDownloader)
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

            byte[] labelResource = resourceDownloader.Download(new Uri(label.LabelDownload.Href), ApiLogSource.DHLExpress, "GetLabel");
            byte[] documentsResource = resourceDownloader.Download(new Uri(label.FormDownload.Href), ApiLogSource.DHLExpress, "GetResource");

            // None of this will work, need to smoke test and fix everything sorry in advance!
            switch (label.LabelFormat)
            {
                case LabelFormatEnum.Pdf:
                    shipment.ActualLabelFormat = (int)ThermalLanguage.None;
                    SavePdfLabel(labelResource, documentsResource);
                    break;
                case LabelFormatEnum.Zpl:
                    shipment.ActualLabelFormat = (int)ThermalLanguage.ZPL;
                    SaveZplLabel(labelResource, documentsResource);
                    break;
                default:
                case LabelFormatEnum.Png:
                    throw new DhlExpressException("DHL Express returned an unsupported label format.");
            }
        }

        /// <summary>
        /// Save the ZPL label
        /// </summary>
        private void SaveZplLabel(byte[] labelResource, byte[] documentsResource)
        {
            resourceManager.CreateFromBytes(labelResource, shipment.ShipmentID, "LabelPrimary");

            if (documentsResource.Any())
            {
                resourceManager.CreateFromBytes(documentsResource, shipment.ShipmentID, "LabelPrimary");
            }
        }

        /// <summary>
        /// Save the PDF label
        /// </summary>
        private void SavePdfLabel(byte[] labelResource, byte[] documentsResource)
        {
            using (MemoryStream pdfBytes = new MemoryStream(labelResource))
            {
                resourceManager.CreateFromPdf(pdfBytes, shipment.ShipmentID, i => i == 0 ? "LabelPrimary" : $"LabelPart{i}", SaveCroppedLabel);
            }

            using (MemoryStream pdfBytes = new MemoryStream(documentsResource))
            {
                resourceManager.CreateFromPdf(pdfBytes, shipment.ShipmentID, i => i == 0 ? "LabelPrimary" : $"LabelPart{i}", SaveCroppedLabel);
            }
        }

        /// <summary>
        /// Save the cropped label
        /// </summary>
        private byte[] SaveCroppedLabel(MemoryStream stream)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (Bitmap labelImage = stream.CropImageStream())
                {
                    labelImage.Save(memoryStream, ImageFormat.Png);
                }

                return memoryStream.ToArray();
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
