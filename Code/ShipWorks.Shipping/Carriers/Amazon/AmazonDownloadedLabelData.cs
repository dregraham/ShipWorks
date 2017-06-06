using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Interapptive.Shared.Imaging;
using Interapptive.Shared.IO.Zip;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Label data that has been downloaded from a carrier
    /// </summary>
    [Component(RegistrationType.Self)]
    public class AmazonDownloadedLabelData : IDownloadedLabelData
    {
        private readonly ShipmentEntity shipment;
        private readonly AmazonShipment labelResponse;
        private readonly IObjectReferenceManager objectReferenceManager;
        private readonly IDataResourceManager resourceManager;
        private readonly IEnumerable<IAmazonLabelEnforcer> labelEnforcers;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonDownloadedLabelData(ShipmentEntity shipment,
            AmazonShipment labelResponse,
            IEnumerable<IAmazonLabelEnforcer> labelEnforcers,
            IObjectReferenceManager objectReferenceManager,
            IDataResourceManager resourceManager)
        {
            this.shipment = shipment;
            this.labelResponse = labelResponse;
            this.labelEnforcers = labelEnforcers;
            this.objectReferenceManager = objectReferenceManager;
            this.resourceManager = resourceManager;
        }

        /// <summary>
        /// Save label data to the database and/or disk
        /// </summary>
        public virtual void Save()
        {
            // Save shipment info
            SaveShipmentInfoToEntity(labelResponse, shipment);

            // Save the label
            SaveLabel(labelResponse.Label.FileContents, shipment.ShipmentID);

            VerifyShipment(shipment);
        }

        /// <summary>
        /// Save the shipment info to the entity
        /// </summary>
        private void SaveShipmentInfoToEntity(AmazonShipment amazonShipment, ShipmentEntity shipment)
        {
            shipment.TrackingNumber = amazonShipment.TrackingId;
            shipment.ShipmentCost = amazonShipment.ShippingService.Rate.Amount;
            shipment.Amazon.AmazonUniqueShipmentID = amazonShipment.ShipmentId;
        }

        /// <summary>
        /// Save a label of the given name to the database from the specified fileContents
        /// </summary>
        private void SaveLabel(FileContents fileContents, long shipmentID)
        {
            // Interapptive users have an unprocess button.  If we are reprocessing we need to clear the old images
            objectReferenceManager.ClearReferences(shipmentID);

            // Decompress the label string
            byte[] labelBytes = GZipUtility.Decompress(Convert.FromBase64String(fileContents.Contents));

            // If it's a PDF we need to convert it
            if (fileContents.FileType == "application/pdf")
            {
                using (MemoryStream pdfBytes = new MemoryStream(labelBytes))
                {
                    resourceManager.CreateFromPdf(pdfBytes, shipmentID,
                        i => i == 0 ? "LabelPrimary" : $"LabelPart{i}",
                        SaveCroppedLabel);
                }
            }
            else
            {
                // Save the label to the database
                resourceManager.CreateFromBytes(labelBytes, shipmentID, "LabelPrimary");
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
        /// Verify the shipment with all registered enforcers
        /// </summary>
        private void VerifyShipment(ShipmentEntity shipment)
        {
            foreach (IAmazonLabelEnforcer enforcer in labelEnforcers)
            {
                enforcer.VerifyShipment(shipment);
            }
        }
    }
}
