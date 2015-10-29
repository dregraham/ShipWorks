using System.IO;
using Interapptive.Shared.Pdf;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Data;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Shipping.Carriers.FedEx;
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
            CreateShipmentResponse labelResponse = webClient.CreateShipment(requestFactory.Create(shipment, order), settings, shipment.Amazon.ShippingServiceID);
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
        /// Save a label of the given name ot the database from the specified label document
        /// </summary>
        private static void SaveLabel()
        {
            //// If its a pdf we need to convert it
            //if (false)
            //{
            //    using (MemoryStream pdfBytes = new MemoryStream(labelDocument.Parts[0].Image))
            //    {
            //        using (PdfDocument pdf = new PdfDocument(pdfBytes))
            //        {
            //            DataResourceManager.CreateFromPdf(pdf, ownerID, name);
            //        }
            //    }
            //}
            //else
            //{
            //    // Convert the string into an image stream
            //    using (MemoryStream imageStream = new MemoryStream(labelDocument.Parts[0].Image))
            //    {
            //        // Save the label image
            //        DataResourceManager.CreateFromBytes(imageStream.ToArray(), ownerID, name);

            //        if (InterapptiveOnly.IsInterapptiveUser)
            //        {
            //            string fileName = FedExUtility.GetCertificationFileName(certificationId, certificationId, name + "_" + ownerID, "PNG", false);
            //            File.WriteAllBytes(fileName, labelDocument.Parts[0].Image);
            //        }
            //    }
            //}
        }
    }
}
