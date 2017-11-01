using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Extra implementation of the FedExShipmentEntity
    /// </summary>
    public partial class ReadOnlyFedExShipmentEntity
    {
        /// <summary>
        /// Get the Broker address
        /// </summary>
        public PersonAdapter BrokerPerson { get; private set; }

        /// <summary>
        /// Get the COD address
        /// </summary>
        public PersonAdapter CodPerson { get; private set; }

        /// <summary>
        /// Get the Hold At Location address
        /// </summary>
        public PersonAdapter HoldPerson { get; private set; }

        /// <summary>
        /// Copy custom data
        /// </summary>
        partial void CopyCustomFedExShipmentData(IFedExShipmentEntity source)
        {
            BrokerPerson = source.BrokerPerson.CopyToNew();
            CodPerson = source.CodPerson.CopyToNew();
            HoldPerson = source.HoldPerson.CopyToNew();
        }
    }
}
