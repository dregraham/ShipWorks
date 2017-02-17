using System.Collections.Generic;
using Autofac;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Shipping.ScanForms
{
    public interface IScanFormCarrierAccount
    {
        /// <summary>
        /// Gets the account entity.
        /// </summary>
        /// <returns>A carrier-specific account entity.</returns>
        IEntity2 GetAccountEntity();

        /// <summary>
        /// Gets the name of the shipping carrier.
        /// </summary>
        /// <value>The name of the shipping carrier.</value>
        string ShippingCarrierName { get; }

        ShipmentTypeCode ShipmentTypeCode { get; }

        /// <summary>
        /// Gets the IDs of the shipments eligible for a SCAN form.
        /// </summary>
        /// <returns>A collection of shipment ID values.</returns>
        IEnumerable<long> GetEligibleShipmentIDs();

        /// <summary>
        /// Gets the existing scan form batches.
        /// </summary>
        /// <returns>A collection for ScanFormBatch objects.</returns>
        IEnumerable<ScanFormBatch> GetExistingScanFormBatches();

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <returns>A string describing the account</returns>
        string GetDescription();

        /// <summary>
        /// Saves the specified scan form batch.
        /// </summary>
        /// <param name="scanFormBatch">The scan form batch.</param>
        /// <returns>A long value representing the SCAN form batch ID.</returns>
        long Save(ScanFormBatch scanFormBatch);

        /// <summary>
        /// Gets the gateway object to use for communicating with the shipping carrier API for generating SCAN forms.
        /// </summary>
        /// <returns>An IScanFormGateway object.</returns>
        IScanFormGateway GetGateway(ILifetimeScope lifetimeScope);

        /// <summary>
        /// Gets the printer to use for printing a carrier's SCAN form.
        /// </summary>
        /// <returns>An IScanFormPrinter object.</returns>
        IScanFormPrinter GetPrinter();
    }
}
