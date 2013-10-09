﻿using System;
using System.IO;
using System.Linq;
using ShipWorks.ApplicationCore;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Shipping.Response
{
    /// <summary>
    /// Responsible for saving retrieved FedEx Labels to Database
    /// </summary>
    public class FedExLabelRepository : ILabelRepository
    {
        /// <summary>
        /// Responsible for saving retrieved FedEx Labels to Database
        /// </summary>
        public void SaveLabels(ICarrierResponse response)
        {
            FedExShipResponse fedExShipResponse = (FedExShipResponse) response;

            ShipmentEntity shipment = fedExShipResponse.Shipment;
            IFedExNativeShipmentReply reply = fedExShipResponse.NativeResponse as IFedExNativeShipmentReply;

            SaveShipmentLabels(reply, shipment);

            SavePackageLabels(reply, shipment);

            SaveAssociatedShipments(reply, shipment);
        }

        /// <summary>
        /// Loops through packages, saving labels
        /// </summary>
        /// <param name="reply">ProcessShipmentReply from FedEx</param>
        /// <param name="shipment">Shipment whose entity information we sent to FedEx </param>
        private void SavePackageLabels(IFedExNativeShipmentReply reply, ShipmentEntity shipment)
        {
            string certificationId = reply.TransactionDetail.CustomerTransactionId;

            // Save the label iamges
            using (SqlAdapter adapter = new SqlAdapter())
            {
                foreach (var packageReply in reply.CompletedShipmentDetail.CompletedPackageDetails)
                {
                    FedExPackageEntity package = shipment.FedEx.Packages[int.Parse(packageReply.SequenceNumber) - 1];

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
                        foreach (ShippingDocument document in packageReply.PackageDocuments
                                                                          .Where(d => d.Type != ReturnedShippingDocumentType.TERMS_AND_CONDITIONS))
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
        private static void SaveShipmentLabels(IFedExNativeShipmentReply reply, ShipmentEntity shipment)
        {
            // Documents
            if (reply.CompletedShipmentDetail.ShipmentDocuments != null)
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    foreach (ShippingDocument document in reply.CompletedShipmentDetail.ShipmentDocuments
                                                               .Where(d => d.Type != ReturnedShippingDocumentType.TERMS_AND_CONDITIONS))
                    {
                        SaveLabel("Document" + GetLabelName(document.Type), document, shipment.ShipmentID, reply.TransactionDetail.CustomerTransactionId);
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
        private void SaveAssociatedShipments(IFedExNativeShipmentReply reply, ShipmentEntity shipment)
        {
            if (reply.CompletedShipmentDetail.AssociatedShipments != null)
            {
                using (SqlAdapter adapter = new SqlAdapter())
                    foreach (AssociatedShipmentDetail associatedShipment in reply.CompletedShipmentDetail.AssociatedShipments
                                                                                 .Where(a => a.Label != null && a.Label.Type == ReturnedShippingDocumentType.COD_RETURN_LABEL))
                    {
                        SaveLabel("COD", associatedShipment.Label, shipment.ShipmentID, reply.TransactionDetail.CustomerTransactionId);
                    }
            }
        }

        /// <summary>
        /// Save a label of the given name ot the database from the specified label document
        /// </summary>
        private static void SaveLabel(string name, ShippingDocument labelDocument, long ownerID, string certificationId)
        {
            // We need to know if this ever happens
            if (labelDocument.Parts.Length != 1)
            {
                throw new ShippingException("Multiple parts returned for label. " + labelDocument);
            }

            // Convert the string into an image stream
            using (MemoryStream imageStream = new MemoryStream(labelDocument.Parts[0].Image))
            {
                // Save the label image
                DataResourceManager.CreateFromBytes(imageStream.ToArray(), ownerID, name);

                if (InterapptiveOnly.IsInterapptiveUser)
                {
                    string fileName = FedExUtility.GetCertificationFileName(certificationId, certificationId, name + "_" + ownerID, "PNG", false);
                    File.WriteAllBytes(fileName, labelDocument.Parts[0].Image);
                }

                // File.WriteAllBytes(string.Format(@"D:\Vista Folders\Desktop\Random\{0}.png", name), labelDocument.Parts[0].Image);
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
            }

            throw new InvalidOperationException("Unhandled label document type: " + documentType);
        }

        /// <summary>
        /// If we had saved an image for this shipment previously, but the shipment errored out later (like for an MPS), then clear before
        /// we start.
        /// </summary>
        public void ClearReferences(ShipmentEntity shipment)
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
    }
}
