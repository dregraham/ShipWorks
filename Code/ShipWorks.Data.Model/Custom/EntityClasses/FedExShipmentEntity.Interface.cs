using Interapptive.Shared.Business;

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
    }
}
