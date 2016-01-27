using System;
using System.IO;
using Autofac;
using Autofac.Features.OwnedInstances;
using ShipWorks.ApplicationCore;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Fims
{
    /// <summary>
    /// Responsible for saving retrieved FedEx FIMS Labels to Database
    /// </summary>
    public class FimsLabelRepository : IFimsLabelRepository
    {
        private readonly IDataResourceManager dataResourceManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public FimsLabelRepository()
        {
            dataResourceManager = IoC.UnsafeGlobalLifetimeScope.Resolve<Owned<IDataResourceManager>>().Value;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public FimsLabelRepository(IDataResourceManager dataResourceManager)
        {
            this.dataResourceManager = dataResourceManager;
        }

        /// <summary>
        /// Responsible for saving retrieved FedEx FIMS Labels to Database
        /// </summary>
        public void SaveLabel(IFimsShipResponse fimsShipResponse, long ownerID)
        {
            if (fimsShipResponse.LabelPdfData == null)
            {
                throw new ArgumentNullException("fimsShipResponse", "The FIMS Label data is required");
            }

            // Create a ShipWorks label from the PDF received from FIMS
            using (MemoryStream pdfBytes = new MemoryStream(fimsShipResponse.LabelPdfData))
            {
                dataResourceManager.CreateFromPdf(pdfBytes, ownerID, "LabelImage");
            }
        }

        /// <summary>
        /// DO NOT USE FOR FIMS
        /// </summary>
        public void SaveLabels(ICarrierResponse response)
        {
            throw new NotImplementedException();
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
