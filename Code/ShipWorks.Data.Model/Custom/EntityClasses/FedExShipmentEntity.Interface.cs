﻿using Interapptive.Shared.Business;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Extra implementation of the FedExShipmentEntity
    /// </summary>
    public partial interface IFedExShipmentEntity
    {
        /// <summary>
        /// Get the Broker address
        /// </summary>
        PersonAdapter BrokerPerson { get; }

        /// <summary>
        /// Get the COD address
        /// </summary>
        PersonAdapter CodPerson { get; }

        /// <summary>
        /// Get the Hold At Location address
        /// </summary>
        PersonAdapter HoldPerson { get; }

        /// <summary>
        /// Get the Importer address
        /// </summary>
        PersonAdapter ImporterPerson { get; }
    }
}
