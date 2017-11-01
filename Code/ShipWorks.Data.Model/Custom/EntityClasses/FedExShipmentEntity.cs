using Interapptive.Shared.Business;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Partial class for builtin FedExShipmentEntity
    /// </summary>
    public partial class FedExShipmentEntity
    {
        /// <summary>
        /// Get the Broker address
        /// </summary>
        public PersonAdapter BrokerPerson => new PersonAdapter(this, "Broker");

        /// <summary>
        /// Get the COD address
        /// </summary>
        public PersonAdapter CodPerson => new PersonAdapter(this, "Cod");

        /// <summary>
        /// Get the Hold At address
        /// </summary>
        public PersonAdapter HoldPerson => new PersonAdapter(this, "Hold");
    }
}
