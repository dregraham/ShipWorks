using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Autofac.Features.Indexed;
using Interapptive.Shared.Imaging;
using Interapptive.Shared.IO.Zip;
using Interapptive.Shared.Utility;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
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
        private readonly IObjectReferenceManager objectReferenceManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonLabelService(IDataResourceManager resourceManager,
            IEnumerable<IAmazonLabelEnforcer> labelEnforcers,
            IIndex<AmazonMwsApiCall, IAmazonShipmentRequest> amazonRequest,
            IObjectReferenceManager objectReferenceManager)
        {
            this.resourceManager = resourceManager;
            this.labelEnforcers = labelEnforcers;
            this.amazonRequest = amazonRequest;
            this.objectReferenceManager = objectReferenceManager;
        }

        /// <summary>
        /// Create the label
        /// </summary>
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
            // Try to crop the label 
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
