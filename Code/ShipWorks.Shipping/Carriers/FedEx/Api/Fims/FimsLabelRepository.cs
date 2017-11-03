using System;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Fims
{
    /// <summary>
    /// Responsible for saving retrieved FedEx FIMS Labels to Database
    /// </summary>
    [Component]
    public class FimsLabelRepository : IFimsLabelRepository
    {
        private readonly IDataResourceManager dataResourceManager;

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
            if (fimsShipResponse.LabelData == null)
            {
                throw new ArgumentNullException("fimsShipResponse", "The FIMS Label data is required");
            }

            dataResourceManager.CreateFromBytes(fimsShipResponse.LabelData, ownerID, "LabelImage");
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
    }
}
