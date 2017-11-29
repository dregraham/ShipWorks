using System.IO;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Pdf;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Ship
{
    /// <summary>
    /// Responsible for saving retrieved FedEx LTL Freight Labels to Database
    /// </summary>
    [Component(RegistrationType.Self)]
    public class FedExLTLFreightLabelRepository : IFedExLabelRepository
    {
        private readonly IDataResourceManager dataResourceManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExLTLFreightLabelRepository(IDataResourceManager dataResourceManager)
        {
            this.dataResourceManager = dataResourceManager;
        }

        /// <summary>
        /// Responsible for saving retrieved FedEx LTL Freight Labels to Database
        /// </summary>
        public void SaveLabels(IShipmentEntity shipment, ProcessShipmentReply reply)
        {
            string certificationId = GetCertificationId(reply);

            SaveLtlPackageLabels(reply, shipment, certificationId);
        }

        /// <summary>
        /// Save LTL package labels (Not standard)
        /// </summary>
        private void SaveLtlPackageLabels(IFedExNativeShipmentReply reply, IShipmentEntity shipment, string certificationId)
        {
            // Save the label images
            foreach (ShippingDocument shippingDocument in reply.CompletedShipmentDetail.ShipmentDocuments)
            {
                IFedExPackageEntity package = shipment.FedEx.Packages.FirstOrDefault();

                // Save the primary label image
                if (shippingDocument?.Parts != null)
                {
                    SaveLabel(shippingDocument, shipment.ShipmentID, package.FedExPackageID, certificationId);
                }
            }
        }

        /// <summary>
        /// Save a label of the given name to the database from the specified label document
        /// </summary>
        private void SaveLabel(ShippingDocument labelDocument, long shipmentID, long ownerID, string certificationId)
        {
            var labelName = GetNameForDocument(labelDocument);

            using (MemoryStream stream = new MemoryStream(labelDocument.Parts[0].Image))
            {
                if (labelDocument.ImageType == ShippingDocumentImageType.PDF)
                {
                    dataResourceManager.CreateFromPdf(PdfDocumentType.Color, stream, shipmentID, i => $"{labelName}-{i}", s => s.ToArray());
                }
                else
                {
                    dataResourceManager.CreateFromBytes(stream.ToArray(), ownerID, labelName);
                }
            }

            if (InterapptiveOnly.IsInterapptiveUser)
            {
                // Save the label for certification purposes
                string format = labelDocument.ImageType == ShippingDocumentImageType.PDF ? "PDF" : "PNG";
                string fileName = FedExUtility.GetCertificationFileName(certificationId, certificationId, "LabelImage" + "_" + shipmentID, format, false);
                File.WriteAllBytes(fileName, labelDocument.Parts[0].Image);
            }
        }

        /// <summary>
        /// If we had saved an image for this shipment previously, but the shipment errored out later (like for an MPS), then clear before
        /// we start.
        /// </summary>
        public void ClearReferences(IShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

            ObjectReferenceManager.ClearReferences(shipment.ShipmentID);
            foreach (var package in shipment.FedEx.Packages)
            {
                ObjectReferenceManager.ClearReferences(package.FedExPackageID);
            }
        }

        /// <summary>
        /// Get the certification id from the given reply
        /// </summary>
        private static string GetCertificationId(IFedExNativeShipmentReply reply)
        {
            return reply.TransactionDetail != null ?
                reply.TransactionDetail.CustomerTransactionId :
                string.Empty;
        }

        /// <summary>
        /// Get the name of the document
        /// </summary>
        private string GetNameForDocument(ShippingDocument labelDocument)
        {
            switch (labelDocument.Type)
            {
                case ReturnedShippingDocumentType.FREIGHT_ADDRESS_LABEL:
                    return "LabelImage";
                case ReturnedShippingDocumentType.OUTBOUND_LABEL:
                    return "BOL";
                default:
                    return "DocumentAuxiliaryLabel";
            }
        }
    }
}
