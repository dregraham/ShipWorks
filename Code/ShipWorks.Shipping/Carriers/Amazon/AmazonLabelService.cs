﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Interapptive.Shared;
using Interapptive.Shared.Imaging;
using Interapptive.Shared.IO.Zip;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Amazon.Mws;

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
        private readonly IObjectReferenceManager objectReferenceManager;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParams]
        public AmazonLabelService(IAmazonShippingWebClient webClient, IAmazonMwsWebClientSettingsFactory settingsFactory,
            IOrderManager orderManager, IAmazonShipmentRequestDetailsFactory requestFactory,
            IDataResourceManager resourceManager, IEnumerable<IAmazonLabelEnforcer> labelEnforcers,
            IObjectReferenceManager objectReferenceManager)
        {
            // TODO: refactor to get parameters down to 5 or less

            this.webClient = webClient;
            this.settingsFactory = settingsFactory;
            this.orderManager = orderManager;
            this.requestFactory = requestFactory;
            this.resourceManager = resourceManager;
            this.labelEnforcers = labelEnforcers;
            this.objectReferenceManager = objectReferenceManager;
        }

        /// <summary>
        /// Create the label
        /// </summary>
        [SuppressMessage("SonarQube", "S3240:The simplest possible condition syntax should be used",
            Justification = "More readable this way.")]
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
