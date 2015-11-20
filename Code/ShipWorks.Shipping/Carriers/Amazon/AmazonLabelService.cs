using System;
using System.IO;
using Interapptive.Shared.IO.Zip;
using Interapptive.Shared.Pdf;
using Interapptive.Shared.Imaging;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using ShipWorks.Data;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using log4net;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Manage label through Amazon
    /// </summary>
    public class AmazonLabelService : IAmazonLabelService
    {
        private readonly IOrderManager orderManager;
        private readonly IAmazonMwsWebClientSettingsFactory settingsFactory;
        private readonly IAmazonShippingWebClient webClient;
        private readonly IAmazonShipmentRequestDetailsFactory requestFactory;
        private readonly IDataResourceManager resourceManager;
        private readonly IEnumerable<IAmazonLabelEnforcer> labelEnforcers;
        private static readonly ILog log = LogManager.GetLogger(typeof(AmazonLabelService));

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonLabelService(IAmazonShippingWebClient webClient, IAmazonMwsWebClientSettingsFactory settingsFactory,
            IOrderManager orderManager, IAmazonShipmentRequestDetailsFactory requestFactory,
            IDataResourceManager resourceManager, IEnumerable<IAmazonLabelEnforcer> labelEnforcers)
        {
            this.webClient = webClient;
            this.settingsFactory = settingsFactory;
            this.orderManager = orderManager;
            this.requestFactory = requestFactory;
            this.resourceManager = resourceManager;
            this.labelEnforcers = labelEnforcers;
        }


        /// <summary>
        /// Create the label
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("SonarQube", "S3240:The simplest possible condition syntax should be used", Justification = "More readable this way.")]
        public void Create(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

            orderManager.PopulateOrderDetails(shipment);
            AmazonOrderEntity order = shipment.Order as AmazonOrderEntity;
            if (order == null)
            {
                throw new ShippingException("Amazon shipping can only be used for Amazon orders");
            }

            EnforceLabelPolicies(shipment);

            IAmazonMwsWebClientSettings settings = settingsFactory.Create(shipment.Amazon);
            ShipmentRequestDetails requestDetails = requestFactory.Create(shipment, order);

            // Send a max of $100 in insured value for carriers who aren't Stamps.  Send $0 for Stamps
            if (!shipment.Amazon.CarrierName.Equals("STAMPS_DOT_COM", StringComparison.OrdinalIgnoreCase))
            {
                requestDetails.ShippingServiceOptions.DeclaredValue.Amount = Math.Min(shipment.Amazon.InsuranceValue, 100m);
            }
            else
            {
                requestDetails.ShippingServiceOptions.DeclaredValue.Amount = 0;
            }

            CreateShipmentResponse labelResponse = webClient.CreateShipment(requestDetails, settings, shipment.Amazon.ShippingServiceID);

            // Save shipment info
            SaveShipmentInfoToEntity(labelResponse.CreateShipmentResult.Shipment, shipment);

            // Save the label
            SaveLabel(labelResponse.CreateShipmentResult.Shipment.Label.FileContents, shipment.ShipmentID);

            VerifyShipment(shipment);
        }

        /// <summary>
        /// Void the Shipment
        /// </summary>
        public void Void(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

            if (shipment.Amazon.AmazonUniqueShipmentID == null)
            {
                log.Error($"Attempting to void shipment with shipment id = {shipment.ShipmentID }, but AmazonUniqueShipmentID was null");
                throw new AmazonShippingException("Amazon shipment is missing the AmazonUniqueShipmentID");
            }

            IAmazonMwsWebClientSettings settings = settingsFactory.Create(shipment.Amazon);
            
            webClient.CancelShipment(settings, shipment.Amazon.AmazonUniqueShipmentID);
        }

        /// <summary>
        /// Save the shipment info to the entity
        /// </summary>
        public void SaveShipmentInfoToEntity(Shipment amazonShipment, ShipmentEntity shipment)
        {
            // Save shipment info to shipment entity
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
