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
                    SaveLabel("LabelImage", shippingDocument, package.FedExPackageID, certificationId);
                }
            }
        }

        /// <summary>
        /// Save a label of the given name to the database from the specified label document
        /// </summary>
        private void SaveLabel(string name, ShippingDocument labelDocument, long ownerID, string certificationId)
        {
            if (labelDocument.ImageType == ShippingDocumentImageType.PDF)
            {
                using (MemoryStream pdfStream = new MemoryStream(labelDocument.Parts[0].Image))
                {
                    if (InterapptiveOnly.IsInterapptiveUser)
                    {
                        string fileName = FedExUtility.GetCertificationFileName(certificationId, certificationId, name + "_" + ownerID, "PDF", false);
                        File.WriteAllBytes(fileName, labelDocument.Parts[0].Image);
                    }

                    dataResourceManager.CreateFromPdf(PdfDocumentType.Color, pdfStream, ownerID, i => i == 0 ? "LabelImage" : $"DocumentAuxiliaryLabel-{i}", s => s.ToArray());
                }
            }
            else
            {
                // Convert the string into an image stream
                using (MemoryStream imageStream = new MemoryStream(labelDocument.Parts[0].Image))
                {
                    // Save the label image
                    dataResourceManager.CreateFromBytes(imageStream.ToArray(), ownerID, name);

                    if (InterapptiveOnly.IsInterapptiveUser)
                    {
                        string fileName = FedExUtility.GetCertificationFileName(certificationId, certificationId, name + "_" + ownerID, "PNG", false);
                        File.WriteAllBytes(fileName, labelDocument.Parts[0].Image);
                    }
                }
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
    }
}
