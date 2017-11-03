using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using Interapptive.Shared;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response
{
    /// <summary>
    /// Responsible for saving retrieved FedEx Labels to Database
    /// </summary>
    [Component]
    public class FedExLabelRepository : IFedExLabelRepository
    {
        private readonly IDataResourceManager dataResourceManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExLabelRepository(IDataResourceManager dataResourceManager)
        {
            this.dataResourceManager = dataResourceManager;
        }

        /// <summary>
        /// Responsible for saving retrieved FedEx Labels to Database
        /// </summary>
        public void SaveLabels(IShipmentEntity shipment, ProcessShipmentReply reply)
        {
            SaveShipmentLabels(reply, shipment);

            SavePackageLabels(reply, shipment);

            SaveAssociatedShipments(reply, shipment);
        }

        /// <summary>
        /// Loops through packages, saving labels
        /// </summary>
        /// <param name="reply">ProcessShipmentReply from FedEx</param>
        /// <param name="shipment">Shipment whose entity information we sent to FedEx </param>
        [NDependIgnoreLongMethod]
        private void SavePackageLabels(IFedExNativeShipmentReply reply, IShipmentEntity shipment)
        {
            string certificationId = GetCertificationId(reply);

            // Save the label images
            using (SqlAdapter adapter = new SqlAdapter())
            {
                foreach (CompletedPackageDetail packageReply in reply.CompletedShipmentDetail.CompletedPackageDetails)
                {
                    var package = shipment.FedEx.Packages.ElementAt(int.Parse(packageReply.SequenceNumber) - 1);

                    // Save the primary label image
                    if (packageReply.Label != null)
                    {
                        SaveLabel("LabelImage", packageReply.Label, package.FedExPackageID, certificationId);
                    }

                    // Package level COD
                    if (shipment.FedEx.CodEnabled && packageReply.CodReturnDetail != null && packageReply.CodReturnDetail.Label != null)
                    {
                        SaveLabel("COD", packageReply.CodReturnDetail.Label, package.FedExPackageID, certificationId);
                    }

                    // Save all the additional labels
                    if (packageReply.PackageDocuments != null)
                    {
                        IEnumerable<ShippingDocument> shippingDocs = packageReply.PackageDocuments
                                                                          .Where(d => d.Type != ReturnedShippingDocumentType.TERMS_AND_CONDITIONS);

                        // Save off any alcohol stickers
                        foreach (ShippingDocument document in shippingDocs.Where(d => d.AccessReference != null && d.AccessReference.ToUpperInvariant() == "ALCOHOL-SEL"))
                        {
                            SaveLabel(GetLabelName(document.Type) + "AlcoholSticker", document, package.FedExPackageID, certificationId);
                        }

                        // Save off non alcohol labels that have an AccessReference value
                        foreach (ShippingDocument document in packageReply.PackageDocuments.Where(d => d.AccessReference != null && d.AccessReference.ToUpperInvariant() != "ALCOHOL-SEL"))
                        {
                            SaveLabel("Document" + GetLabelName(document.Type), document, package.FedExPackageID, certificationId);
                        }

                        // Save off the OP-900 document (AccessReference is null, so it's not captured in the section above)
                        foreach (ShippingDocument document in packageReply.PackageDocuments.Where(d => d.Type == ReturnedShippingDocumentType.OP_900))
                        {
                            SaveLabel("Document" + GetLabelName(document.Type), document, package.FedExPackageID, certificationId);
                        }

                        // Save off the Dangerous Goods Shipper Declaration document (AccessReference is null, so it's not captured in the section above)
                        foreach (ShippingDocument document in packageReply.PackageDocuments.Where(d => d.Type == ReturnedShippingDocumentType.DANGEROUS_GOODS_SHIPPERS_DECLARATION))
                        {
                            SaveLabel("Document" + GetLabelName(document.Type), document, package.FedExPackageID, certificationId);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Saves labels at the shipment level
        /// </summary>
        /// <param name="reply">ProcessShipmentReply from FedEx</param>
        /// <param name="shipment">Shipment whose entity information we sent to FedEx </param>
        private void SaveShipmentLabels(IFedExNativeShipmentReply reply, IShipmentEntity shipment)
        {
            // Documents
            if (reply.CompletedShipmentDetail.ShipmentDocuments != null)
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    foreach (ShippingDocument document in reply.CompletedShipmentDetail.ShipmentDocuments
                                                               .Where(d => d.Type != ReturnedShippingDocumentType.TERMS_AND_CONDITIONS))
                    {
                        SaveLabel("Document" + GetLabelName(document.Type), document, shipment.ShipmentID, GetCertificationId(reply));
                    }
                }
            }
        }

        /// <summary>
        /// Saves the associated shipments.
        /// </summary>
        /// <param name="reply">The reply.</param>
        /// <param name="shipment">The shipment.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void SaveAssociatedShipments(IFedExNativeShipmentReply reply, IShipmentEntity shipment)
        {
            if (reply.CompletedShipmentDetail.AssociatedShipments != null)
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    foreach (AssociatedShipmentDetail associatedShipment in reply.CompletedShipmentDetail.AssociatedShipments
                                                                                 .Where(a => a.Label != null && a.Label.Type == ReturnedShippingDocumentType.COD_RETURN_LABEL))
                    {
                        SaveLabel("COD", associatedShipment.Label, shipment.ShipmentID, GetCertificationId(reply));
                    }
                }
            }
        }

        /// <summary>
        /// Save a label of the given name to the database from the specified label document
        /// </summary>
        private void SaveLabel(string name, ShippingDocument labelDocument, long ownerID, string certificationId)
        {
            // We need to know if this ever happens
            if (labelDocument.Parts.Length != 1)
            {
                throw new ShippingException("Multiple parts returned for label. " + labelDocument);
            }

            if (labelDocument.ImageType == ShippingDocumentImageType.PDF)
            {
                using (MemoryStream pdfBytes = new MemoryStream(labelDocument.Parts[0].Image))
                {
                    if (InterapptiveOnly.IsInterapptiveUser)
                    {
                        string fileName = FedExUtility.GetCertificationFileName(certificationId, certificationId, name + "_" + ownerID, "PDF", false);
                        File.WriteAllBytes(fileName, labelDocument.Parts[0].Image);
                    }

                    dataResourceManager.CreateFromPdf(pdfBytes, ownerID, name);
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
        /// Get the name of the label to be used for the specified document type
        /// </summary>
        private static string GetLabelName(ReturnedShippingDocumentType documentType)
        {
            switch (documentType)
            {
                case ReturnedShippingDocumentType.AUXILIARY_LABEL:
                    return "DocumentAuxiliaryLabel";

                case ReturnedShippingDocumentType.TERMS_AND_CONDITIONS:
                    return "DocumentTermsAndConditions";

                case ReturnedShippingDocumentType.COD_RETURN_LABEL:
                    return "COD";

                case ReturnedShippingDocumentType.OP_900:
                    return "OP-900";

                case ReturnedShippingDocumentType.COMMERCIAL_INVOICE:
                    return "CommercialInvoice";

                case ReturnedShippingDocumentType.DANGEROUS_GOODS_SHIPPERS_DECLARATION:
                    return "DangerousGoodsShippersDeclaration";
            }

            throw new InvalidOperationException("Unhandled label document type: " + documentType);
        }

        /// <summary>
        /// If we had saved an image for this shipment previously, but the shipment errored out later (like for an MPS), then clear before
        /// we start.
        /// </summary>
        public void ClearReferences(IShipmentEntity shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            using (SqlAdapter adapter = new SqlAdapter())
            {
                ObjectReferenceManager.ClearReferences(shipment.ShipmentID);
                foreach (var package in shipment.FedEx.Packages)
                {
                    ObjectReferenceManager.ClearReferences(package.FedExPackageID);
                }
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
