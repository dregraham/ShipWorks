using System;
using System.IO;
using Interapptive.Shared.IO.Zip;
using Interapptive.Shared.Pdf;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using ShipWorks.Data;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using System.Collections.Generic;
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
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonLabelService(IAmazonShippingWebClient webClient, IAmazonMwsWebClientSettingsFactory settingsFactory,
            IOrderManager orderManager, IAmazonShipmentRequestDetailsFactory requestFactory,
            IDataResourceManager resourceManager, IEnumerable<IAmazonLabelEnforcer> labelEnforcers)
            : this(webClient, settingsFactory, orderManager, requestFactory, resourceManager, labelEnforcers, LogManager.GetLogger(typeof(AmazonLabelService)))
        {}

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonLabelService(IAmazonShippingWebClient webClient, IAmazonMwsWebClientSettingsFactory settingsFactory,
            IOrderManager orderManager, IAmazonShipmentRequestDetailsFactory requestFactory,
            IDataResourceManager resourceManager, IEnumerable<IAmazonLabelEnforcer> labelEnforcers, ILog log)
        {
            this.webClient = webClient;
            this.settingsFactory = settingsFactory;
            this.orderManager = orderManager;
            this.requestFactory = requestFactory;
            this.resourceManager = resourceManager;
            this.labelEnforcers = labelEnforcers;
            this.log = log;
        }

        /// <summary>
        /// Create the label
        /// </summary>
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
                        resourceManager.CreateFromPdf(pdf, shipmentID, "LabelPrimary");
                    }
                }
            }
            else
            {
                //Convert the string into an image stream
                resourceManager.CreateFromBytes(labelBytes, shipmentID, "LabelPrimary");
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
