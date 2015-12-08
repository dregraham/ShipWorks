using System;
using System.IO;
using Interapptive.Shared.IO.Zip;
using Interapptive.Shared.Pdf;
using Interapptive.Shared.Imaging;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using ShipWorks.Data;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using Autofac.Features.Indexed;
using ShipWorks.Stores.Platforms.Amazon.Mws;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Manage label through Amazon
    /// </summary>
    public class AmazonLabelService : ILabelService
    {
        private readonly IDataResourceManager resourceManager;
        private readonly IEnumerable<IAmazonLabelEnforcer> labelEnforcers;
        private readonly IIndex<AmazonMwsApiCall, IAmazonShipmentRequest> amazonRequest;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonLabelService(IDataResourceManager resourceManager, IEnumerable<IAmazonLabelEnforcer> labelEnforcers, IIndex<AmazonMwsApiCall, IAmazonShipmentRequest> amazonRequest)
        {
            this.resourceManager = resourceManager;
            this.labelEnforcers = labelEnforcers;
            this.amazonRequest = amazonRequest;
        }
        
        /// <summary>
        /// Create the label
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("SonarQube", "S3240:The simplest possible condition syntax should be used", Justification = "More readable this way.")]
        public void Create(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
            
            EnforceLabelPolicies(shipment);

            AmazonShipment labelResponse = amazonRequest[AmazonMwsApiCall.CreateShipment].Submit(shipment);

            // Save shipment info
            SaveShipmentInfoToEntity(labelResponse, shipment);

            // Save the label
            SaveLabel(labelResponse.Label.FileContents, shipment.ShipmentID);

            VerifyShipment(shipment);
        }

        /// <summary>
        /// Void the Shipment
        /// </summary>
        public void Void(ShipmentEntity shipment)
        {
            amazonRequest[AmazonMwsApiCall.CancelShipment].Submit(shipment);
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
            // Decompress the label string
            byte[] labelBytes = GZipUtility.Decompress(Convert.FromBase64String(fileContents.Contents));

            // If its a pdf we need to convert it
            if (fileContents.FileType == "application/pdf")
            {
                using (MemoryStream pdfBytes = new MemoryStream(labelBytes))
                {
                    using (PdfDocument pdf = new PdfDocument(pdfBytes))
                    {
                        // Convert the PDF to images and then save to database
                        // If cropping fails we save the pdf without cropping
                        if (!SavePdfLabel(pdf, shipmentID))
                        {
                            resourceManager.CreateFromPdf(pdf, shipmentID, "LabelPrimary");
                        }
                    }
                }
            }
            else
            {
                // Save the label to the database
                resourceManager.CreateFromBytes(labelBytes, shipmentID, "LabelPrimary");
            }
        }

        /// <summary>
        /// Converts Ppf to Cropped label image
        /// </summary>
        private bool SavePdfLabel(PdfDocument pdf, long shipmentID)
        {
            // We need to convert the PDF into images and register each image as a resource in the database
            List<Stream> images = pdf.ToImages().ToList();

            // Try to crop the label 
            try
            {
                // loop each image, if the pdf had multiple pages
                for (int i = 0; i < images.Count; i++)
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        // Create a cropped image
                        Bitmap labelImage = EdgeDetection.Crop(images[i]);

                        // Save the cropped image
                        labelImage.Save(memoryStream, ImageFormat.Png);

                        // If for some reason its a multi part label
                        string labelName = i == 0 ? "LabelPrimary" : $"LabelPart{i}";

                        // Save the label
                        resourceManager.CreateFromBytes(memoryStream.ToArray(), shipmentID, labelName);
                    }
                }
            }
            catch (Exception)
            {
                // Something went wrong with the cropping
                // return false and use the old method
                // of saving a pdf
                return false;
            }
            // everything worked
            return true;
        }

        /// <summary>
        /// Enforce label policies for Amazon
        /// </summary>
        private void EnforceLabelPolicies(ShipmentEntity shipment)
        {
            EnforcementResult result = labelEnforcers.Select(x => x.CheckRestriction(shipment))
                .FirstOrDefault(x => x != EnforcementResult.Success);

            if (result != null)
            {
                throw new AmazonShippingException(result.FailureReason);
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
