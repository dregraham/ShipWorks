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

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonLabelService(IAmazonShippingWebClient webClient, IAmazonMwsWebClientSettingsFactory settingsFactory, IOrderManager orderManager, IAmazonShipmentRequestDetailsFactory requestFactory)
        {
            this.webClient = webClient;
            this.settingsFactory = settingsFactory;
            this.orderManager = orderManager;
            this.requestFactory = requestFactory;
        }

        /// <summary>
        /// Create the label
        /// </summary>
        /// <param name="shipment"></param>
        public void Create(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

            orderManager.PopulateOrderDetails(shipment);
            AmazonOrderEntity order = shipment.Order as AmazonOrderEntity;
            if (order == null)
            {
                throw new ShippingException("Amazon shipping can only be used for Amazon orders");
            }

            AmazonMwsWebClientSettings settings = settingsFactory.Create(shipment.Amazon);

            try
            {
                CreateShipmentResponse labelResponse = webClient.CreateShipment(requestFactory.Create(shipment, order), settings, shipment.Amazon.ShippingServiceID);

                // Save shipment info
                SaveShipmentInfoToEntity(labelResponse, shipment);

                // Save the label
                SaveLabel(labelResponse, shipment);
            }
            catch (AmazonShipperException ex)
            {
                // something went wrong
                throw new ShippingException(ex.Message);
            }

        }

        /// <summary>
        /// Void the Shipment
        /// </summary>
        public void Void(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

            AmazonMwsWebClientSettings settings = settingsFactory.Create(shipment.Amazon);
            webClient.CancelShipment(settings, shipment.Amazon.AmazonUniqueShipmentID);
        }

        /// <summary>
        /// Save the shipment info to the entity
        /// </summary>
        public void SaveShipmentInfoToEntity(CreateShipmentResponse labelResponse, ShipmentEntity shipment)
        {
            // Get the Amazon Shipment info
            Shipment amazonShipment = labelResponse.CreateShipmentResult.Shipment;

            // Save shipment info to shipment entity
            shipment.TrackingNumber = amazonShipment.TrackingId;
            shipment.ShipmentCost = amazonShipment.ShippingService.Rate.Amount;
            shipment.Amazon.AmazonUniqueShipmentID = amazonShipment.ShipmentId;
        }


        /// <summary>
        /// Save a label of the given name ot the database from the specified label document
        /// </summary>
        private static void SaveLabel(CreateShipmentResponse labelResponse, ShipmentEntity shipment)
        {
            // Grab the Amazon Shipment info
            Shipment amazonShipment = labelResponse.CreateShipmentResult.Shipment;

            // Decompress the label string
            byte[] label = GZipUtility.Decompress(Convert.FromBase64String(amazonShipment.Label.FileContents.Contents));

            // If its a pdf we need to convert it
            if (amazonShipment.Label.FileContents.FileType == "application/pdf")
            {
                using (MemoryStream pdfBytes = new MemoryStream(label))
                {
                    using (PdfDocument pdf = new PdfDocument(pdfBytes))
                    {
                        DataResourceManager.CreateFromPdf(pdf, shipment.ShipmentID, "LabelPrimary");
                    }
                }
            }
            else
            {
                //Convert the string into an image stream
                DataResourceManager.CreateFromBytes(label, shipment.ShipmentID, "LabelPrimary");
            }
        }
    }
}
