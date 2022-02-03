﻿using System;
using System.Drawing;
using System.IO;
using Interapptive.Shared.Pdf;
using Interapptive.Shared.Utility;
using ShipEngine.CarrierApi.Client.Model;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using static ShipEngine.CarrierApi.Client.Model.Label;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Base class for saving ShipEngine label data
    /// </summary>
    public abstract class ShipEngineDownloadedLabelData : IDownloadedLabelData
    {
        private readonly ShipmentEntity shipment;
        private readonly Label label;
        private readonly IDataResourceManager resourceManager;
        private readonly IShipEngineResourceDownloader resourceDownloader;

        /// <summary>
        /// Constructor
        /// </summary>
        protected ShipEngineDownloadedLabelData(ShipmentEntity shipment,
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

            labelResource = resourceDownloader.Download(new Uri(label.LabelDownload.Href));

            switch (label.LabelFormat)
            {
                case LabelFormatEnum.Pdf:
                    SavePdfLabel(labelResource);
                    break;
                case LabelFormatEnum.Zpl:
                    shipment.ActualLabelFormat = (int) ThermalLanguage.ZPL;
                    SaveZplLabel(labelResource);
                    break;
                default:
                    throw new ShipEngineException($"{EnumHelper.GetDescription(shipment.ShipmentTypeCode)} returned an unsupported label format.");
            }
        }

        /// <summary>
        /// Save the ZPL label
        /// </summary>
        private void SaveZplLabel(byte[] labelResource)
        {
            resourceManager.CreateFromBytes(labelResource, shipment.ShipmentID, "LabelPrimary", true);
        }

        /// <summary>
        /// Save the PDF label
        /// </summary>
        private void SavePdfLabel(byte[] labelResource)
        {
            using (MemoryStream pdfData = new MemoryStream(labelResource))
            {
                resourceManager.CreateFromPdf(
                    PdfDocumentType.BlackAndWhite, pdfData, shipment.ShipmentID, i => i == 0 ? "LabelPrimary" : $"LabelPart{i}", (m) => m.ToArray(), true);
            }
        }

        /// <summary>
        /// Save the label info to the shipment
        /// </summary>
        /// <param name="shipment"></param>
        /// <param name="label"></param>
        protected virtual void SaveLabelInfoToEntity(ShipmentEntity shipment, Label label)
        {
            shipment.TrackingNumber = label.TrackingNumber;
            shipment.ShipmentCost = (decimal) label.ShipmentCost.Amount;
            SaveShipEngineLabelID(shipment, label);
        }

        /// <summary>
        /// Save the ShipEngine label ID to the carrier specific shipment
        /// </summary>
        protected abstract void SaveShipEngineLabelID(ShipmentEntity shipment, Label label);
    }
}

