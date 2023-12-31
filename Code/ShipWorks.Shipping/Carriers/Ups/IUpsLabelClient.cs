﻿using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Ups
{
    /// <summary>
    /// represents a client for getting ups labels
    /// </summary>
    public interface IUpsLabelClient
    {
        /// <summary>
        /// Get a label for the given shipment
        /// </summary>
        Task<TelemetricResult<IDownloadedLabelData>> GetLabel(ShipmentEntity shipment);

        /// <summary>
        /// Void the given shipment
        /// </summary>
        /// <param name="shipment"></param>
        void VoidLabel(ShipmentEntity shipment);
    }
}
